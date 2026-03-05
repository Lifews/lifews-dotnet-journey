using MyLogging;

namespace MyServices;

public class HardwareService
{
    private readonly IMyLogger logger;

    public HardwareService(IMyLogger logger)
    {
        logger = logger;
    }
}
