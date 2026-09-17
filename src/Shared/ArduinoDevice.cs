using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace ArduinoAsDevice
{
    /// <summary>
    /// Client per il firmware ArduinoAsDevice: comunica con Arduino via seriale
    /// usando il protocollo testuale (un comando per riga).
    /// </summary>
    public sealed class ArduinoDevice : IDisposable
    {
        private readonly SerialPort _port;
        private readonly object _syncLock = new object();
        private readonly StringBuilder _lineBuf = new StringBuilder();
        private readonly AutoResetEvent _responseReady = new AutoResetEvent(false);
        private volatile string _lastResponse;
        private Thread _reader;
        private volatile bool _running;
        private bool _disposed;
        private volatile bool _streamAnalog;

        /// <summary>Timeout di attesa risposta ai comandi (default 2s).</summary>
        public TimeSpan ResponseTimeout { get; set; } = TimeSpan.FromSeconds(2);

        /// <summary>Scatenato per ogni lettura inviata in automatico dallo streaming.</summary>
        public event EventHandler<PinReadingEventArgs> ReadingReceived;

        /// <summary>
        /// Callback opzionale per loggare tutta la comunicazione seriale.
        /// Il primo parametro è la riga, il secondo indica la direzione
        /// (true = inviata al device, false = ricevuta dal device).
        /// </summary>
        public Action<string, bool> Logger { get; set; }

        /// <summary>
        /// Se impostato, scrive il log della comunicazione seriale su questo writer
        /// (es. Console.Out, uno StreamWriter su file). Alternative a <see cref="Logger"/>.
        /// </summary>
        public TextWriter LogOutput { get; set; }

        public bool IsOpen { get { return _port.IsOpen; } }

        public ArduinoDevice(string portName, int baudRate = 115200)
        {
            _port = new SerialPort(portName, baudRate)
            {
                NewLine = "\n",
                ReadTimeout = 500,
                DtrEnable = true, // reset automatico delle schede AVR
                RtsEnable = true
            };
        }

        public void Open()
        {
            _port.Open();
            // Dopo l'apertura le schede AVR si resettano: attendi il boot del bootloader
            Thread.Sleep(2000);
            _port.DiscardInBuffer();

            _running = true;
            _reader = new Thread(ReaderLoop) { IsBackground = true, Name = "ArduinoDeviceReader" };
            _reader.Start();

            Ping(); // verifica connessione
        }

        public string Ping()
        {
            string r = SendCommand("PING");
            const string prefix = "PONG ";
            if (!r.StartsWith(prefix, StringComparison.Ordinal))
                throw new ArduinoException("Risposta inattesa a PING: " + r);
            return r.Substring(prefix.Length);
        }

        public void SetPinMode(int pin, PinMode mode)
        {
            string m = mode == PinMode.Output ? "OUT" : mode == PinMode.InputPullup ? "IN_PULLUP" : "IN";
            ExpectOk("MODE " + pin + " " + m);
        }

        public void DigitalWrite(int pin, bool value)
        {
            ExpectOk("DWRITE " + pin + " " + (value ? "1" : "0"));
        }

        public bool DigitalRead(int pin)
        {
            return ParseValue(SendCommand("DREAD " + pin), "D", pin) != 0;
        }

        /// <summary>Imposta il duty cycle PWM (ampiezza) 0-255.</summary>
        public void AnalogWrite(int pin, int value)
        {
            if (value < 0 || value > 255) throw new ArgumentOutOfRangeException(nameof(value), "0-255");
            ExpectOk("AWRITE " + pin + " " + value);
        }

        public int AnalogRead(int pin)
        {
            return ParseValue(SendCommand("AREAD " + pin), "A", pin);
        }

        /// <summary>Genera un'onda quadra alla frequenza indicata (Hz). 0 = stop.</summary>
        public void SetTone(int pin, int frequencyHz)
        {
            if (frequencyHz < 0) throw new ArgumentOutOfRangeException(nameof(frequencyHz));
            ExpectOk("TONE " + pin + " " + frequencyHz);
        }

        /// <summary>Avvia l'invio automatico delle letture di un pin ogni intervalMs.</summary>
        public void StartStream(int pin, bool analog, int intervalMs = 100)
        {
            if (intervalMs < 1) throw new ArgumentOutOfRangeException(nameof(intervalMs));
            ExpectOk("STREAM " + (analog ? "A" : "D") + " " + pin + " " + intervalMs);
            _streamAnalog = analog;
        }

        public void StopStream()
        {
            ExpectOk("STREAM STOP");
        }

        public void Close()
        {
            Dispose();
        }

        private void Log(string line, bool sent)
        {
            var logger = Logger;
            if (logger != null)
                logger(line, sent);
            var writer = LogOutput;
            if (writer != null)
                writer.WriteLine("{0:HH:mm:ss.fff} {1} {2}", DateTime.Now, sent ? ">>" : "<<", line);
        }

        // ----- protocollo -----

        private void ExpectOk(string command)
        {
            string r = SendCommand(command);
            if (r != "OK")
                throw new ArduinoException("Comando '" + command + "' fallito: " + r);
        }

        private string SendCommand(string command)
        {
            lock (_syncLock)
            {
                _lastResponse = null;
                Log(command, true);
                _port.WriteLine(command);
                if (!_responseReady.WaitOne(ResponseTimeout))
                    throw new ArduinoTimeoutException(command);
                string r = _lastResponse;
                if (r != null && r.StartsWith("ERR", StringComparison.Ordinal))
                    throw new ArduinoException("Errore firmware: " + r);
                return r;
            }
        }

        private static int ParseValue(string response, string prefix, int pin)
        {
            // formato atteso: "<prefix> <pin> <value>"
            var parts = response.Split(' ');
            if (parts.Length != 3 || parts[0] != prefix || parts[1] != pin.ToString())
                throw new ArduinoException("Risposta malformata: " + response);
            return int.Parse(parts[2]);
        }

        // ----- lettura seriale -----

        private void ReaderLoop()
        {
            var buf = new byte[256];
            while (_running)
            {
                int n;
                try
                {
                    n = _port.Read(buf, 0, buf.Length);
                }
                catch (TimeoutException)
                {
                    continue;
                }
                catch (Exception)
                {
                    if (_running) throw;
                    break;
                }

                for (int i = 0; i < n; i++)
                {
                    char c = (char)buf[i];
                    if (c == '\r') continue;
                    if (c == '\n')
                    {
                        string line = _lineBuf.ToString();
                        _lineBuf.Length = 0;
                        DispatchLine(line);
                    }
                    else
                    {
                        _lineBuf.Append(c);
                    }
                }
            }
        }

        private void DispatchLine(string line)
        {
            if (line.Length == 0) return;
            Log(line, false);

            // evento di streaming: "S <pin> <value>"
            if (line.StartsWith("S ", StringComparison.Ordinal))
            {
                var parts = line.Split(' ');
                if (parts.Length == 3)
                {
                    int pin, value;
                    if (int.TryParse(parts[1], out pin) && int.TryParse(parts[2], out value))
                    {
                        var handler = ReadingReceived;
                        if (handler != null)
                            handler(this, new PinReadingEventArgs(pin, value, _streamAnalog));
                    }
                }
                return;
            }

            // risposta sincrona a un comando
            _lastResponse = line;
            _responseReady.Set();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _running = false;
            try
            {
                if (_port.IsOpen)
                {
                    try { StopStream(); } catch { /* best effort */ }
                    _port.Close();
                }
            }
            finally
            {
                if (_reader != null && !_reader.Join(1000))
                {
                    // thread bloccato su Read: verrà comunque terminato come background
                }
                _port.Dispose();
                _responseReady.Dispose();
            }
        }
    }
}
