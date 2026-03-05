// 模仿实现PropertyGrid

using System.Windows;
using About.ReflectionAttribute.View;

namespace About.ReflectionAttribute;

class Program
{
    [STAThread]
    static void Main()
    {
        var stu = new Student
        {
            Id = 1,
            Name = "James",
            Age = 18,
            Class = "1",
            Gender = Gender.Male,
        };

        var app = new Application();
        var editorWindow = new MyPropertyEditor(stu);
        app.Run(editorWindow);
    }
}
