using System;

namespace ArduinoAsDevice
{
    public sealed class PinReadingEventArgs : EventArgs
    {
        public PinReadingEventArgs(int pin, int value, bool isAnalog)
        {
            Pin = pin;
            Value = value;
            IsAnalog = isAnalog;
        }

        public int Pin { get; }
        public int Value { get; }
        public bool IsAnalog { get; }
    }
}
