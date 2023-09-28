using Avalonia.Markup.Xaml;
using AvaloniaStyles = Avalonia.Styling.Styles;

namespace BoilerplateAvaloniaApp.WebViewImplementation.Themes;

public class DarkTheme : AvaloniaStyles, ITheme {
    public DarkTheme() {
        AvaloniaXamlLoader.Load(this);
    }
}
