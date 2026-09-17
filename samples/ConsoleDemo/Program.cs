using System;
using System.IO.Ports;
using ArduinoAsDevice;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Uso: ConsoleDemo <porta>  (es. COM3 oppure /dev/ttyUSB0)");
            Console.WriteLine("Porte disponibili:");
            foreach (var p in SerialPort.GetPortNames())
                Console.WriteLine("  " + p);
            return 1;
        }

        using (var dev = new ArduinoDevice(args[0]))
        {
            dev.Open();
            Console.WriteLine("Firmware: " + dev.Ping());

            // LED integrato: blink
            dev.SetPinMode(13, PinMode.Output);
            for (int i = 0; i < 4; i++)
            {
                dev.DigitalWrite(13, i % 2 == 0);
                System.Threading.Thread.Sleep(200);
            }

            // Streaming analogico su A0
            dev.SetPinMode(14 /* A0 */, PinMode.Input);
            dev.ReadingReceived += (s, e) =>
                Console.WriteLine($"[stream] pin {e.Pin} = {e.Value}");
            dev.StartStream(14, analog: true, intervalMs: 250);

            Console.WriteLine("Streaming attivo 5s...");
            System.Threading.Thread.Sleep(5000);
            dev.StopStream();
        }

        Console.WriteLine("Fine.");
        return 0;
    }
}
