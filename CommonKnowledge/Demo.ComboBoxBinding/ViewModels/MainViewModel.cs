using CommunityToolkit.Mvvm.ComponentModel;

namespace Demo.ComboBoxBinding;

partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    DateOfWeek dateOfWeek;
}
