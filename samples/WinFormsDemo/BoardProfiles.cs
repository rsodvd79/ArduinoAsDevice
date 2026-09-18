namespace WinFormsDemo;

internal sealed class PinDefinition
{
    public PinDefinition(
        int number,
        string name,
        bool supportsDigital,
        bool supportsAnalogInput,
        bool supportsPwm,
        bool usesSerial = false)
    {
        Number = number;
        Name = name;
        SupportsDigital = supportsDigital;
        SupportsAnalogInput = supportsAnalogInput;
        SupportsPwm = supportsPwm;
        UsesSerial = usesSerial;
    }

    public int Number { get; }
    public string Name { get; }
    public bool SupportsDigital { get; }
    public bool SupportsAnalogInput { get; }
    public bool SupportsPwm { get; }
    public bool SupportsTone => SupportsDigital;
    public bool UsesSerial { get; }

    public string Capabilities
    {
        get
        {
            var capabilities = new List<string>();
            if (SupportsDigital) capabilities.Add("Digitale");
            if (SupportsAnalogInput) capabilities.Add("Analog IN");
            if (SupportsPwm) capabilities.Add("PWM");
            if (SupportsTone) capabilities.Add("Tone");
            if (UsesSerial) capabilities.Add("Seriale");
            return string.Join(", ", capabilities);
        }
    }

    public override string ToString() => Name;
}

internal sealed class BoardProfile
{
    public BoardProfile(string id, string displayName, IReadOnlyList<PinDefinition> pins)
    {
        Id = id;
        DisplayName = displayName;
        Pins = pins;
    }

    public string Id { get; }
    public string DisplayName { get; }
    public IReadOnlyList<PinDefinition> Pins { get; }

    public PinDefinition FindPin(int number) =>
        Pins.FirstOrDefault(pin => pin.Number == number);

    public override string ToString() => DisplayName;
}

internal static class BoardProfiles
{
    public static IReadOnlyList<BoardProfile> All { get; } =
    [
        CreateUno(),
        CreateNano(),
        CreateMega()
    ];

    public static BoardProfile Get(string id) =>
        All.FirstOrDefault(profile => string.Equals(profile.Id, id, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidDataException($"Profilo scheda sconosciuto: {id}");

    private static BoardProfile CreateUno() =>
        new("uno", "Arduino Uno", CreatePins(13, 6, 14, [3, 5, 6, 9, 10, 11]));

    private static BoardProfile CreateNano() =>
        new("nano", "Arduino Nano", CreatePins(13, 8, 14, [3, 5, 6, 9, 10, 11], analogOnlyFrom: 6));

    private static BoardProfile CreateMega() =>
        new("mega", "Arduino Mega", CreatePins(53, 16, 54, [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 44, 45, 46]));

    private static IReadOnlyList<PinDefinition> CreatePins(
        int lastDigitalPin,
        int analogCount,
        int firstAnalogPin,
        IReadOnlyCollection<int> pwmPins,
        int analogOnlyFrom = int.MaxValue)
    {
        var pins = new List<PinDefinition>();

        for (var pin = 0; pin <= lastDigitalPin; pin++)
            pins.Add(new PinDefinition(pin, $"D{pin}", true, false, pwmPins.Contains(pin), pin is 0 or 1));

        for (var analog = 0; analog < analogCount; analog++)
        {
            var supportsDigital = analog < analogOnlyFrom;
            pins.Add(new PinDefinition(firstAnalogPin + analog, $"A{analog}", supportsDigital, true, false));
        }

        return pins;
    }
}
