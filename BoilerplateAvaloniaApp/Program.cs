using Avalonia;
using Avalonia.Controls;
using BoilerplateAvaloniaApp.Presenter;
using BoilerplateAvaloniaApp.View;
using BoilerplateAvaloniaApp.WebViewImplementation;
using AppLifetime = Avalonia.Controls.ApplicationLifetimes.ClassicDesktopStyleApplicationLifetime;

namespace BoilerplateAvaloniaApp;

public static class Program {
    [STAThread]
    private static int Main(string[] args) {
        return Start(args);
    }

    private static int Start(string[] args) {
        var mainThread = Thread.CurrentThread;
        var appBuilder = new Lazy<AppBuilder>(() => BuildApp(mainThread));

        AppLifetime GetAppLifetime() => (AppLifetime)appBuilder.Value.Instance.ApplicationLifetime;

        StartRuntime(appBuilder, args, mainThread).Wait();

        if (!RunApplication(appBuilder)) {
            return 0; // view not started (probably an headless session), exit
        }

        return GetAppLifetime().Start(args);
    }

    private static Task StartRuntime(Lazy<AppBuilder> appBuilder, string[] args, Thread mainThread) {
        var viewInitializationTaskSource = new TaskCompletionSource();
        ViewImplementationProvider.Starting += () => viewInitializationTaskSource.SetResult();

        var initializationTask = Task.Run(() => {
            ViewImplementationProvider.SetInstance(() => new ExtendedViewImplementationProvider());
            var runtime = new RuntimeImplementation(args,null, null, mainThread);
            runtime.Start();
        });

        // wait until end or view starts, whatever comes first... in the last case we must proceed for the view to start
        return Task.WhenAny(initializationTask, viewInitializationTaskSource.Task);
    }

    private static bool RunApplication(Lazy<AppBuilder> appBuilder) {
        if (ViewImplementationProvider.HasStarted) {
            ExtendedViewImplementationProvider.Instance.RunApplication((App)appBuilder.Value.Instance);
            return true;
        }

        return false;
    }

    private static AppBuilder BuildApp(Thread mainThread) {
        if (Thread.CurrentThread != mainThread) {
            throw new Exception("Must be called on main thread");
        }
        return AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .With(new Win32PlatformOptions {
                UseWindowsUIComposition = false
            })
            .SetupWithLifetime(new AppLifetime() {
                ShutdownMode = ShutdownMode.OnLastWindowClose,
            });
    }
}
