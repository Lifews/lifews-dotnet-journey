using SqlSugar;
using System.Linq.Expressions;

namespace CommonDb;

/// <summary>
/// 用于自动分表的实体类，基础CURD，sqlsugar实现
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class SplitTableBaseService<TEntity> : ISplitTableBaseService<TEntity> where TEntity : class, new()
{
    private BaseDbContext context;
    private SqlSugarClient db;
    private SimpleClient<TEntity> entityDB;


    public BaseDbContext Context
    {
        get { return context; }
        set { context = value; }
    }

    internal SqlSugarClient Db
    {
        get { return db; }
        private set { db = value; }
    }

    internal SimpleClient<TEntity> EntityDB
    {
        get { return entityDB; }
        private set { entityDB = value; }
    }

    public SplitTableBaseService(BaseDbContext dbContext)
    {
        context = dbContext;
        db = dbContext.Db;
        entityDB = dbContext.GetEntityDB<TEntity>(db);
    }


    #region 添加

    /// <summary>
    /// 插入一条数据<br/>
    /// 自动生成long类型的主键（雪花ID）
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(TEntity model)
    {
        long snowflakeId = await Task.Run(() => db.Insertable(model).SplitTable().ExecuteReturnSnowflakeId());
        //如果插入成功会返回雪花ID，且雪花ID>0
        return snowflakeId > 0;
    }

    /// <summary>
    /// 批量添加数据<br/>
    /// 批量生成long类型的主键（雪花ID）
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddListAsync(List<TEntity> model)
    {
        List<long> snowflakeIdList = await Task.Run(() => db.Insertable(model).SplitTable().ExecuteReturnSnowflakeIdList());
        return snowflakeIdList.Count > 0;
    }

    #endregion


    #region  删除

    /// <summary>
    /// 根据实体删除一条数据<br/>
    /// 本质上是根据主键删除
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(TEntity model)
    {
        long i = await Task.Run(() => db.Deleteable(model).SplitTable().ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 根据实体集合删除数据<br/>
    /// 本质上是根据主键删除
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(List<TEntity> model)
    {
        long i = await Task.Run(() => db.Deleteable(model).SplitTable().ExecuteCommand());
        return i > 0;
    }

    #endregion


    #region 更新

    /// <summary>
    /// 根据实体更新一条数据<br/>
    /// 本质上是根据主键更新
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(TEntity model)
    {
        long i = await Task.Run(() => db.Updateable(model).SplitTable().ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 根据实体集合更新数据<br/>
    /// 本质上是根据主键更新
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(List<TEntity> model)
    {
        long i = await Task.Run(() => db.Updateable(model).SplitTable().ExecuteCommand());
        return i > 0;
    }

    #endregion

    #region 查询

    /// <summary>
    /// 时间过滤查询
    /// </summary>
    /// <param name="beginTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> QueryAsync(DateTime beginTime, DateTime endTime)
    {
        return await Task.Run(() => db.Queryable<TEntity>().SplitTable(beginTime, endTime).ToList());
    }

    /// <summary>
    /// 时间过滤分页查询
    /// </summary>
    /// <param name="beginTime"></param>
    /// <param name="endTime"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> QueryAsync(DateTime beginTime, DateTime endTime, int pageNumber, int pageSize)
    {
        return await Task.Run(() => db.Queryable<TEntity>().SplitTable(beginTime, endTime).ToPageList(pageNumber, pageSize));
    }

    /// <summary>
    /// 时间过滤条件查询
    /// </summary>
    /// <param name="beginTime"></param>
    /// <param name="endTime"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> QueryAsync(DateTime beginTime, DateTime endTime, Expression<Func<TEntity, bool>> where)
    {
        return await Task.Run(() => db.Queryable<TEntity>().Where(where).SplitTable(beginTime, endTime).ToList());
    }

    /// <summary>
    /// 时间过滤条件分页查询
    /// </summary>
    /// <param name="beginTime"></param>
    /// <param name="endTime"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> QueryAsync(DateTime beginTime, DateTime endTime, int pageNumber, int pageSize, Expression<Func<TEntity, bool>> where)
    {
        return await Task.Run(() => db.Queryable<TEntity>().Where(where).SplitTable(beginTime, endTime).ToPageList(pageNumber, pageSize));
    }

    #endregion

}

