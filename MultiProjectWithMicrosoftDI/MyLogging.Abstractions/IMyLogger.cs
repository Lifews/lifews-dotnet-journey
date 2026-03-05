namespace MyLogging;

public interface IMyLogger
{
    public void LogInformation(string message);
    public void LogWarning(string message);
    public void LogError(string message);
}
