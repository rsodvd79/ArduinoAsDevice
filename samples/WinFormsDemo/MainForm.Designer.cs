namespace WinFormsDemo
{
    partial class MainForm
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            _boardLabel = new Label();
            _boardCombo = new ComboBox();
            _portLabel = new Label();
            _portCombo = new ComboBox();
            _baudLabel = new Label();
            _baudCombo = new ComboBox();
            _refreshPortsButton = new Button();
            _connectButton = new Button();
            _loadButton = new Button();
            _saveButton = new Button();
            _applyAllButton = new Button();
            _connectionBar = new FlowLayoutPanel();
            _pinGrid = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            _selectedPinLabel = new Label();
            _pinWarningLabel = new Label();
            _modeRowLabel = new Label();
            _modeCombo = new ComboBox();
            _applyModeButton = new Button();
            _modeRow = new FlowLayoutPanel();
            _readRowLabel = new Label();
            _readTypeCombo = new ComboBox();
            _readButton = new Button();
            _currentValueLabel = new Label();
            _readRow = new FlowLayoutPanel();
            _outputRowLabel = new Label();
            _outputKindCombo = new ComboBox();
            _outputValue = new NumericUpDown();
            _writeButton = new Button();
            _outputRow = new FlowLayoutPanel();
            _pinControls = new FlowLayoutPanel();
            _pinGroup = new GroupBox();
            _streamPinLabel = new Label();
            _streamPinCombo = new ComboBox();
            _streamTypeLabel = new Label();
            _streamTypeCombo = new ComboBox();
            _streamIntervalLabel = new Label();
            _streamInterval = new NumericUpDown();
            _streamButton = new Button();
            _streamBar = new FlowLayoutPanel();
            _chart = new StreamChart();
            _streamPanel = new Panel();
            _streamGroup = new GroupBox();
            _clearLogButton = new Button();
            _logToolbar = new FlowLayoutPanel();
            _serialLog = new RichTextBox();
            _logLayout = new TableLayoutPanel();
            _logGroup = new GroupBox();
            _monitorSplit = new SplitContainer();
            _rightLayout = new TableLayoutPanel();
            _mainSplit = new SplitContainer();
            _statusLabel = new ToolStripStatusLabel();
            _statusStrip = new StatusStrip();
            _root = new TableLayoutPanel();
            _connectionBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_pinGrid).BeginInit();
            _modeRow.SuspendLayout();
            _readRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_outputValue).BeginInit();
            _outputRow.SuspendLayout();
            _pinControls.SuspendLayout();
            _pinGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_streamInterval).BeginInit();
            _streamBar.SuspendLayout();
            _streamPanel.SuspendLayout();
            _streamGroup.SuspendLayout();
            _logToolbar.SuspendLayout();
            _logLayout.SuspendLayout();
            _logGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_monitorSplit).BeginInit();
            _monitorSplit.Panel1.SuspendLayout();
            _monitorSplit.Panel2.SuspendLayout();
            _monitorSplit.SuspendLayout();
            _rightLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_mainSplit).BeginInit();
            _mainSplit.Panel1.SuspendLayout();
            _mainSplit.Panel2.SuspendLayout();
            _mainSplit.SuspendLayout();
            _statusStrip.SuspendLayout();
            _root.SuspendLayout();
            SuspendLayout();
            // 
            // _boardLabel
            // 
            _boardLabel.AutoSize = true;
            _boardLabel.Location = new Point(14, 16);
            _boardLabel.Margin = new Padding(6, 8, 2, 0);
            _boardLabel.Name = "_boardLabel";
            _boardLabel.Size = new Size(45, 15);
            _boardLabel.TabIndex = 0;
            _boardLabel.Text = "Scheda";
            // 
            // _boardCombo
            // 
            _boardCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _boardCombo.Location = new Point(64, 11);
            _boardCombo.Name = "_boardCombo";
            _boardCombo.Size = new Size(140, 23);
            _boardCombo.TabIndex = 1;
            // 
            // _portLabel
            // 
            _portLabel.AutoSize = true;
            _portLabel.Location = new Point(213, 16);
            _portLabel.Margin = new Padding(6, 8, 2, 0);
            _portLabel.Name = "_portLabel";
            _portLabel.Size = new Size(35, 15);
            _portLabel.TabIndex = 2;
            _portLabel.Text = "Porta";
            // 
            // _portCombo
            // 
            _portCombo.Location = new Point(253, 11);
            _portCombo.Name = "_portCombo";
            _portCombo.Size = new Size(100, 23);
            _portCombo.TabIndex = 3;
            // 
            // _baudLabel
            // 
            _baudLabel.AutoSize = true;
            _baudLabel.Location = new Point(362, 16);
            _baudLabel.Margin = new Padding(6, 8, 2, 0);
            _baudLabel.Name = "_baudLabel";
            _baudLabel.Size = new Size(34, 15);
            _baudLabel.TabIndex = 4;
            _baudLabel.Text = "Baud";
            // 
            // _baudCombo
            // 
            _baudCombo.Location = new Point(401, 11);
            _baudCombo.Name = "_baudCombo";
            _baudCombo.Size = new Size(90, 23);
            _baudCombo.TabIndex = 5;
            // 
            // _refreshPortsButton
            // 
            _refreshPortsButton.AutoSize = true;
            _refreshPortsButton.Location = new Point(497, 11);
            _refreshPortsButton.Name = "_refreshPortsButton";
            _refreshPortsButton.Size = new Size(97, 25);
            _refreshPortsButton.TabIndex = 6;
            _refreshPortsButton.Text = "Aggiorna porte";
            _refreshPortsButton.UseVisualStyleBackColor = true;
            // 
            // _connectButton
            // 
            _connectButton.AutoSize = true;
            _connectButton.Location = new Point(600, 11);
            _connectButton.Name = "_connectButton";
            _connectButton.Size = new Size(75, 25);
            _connectButton.TabIndex = 7;
            _connectButton.Text = "Avvia";
            _connectButton.UseVisualStyleBackColor = true;
            // 
            // _loadButton
            // 
            _loadButton.AutoSize = true;
            _loadButton.Location = new Point(681, 11);
            _loadButton.Name = "_loadButton";
            _loadButton.Size = new Size(75, 25);
            _loadButton.TabIndex = 8;
            _loadButton.Text = "Carica...";
            _loadButton.UseVisualStyleBackColor = true;
            // 
            // _saveButton
            // 
            _saveButton.AutoSize = true;
            _saveButton.Location = new Point(762, 11);
            _saveButton.Name = "_saveButton";
            _saveButton.Size = new Size(75, 25);
            _saveButton.TabIndex = 9;
            _saveButton.Text = "Salva...";
            _saveButton.UseVisualStyleBackColor = true;
            // 
            // _applyAllButton
            // 
            _applyAllButton.AutoSize = true;
            _applyAllButton.Location = new Point(843, 11);
            _applyAllButton.Name = "_applyAllButton";
            _applyAllButton.Size = new Size(139, 25);
            _applyAllButton.TabIndex = 10;
            _applyAllButton.Text = "Applica configurazione";
            _applyAllButton.UseVisualStyleBackColor = true;
            // 
            // _connectionBar
            // 
            _connectionBar.AutoSize = true;
            _connectionBar.Controls.Add(_boardLabel);
            _connectionBar.Controls.Add(_boardCombo);
            _connectionBar.Controls.Add(_portLabel);
            _connectionBar.Controls.Add(_portCombo);
            _connectionBar.Controls.Add(_baudLabel);
            _connectionBar.Controls.Add(_baudCombo);
            _connectionBar.Controls.Add(_refreshPortsButton);
            _connectionBar.Controls.Add(_connectButton);
            _connectionBar.Controls.Add(_loadButton);
            _connectionBar.Controls.Add(_saveButton);
            _connectionBar.Controls.Add(_applyAllButton);
            _connectionBar.Dock = DockStyle.Fill;
            _connectionBar.Location = new Point(3, 3);
            _connectionBar.Name = "_connectionBar";
            _connectionBar.Padding = new Padding(8);
            _connectionBar.Size = new Size(1174, 47);
            _connectionBar.TabIndex = 0;
            // 
            // _pinGrid
            // 
            _pinGrid.AllowUserToAddRows = false;
            _pinGrid.AllowUserToDeleteRows = false;
            _pinGrid.AllowUserToResizeRows = false;
            _pinGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _pinGrid.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            _pinGrid.Dock = DockStyle.Fill;
            _pinGrid.Location = new Point(0, 0);
            _pinGrid.MultiSelect = false;
            _pinGrid.Name = "_pinGrid";
            _pinGrid.ReadOnly = true;
            _pinGrid.RowHeadersVisible = false;
            _pinGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _pinGrid.Size = new Size(391, 679);
            _pinGrid.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Pin";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Funzioni";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Modalità";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Stato / valore";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // _selectedPinLabel
            // 
            _selectedPinLabel.AutoSize = true;
            _selectedPinLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _selectedPinLabel.Location = new Point(11, 8);
            _selectedPinLabel.Name = "_selectedPinLabel";
            _selectedPinLabel.Size = new Size(67, 15);
            _selectedPinLabel.TabIndex = 0;
            _selectedPinLabel.Text = "Nessun pin";
            // 
            // _pinWarningLabel
            // 
            _pinWarningLabel.AutoSize = true;
            _pinWarningLabel.ForeColor = Color.DarkOrange;
            _pinWarningLabel.Location = new Point(11, 23);
            _pinWarningLabel.Name = "_pinWarningLabel";
            _pinWarningLabel.Size = new Size(0, 15);
            _pinWarningLabel.TabIndex = 1;
            // 
            // _modeRowLabel
            // 
            _modeRowLabel.AutoSize = true;
            _modeRowLabel.Location = new Point(0, 8);
            _modeRowLabel.Margin = new Padding(0, 8, 4, 0);
            _modeRowLabel.Name = "_modeRowLabel";
            _modeRowLabel.Size = new Size(54, 15);
            _modeRowLabel.TabIndex = 0;
            _modeRowLabel.Text = "Modalità";
            // 
            // _modeCombo
            // 
            _modeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _modeCombo.Location = new Point(61, 3);
            _modeCombo.Name = "_modeCombo";
            _modeCombo.Size = new Size(130, 23);
            _modeCombo.TabIndex = 1;
            // 
            // _applyModeButton
            // 
            _applyModeButton.AutoSize = true;
            _applyModeButton.Location = new Point(197, 3);
            _applyModeButton.Name = "_applyModeButton";
            _applyModeButton.Size = new Size(107, 25);
            _applyModeButton.TabIndex = 2;
            _applyModeButton.Text = "Applica modalità";
            _applyModeButton.UseVisualStyleBackColor = true;
            // 
            // _modeRow
            // 
            _modeRow.AutoSize = true;
            _modeRow.Controls.Add(_modeRowLabel);
            _modeRow.Controls.Add(_modeCombo);
            _modeRow.Controls.Add(_applyModeButton);
            _modeRow.Location = new Point(11, 41);
            _modeRow.Name = "_modeRow";
            _modeRow.Size = new Size(307, 31);
            _modeRow.TabIndex = 2;
            _modeRow.WrapContents = false;
            // 
            // _readRowLabel
            // 
            _readRowLabel.AutoSize = true;
            _readRowLabel.Location = new Point(0, 8);
            _readRowLabel.Margin = new Padding(0, 8, 4, 0);
            _readRowLabel.Name = "_readRowLabel";
            _readRowLabel.Size = new Size(44, 15);
            _readRowLabel.TabIndex = 0;
            _readRowLabel.Text = "Lettura";
            // 
            // _readTypeCombo
            // 
            _readTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _readTypeCombo.Location = new Point(51, 3);
            _readTypeCombo.Name = "_readTypeCombo";
            _readTypeCombo.Size = new Size(100, 23);
            _readTypeCombo.TabIndex = 1;
            // 
            // _readButton
            // 
            _readButton.AutoSize = true;
            _readButton.Location = new Point(157, 3);
            _readButton.Name = "_readButton";
            _readButton.Size = new Size(75, 25);
            _readButton.TabIndex = 2;
            _readButton.Text = "Leggi";
            _readButton.UseVisualStyleBackColor = true;
            // 
            // _currentValueLabel
            // 
            _currentValueLabel.AutoSize = true;
            _currentValueLabel.Location = new Point(238, 0);
            _currentValueLabel.Name = "_currentValueLabel";
            _currentValueLabel.Size = new Size(57, 15);
            _currentValueLabel.TabIndex = 3;
            _currentValueLabel.Text = "Valore: —";
            // 
            // _readRow
            // 
            _readRow.AutoSize = true;
            _readRow.Controls.Add(_readRowLabel);
            _readRow.Controls.Add(_readTypeCombo);
            _readRow.Controls.Add(_readButton);
            _readRow.Controls.Add(_currentValueLabel);
            _readRow.Location = new Point(11, 78);
            _readRow.Name = "_readRow";
            _readRow.Size = new Size(298, 31);
            _readRow.TabIndex = 3;
            _readRow.WrapContents = false;
            // 
            // _outputRowLabel
            // 
            _outputRowLabel.AutoSize = true;
            _outputRowLabel.Location = new Point(0, 8);
            _outputRowLabel.Margin = new Padding(0, 8, 4, 0);
            _outputRowLabel.Name = "_outputRowLabel";
            _outputRowLabel.Size = new Size(39, 15);
            _outputRowLabel.TabIndex = 0;
            _outputRowLabel.Text = "Uscita";
            // 
            // _outputKindCombo
            // 
            _outputKindCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _outputKindCombo.Location = new Point(46, 3);
            _outputKindCombo.Name = "_outputKindCombo";
            _outputKindCombo.Size = new Size(100, 23);
            _outputKindCombo.TabIndex = 1;
            // 
            // _outputValue
            // 
            _outputValue.Location = new Point(152, 3);
            _outputValue.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            _outputValue.Name = "_outputValue";
            _outputValue.Size = new Size(100, 23);
            _outputValue.TabIndex = 2;
            // 
            // _writeButton
            // 
            _writeButton.AutoSize = true;
            _writeButton.Location = new Point(258, 3);
            _writeButton.Name = "_writeButton";
            _writeButton.Size = new Size(75, 25);
            _writeButton.TabIndex = 3;
            _writeButton.Text = "Scrivi";
            _writeButton.UseVisualStyleBackColor = true;
            // 
            // _outputRow
            // 
            _outputRow.AutoSize = true;
            _outputRow.Controls.Add(_outputRowLabel);
            _outputRow.Controls.Add(_outputKindCombo);
            _outputRow.Controls.Add(_outputValue);
            _outputRow.Controls.Add(_writeButton);
            _outputRow.Location = new Point(11, 115);
            _outputRow.Name = "_outputRow";
            _outputRow.Size = new Size(336, 31);
            _outputRow.TabIndex = 4;
            _outputRow.WrapContents = false;
            // 
            // _pinControls
            // 
            _pinControls.Controls.Add(_selectedPinLabel);
            _pinControls.Controls.Add(_pinWarningLabel);
            _pinControls.Controls.Add(_modeRow);
            _pinControls.Controls.Add(_readRow);
            _pinControls.Controls.Add(_outputRow);
            _pinControls.Dock = DockStyle.Fill;
            _pinControls.FlowDirection = FlowDirection.TopDown;
            _pinControls.Location = new Point(3, 19);
            _pinControls.Name = "_pinControls";
            _pinControls.Padding = new Padding(8);
            _pinControls.Size = new Size(767, 162);
            _pinControls.TabIndex = 0;
            // 
            // _pinGroup
            // 
            _pinGroup.Controls.Add(_pinControls);
            _pinGroup.Dock = DockStyle.Fill;
            _pinGroup.Location = new Point(3, 3);
            _pinGroup.Name = "_pinGroup";
            _pinGroup.Size = new Size(773, 184);
            _pinGroup.TabIndex = 0;
            _pinGroup.TabStop = false;
            _pinGroup.Text = "Pin selezionato";
            // 
            // _streamPinLabel
            // 
            _streamPinLabel.AutoSize = true;
            _streamPinLabel.Location = new Point(14, 16);
            _streamPinLabel.Margin = new Padding(6, 8, 2, 0);
            _streamPinLabel.Name = "_streamPinLabel";
            _streamPinLabel.Size = new Size(24, 15);
            _streamPinLabel.TabIndex = 0;
            _streamPinLabel.Text = "Pin";
            // 
            // _streamPinCombo
            // 
            _streamPinCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _streamPinCombo.Location = new Point(43, 11);
            _streamPinCombo.Name = "_streamPinCombo";
            _streamPinCombo.Size = new Size(100, 23);
            _streamPinCombo.TabIndex = 1;
            // 
            // _streamTypeLabel
            // 
            _streamTypeLabel.AutoSize = true;
            _streamTypeLabel.Location = new Point(152, 16);
            _streamTypeLabel.Margin = new Padding(6, 8, 2, 0);
            _streamTypeLabel.Name = "_streamTypeLabel";
            _streamTypeLabel.Size = new Size(31, 15);
            _streamTypeLabel.TabIndex = 2;
            _streamTypeLabel.Text = "Tipo";
            // 
            // _streamTypeCombo
            // 
            _streamTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _streamTypeCombo.Location = new Point(188, 11);
            _streamTypeCombo.Name = "_streamTypeCombo";
            _streamTypeCombo.Size = new Size(100, 23);
            _streamTypeCombo.TabIndex = 3;
            // 
            // _streamIntervalLabel
            // 
            _streamIntervalLabel.AutoSize = true;
            _streamIntervalLabel.Location = new Point(297, 16);
            _streamIntervalLabel.Margin = new Padding(6, 8, 2, 0);
            _streamIntervalLabel.Name = "_streamIntervalLabel";
            _streamIntervalLabel.Size = new Size(75, 15);
            _streamIntervalLabel.TabIndex = 4;
            _streamIntervalLabel.Text = "Intervallo ms";
            // 
            // _streamInterval
            // 
            _streamInterval.Location = new Point(377, 11);
            _streamInterval.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
            _streamInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _streamInterval.Name = "_streamInterval";
            _streamInterval.Size = new Size(90, 23);
            _streamInterval.TabIndex = 5;
            _streamInterval.Value = new decimal(new int[] { 250, 0, 0, 0 });
            // 
            // _streamButton
            // 
            _streamButton.AutoSize = true;
            _streamButton.Location = new Point(473, 11);
            _streamButton.Name = "_streamButton";
            _streamButton.Size = new Size(85, 25);
            _streamButton.TabIndex = 6;
            _streamButton.Text = "Avvia stream";
            _streamButton.UseVisualStyleBackColor = true;
            // 
            // _streamBar
            // 
            _streamBar.AutoSize = true;
            _streamBar.Controls.Add(_streamPinLabel);
            _streamBar.Controls.Add(_streamPinCombo);
            _streamBar.Controls.Add(_streamTypeLabel);
            _streamBar.Controls.Add(_streamTypeCombo);
            _streamBar.Controls.Add(_streamIntervalLabel);
            _streamBar.Controls.Add(_streamInterval);
            _streamBar.Controls.Add(_streamButton);
            _streamBar.Dock = DockStyle.Top;
            _streamBar.Location = new Point(0, 0);
            _streamBar.Name = "_streamBar";
            _streamBar.Padding = new Padding(8);
            _streamBar.Size = new Size(767, 47);
            _streamBar.TabIndex = 0;
            // 
            // _chart
            // 
            _chart.BackColor = Color.FromArgb(24, 27, 32);
            _chart.Dock = DockStyle.Fill;
            _chart.ForeColor = Color.FromArgb(78, 201, 176);
            _chart.IsAnalog = false;
            _chart.Location = new Point(0, 47);
            _chart.MinimumSize = new Size(240, 160);
            _chart.Name = "_chart";
            _chart.Size = new Size(767, 172);
            _chart.TabIndex = 1;
            // 
            // _streamPanel
            // 
            _streamPanel.Controls.Add(_chart);
            _streamPanel.Controls.Add(_streamBar);
            _streamPanel.Dock = DockStyle.Fill;
            _streamPanel.Location = new Point(3, 19);
            _streamPanel.Name = "_streamPanel";
            _streamPanel.Size = new Size(767, 219);
            _streamPanel.TabIndex = 0;
            // 
            // _streamGroup
            // 
            _streamGroup.Controls.Add(_streamPanel);
            _streamGroup.Dock = DockStyle.Fill;
            _streamGroup.Location = new Point(0, 0);
            _streamGroup.Name = "_streamGroup";
            _streamGroup.Size = new Size(773, 241);
            _streamGroup.TabIndex = 0;
            _streamGroup.TabStop = false;
            _streamGroup.Text = "Stream";
            // 
            // _clearLogButton
            // 
            _clearLogButton.Anchor = AnchorStyles.Right;
            _clearLogButton.AutoSize = true;
            _clearLogButton.Location = new Point(683, 3);
            _clearLogButton.Name = "_clearLogButton";
            _clearLogButton.Size = new Size(75, 25);
            _clearLogButton.TabIndex = 0;
            _clearLogButton.Text = "Pulisci";
            _clearLogButton.UseVisualStyleBackColor = true;
            // 
            // _logToolbar
            // 
            _logToolbar.AutoSize = true;
            _logToolbar.Controls.Add(_clearLogButton);
            _logToolbar.Dock = DockStyle.Fill;
            _logToolbar.FlowDirection = FlowDirection.RightToLeft;
            _logToolbar.Location = new Point(3, 3);
            _logToolbar.Name = "_logToolbar";
            _logToolbar.Size = new Size(761, 31);
            _logToolbar.TabIndex = 0;
            // 
            // _serialLog
            // 
            _serialLog.BackColor = Color.FromArgb(24, 27, 32);
            _serialLog.Dock = DockStyle.Fill;
            _serialLog.Font = new Font("Consolas", 9F);
            _serialLog.ForeColor = Color.Gainsboro;
            _serialLog.Location = new Point(3, 40);
            _serialLog.Name = "_serialLog";
            _serialLog.ReadOnly = true;
            _serialLog.Size = new Size(761, 173);
            _serialLog.TabIndex = 1;
            _serialLog.Text = "";
            _serialLog.WordWrap = false;
            // 
            // _logLayout
            // 
            _logLayout.ColumnCount = 1;
            _logLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _logLayout.Controls.Add(_logToolbar, 0, 0);
            _logLayout.Controls.Add(_serialLog, 0, 1);
            _logLayout.Dock = DockStyle.Fill;
            _logLayout.Location = new Point(3, 19);
            _logLayout.Name = "_logLayout";
            _logLayout.RowCount = 2;
            _logLayout.RowStyles.Add(new RowStyle());
            _logLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _logLayout.Size = new Size(767, 216);
            _logLayout.TabIndex = 0;
            // 
            // _logGroup
            // 
            _logGroup.Controls.Add(_logLayout);
            _logGroup.Dock = DockStyle.Fill;
            _logGroup.Location = new Point(0, 0);
            _logGroup.Name = "_logGroup";
            _logGroup.Size = new Size(773, 238);
            _logGroup.TabIndex = 0;
            _logGroup.TabStop = false;
            _logGroup.Text = "Log comunicazione seriale";
            // 
            // _monitorSplit
            // 
            _monitorSplit.Dock = DockStyle.Fill;
            _monitorSplit.Location = new Point(3, 193);
            _monitorSplit.Name = "_monitorSplit";
            _monitorSplit.Orientation = Orientation.Horizontal;
            // 
            // _monitorSplit.Panel1
            // 
            _monitorSplit.Panel1.Controls.Add(_streamGroup);
            // 
            // _monitorSplit.Panel2
            // 
            _monitorSplit.Panel2.Controls.Add(_logGroup);
            _monitorSplit.Size = new Size(773, 483);
            _monitorSplit.SplitterDistance = 241;
            _monitorSplit.TabIndex = 0;
            // 
            // _rightLayout
            // 
            _rightLayout.ColumnCount = 1;
            _rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _rightLayout.Controls.Add(_pinGroup, 0, 0);
            _rightLayout.Controls.Add(_monitorSplit, 0, 1);
            _rightLayout.Dock = DockStyle.Fill;
            _rightLayout.Location = new Point(0, 0);
            _rightLayout.Name = "_rightLayout";
            _rightLayout.RowCount = 2;
            _rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            _rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _rightLayout.Size = new Size(779, 679);
            _rightLayout.TabIndex = 0;
            // 
            // _mainSplit
            // 
            _mainSplit.Dock = DockStyle.Fill;
            _mainSplit.Location = new Point(3, 56);
            _mainSplit.Name = "_mainSplit";
            // 
            // _mainSplit.Panel1
            // 
            _mainSplit.Panel1.Controls.Add(_pinGrid);
            // 
            // _mainSplit.Panel2
            // 
            _mainSplit.Panel2.Controls.Add(_rightLayout);
            _mainSplit.Size = new Size(1174, 679);
            _mainSplit.SplitterDistance = 391;
            _mainSplit.TabIndex = 1;
            // 
            // _statusLabel
            // 
            _statusLabel.Name = "_statusLabel";
            _statusLabel.Size = new Size(1165, 17);
            _statusLabel.Spring = true;
            _statusLabel.Text = "Disconnesso";
            _statusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _statusStrip
            // 
            _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel });
            _statusStrip.Location = new Point(0, 738);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Size = new Size(1180, 22);
            _statusStrip.TabIndex = 2;
            // 
            // _root
            // 
            _root.ColumnCount = 1;
            _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _root.Controls.Add(_connectionBar, 0, 0);
            _root.Controls.Add(_mainSplit, 0, 1);
            _root.Controls.Add(_statusStrip, 0, 2);
            _root.Dock = DockStyle.Fill;
            _root.Location = new Point(0, 0);
            _root.Name = "_root";
            _root.RowCount = 3;
            _root.RowStyles.Add(new RowStyle());
            _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _root.RowStyles.Add(new RowStyle());
            _root.Size = new Size(1180, 760);
            _root.TabIndex = 0;
            // 
            // MainForm
            // 
            ClientSize = new Size(1180, 760);
            Controls.Add(_root);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(960, 640);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ArduinoAsDevice - WinForms Demo";
            _connectionBar.ResumeLayout(false);
            _connectionBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_pinGrid).EndInit();
            _modeRow.ResumeLayout(false);
            _modeRow.PerformLayout();
            _readRow.ResumeLayout(false);
            _readRow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_outputValue).EndInit();
            _outputRow.ResumeLayout(false);
            _outputRow.PerformLayout();
            _pinControls.ResumeLayout(false);
            _pinControls.PerformLayout();
            _pinGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_streamInterval).EndInit();
            _streamBar.ResumeLayout(false);
            _streamBar.PerformLayout();
            _streamPanel.ResumeLayout(false);
            _streamPanel.PerformLayout();
            _streamGroup.ResumeLayout(false);
            _logToolbar.ResumeLayout(false);
            _logToolbar.PerformLayout();
            _logLayout.ResumeLayout(false);
            _logLayout.PerformLayout();
            _logGroup.ResumeLayout(false);
            _monitorSplit.Panel1.ResumeLayout(false);
            _monitorSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_monitorSplit).EndInit();
            _monitorSplit.ResumeLayout(false);
            _rightLayout.ResumeLayout(false);
            _mainSplit.Panel1.ResumeLayout(false);
            _mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_mainSplit).EndInit();
            _mainSplit.ResumeLayout(false);
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            _root.ResumeLayout(false);
            _root.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label _boardLabel;
        private System.Windows.Forms.ComboBox _boardCombo;
        private System.Windows.Forms.Label _portLabel;
        private System.Windows.Forms.ComboBox _portCombo;
        private System.Windows.Forms.Label _baudLabel;
        private System.Windows.Forms.ComboBox _baudCombo;
        private System.Windows.Forms.Button _refreshPortsButton;
        private System.Windows.Forms.Button _connectButton;
        private System.Windows.Forms.Button _loadButton;
        private System.Windows.Forms.Button _saveButton;
        private System.Windows.Forms.Button _applyAllButton;
        private System.Windows.Forms.FlowLayoutPanel _connectionBar;
        private System.Windows.Forms.DataGridView _pinGrid;
        private System.Windows.Forms.Label _selectedPinLabel;
        private System.Windows.Forms.Label _pinWarningLabel;
        private System.Windows.Forms.Label _modeRowLabel;
        private System.Windows.Forms.ComboBox _modeCombo;
        private System.Windows.Forms.Button _applyModeButton;
        private System.Windows.Forms.FlowLayoutPanel _modeRow;
        private System.Windows.Forms.Label _readRowLabel;
        private System.Windows.Forms.ComboBox _readTypeCombo;
        private System.Windows.Forms.Button _readButton;
        private System.Windows.Forms.Label _currentValueLabel;
        private System.Windows.Forms.FlowLayoutPanel _readRow;
        private System.Windows.Forms.Label _outputRowLabel;
        private System.Windows.Forms.ComboBox _outputKindCombo;
        private System.Windows.Forms.NumericUpDown _outputValue;
        private System.Windows.Forms.Button _writeButton;
        private System.Windows.Forms.FlowLayoutPanel _outputRow;
        private System.Windows.Forms.FlowLayoutPanel _pinControls;
        private System.Windows.Forms.GroupBox _pinGroup;
        private System.Windows.Forms.Label _streamPinLabel;
        private System.Windows.Forms.ComboBox _streamPinCombo;
        private System.Windows.Forms.Label _streamTypeLabel;
        private System.Windows.Forms.ComboBox _streamTypeCombo;
        private System.Windows.Forms.Label _streamIntervalLabel;
        private System.Windows.Forms.NumericUpDown _streamInterval;
        private System.Windows.Forms.Button _streamButton;
        private System.Windows.Forms.FlowLayoutPanel _streamBar;
        private WinFormsDemo.StreamChart _chart;
        private System.Windows.Forms.Panel _streamPanel;
        private System.Windows.Forms.GroupBox _streamGroup;
        private System.Windows.Forms.RichTextBox _serialLog;
        private System.Windows.Forms.Button _clearLogButton;
        private System.Windows.Forms.FlowLayoutPanel _logToolbar;
        private System.Windows.Forms.TableLayoutPanel _logLayout;
        private System.Windows.Forms.GroupBox _logGroup;
        private System.Windows.Forms.SplitContainer _monitorSplit;
        private System.Windows.Forms.TableLayoutPanel _rightLayout;
        private System.Windows.Forms.SplitContainer _mainSplit;
        private System.Windows.Forms.ToolStripStatusLabel _statusLabel;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.TableLayoutPanel _root;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}
