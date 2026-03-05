using SqlSugar;
using System.Linq.Expressions;

namespace CommonDb;

/// <summary>
/// 基础CURD，sqlsugar实现
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
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

    public BaseService(BaseDbContext dbContext)
    {
        context = dbContext;
        db = dbContext.Db;
        entityDB = dbContext.GetEntityDB<TEntity>(db);
    }


    #region 添加

    /// <summary>
    /// 添加一条数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(TEntity model)
    {
        long i = await Task.Run(() => db.Insertable(model).ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 批量添加数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddListAsync(List<TEntity> model)
    {
        long i = await Task.Run(() => db.Insertable(model).ExecuteCommand());
        return i > 0;
    }

    #endregion


    #region  删除

    /// <summary>
    /// 根据主键删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteByIdAsync(object id)
    {
        long i = await Task.Run(() => db.Deleteable<TEntity>(id).ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 删除一条或多条数据
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> where)
    {
        long i = await Task.Run(() => db.Deleteable<TEntity>().Where(where).ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 根据实体删除一条数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(TEntity model)
    {
        long i = await Task.Run(() => db.Deleteable<TEntity>(model).ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 假删除
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(Expression<Func<TEntity, object>> columns, Expression<Func<TEntity, bool>> where)
    {
        long i = await Task.Run(() => db.Updateable<TEntity>(where).UpdateColumns(columns).ExecuteCommand());
        return i > 0;
    }

    #endregion


    #region  更新

    /// <summary>
    /// 更新实体数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(TEntity model)
    {
        long i = await Task.Run(() => db.Updateable<TEntity>(model).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 根据条件修改数据
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="strWhere"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(TEntity entity, string strWhere)
    {
        long i = await Task.Run(() => db.Updateable<TEntity>().UpdateColumns(it => entity).Where(strWhere).ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 根据条件修改数据
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="strWhere"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> where)
    {
        long i = await Task.Run(() => db.Updateable<TEntity>().UpdateColumns(it => entity).Where(where).ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 根据条件实体修改某些列
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(Expression<Func<TEntity, object>> columns, Expression<Func<TEntity, bool>> where)
    {
        //需要注意 当WhereColumns和UpdateColumns一起用时，需要把wherecolumns中的列加到UpdateColumns中
        long i = await Task.Run(() => db.Updateable<TEntity>().UpdateColumns(columns).Where(where).ExecuteCommand());
        return i > 0;
    }

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="parm"></param>
    /// <returns></returns>
    public async Task<bool> UpdateListAsync(List<TEntity> data)
    {
        long i = await Task.Run(() => db.Updateable(data).ExecuteCommand());
        return i > 0;
    }

    #endregion


    #region  查询

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="objId"></param>
    /// <returns></returns>
    public async Task<TEntity> QueryByIDAsync(object objId)
    {
        return await Task.Run(() => db.Queryable<TEntity>().InSingle(objId));
    }

    /// <summary>
    /// 根据主键查询 是否使用缓存
    /// </summary>
    /// <param name="objId"></param>
    /// <param name="blnUseCache"></param>
    /// <returns></returns>
    public async Task<TEntity> QueryByIDAsync(object objId, bool blnUseCache = false)
    {
        return await Task.Run(() => db.Queryable<TEntity>().WithCacheIF(blnUseCache).InSingle(objId));
    }

    /// <summary>
    /// 根据主键列表查询
    /// </summary>
    /// <param name="lstIds"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> QueryByIDsAsync(object[] lstIds)
    {
        return await Task.Run(() => db.Queryable<TEntity>().In(lstIds).ToList());
    }

    /// <summary>
    /// 查询全部
    /// </summary>
    /// <returns></returns>
    public async Task<List<TEntity>> QueryAsync()
    {
        return await Task.Run(() => entityDB.GetList());
    }

    /// <summary>
    /// 根据条件查询
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>>? strOrderByFileds = null)
    {
        return await db.Queryable<TEntity>().OrderByIF(strOrderByFileds != null, strOrderByFileds, OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
    }

    /// <summary>
    /// 根据条件查询一条
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public async Task<TEntity> QueryFirstAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Task.Run(() => Db.Queryable<TEntity>().Where(whereExpression).FirstAsync());
    }

    /// <summary>
    /// 根据条件分页查询（带条件，页码，排序字段，排序方式）
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <param name="intPageIndex"></param>
    /// <param name="intPageSize"></param>
    /// <param name="strOrderByFileds"></param>
    /// <param name="isAsc"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> QueryPageAsync(Expression<Func<TEntity, bool>> whereExpression, int PageIndex = 0, int PageSize = 20, Expression<Func<TEntity, object>>? strOrderByFileds = null)
    {
        return await Task.Run(() => db.Queryable<TEntity>().OrderByIF(strOrderByFileds != null, strOrderByFileds, OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToPageList(PageIndex, PageSize));
    }

    #endregion
}
