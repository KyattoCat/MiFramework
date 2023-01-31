namespace MiFramework.Pool
{
    public interface IPoolObject
    {
        uint PoolObjectID { get; set; }
        bool IsActive { get; set; }
        void Clear();

    }
}
