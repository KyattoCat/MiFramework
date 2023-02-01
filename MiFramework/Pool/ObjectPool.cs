namespace MiFramework.Pool
{
    /// <summary>
    /// 参考UnityUI对象池<br/>
    /// 感觉像是把创建和回收时，对象要执行的清理方法或别的什么方法交给了调用方处理<br/>
    /// 这个池针对引用类型回收做了一点手脚，回收后引用置空避免调用方仍然能继续使用该引用<br/>
    /// 但是这样做就不能将值类型作为池内对象来存储了
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : class, new()
    {
        private readonly Action<T>? OnGetObject;
        private readonly Action<T>? OnReleaseObject;
        private readonly Stack<T> stack;

        public ObjectPool(Action<T>? onSpawnObject, Action<T>? onDespawn, int capacity = 16)
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

        public void Release(ref T? element)
        {
            if (element == null || stack.Contains(element))
                return;

            OnReleaseObject?.Invoke(element);

            T temp = element;

            stack.Push(temp);

            element = null;
        }
    }
}
