using BoilerplateAvaloniaApp.View;

namespace BoilerplateAvaloniaApp.Presenter;

public interface ITopLevelPresenter  {
    ITopLevelView View { get; }
    ITopLevelView GetView();
}
