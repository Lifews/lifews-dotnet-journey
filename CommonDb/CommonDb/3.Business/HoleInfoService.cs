namespace CommonDb;

public class HoleInfoService : SplitTableBaseService<HoleInfo>, IHoleInfoService
{
    public HoleInfoService(BaseDbContext dbContext) : base(dbContext)
    {

    }
}
