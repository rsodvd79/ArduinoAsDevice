using System.Drawing.Drawing2D;

namespace WinFormsDemo;

internal sealed class StreamChart : Control
{
    private const int MaximumSamples = 4000;
    private const float PixelsPerSample = 3f;
    private readonly Queue<int> _samples = new();

    public StreamChart()
    {
        DoubleBuffered = true;
        BackColor = Color.FromArgb(24, 27, 32);
        ForeColor = Color.FromArgb(78, 201, 176);
        MinimumSize = new Size(240, 160);
    }

    public bool IsAnalog { get; set; }

    public void AddSample(int value)
    {
        _samples.Enqueue(value);
        while (_samples.Count > MaximumSamples)
            _samples.Dequeue();
        Invalidate();
    }

    public void ClearSamples()
    {
        _samples.Clear();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var bounds = ClientRectangle;
        if (bounds.Width < 2 || bounds.Height < 2)
            return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var gridPen = new Pen(Color.FromArgb(55, 60, 68));
        using var linePen = new Pen(ForeColor, 2f);
        using var labelBrush = new SolidBrush(Color.Gainsboro);

        for (var i = 1; i < 4; i++)
        {
            var y = bounds.Top + bounds.Height * i / 4f;
            e.Graphics.DrawLine(gridPen, bounds.Left, y, bounds.Right, y);
        }

        var maximum = IsAnalog ? 1023 : 1;
        e.Graphics.DrawString(maximum.ToString(), Font, labelBrush, 4, 4);
        e.Graphics.DrawString("0", Font, labelBrush, 4, bounds.Bottom - Font.Height - 4);

        // Scorrimento: passo fisso per campione, si mostrano solo gli ultimi
        // campioni che entrano nella larghezza corrente (i più vecchi escono a sinistra).
        var visibleCount = Math.Min(_samples.Count, (int)(bounds.Width / PixelsPerSample) + 1);
        if (visibleCount < 2)
            return;

        var samples = _samples.Skip(_samples.Count - visibleCount).ToArray();
        var points = new PointF[samples.Length];
        for (var i = 0; i < samples.Length; i++)
        {
            var x = bounds.Right - 1f - (samples.Length - 1 - i) * PixelsPerSample;
            var normalized = Math.Clamp(samples[i], 0, maximum) / (float)maximum;
            var y = bounds.Bottom - 1f - normalized * (bounds.Height - 2f);
            points[i] = new PointF(x, y);
        }

        e.Graphics.DrawLines(linePen, points);
    }
}
