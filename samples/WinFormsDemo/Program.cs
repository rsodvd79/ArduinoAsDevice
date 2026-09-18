namespace WinFormsDemo;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        if (args.Contains("--self-test", StringComparer.OrdinalIgnoreCase))
        {
            ApplicationConfiguration.Initialize();
            ConfigurationSelfCheck.Run();
            using var form = new MainForm
            {
                StartPosition = FormStartPosition.Manual,
                Location = new System.Drawing.Point(-4000, -4000)
            };
            form.Show(); // forza OnLoad (es. calcolo SplitterDistance) fuori dallo schermo visibile
            Application.DoEvents();
            form.Close();
            return;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
