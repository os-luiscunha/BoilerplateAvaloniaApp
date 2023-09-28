using Avalonia.Markup.Xaml;
using AvaloniaStyles = Avalonia.Styling.Styles;

namespace BoilerplateAvaloniaApp.WebViewImplementation.Themes;

public class LightTheme : AvaloniaStyles, ITheme {
    public LightTheme() {
        AvaloniaXamlLoader.Load(this);
    }
}
