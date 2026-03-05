using SqlSugar;

namespace CommonDb;


/// <summary>
/// 重写一些方法，暂时没用上
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseEntity<T>
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, Length = 64)]
    public T Id { get; set; }

    /// <summary>
    /// 重写方法 相等运算
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        var compareTo = obj as BaseEntity<T>;
        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;

        return Id.Equals(compareTo.Id);
    }

    /// <summary>
    /// 重写方法 实体比较 ==
    /// </summary>
    /// <param name="a">领域实体a</param>
    /// <param name="b">领域实体b</param>
    /// <returns></returns>
    public static bool operator ==(BaseEntity<T> a, BaseEntity<T> b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    /// <summary>
    /// 重写方法 实体比较 !=
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator !=(BaseEntity<T> a, BaseEntity<T> b)
    {
        return !(a == b);
    }

    /// <summary>
    /// 获取哈希
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907) + Id.GetHashCode();
    }
}