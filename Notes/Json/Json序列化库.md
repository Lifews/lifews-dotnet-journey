System.Text.Json

Newtonsoft.Json





**Newtonsoft.Json** 是一个流行、高性能的 **JSON 框架**（或者说库/包），用于 .NET 应用程序。它的核心功能是：

- **序列化 (Serialization)**：将 .NET 对象（例如一个 `Person` 类的实例）转换（序列化）成 JSON 字符串。
- **反序列化 (Deserialization)**：将 JSON 字符串转换（反序列化）回 .NET 对象。



核心类就是 `JsonConvert` 和它的两个方法 `SerializeObject` 和 `DeserializeObject`



在现代软件开发中，JSON 是应用程序之间交换数据的**通用语言**。Newtonsoft.Json 的应用场景极其广泛，几乎所有需要处理 JSON 的 .NET 程序都会用到它，例如：

- **Web API 开发**：
  - **客户端**：当你用 C# 写的客户端程序（如桌面应用、移动应用）调用一个 Web API 时，API 通常会返回 JSON 数据。你需要用 Newtonsoft.Json 将这些 JSON 数据反序列化成 C# 对象，以便在代码中操作。
  - **服务端**：当你用 ASP.NET Core 或 ASP.NET MVC 编写 Web API 时，你需要将数据库查询到的 C# 对象序列化成 JSON 字符串，然后通过 HTTP 响应发送给客户端。
- **应用程序配置文件**：读取和写入 JSON 格式的配置文件（如 `appsettings.json`），虽然 .NET Core 有内置的机制，但有时需要更复杂的操作时会用到 Newtonsoft.Json。
- **数据存储**：将对象以 JSON 字符串的形式保存到数据库（如 SQL Server、SQLite）或文件中。
- **消息队列**：在微服务或分布式系统中，服务之间通过消息队列（如 RabbitMQ, Azure Service Bus）传递消息，这些消息体常常是 JSON 格式。





**Newtonsoft.Json 与微软官方的 System.Text.Json**

- **Newtonsoft.Json** 是第三方库，由 **James Newton-King** 开发。在过去的十多年里，它几乎是 .NET 中处理 JSON 的**事实标准**，因为它功能强大、灵活且易用。
- **System.Text.Json** 是微软在 .NET Core 3.0 及以后版本中**官方推出**的 JSON 库。微软推出它的目的是为了提供更高性能、更低内存分配且无需依赖第三方库的解决方案。



| 特性     | Newtonsoft.Json                          | System.Text.Json (官方)                        |
| :------- | :--------------------------------------- | :--------------------------------------------- |
| **历史** | 历史悠久，功能极其丰富                   | 较新（2019年推出）                             |
| **性能** | 很好                                     | **通常更好**（更快，更省内存）                 |
| **功能** | **非常全面**（支持各种复杂场景和自定义） | 基本功能完善，但一些高级特性仍在增加           |
| **依赖** | 需要单独安装 NuGet 包                    | **内置**于 .NET Core 3.0+ SDK 中，无需单独安装 |

**建议：**

1. **新项目**：除非你有明确需要 Newtonsoft.Json 特有功能的理由，否则**优先使用内置的 `System.Text.Json`**。这是微软现在的推荐做法，也是未来的方向。
2. **旧项目/现有项目**：很多已有的项目大量使用了 Newtonsoft.Json，迁移到 System.Text.Json 需要工作和测试，所以**继续使用 Newtonsoft.Json 是完全没问题的**。它是一个非常稳定和可靠的库。
3. **需要高级功能时**：如果你需要一些 System.Text.Json 尚未完美支持的高级功能（如更灵活的多态序列化、更强大的自定义转换器），那么 **Newtonsoft.Json 仍然是更好的选择**。