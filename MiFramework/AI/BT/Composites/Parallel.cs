namespace MiFramework.AI.BT
{
    public class Parallel : Composite
    {
        protected Policy mSuccessPolicy;
        protected Policy mFailurePolicy;
        public enum Policy { RequireOne, RequireAll }

        public Parallel(Policy mSuccessPolicy, Policy mFailurePolicy)
        {
            this.mSuccessPolicy = mSuccessPolicy;
            this.mFailurePolicy = mFailurePolicy;
        }

        protected override Status OnUpdate()
        {
            int successCount = 0;
            int failureCount = 0;
            var currentChild = children.First;
            var childCount = children.Count;
            for (int i = 0; i < childCount; i++)
            {
                var child = (currentChild?.Value) ?? throw new BTException("Child is null");
                if (!child.IsTerminated)
                    child.Tick();
                currentChild = currentChild.Next;
                if (child.IsSuccess)
                {
                    successCount++;
                    if (mSuccessPolicy == Policy.RequireOne)
                        return Status.Success;
                }
                else if (child.IsFailure)
                {
                    failureCount++;
                    if (mFailurePolicy == Policy.RequireOne)
                        return Status.Failure;
                }
            }

            if (mFailurePolicy == Policy.RequireAll && failureCount == childCount)
                return Status.Failure;
            if (mSuccessPolicy == Policy.RequireAll && successCount == childCount)
                return Status.Success;
            return Status.Running;
        }

        protected override void OnTerminate()
        {
            foreach (var child in children)
            {
                if (child.IsRunning)
                    child.Abort();
            }
        }
    }
}