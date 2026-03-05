using System.Windows;

namespace About.Ioc.Services;

internal class MessageBoxService : IMessageBoxService
{
    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}
