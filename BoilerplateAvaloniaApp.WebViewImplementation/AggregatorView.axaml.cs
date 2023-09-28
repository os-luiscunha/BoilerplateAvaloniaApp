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
        var w = new Window {
            Width = 500,
            Height = 300
        };
        w.Show(this.GetVisualRoot() as Window);
    }

    private void OnNewModalDialogClick(object sender, RoutedEventArgs e) {
        var w = new Window {
            Width = 500,
            Height = 300
        };
        w.ShowDialog(this.GetVisualRoot() as Window);
    }

    private void OnNewStandaloneDialogClick(object sender, RoutedEventArgs e) {
        var w = new Window {
            Width = 500,
            Height = 300,
            ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.PreferSystemChrome,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ShowActivated = true
        };
        w.Show();
    }

    private void OnNewAggregatorWindowClick(object sender, RoutedEventArgs e) {
        var currentAggregatorWindow = (AggregatorWindow)this.GetVisualRoot();

        var height = currentAggregatorWindow.Height is not double.NaN
            ? currentAggregatorWindow.Height
            : currentAggregatorWindow.Bounds.Height;
        var width = currentAggregatorWindow.Width is not double.NaN
            ? currentAggregatorWindow.Width
            : currentAggregatorWindow.Bounds.Width;

        var newAggregatorWindow = new AggregatorWindow {
            Height = height,
            Width = width,
            Position = currentAggregatorWindow.Position
        };

        newAggregatorWindow.Show();
    }
}
