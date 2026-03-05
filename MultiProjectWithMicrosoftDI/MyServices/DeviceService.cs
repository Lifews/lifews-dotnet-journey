using MyLogging;

namespace MyServices;

public class DeviceService
{
    private readonly IMyLogger logger;

    public DeviceService(IMyLogger logger)
    {
        this.logger = logger;
    }

    public void CheckDevice()
    {
        if (Random.Shared.NextDouble() > 0.5)
        {
            logger.LogInformation("Device is OK.");
        }
        else
        {
            logger.LogError("Device is broken.");
        }
    }
}
