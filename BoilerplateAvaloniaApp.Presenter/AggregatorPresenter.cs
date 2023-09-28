using BoilerplateAvaloniaApp.View;

namespace BoilerplateAvaloniaApp.Presenter;

public sealed partial class AggregatorPresenter {
    private readonly TitleAndStatusBarManager titleAndStatusBarManager;

    internal AggregatorPresenter(
        AggregatorWindowPresenter parent,
        IAggregatorView view) {
        View = view;
        titleAndStatusBarManager = new TitleAndStatusBarManager(this);
        RuntimeImplementation.Instance.AddAggregator(this);
    }

    public IAggregatorView View { get; }
}
