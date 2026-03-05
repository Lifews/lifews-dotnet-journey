using System.Windows;

namespace About.Event;

class Program
{
    [STAThread]
    static void Main()
    {
        var app = new Application();
        var window = new MainWindow();
        app.Run(window);
    }
}
