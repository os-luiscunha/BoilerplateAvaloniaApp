using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using BoilerplateAvaloniaApp.View;
using BoilerplateAvaloniaApp.WebViewImplementation.Framework;
using BoilerplateAvaloniaApp.WebViewImplementation.Framework.Tooltip;

namespace BoilerplateAvaloniaApp.WebViewImplementation;

public class ExtendedViewImplementationProvider : ViewImplementationProvider {
    private enum AppStartStatus {
        NotStarted,
        Starting,
        Started
    }

    private readonly StateMachine<AppStartStatus> appStartStateMachine = new();

    public ExtendedViewImplementationProvider() {
        TooltipServiceProvider.CreateTooltipService(() => new TooltipView());
    }

    public new static ExtendedViewImplementationProvider Instance => (ExtendedViewImplementationProvider)ViewImplementationProvider.Instance;

    public override IAggregatorWindowView CreateAggregatorWindow() {
        return Dispatcher.UIThread.ExecuteInUIThread(() => {
            var aggregatorWindow = new AggregatorWindow();

            return aggregatorWindow;
        });
    }

    public override void Start() {
        appStartStateMachine.SetState(AppStartStatus.Starting);

        HasStarted = true;
        RaiseStarting();

        appStartStateMachine.WaitFor(AppStartStatus.Started);
    }

    public void RunApplication(App app) {
        if (!HasStarted) {
            return;
        }

        void Setup() {
            NativeThemeProvider.Initialize();

            Dispatcher.UIThread.AddUnhandledExceptionHandler(TriggerUnhandledExceptionEventHandler);

            appStartStateMachine.SetState(AppStartStatus.Started);
        }

        var appLifetime = (ClassicDesktopStyleApplicationLifetime)app.ApplicationLifetime;
        if (app.HasStarted) {
            Dispatcher.UIThread.ExecuteInUIThread(Setup);
        } else {
            appLifetime.Startup += delegate { Setup(); };
        }
    }
}
