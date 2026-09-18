using System.Text.Json;
using System.Text.Json.Serialization;
using ArduinoAsDevice;

namespace WinFormsDemo;

internal enum PinOutputKind
{
    Digital,
    Pwm,
    Tone
}

internal sealed class PinConfiguration
{
    public int Pin { get; set; }
    public PinMode Mode { get; set; } = PinMode.Input;
    public PinOutputKind OutputKind { get; set; }
    public bool DigitalValue { get; set; }
    public int PwmValue { get; set; }
    public int ToneFrequency { get; set; }
}

internal sealed class StreamConfiguration
{
    public int? Pin { get; set; }
    public bool Analog { get; set; }
    public int IntervalMs { get; set; } = 250;
}

internal sealed class DemoConfiguration
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public string BoardProfile { get; set; } = "uno";
    public string PortName { get; set; } = "";
    public int BaudRate { get; set; } = 115200;
    public List<PinConfiguration> Pins { get; set; } = [];
    public StreamConfiguration Stream { get; set; } = new();

    public static DemoConfiguration Load(string path)
    {
        return FromJson(File.ReadAllText(path));
    }

    public static DemoConfiguration FromJson(string json)
    {
        var configuration = JsonSerializer.Deserialize<DemoConfiguration>(json, JsonOptions)
            ?? throw new InvalidDataException("Il file non contiene una configurazione valida.");
        configuration.Validate();
        return configuration;
    }

    public void Save(string path)
    {
        File.WriteAllText(path, ToJson());
    }

    public string ToJson()
    {
        Validate();
        return JsonSerializer.Serialize(this, JsonOptions);
    }

    public void Validate()
    {
        var profile = BoardProfiles.Get(BoardProfile);

        if (PortName == null)
            throw new InvalidDataException("Il nome della porta non può essere null.");
        if (BaudRate is < 300 or > 2_000_000)
            throw new InvalidDataException("Il baud rate deve essere compreso tra 300 e 2000000.");
        if (Pins == null)
            throw new InvalidDataException("La configurazione dei pin è mancante.");
        if (Stream == null)
            throw new InvalidDataException("La configurazione dello stream è mancante.");

        var duplicates = Pins.GroupBy(pin => pin.Pin).FirstOrDefault(group => group.Count() > 1);
        if (duplicates != null)
            throw new InvalidDataException($"Il pin {duplicates.Key} è configurato più volte.");

        foreach (var configuredPin in Pins)
        {
            if (configuredPin == null)
                throw new InvalidDataException("La configurazione contiene un pin null.");
            var pin = profile.FindPin(configuredPin.Pin)
                ?? throw new InvalidDataException($"Il pin {configuredPin.Pin} non appartiene a {profile.DisplayName}.");

            if (!Enum.IsDefined(configuredPin.Mode) || !Enum.IsDefined(configuredPin.OutputKind))
                throw new InvalidDataException($"Configurazione non valida per {pin.Name}.");
            if (pin.UsesSerial && configuredPin.Mode != PinMode.Input)
                throw new InvalidDataException($"{pin.Name} è riservato alla comunicazione seriale.");
            if (!pin.SupportsDigital && configuredPin.Mode != PinMode.Input)
                throw new InvalidDataException($"{pin.Name} supporta solo l'ingresso analogico.");
            if (configuredPin.PwmValue is < 0 or > 255)
                throw new InvalidDataException($"Il valore PWM di {pin.Name} deve essere tra 0 e 255.");
            if (configuredPin.ToneFrequency is < 0 or > 20_000)
                throw new InvalidDataException($"La frequenza di {pin.Name} deve essere tra 0 e 20000 Hz.");
            if (configuredPin.OutputKind == PinOutputKind.Pwm && !pin.SupportsPwm)
                throw new InvalidDataException($"{pin.Name} non supporta PWM.");
            if (configuredPin.OutputKind == PinOutputKind.Tone && !pin.SupportsTone)
                throw new InvalidDataException($"{pin.Name} non supporta tone.");
        }

        if (Stream.IntervalMs is < 1 or > 60_000)
            throw new InvalidDataException("L'intervallo stream deve essere tra 1 e 60000 ms.");
        if (Stream.Pin is int streamPin)
        {
            var pin = profile.FindPin(streamPin)
                ?? throw new InvalidDataException($"Il pin stream {streamPin} non appartiene a {profile.DisplayName}.");
            if (Stream.Analog && !pin.SupportsAnalogInput)
                throw new InvalidDataException($"{pin.Name} non supporta letture analogiche.");
            if (!Stream.Analog && !pin.SupportsDigital)
                throw new InvalidDataException($"{pin.Name} non supporta letture digitali.");
        }
    }
}

internal static class ConfigurationSelfCheck
{
    public static void Run()
    {
        foreach (var profile in BoardProfiles.All)
        {
            if (profile.Pins.Select(pin => pin.Number).Distinct().Count() != profile.Pins.Count)
                throw new InvalidOperationException($"Il profilo {profile.Id} contiene pin duplicati.");
        }

        var expected = new DemoConfiguration
        {
            BoardProfile = "uno",
            PortName = "COM3",
            BaudRate = 115200,
            Pins =
            [
                new PinConfiguration
                {
                    Pin = 13,
                    Mode = PinMode.Output,
                    OutputKind = PinOutputKind.Digital,
                    DigitalValue = true
                }
            ],
            Stream = new StreamConfiguration { Pin = 14, Analog = true, IntervalMs = 250 }
        };

        var actual = DemoConfiguration.FromJson(expected.ToJson());
        if (actual.BoardProfile != expected.BoardProfile
            || actual.PortName != expected.PortName
            || actual.Pins.Count != 1
            || !actual.Pins[0].DigitalValue
            || actual.Stream.Pin != 14
            || !actual.Stream.Analog)
        {
            throw new InvalidOperationException("Round-trip JSON non riuscito.");
        }
    }
}
