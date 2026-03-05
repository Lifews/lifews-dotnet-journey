using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MyLogging;
using MyServices;

namespace EntryProject;

public partial class App : Application
{
    public static new App Current => (App)Application.Current;

    public IServiceProvider Services { get; }

    public App()
    {
        Services = ConfigureServices();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddMyServices();
        services.AddMyLogging();

        // 用这种方式进行View和ViewModel的绑定时，要多注意，多检查，绑定不成功这里也不会报错
        services.AddSingleton<MainWindow>(sp => new MainWindow
        {
            DataContext = sp.GetRequiredService<MainViewModel>(),
        });
        services.AddSingleton<MainViewModel>();

        return services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow = Services.GetRequiredService<MainWindow>();
        MainWindow!.Show();
    }
}
