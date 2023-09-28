using Avalonia.Controls;
using Avalonia.Threading;
using BoilerplateAvaloniaApp.View;
using BoilerplateAvaloniaApp.WebViewImplementation.Framework;

namespace BoilerplateAvaloniaApp.WebViewImplementation;

public partial class AggregatorView : IAggregatorView, ITopLevelView, IControl {
    string IView.Caption {
        get => TabHeader.Caption;
        set => Dispatcher.UIThread.AsyncExecuteInUIThread(() => TabHeader.Caption = value);
    }

    void IView.Activate() { }

    public void Dispose() {
        throw new System.NotImplementedException();
    }
}
