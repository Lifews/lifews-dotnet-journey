# Microsoft.Extensions.Logging

WPF 本身内置的Logging类，主要用于调试输出，但它们通常只输出到 Visual Studio 的输出窗口或控制台，不能直接保存到文件。如果要保存到文件，推荐使用 Serilog 。



# Serilog

Serilog 是一个开源的结构化日志记录库，旨在简化日志的生成和管理。与传统的日志工具不同，Serilog 强调使用结构化数据，使日志更易于查询和分析。它可以将日志输出到多种目的地，包括控制台、文件、数据库等。

**主要特点：**

- **结构化日志**：以键值对的形式记录日志，使得日志数据更具可读性和可查询性。
- **灵活配置**：用户可以根据需求自定义输出格式和接收器。
- **高性能**：支持异步日志记录，适合高并发场景。
- **丰富的扩展性**：可以与多种日志管理工具和服务集成。

**相关NuGet包**

```c#
- Serilog
- Serilog.Enrichers.Thread
- Serilog.Sinks.Async
- Serilog.Sinks.File
```







