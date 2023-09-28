using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace BoilerplateAvaloniaApp.WebViewImplementation;

public partial class AggregatorView : UserControl {
    public TabHeaderInfo TabHeader { get; }

    public AggregatorView() { }

    public AggregatorView(TabHeaderInfo tabHeaderInfo) {
        AvaloniaXamlLoader.Load(this);
        var newDialogButton = this.FindControl<Button>("Btn-Dialog");
        newDialogButton.Click += OnNewDialogClick;

        var newModalDialogButton = this.FindControl<Button>("Btn-Modal-Dialog");
        newModalDialogButton.Click += OnNewModalDialogClick;

        var newStandaloneDialogButton = this.FindControl<Button>("Btn-Standalone-Dialog");
        newStandaloneDialogButton.Click += OnNewStandaloneDialogClick;

        var newAggregatorWindowButton = this.FindControl<Button>("Btn-Aggregator-Window");
        newAggregatorWindowButton.Click += OnNewAggregatorWindowClick;

        TabHeader = tabHeaderInfo;
    }

    private void OnNewDialogClick(object sender, RoutedEventArgs e) {
        var w = new Window { Width = 200, Height = 200 };
        w.Show(this.GetVisualRoot() as Window);
    }

    private void OnNewModalDialogClick(object sender, RoutedEventArgs e) {
        var w = new Window { Width = 200, Height = 200 };
        w.ShowDialog(this.GetVisualRoot() as Window);
    }

    private void OnNewStandaloneDialogClick(object sender, RoutedEventArgs e) {
        var w = new Window { Width = 200, Height = 200 };
        w.ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.PreferSystemChrome;
        w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        w.ShowActivated = true;
        w.Show();
    }

    private void OnNewAggregatorWindowClick(object sender, RoutedEventArgs e) {
        var w = new AggregatorWindow();
        w.Show();
    }
}
