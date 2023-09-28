using BoilerplateAvaloniaApp.View;

namespace BoilerplateAvaloniaApp.Presenter;

public partial class AggregatorPresenter : IAggregatorPresenter {
    ITopLevelView ITopLevelPresenter.View => View;
    public ITopLevelView GetView() => View;

    void IAggregatorPresenter.RefreshTitleBarAndStatusBar() {
        titleAndStatusBarManager.RefreshTitleBarAndStatusBar();
    }
}
