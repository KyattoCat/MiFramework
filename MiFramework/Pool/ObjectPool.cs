namespace MiFramework.Pool
{
    public class ObjectPool<T> where T : IPoolObject, new()
    {
        private uint idCounter;
        private readonly Dictionary<uint, T> objects;
        private readonly Stack<T> stack;

        public ObjectPool()
        {
            objects = new Dictionary<uint, T>();
            stack = new Stack<T>();
        }

        public ObjectPool(int capacity)
        {
            objects = new Dictionary<uint, T>(capacity);
            stack = new Stack<T>(capacity);
        }

        public T Get()
        {
            T element;

            if (stack.Count == 0)
            {
                element = new T() { PoolObjectID = idCounter++ };
                objects.Add(element.PoolObjectID, element);
            }
            else
            {
                element = stack.Pop();
            }
            
            element.Clear();
            element.IsActive = true;

            return element;
        }

        public void Release(T element)
        {
            if (objects.TryGetValue(element.PoolObjectID, out var e) && e.IsActive)
            {
                element.Clear();
                element.IsActive = false;

                stack.Push(element);
            }
        }
    }
}
