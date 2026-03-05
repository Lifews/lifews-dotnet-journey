#if false

using System.Threading;
using System.Windows;

namespace About.ReflectionAttribute;

class Program
{
    [STAThread]
    static void Main()
    {
        var app = new Application();
        var window = new DefinitionWindow();
        app.Run(window);
    }
}

#endif
