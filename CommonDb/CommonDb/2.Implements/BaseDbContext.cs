using SqlSugar;

namespace CommonDb;

public class BaseDbContext
{
    private static string? _connectionString;
    private static DbType _dbType;
    private SqlSugarClient _db;


    /// <summary>
    /// 连接字符串 
    /// </summary>
    public static string? ConnectionString
    {
        get { return _connectionString; }
        set { _connectionString = value; }
    }


    /// <summary>
    /// 数据库类型 
    /// </summary>
    public static DbType DbType
    {
        get { return _dbType; }
        set { _dbType = value; }
    }


    /// <summary>
    /// 数据连接对象 
    /// </summary>
    public SqlSugarClient Db
    {
        get { return _db; }
        private set { _db = value; }
    }


    public BaseDbContext()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new ArgumentNullException("数据库连接字符串为空");

        _db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = _connectionString,
            DbType = _dbType,
            IsAutoCloseConnection = true,//SqlSugar 会在每次操作完成后自动关闭数据库连接
                                         //下次再通过 BaseDbContext.Context 获取新实例执行操作时，它会从线程池中获取一个新的连接。
        });

        //调式代码 用来打印SQL  可删
        _db.Aop.OnLogExecuting = (sql, pars) =>
        {
            Console.WriteLine(sql);
        };

    }


    /// <summary>
    /// 功能描述:设置初始化参数
    /// </summary>
    /// <param name="strConnectionString">连接字符串</param>
    /// <param name="enmDbType">数据库类型</param>
    public static void Init(string strConnectionString, DbType enmDbType = SqlSugar.DbType.MySql)
    {
        _connectionString = strConnectionString;
        _dbType = enmDbType;
    }


    /// <summary>
    /// 功能描述:获取数据库处理对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <returns></returns>
    public SimpleClient<T> GetEntityDB<T>(SqlSugarClient db) where T : class, new()
    {
        return new SimpleClient<T>(db);
    }


}

