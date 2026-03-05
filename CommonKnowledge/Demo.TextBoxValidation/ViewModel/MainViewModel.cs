using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Demo.TextBoxValidation;

public class MainViewModel : ObservableObject, IDataErrorInfo
{
    private string? userName;
    public string? UserName
    {
        get => userName;
        set
        {
            //方法一
            //if (value.Length is < 6 or > 10)
            //    throw new ArgumentException("user name should be between 6 and 10 characters long");
            SetProperty(ref userName, value);
        }
    }

    private int age;
    public int Age
    {
        get => age;
        set { SetProperty(ref age, value); }
    }

    //方法三，使用IDataErrorInfo
    //IDataErrorInfo是比较旧的接口，功能有限
    //注意，这里的Error是一个属性
    public string Error => null;

    public string this[string columnName]
    {
        get
        {
            if (columnName == nameof(UserName))
            {
                if (string.IsNullOrEmpty(UserName))
                    return "User Name is required";
                if (UserName.Length < 6)
                    return "Value should be at least 6 characters long";
                if (UserName.Length > 10)
                    return "Value should be at most 10 characters long";
            }
            else if (columnName == nameof(Age))
            {
                if (Age < 0)
                    return "Value should be at least 0";
                if (Age > 120)
                    return "Value should be at most 120";
            }
            return null;
        }
    }
}

//方法二：自定义ValidationRule
//适合简单的、独立的验证规则
class StringLengthRule : ValidationRule
{
    public int? MaxLength { get; set; }
    public int? MinLength { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is not string text)
            return new ValidationResult(false, "Value should be a string");

        if (MinLength is not null && text.Length < MinLength)
            return new ValidationResult(
                false,
                $"Value should be at least {MinLength} characters long"
            );

        if (MaxLength is not null && text.Length > MaxLength)
            return new ValidationResult(
                false,
                $"Value should be at most {MaxLength} characters long"
            );

        return ValidationResult.ValidResult;
    }
}
