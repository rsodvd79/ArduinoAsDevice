using System.IO.Ports;
using ArduinoAsDevice;

namespace WinFormsDemo;

internal sealed class MainForm : Form
{
    private readonly ComboBox _boardCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 140 };
    private readonly ComboBox _portCombo = new() { Width = 100 };
    private readonly ComboBox _baudCombo = new() { Width = 90 };
    private readonly Button _refreshPortsButton = new() { Text = "Aggiorna porte", AutoSize = true };
    private readonly Button _connectButton = new() { Text = "Avvia", AutoSize = true };
    private readonly Button _loadButton = new() { Text = "Carica...", AutoSize = true };
    private readonly Button _saveButton = new() { Text = "Salva...", AutoSize = true };
    private readonly Button _applyAllButton = new() { Text = "Applica configurazione", AutoSize = true };
    private readonly DataGridView _pinGrid = new()
    {
        Dock = DockStyle.Fill,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        AllowUserToResizeRows = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        MultiSelect = false,
        ReadOnly = true,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect
    };
    private readonly Label _selectedPinLabel = new() { Text = "Nessun pin", AutoSize = true };
    private readonly Label _pinWarningLabel = new() { AutoSize = true, ForeColor = Color.DarkOrange };
    private readonly ComboBox _modeCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly Button _applyModeButton = new() { Text = "Applica modalità", AutoSize = true };
    private readonly ComboBox _readTypeCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
    private readonly Button _readButton = new() { Text = "Leggi", AutoSize = true };
    private readonly Label _currentValueLabel = new() { Text = "Valore: —", AutoSize = true };
    private readonly ComboBox _outputKindCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
    private readonly NumericUpDown _outputValue = new() { Minimum = 0, Maximum = 1, Width = 100 };
    private readonly Button _writeButton = new() { Text = "Scrivi", AutoSize = true };
    private readonly ComboBox _streamPinCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
    private readonly ComboBox _streamTypeCombo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
    private readonly NumericUpDown _streamInterval = new() { Minimum = 1, Maximum = 60_000, Value = 250, Width = 90 };
    private readonly Button _streamButton = new() { Text = "Avvia stream", AutoSize = true };
    private readonly StreamChart _chart = new() { Dock = DockStyle.Fill };
    private readonly RichTextBox _serialLog = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        WordWrap = false,
        BackColor = Color.FromArgb(24, 27, 32),
        ForeColor = Color.Gainsboro,
        Font = new Font("Consolas", 9f)
    };
    private readonly Button _clearLogButton = new() { Text = "Pulisci", AutoSize = true, Anchor = AnchorStyles.Right };
    private readonly ToolStripStatusLabel _statusLabel = new() { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
    private readonly SemaphoreSlim _operationGate = new(1, 1);
    private readonly Dictionary<int, PinConfiguration> _pinStates = [];

    private ArduinoDevice _device;
    private BoardProfile _profile;
    private bool _busy;
    private bool _streamActive;
    private bool _closing;
    private SplitContainer _mainSplit;
    private SplitContainer _monitorSplit;

    public MainForm()
    {
        Text = "ArduinoAsDevice - WinForms Demo";
        Icon = LoadEmbeddedIcon();
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(960, 640);
        Size = new Size(1180, 760);

        BuildLayout();
        WireEvents();

        _boardCombo.DataSource = BoardProfiles.All.ToList();
        _modeCombo.DataSource = Enum.GetValues<PinMode>();
        _streamTypeCombo.Items.AddRange(["Digitale", "Analogico"]);
        _streamTypeCombo.SelectedIndex = 0;
        _baudCombo.Items.AddRange(["9600", "19200", "38400", "57600", "115200", "230400"]);
        _baudCombo.Text = "115200";
        RefreshPorts();
        LoadBoard((BoardProfile)_boardCombo.SelectedItem);
    }

    private void BuildLayout()
    {
        _pinGrid.Columns.Add("Pin", "Pin");
        _pinGrid.Columns.Add("Capabilities", "Funzioni");
        _pinGrid.Columns.Add("Mode", "Modalità");
        _pinGrid.Columns.Add("Value", "Stato / valore");
        _pinGrid.Columns[0].FillWeight = 35;
        _pinGrid.Columns[1].FillWeight = 100;
        _pinGrid.Columns[2].FillWeight = 55;
        _pinGrid.Columns[3].FillWeight = 55;

        var connectionBar = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Padding = new Padding(8),
            WrapContents = true
        };
        AddLabeledControl(connectionBar, "Scheda", _boardCombo);
        AddLabeledControl(connectionBar, "Porta", _portCombo);
        AddLabeledControl(connectionBar, "Baud", _baudCombo);
        connectionBar.Controls.AddRange(
        [
            _refreshPortsButton,
            _connectButton,
            _loadButton,
            _saveButton,
            _applyAllButton
        ]);

        var pinControls = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(8),
            WrapContents = false
        };
        _selectedPinLabel.Font = new Font(Font, FontStyle.Bold);
        pinControls.Controls.Add(_selectedPinLabel);
        pinControls.Controls.Add(_pinWarningLabel);
        pinControls.Controls.Add(CreateRow("Modalità", _modeCombo, _applyModeButton));
        pinControls.Controls.Add(CreateRow("Lettura", _readTypeCombo, _readButton, _currentValueLabel));
        pinControls.Controls.Add(CreateRow("Uscita", _outputKindCombo, _outputValue, _writeButton));

        var pinGroup = new GroupBox { Text = "Pin selezionato", Dock = DockStyle.Fill };
        pinGroup.Controls.Add(pinControls);

        var streamBar = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            Padding = new Padding(8),
            WrapContents = true
        };
        AddLabeledControl(streamBar, "Pin", _streamPinCombo);
        AddLabeledControl(streamBar, "Tipo", _streamTypeCombo);
        AddLabeledControl(streamBar, "Intervallo ms", _streamInterval);
        streamBar.Controls.Add(_streamButton);

        var streamPanel = new Panel { Dock = DockStyle.Fill };
        streamPanel.Controls.Add(_chart);
        streamPanel.Controls.Add(streamBar);
        _chart.BringToFront();

        var streamGroup = new GroupBox { Text = "Stream", Dock = DockStyle.Fill };
        streamGroup.Controls.Add(streamPanel);

        var logLayout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
        logLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        logLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var logToolbar = new FlowLayoutPanel
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft
        };
        logToolbar.Controls.Add(_clearLogButton);
        logLayout.Controls.Add(logToolbar, 0, 0);
        logLayout.Controls.Add(_serialLog, 0, 1);

        var logGroup = new GroupBox { Text = "Log comunicazione seriale", Dock = DockStyle.Fill };
        logGroup.Controls.Add(logLayout);

        _monitorSplit = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Horizontal
        };
        _monitorSplit.Panel1.Controls.Add(streamGroup);
        _monitorSplit.Panel2.Controls.Add(logGroup);

        var rightLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 1
        };
        rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180));
        rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        rightLayout.Controls.Add(pinGroup, 0, 0);
        rightLayout.Controls.Add(_monitorSplit, 0, 1);

        _mainSplit = new SplitContainer
        {
            Dock = DockStyle.Fill
        };
        _mainSplit.Panel1.Controls.Add(_pinGrid);
        _mainSplit.Panel2.Controls.Add(rightLayout);

        var statusStrip = new StatusStrip();
        statusStrip.Items.Add(_statusLabel);
        _statusLabel.Text = "Disconnesso";

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(connectionBar, 0, 0);
        root.Controls.Add(_mainSplit, 0, 1);
        root.Controls.Add(statusStrip, 0, 2);
        Controls.Add(root);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Proporzioni allineate allo screenshot di riferimento (Scrennshot\WinFormaDemo.png):
        // griglia pin ~28% della larghezza, area grafico ~53% dell'altezza del pannello monitor.
        _mainSplit.SplitterDistance = (int)(_mainSplit.Width * 0.28);
        _monitorSplit.SplitterDistance = (int)(_monitorSplit.Height * 0.53);
    }

    private void WireEvents()
    {
        _boardCombo.SelectedIndexChanged += (_, _) =>
        {
            if (_boardCombo.SelectedItem is BoardProfile profile)
                LoadBoard(profile);
        };
        _refreshPortsButton.Click += (_, _) => RefreshPorts();
        _connectButton.Click += async (_, _) =>
        {
            if (_device == null)
                await ConnectAsync();
            else
                await DisconnectAsync();
        };
        _pinGrid.SelectionChanged += (_, _) => ShowSelectedPin();
        _modeCombo.SelectedIndexChanged += (_, _) => UpdateControlState();
        _readTypeCombo.SelectedIndexChanged += (_, _) => UpdateControlState();
        _outputKindCombo.SelectedIndexChanged += (_, _) => UpdateOutputEditor();
        _applyModeButton.Click += async (_, _) => await ApplySelectedModeAsync();
        _readButton.Click += async (_, _) => await ReadSelectedPinAsync();
        _writeButton.Click += async (_, _) => await WriteSelectedPinAsync();
        _applyAllButton.Click += async (_, _) => await ApplyAllAsync();
        _streamTypeCombo.SelectedIndexChanged += (_, _) => UpdateStreamState();
        _streamPinCombo.SelectedIndexChanged += (_, _) => UpdateStreamState();
        _streamButton.Click += async (_, _) => await ToggleStreamAsync();
        _clearLogButton.Click += (_, _) => _serialLog.Clear();
        _saveButton.Click += (_, _) => SaveConfiguration();
        _loadButton.Click += (_, _) => LoadConfiguration();
        FormClosing += MainForm_FormClosing;
    }

    private static void AddLabeledControl(Control parent, string label, Control control)
    {
        parent.Controls.Add(new Label
        {
            Text = label,
            AutoSize = true,
            Margin = new Padding(6, 8, 2, 0)
        });
        parent.Controls.Add(control);
    }

    private static FlowLayoutPanel CreateRow(string label, params Control[] controls)
    {
        var row = new FlowLayoutPanel { AutoSize = true, WrapContents = false };
        row.Controls.Add(new Label
        {
            Text = label,
            AutoSize = true,
            Width = 65,
            Margin = new Padding(0, 8, 4, 0)
        });
        row.Controls.AddRange(controls);
        return row;
    }

    private void RefreshPorts()
    {
        var selectedPort = _portCombo.Text;
        var ports = SerialPort.GetPortNames().OrderBy(name => name, StringComparer.OrdinalIgnoreCase).ToArray();
        _portCombo.Items.Clear();
        _portCombo.Items.AddRange(ports);

        if (!string.IsNullOrWhiteSpace(selectedPort))
            _portCombo.Text = selectedPort;
        else if (ports.Length > 0)
            _portCombo.SelectedIndex = 0;

        SetStatus(ports.Length == 0 ? "Nessuna porta seriale rilevata." : $"{ports.Length} porte rilevate.");
    }

    private void LoadBoard(BoardProfile profile)
    {
        _profile = profile;
        _pinStates.Clear();
        _pinGrid.Rows.Clear();

        foreach (var pin in profile.Pins)
        {
            var state = new PinConfiguration { Pin = pin.Number };
            _pinStates.Add(pin.Number, state);
            var rowIndex = _pinGrid.Rows.Add(pin.Name, pin.Capabilities, state.Mode, "—");
            _pinGrid.Rows[rowIndex].Tag = pin;
            if (pin.UsesSerial)
                _pinGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkOrange;
        }

        _streamPinCombo.DataSource = profile.Pins.ToList();
        _pinGrid.ClearSelection();
        if (_pinGrid.Rows.Count > 0)
            _pinGrid.Rows[0].Selected = true;
        ShowSelectedPin();
    }

    private PinDefinition SelectedPin =>
        _pinGrid.SelectedRows.Count == 1 ? _pinGrid.SelectedRows[0].Tag as PinDefinition : null;

    private void ShowSelectedPin()
    {
        var pin = SelectedPin;
        if (pin == null)
        {
            _selectedPinLabel.Text = "Nessun pin";
            _pinWarningLabel.Text = "";
            UpdateControlState();
            return;
        }

        var state = _pinStates[pin.Number];
        _selectedPinLabel.Text = $"{pin.Name} (protocollo: {pin.Number})";
        _pinWarningLabel.Text = pin.UsesSerial
            ? "Riservato alla comunicazione seriale: i comandi sono disabilitati."
            : pin.SupportsDigital ? "" : "Pin analogico di solo ingresso.";
        _modeCombo.SelectedItem = state.Mode;

        var readTypes = new List<string>();
        if (pin.SupportsDigital) readTypes.Add("Digitale");
        if (pin.SupportsAnalogInput) readTypes.Add("Analogica");
        _readTypeCombo.DataSource = readTypes;

        var outputKinds = new List<PinOutputKind>();
        if (pin.SupportsDigital) outputKinds.Add(PinOutputKind.Digital);
        if (pin.SupportsPwm) outputKinds.Add(PinOutputKind.Pwm);
        if (pin.SupportsTone) outputKinds.Add(PinOutputKind.Tone);
        _outputKindCombo.DataSource = outputKinds;
        if (outputKinds.Contains(state.OutputKind))
            _outputKindCombo.SelectedItem = state.OutputKind;

        UpdateOutputEditor();
        UpdateControlState();
    }

    private void UpdateOutputEditor()
    {
        var pin = SelectedPin;
        if (pin == null || _outputKindCombo.SelectedItem is not PinOutputKind kind)
            return;

        var state = _pinStates[pin.Number];
        _outputValue.Maximum = kind switch
        {
            PinOutputKind.Digital => 1,
            PinOutputKind.Pwm => 255,
            PinOutputKind.Tone => 20_000,
            _ => 1
        };
        _outputValue.Value = kind switch
        {
            PinOutputKind.Digital => state.DigitalValue ? 1 : 0,
            PinOutputKind.Pwm => state.PwmValue,
            PinOutputKind.Tone => Math.Min(state.ToneFrequency, (int)_outputValue.Maximum),
            _ => 0
        };
        UpdateControlState();
    }

    private void UpdateControlState()
    {
        var connected = _device != null;
        var pin = SelectedPin;
        var usablePin = connected && !_busy && pin != null && !pin.UsesSerial;
        var appliedMode = pin == null ? PinMode.Input : _pinStates[pin.Number].Mode;

        _boardCombo.Enabled = !connected && !_busy;
        _portCombo.Enabled = !connected && !_busy;
        _baudCombo.Enabled = !connected && !_busy;
        _refreshPortsButton.Enabled = !connected && !_busy;
        _connectButton.Enabled = !_busy;
        _connectButton.Text = connected ? "Ferma" : "Avvia";
        _loadButton.Enabled = !_busy;
        _saveButton.Enabled = !_busy;
        _applyAllButton.Enabled = connected && !_busy;
        _modeCombo.Enabled = usablePin && pin.SupportsDigital;
        _applyModeButton.Enabled = usablePin && pin.SupportsDigital;
        _readTypeCombo.Enabled = usablePin;
        _readButton.Enabled = usablePin && _readTypeCombo.Items.Count > 0;
        _outputKindCombo.Enabled = usablePin && appliedMode == PinMode.Output;
        _outputValue.Enabled = _outputKindCombo.Enabled;
        _writeButton.Enabled = _outputKindCombo.Enabled && _outputKindCombo.Items.Count > 0;
        UpdateStreamState();
    }

    private void UpdateStreamState()
    {
        var pin = _streamPinCombo.SelectedItem as PinDefinition;
        var analog = _streamTypeCombo.SelectedIndex == 1;
        var compatible = pin != null && (analog ? pin.SupportsAnalogInput : pin.SupportsDigital) && !pin.UsesSerial;
        var canEdit = _device != null && !_busy && !_streamActive;

        _streamPinCombo.Enabled = canEdit;
        _streamTypeCombo.Enabled = canEdit;
        _streamInterval.Enabled = canEdit;
        _streamButton.Enabled = _device != null && !_busy && (_streamActive || compatible);
        _streamButton.Text = _streamActive ? "Ferma stream" : "Avvia stream";
    }

    private async Task ConnectAsync()
    {
        if (string.IsNullOrWhiteSpace(_portCombo.Text))
        {
            ShowError("Selezionare o inserire una porta COM.");
            return;
        }

        if (!int.TryParse(_baudCombo.Text, out var baudRate) || baudRate is < 300 or > 2_000_000)
        {
            ShowError("Il baud rate deve essere compreso tra 300 e 2000000.");
            return;
        }

        SetBusy(true);
        ArduinoDevice device = null;
        try
        {
            device = new ArduinoDevice(_portCombo.Text.Trim(), baudRate);
            device.ReadingReceived += Device_ReadingReceived;
            device.Logger = AppendSerialLog;
            SetStatus($"Connessione a {_portCombo.Text.Trim()}...");
            string firmware = null;
            await Task.Run(() =>
            {
                device.Open();
                firmware = device.Ping();
            });
            _device = device;
            SetStatus($"Connesso a {_portCombo.Text.Trim()} - firmware {firmware}.");
        }
        catch (Exception ex)
        {
            if (device != null)
            {
                device.ReadingReceived -= Device_ReadingReceived;
                device.Logger = null;
                await Task.Run(device.Dispose);
            }
            ShowError($"Connessione non riuscita: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task DisconnectAsync()
    {
        var device = _device;
        if (device == null)
            return;

        SetBusy(true);
        _streamActive = false;
        _device = null;
        device.ReadingReceived -= Device_ReadingReceived;
        try
        {
            SetStatus("Disconnessione...");
            await Task.Run(device.Dispose);
            device.Logger = null;
            SetStatus("Disconnesso.");
        }
        catch (Exception ex)
        {
            ShowError($"Errore durante la disconnessione: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async Task ApplySelectedModeAsync()
    {
        var pin = SelectedPin;
        if (pin == null || _modeCombo.SelectedItem is not PinMode mode)
            return;

        if (await ExecuteAsync(device => device.SetPinMode(pin.Number, mode), $"{pin.Name}: modalità {mode}."))
        {
            _pinStates[pin.Number].Mode = mode;
            UpdateGridRow(pin.Number, mode.ToString(), null);
            UpdateControlState();
        }
    }

    private async Task ReadSelectedPinAsync()
    {
        var pin = SelectedPin;
        if (pin == null)
            return;

        var analog = string.Equals(_readTypeCombo.SelectedItem as string, "Analogica", StringComparison.Ordinal);
        var value = 0;
        if (await ExecuteAsync(
            device => value = analog ? device.AnalogRead(pin.Number) : device.DigitalRead(pin.Number) ? 1 : 0,
            $"{pin.Name}: lettura completata."))
        {
            SetPinValue(pin.Number, value);
        }
    }

    private async Task WriteSelectedPinAsync()
    {
        var pin = SelectedPin;
        if (pin == null || _outputKindCombo.SelectedItem is not PinOutputKind kind)
            return;

        var value = (int)_outputValue.Value;
        var success = await ExecuteAsync(device =>
        {
            switch (kind)
            {
                case PinOutputKind.Digital:
                    device.DigitalWrite(pin.Number, value != 0);
                    break;
                case PinOutputKind.Pwm:
                    device.AnalogWrite(pin.Number, value);
                    break;
                case PinOutputKind.Tone:
                    device.SetTone(pin.Number, value);
                    break;
            }
        }, $"{pin.Name}: uscita {kind} impostata a {value}.");

        if (!success)
            return;

        var state = _pinStates[pin.Number];
        state.OutputKind = kind;
        state.DigitalValue = value != 0;
        state.PwmValue = kind == PinOutputKind.Pwm ? value : state.PwmValue;
        state.ToneFrequency = kind == PinOutputKind.Tone ? value : state.ToneFrequency;
        SetPinValue(pin.Number, value);
    }

    private async Task ApplyAllAsync()
    {
        var states = _pinStates.Values.Select(ClonePinConfiguration).ToArray();
        if (await ExecuteAsync(device =>
        {
            foreach (var state in states)
            {
                var pin = _profile.FindPin(state.Pin);
                if (pin == null || pin.UsesSerial)
                    continue;

                if (pin.SupportsDigital)
                    device.SetPinMode(pin.Number, state.Mode);
                if (state.Mode != PinMode.Output)
                    continue;

                switch (state.OutputKind)
                {
                    case PinOutputKind.Digital:
                        device.DigitalWrite(pin.Number, state.DigitalValue);
                        break;
                    case PinOutputKind.Pwm when pin.SupportsPwm:
                        device.AnalogWrite(pin.Number, state.PwmValue);
                        break;
                    case PinOutputKind.Tone when pin.SupportsTone:
                        device.SetTone(pin.Number, state.ToneFrequency);
                        break;
                }
            }
        }, "Configurazione applicata al dispositivo."))
        {
            RefreshGrid();
        }
    }

    private async Task ToggleStreamAsync()
    {
        if (_streamActive)
        {
            if (await ExecuteAsync(device => device.StopStream(), "Stream fermato."))
            {
                _streamActive = false;
                UpdateStreamState();
            }
            return;
        }

        if (_streamPinCombo.SelectedItem is not PinDefinition pin)
            return;

        var analog = _streamTypeCombo.SelectedIndex == 1;
        var interval = (int)_streamInterval.Value;
        _chart.IsAnalog = analog;
        _chart.ClearSamples();

        if (await ExecuteAsync(
            device => device.StartStream(pin.Number, analog, interval),
            $"Stream {pin.Name} avviato ogni {interval} ms."))
        {
            _streamActive = true;
            UpdateStreamState();
        }
    }

    private async Task<bool> ExecuteAsync(Action<ArduinoDevice> action, string successMessage)
    {
        var device = _device;
        if (device == null)
        {
            ShowError("Il dispositivo non è connesso.");
            return false;
        }

        await _operationGate.WaitAsync();
        SetBusy(true);
        try
        {
            await Task.Run(() => action(device));
            SetStatus(successMessage);
            return true;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            return false;
        }
        finally
        {
            SetBusy(false);
            _operationGate.Release();
        }
    }

    private void Device_ReadingReceived(object sender, PinReadingEventArgs e)
    {
        if (IsDisposed || !IsHandleCreated)
            return;

        BeginInvoke(() =>
        {
            if (!_streamActive)
                return;
            _chart.AddSample(e.Value);
            SetPinValue(e.Pin, e.Value);
        });
    }

    private void AppendSerialLog(string line, bool sent)
    {
        if (IsDisposed || !IsHandleCreated)
            return;

        BeginInvoke(() =>
        {
            const int maximumCharacters = 100_000;
            if (_serialLog.TextLength > maximumCharacters)
            {
                _serialLog.Select(0, _serialLog.TextLength - maximumCharacters);
                _serialLog.SelectedText = "";
            }

            _serialLog.AppendText($"{DateTime.Now:HH:mm:ss.fff} {(sent ? "TX" : "RX")} {line}{Environment.NewLine}");
            _serialLog.SelectionStart = _serialLog.TextLength;
            _serialLog.ScrollToCaret();
        });
    }

    private void SetPinValue(int pin, int value)
    {
        UpdateGridRow(pin, null, value.ToString());
        if (SelectedPin?.Number == pin)
            _currentValueLabel.Text = $"Valore: {value}";
    }

    private void UpdateGridRow(int pin, string mode, string value)
    {
        foreach (DataGridViewRow row in _pinGrid.Rows)
        {
            if (row.Tag is not PinDefinition definition || definition.Number != pin)
                continue;
            if (mode != null) row.Cells["Mode"].Value = mode;
            if (value != null) row.Cells["Value"].Value = value;
            break;
        }
    }

    private void RefreshGrid()
    {
        foreach (var state in _pinStates.Values)
        {
            var value = state.Mode == PinMode.Output
                ? state.OutputKind switch
                {
                    PinOutputKind.Digital => state.DigitalValue ? "1" : "0",
                    PinOutputKind.Pwm => state.PwmValue.ToString(),
                    PinOutputKind.Tone => $"{state.ToneFrequency} Hz",
                    _ => "—"
                }
                : "—";
            UpdateGridRow(state.Pin, state.Mode.ToString(), value);
        }
        ShowSelectedPin();
    }

    private void SaveConfiguration()
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "Configurazione Arduino (*.json)|*.json|Tutti i file (*.*)|*.*",
            DefaultExt = "json",
            AddExtension = true,
            FileName = $"{_profile.Id}-pins.json"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            CaptureConfiguration().Save(dialog.FileName);
            SetStatus($"Configurazione salvata in {dialog.FileName}.");
        }
        catch (Exception ex)
        {
            ShowError($"Salvataggio non riuscito: {ex.Message}");
        }
    }

    private void LoadConfiguration()
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Configurazione Arduino (*.json)|*.json|Tutti i file (*.*)|*.*",
            CheckFileExists = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            var configuration = DemoConfiguration.Load(dialog.FileName);
            if (_device != null && !string.Equals(configuration.BoardProfile, _profile.Id, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Disconnettere il dispositivo prima di caricare un profilo scheda diverso.");

            ApplyConfigurationToUi(configuration);
            SetStatus("Configurazione caricata. Usare \"Applica configurazione\" per inviarla al dispositivo.");
        }
        catch (Exception ex)
        {
            ShowError($"Caricamento non riuscito: {ex.Message}");
        }
    }

    private DemoConfiguration CaptureConfiguration()
    {
        if (!int.TryParse(_baudCombo.Text, out var baudRate))
            throw new InvalidDataException("Baud rate non valido.");

        return new DemoConfiguration
        {
            BoardProfile = _profile.Id,
            PortName = _portCombo.Text.Trim(),
            BaudRate = baudRate,
            Pins = _pinStates.Values.Select(ClonePinConfiguration).ToList(),
            Stream = new StreamConfiguration
            {
                Pin = (_streamPinCombo.SelectedItem as PinDefinition)?.Number,
                Analog = _streamTypeCombo.SelectedIndex == 1,
                IntervalMs = (int)_streamInterval.Value
            }
        };
    }

    private void ApplyConfigurationToUi(DemoConfiguration configuration)
    {
        var profile = BoardProfiles.Get(configuration.BoardProfile);
        _boardCombo.SelectedItem = BoardProfiles.All.First(item => item.Id == profile.Id);
        if (_profile != profile)
            LoadBoard(profile);

        _portCombo.Text = configuration.PortName;
        _baudCombo.Text = configuration.BaudRate.ToString();
        foreach (var configuredPin in configuration.Pins)
            _pinStates[configuredPin.Pin] = ClonePinConfiguration(configuredPin);

        if (configuration.Stream.Pin is int streamPin)
        {
            var streamItem = _profile.Pins.First(pin => pin.Number == streamPin);
            _streamPinCombo.SelectedItem = streamItem;
        }
        _streamTypeCombo.SelectedIndex = configuration.Stream.Analog ? 1 : 0;
        _streamInterval.Value = Math.Clamp(
            configuration.Stream.IntervalMs,
            (int)_streamInterval.Minimum,
            (int)_streamInterval.Maximum);
        RefreshGrid();
    }

    private static Icon LoadEmbeddedIcon()
    {
        var assembly = typeof(MainForm).Assembly;
        using var stream = assembly.GetManifestResourceStream("WinFormsDemo.AppIcon.ico");
        return stream != null ? new Icon(stream) : null;
    }

    private static PinConfiguration ClonePinConfiguration(PinConfiguration source) =>
        new()
        {
            Pin = source.Pin,
            Mode = source.Mode,
            OutputKind = source.OutputKind,
            DigitalValue = source.DigitalValue,
            PwmValue = source.PwmValue,
            ToneFrequency = source.ToneFrequency
        };

    private void SetBusy(bool busy)
    {
        _busy = busy;
        UseWaitCursor = busy;
        UpdateControlState();
    }

    private void SetStatus(string message)
    {
        _statusLabel.ForeColor = SystemColors.ControlText;
        _statusLabel.Text = message;
    }

    private void ShowError(string message)
    {
        _statusLabel.ForeColor = Color.Firebrick;
        _statusLabel.Text = message;
        MessageBox.Show(this, message, "ArduinoAsDevice", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void InitializeComponent()
    {

    }

    private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_closing || _device == null)
            return;

        e.Cancel = true;
        _closing = true;
        await DisconnectAsync();
        Close();
    }
}
