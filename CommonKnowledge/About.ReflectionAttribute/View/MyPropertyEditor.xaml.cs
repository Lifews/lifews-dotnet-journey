using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace About.ReflectionAttribute.View;

public partial class MyPropertyEditor : Window
{
    public MyPropertyEditor(object selectedObject)
    {
        InitializeComponent();
        CreatePropertyEditors(selectedObject);
    }

    void CreatePropertyEditors(object selectedObject)
    {
        this.Content = BuildPropertyEditorUI(selectedObject);
    }

    UIElement BuildPropertyEditorUI(object selectedObject)
    {
        var panel = new StackPanel();

        var properties = selectedObject.GetType().GetProperties();
        foreach (var property in properties)
        {
            // 创建标签
            var label = new TextBlock { Text = property.Name, Margin = new Thickness(0, 5, 0, 2) };

            // 创建编辑器
            var editor = GetEditor(property, selectedObject);

            panel.Children.Add(label);
            panel.Children.Add(editor);
        }

        return new ScrollViewer { Content = panel };
    }

    UIElement GetEditor(PropertyInfo propertyInfo, object dataContext)
    {
        // 根据属性类型返回不同的编辑器
        if (propertyInfo.PropertyType == typeof(string))
        {
            var textBox = new TextBox();
            textBox.SetBinding(TextBox.TextProperty, propertyInfo.Name);
            textBox.DataContext = dataContext;
            return textBox;
        }
        else if (propertyInfo.PropertyType == typeof(int))
        {
            var textBox = new TextBox();
            textBox.SetBinding(TextBox.TextProperty, propertyInfo.Name);
            textBox.DataContext = dataContext;
            return textBox;
        }
        else if (propertyInfo.PropertyType.IsEnum)
        {
            var comboBox = new ComboBox();
            comboBox.ItemsSource = Enum.GetValues(propertyInfo.PropertyType);
            comboBox.SetBinding(ComboBox.SelectedItemProperty, propertyInfo.Name);
            comboBox.DataContext = dataContext;
            return comboBox;
        }

        return new TextBox();
    }
}

class Student
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Age { get; set; }

    public string? Class { get; set; }

    public Gender Gender { get; set; }
}

enum Gender
{
    Male,
    Female,
}
