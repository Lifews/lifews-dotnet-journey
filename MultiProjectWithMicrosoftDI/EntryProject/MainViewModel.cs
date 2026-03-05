using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyLogging;
using MyServices;

namespace EntryProject;

public partial class MainViewModel : ObservableObject
{
    private readonly IMyLogger logger;
    private readonly DeviceService deviceService;

    [ObservableProperty]
    private string author = "Basic Learning of WPF";

    public MainViewModel(IMyLogger logger, DeviceService deviceService)
    {
        this.logger = logger;
        this.deviceService = deviceService;
        logger.LogInformation("MainViewModel is created.");
    }

    [RelayCommand]
    private void CheckDevice()
    {
        deviceService.CheckDevice();
    }
}
