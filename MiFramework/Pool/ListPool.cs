namespace MiFramework.Pool
{
    /// <summary>
    /// 参考UnityUI对象池<br/>
    /// List的对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ListPool<T>
    {
        private static readonly ObjectPool<List<T>> listPool = new(null, Clear);

        private static void Clear(List<T> list)
        {
            list.Clear();
        }

        public static List<T> Get()
        {
            return listPool.Get();
        }

        public static void Release(List<T>? list)
        {
            listPool.Release(list);
        }
    }
}
