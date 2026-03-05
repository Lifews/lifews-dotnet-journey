using System.Linq.Expressions;

namespace CommonDb;

public interface ISplitTableBaseService<TEntity> where TEntity : class
{

    #region 添加 

    /// <summary>
    /// 添加一条数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> AddAsync(TEntity model);

    /// <summary>
    /// 批量添加数据
    /// </summary>
    /// <param name="parm"></param>
    /// <returns></returns>
    Task<bool> AddListAsync(List<TEntity> parm);

    #endregion

    #region 删除 

    /// <summary>
    /// 根据实体删除数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(TEntity model);

    /// <summary>
    /// 根据实体删除数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(List<TEntity> model);

    #endregion

    #region 更新

    /// <summary>
    /// 根据实体数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(TEntity model);

    /// <summary>
    /// 根据实体数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(List<TEntity> model);

    #endregion

    #region  查询

    /// <summary>
    /// 时间过滤查询
    /// </summary>
    /// <param name="beginTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    Task<List<TEntity>> QueryAsync(DateTime beginTime, DateTime endTime);

    /// <summary>
    /// 时间过滤分页查询
    /// </summary>
    /// <param name="beginTime"></param>
    /// <param name="endTime"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<List<TEntity>> QueryAsync(DateTime beginTime, DateTime endTime, int pageNumber, int pageSize);

    /// <summary>
    /// 时间过滤条件查询
    /// </summary>
    /// <param name="beginTime"></param>
    /// <param name="endTime"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    Task<List<TEntity>> QueryAsync(DateTime beginTime, DateTime endTime, Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// 时间过滤条件分页查询
    /// </summary>
    /// <param name="beginTime"></param>
    /// <param name="endTime"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    Task<List<TEntity>> QueryAsync(DateTime beginTime, DateTime endTime, int pageNumber, int pageSize, Expression<Func<TEntity, bool>> where);

    #endregion


}
