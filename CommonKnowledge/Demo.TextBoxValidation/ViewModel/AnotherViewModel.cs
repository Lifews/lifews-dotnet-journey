using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Demo.TextBoxValidation;

//推荐使用该方法
public class AnotherViewModel : ObservableValidator
{
    private string? userName;

    [Required(ErrorMessage = "User Name is required")]
    [MaxLength(10, ErrorMessage = "Value should be between 6 and 10 characters long")]
    [MinLength(6, ErrorMessage = "Value should be between 6 and 10 characters long")]
    public string? UserName
    {
        get => userName;
        set { SetProperty(ref userName, value, true); }
    }

    private int age;

    [Range(0, 120)]
    public int Age
    {
        get => age;
        set { SetProperty(ref age, value); }
    }
}
