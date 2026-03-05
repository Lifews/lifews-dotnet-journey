using Wpf.Ui.Controls;

namespace About.ValueConverter;

public partial class MainWindow : FluentWindow
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new MainWindowViewModel();
    }
}

public partial class MainWindowViewModel
{
    public static FuncValueConverter<double, string> NumberToStringConverter { get; }

    static MainWindowViewModel()
    {
        NumberToStringConverter = new(
            (f) =>
            {
                return (int)f switch
                {
                    1 => "One",
                    2 => "Two",
                    _ => "N/A",
                };
            }
        );
    }
}
