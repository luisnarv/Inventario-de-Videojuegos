using System;
using Avalonia;
using InventarioVideojuegos.Data;

namespace InventarioVideojuegos;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        DatabaseHelper.InicializarBaseDatos();
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
