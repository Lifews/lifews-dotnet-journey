using System.Diagnostics;

namespace MyLogging;

public class MyLogger : IMyLogger
{
    public void LogInformation(string message) => Debug.WriteLine($"Information: {message}");

    public void LogWarning(string message) => Debug.WriteLine($"Warning: {message}");

    public void LogError(string message) => Debug.WriteLine($"Error: {message}");
}
