﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;

namespace BoilerplateAvaloniaApp.WebViewImplementation;

public class TabHeaderInfo : INotifyPropertyChanged {
    private Func<Task> close;
    private Func<string> toolTipRequested;

    public TabHeaderInfo(bool isFixedSize = false) {
        IsFixedSize = isFixedSize;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public bool IsFixedSize { get; private set; }

    private string caption;
    public string Caption {
        get => caption;
        set {
            if (value != caption) {
                caption = value;
                OnPropertyChanged();
            }
        }
    }

    private Bitmap imageSource;
    public Bitmap ImageSource {
        get => imageSource;
        set {
            if (value != imageSource) {
                imageSource = value;
                OnPropertyChanged();
            }
            ImageVisibility = true;
        }
    }

    private bool imageVisibility;
    public bool ImageVisibility {
        get => imageVisibility;
        set {
            if (value != imageVisibility) {
                imageVisibility = value;
                OnPropertyChanged();
            }
        }
    }

    private bool allowClose;
    public bool AllowClose {
        get => allowClose;
        set {
            if (value != allowClose) {
                allowClose = value;
                OnPropertyChanged();
            }
        }
    }

    private bool isEnabled = true;
    public bool IsEnabled {
        get => isEnabled;
        set {
            if (value != isEnabled) {
                isEnabled = value;
                OnPropertyChanged();
            }
        }
    }

    internal Task TriggerClose() => close?.Invoke();

    internal string TriggerToolTipRequested() => toolTipRequested?.Invoke() ?? string.Empty;

    internal void SetCloseHandler(Func<Task> close) => this.close = close;

    internal void SetToolTipRequestedHandler(Func<string> toolTipRequested) => this.toolTipRequested = toolTipRequested;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
