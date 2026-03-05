// 鸭子类型
// 只要一个类，包含某个函数签名的方法，那它就有可能被编译器识别为这个类，确实拥有该方法，即便该方法是通过扩展方法实现的。

#if false

// 思考：如要用扩展方法实现如下功能，改怎么写

using System.Collections;

List<int> _ = [1, 2, 3];

MyCollection<int> collection = [1, 2, 3];

class MyCollection<T> : IEnumerable
{
    private readonly List<T> _items = new List<T>();

    public IEnumerator GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public void Add(T item)
    {
        _items.Add(item);
    }
}

#endif

#if false

using System.Collections;

List<int> _ = [1, 2, 3];

MyCollection<int> collection = [1, 2, 3];

class MyCollection<T> : IEnumerable
{
    private readonly List<T> _items = new List<T>();

    public IEnumerator GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public void Push(T item)
    {
        _items.Add(item);
    }
}

static class MyCollectionExtension
{
    public static void Add<T>(this MyCollection<T> collection, T item)
    {
        collection.Push(item);
    }
}

#endif
