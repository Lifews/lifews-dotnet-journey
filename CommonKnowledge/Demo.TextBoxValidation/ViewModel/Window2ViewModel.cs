using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Demo.TextBoxValidation;

//IDataErrorInfo的基础用法
public class Window2ViewModel : ObservableObject, IDataErrorInfo
{
    private string? userName;
    public string? UserName
    {
        get => userName;
        set
        {
            SetProperty(ref userName, value);
            OnPropertyChanged(nameof(Error));
        }
    }

    private int age;
    public int Age
    {
        get => age;
        set
        {
            SetProperty(ref age, value);
            OnPropertyChanged(nameof(Error));
        }
    }

    //注意，这里的Error是一个属性
    public string Error
    {
        get
        {
            var errors = new List<string> { this[nameof(UserName)], this[nameof(Age)] };
            return string.Join(Environment.NewLine, errors.Where(x => !string.IsNullOrEmpty(x)));
        }
    }

    public string this[string columnName]
    {
        get
        {
            return columnName switch
            {
                nameof(UserName) when string.IsNullOrWhiteSpace(UserName) =>
                    "User Name is required",
                nameof(UserName) when UserName.Length is < 6 or > 10 =>
                    "Value should be between 6 and 10 characters long",
                nameof(Age) when Age is < 0 or > 120 => "Age should be between 0 and 120",
                _ => string.Empty,
            };
        }
    }
}
