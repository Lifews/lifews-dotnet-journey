using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace About.ReflectionAttribute;

public partial class DefinitionViewModel : ObservableObject
{
    [ObservableProperty]
    private Product currentProduct;

    public DefinitionViewModel()
    {
        CurrentProduct = new Product();
    }
}

public class Product : INotifyPropertyChanged
{
    private string _name;
    private decimal _price;
    private int _stock;
    private DateTime _createDate;
    private ProductCategory _category;
    private Status _status;
    private Color _themeColor;
    private bool _isFeatured;

    [Category("基本信息")]
    [DisplayName("产品名称")]
    [Description("产品的完整名称")]
    [PropertyOrder(1)]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    [Category("基本信息")]
    [DisplayName("价格")]
    [Description("产品的销售价格")]
    [PropertyOrder(2)]
    [Range(0, 10000, ErrorMessage = "价格必须在0-10000之间")]
    public decimal Price
    {
        get => _price;
        set
        {
            _price = value;
            OnPropertyChanged(nameof(Price));
        }
    }

    [Category("库存信息")]
    [DisplayName("库存数量")]
    [Description("当前库存数量")]
    [PropertyOrder(3)]
    public int Stock
    {
        get => _stock;
        set
        {
            _stock = value;
            OnPropertyChanged(nameof(Stock));
        }
    }

    [Category("分类信息")]
    [DisplayName("产品分类")]
    [Description("选择产品所属分类")]
    [PropertyOrder(4)]
    public ProductCategory Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged(nameof(Category));
        }
    }

    [Category("分类信息")]
    [DisplayName("产品状态")]
    [PropertyOrder(5)]
    public Status Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged(nameof(Status));
        }
    }

    [Category("外观设置")]
    [DisplayName("主题颜色")]
    [PropertyOrder(6)]
    public Color ThemeColor
    {
        get => _themeColor;
        set
        {
            _themeColor = value;
            OnPropertyChanged(nameof(ThemeColor));
        }
    }

    [Category("高级设置")]
    [DisplayName("是否推荐")]
    [Description("是否在首页推荐此产品")]
    [PropertyOrder(7)]
    public bool IsFeatured
    {
        get => _isFeatured;
        set
        {
            _isFeatured = value;
            OnPropertyChanged(nameof(IsFeatured));
        }
    }

    [Category("高级设置")]
    [DisplayName("创建日期")]
    [Description("产品创建日期")]
    [PropertyOrder(8)]
    public DateTime CreateDate
    {
        get => _createDate;
        set
        {
            _createDate = value;
            OnPropertyChanged(nameof(CreateDate));
        }
    }

    [Browsable(false)] // 这个属性不会在PropertyGrid中显示
    public string InternalCode { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public Product()
    {
        Name = "新产品";
        Price = 100m;
        Stock = 50;
        Category = ProductCategory.Electronics;
        Status = Status.Active;
        ThemeColor = Colors.Blue;
        IsFeatured = true;
        CreateDate = DateTime.Now;
        InternalCode = "PROD-001";
    }
}

public enum ProductCategory
{
    [Description("电子产品")]
    Electronics,

    [Description("服装鞋帽")]
    Clothing,

    [Description("家居用品")]
    HomeGoods,

    [Description("图书文具")]
    Books,
}

public enum Status
{
    [Description("活跃")]
    Active,

    [Description("停用")]
    Inactive,

    [Description("缺货")]
    OutOfStock,
}
