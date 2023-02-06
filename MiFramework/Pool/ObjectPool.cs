namespace MiFramework.Pool
{
    /// <summary>
    /// 参考UnityUI对象池<br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : new()
    {
        private readonly Action<T>? OnGetObject;
        private readonly Action<T>? OnReleaseObject;
        private readonly Stack<T> stack;

        public ObjectPool(Action<T>? onSpawnObject = null, Action<T>? onDespawn = null, int capacity = 16)
        {
            OnGetObject = onSpawnObject;
            OnReleaseObject = onDespawn;

            stack = new Stack<T>(capacity);
        }

        public T Get()
        {
            T element;

            if (stack.Count > 0)
            {
                element = stack.Pop();
            }
            else
            {
                element = new();
            }

            OnGetObject?.Invoke(element);

            return element;
        }

        public void Release(T? element)
        {
            if (element == null || stack.Contains(element))
                return;

            OnReleaseObject?.Invoke(element);

            stack.Push(element);
        }
    }
}
