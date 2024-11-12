namespace MiFramework.AI.BT
{
    public enum Status
    {
        Invalid,
        Success,
        Failure,
        Running,
        Aborted,
    }
    
    public abstract class Behaviour
    {
        public bool IsTerminated => IsSuccess || IsFailure;
        public bool IsSuccess => status == Status.Success;
        public bool IsFailure => status == Status.Failure;
        public bool IsRunning => status == Status.Running;
        protected Status status = Status.Invalid;

        protected virtual void OnInitailize() { }
        protected abstract Status OnUpdate();
        protected virtual void OnTerminate() { }

        public Status Tick()
        {
            if (!IsRunning)
                OnInitailize();
            status = OnUpdate();
            if (IsTerminated)
                OnTerminate();
            return status;
        }

        public virtual void AddChild(Behaviour child) { }

        public void Reset()
        {
            status = Status.Invalid;
        }

        public void Abort()
        {
            OnTerminate();
            status = Status.Aborted;
        }
    }
}