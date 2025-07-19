using CommunityToolkit.Maui;
using MauiGestures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Vapolia.Svgs;

namespace DemoApp;

public static partial class MauiProgram
{
#if DEBUG && WINDOWS
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    static extern bool AllocConsole();
#endif

    public static MauiApp CreateMauiApp()
    {
#if DEBUG && WINDOWS
        AllocConsole();
        
        // Redirect Console.WriteLine to that console
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        Console.SetError(new StreamWriter(Console.OpenStandardError()) { AutoFlush = true });
        
        Microsoft.UI.Xaml.Application.Current.UnhandledException += (sender, e) =>
        {
            Console.WriteLine("WinUI UnhandledException");
            Console.WriteLine(e.ToString());
        };
#endif
        
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            })
            .UseAdvancedGestures()
            .UseEasySvg()
            .UseMauiCommunityToolkit(o =>
            {
                o.SetShouldEnableSnackbarOnWindows(true);
            });

#if DEBUG
        builder.Logging.AddDebug();
        builder.Logging.AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.IncludeScopes = true;
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss ";
        });
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            var ex = e.ExceptionObject as Exception;
            Console.WriteLine("AppDomain UnhandledException");
            Console.WriteLine(ex.ToString());
        };
        
        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            //LogCrash("TaskScheduler.UnobservedTaskException", e.Exception);
            //e.SetObserved(); // Empêche l’app de crasher
            Console.WriteLine("TaskScheduler UnobservedTaskException");
            Console.WriteLine(e.ToString());
        };
#endif
        
        return builder.Build();
    }
}