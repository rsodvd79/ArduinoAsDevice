using System;

namespace ArduinoAsDevice
{
    public class ArduinoException : Exception
    {
        public ArduinoException(string message) : base(message) { }
        public ArduinoException(string message, Exception inner) : base(message, inner) { }
    }

    public class ArduinoTimeoutException : ArduinoException
    {
        public ArduinoTimeoutException(string command)
            : base("Timeout in attesa della risposta per il comando: " + command) { }
    }
}
