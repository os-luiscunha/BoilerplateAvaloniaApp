using BoilerplateAvaloniaApp.View;

namespace BoilerplateAvaloniaApp.Presenter;

public interface IAggregatorPresenter : ITopLevelPresenter {
    internal void RefreshTitleBarAndStatusBar();
    new IAggregatorView View { get; }
}
