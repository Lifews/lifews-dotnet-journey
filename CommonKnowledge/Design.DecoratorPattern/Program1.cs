// 通过接口方式实现装饰器模式

IProductService service = new ProductService();
IProductService validatedService = new ValidationProductServiceDecorator(service);
IProductService loggingService = new LoggingProductServiceDecorator(validatedService);

//var myService = new MyService(loggingService);

Console.WriteLine("------ basic service ------");
var product1 = service.GetProduct(-1);
Console.WriteLine($"Product:{product1}");

Console.WriteLine("------ Validation Decorator ------");
var product2 = validatedService.GetProduct(-1);
Console.WriteLine($"Product:{product2}");

Console.WriteLine("------ Logging Decorator ------");
var product3 = loggingService.GetProduct(-1);
Console.WriteLine($"Product:{product3}");

#region 假设有原始类ProductService，我们要给他加功能

class MyService
{
    private readonly IProductService _productService;

    public MyService(IProductService productService)
    {
        _productService = productService;
    }

    public void Foo()
    {
        var product = _productService.GetProduct(1);
        Console.WriteLine(product);
    }
}

class ProductService : IProductService
{
    public Product? GetProduct(int id)
    {
        return new Product(id, "Sample Product", 9.99m);
    }
}

interface IProductService
{
    Product? GetProduct(int Id);
}

record Product(int Id, string Name, decimal Price);

#endregion


#region DecoratorPattern

/// <summary>
/// 扩展ProductService的功能
/// </summary>
class ValidationProductServiceDecorator : IProductService
{
    private readonly IProductService _service;

    public ValidationProductServiceDecorator(IProductService service)
    {
        _service = service;
    }

    public Product? GetProduct(int Id)
    {
        if (Id < 0)
        {
            Console.WriteLine("Invalid product ID.");
            return null;
        }
        return _service.GetProduct(Id);
    }
}

/// <summary>
/// 扩展ProductService的功能
/// </summary>
class LoggingProductServiceDecorator : IProductService
{
    private readonly IProductService _service;

    public LoggingProductServiceDecorator(IProductService service)
    {
        _service = service;
    }

    public Product? GetProduct(int id)
    {
        Console.WriteLine($"[LOG] Fetching product with ID :{id}");
        return _service.GetProduct(id);
    }
}

#endregion
