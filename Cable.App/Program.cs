namespace Cable.App;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        using var uwpApp = new UWP.App();

        var app = new App();
        app.InitializeComponent();
        app.Run();
    }
}
