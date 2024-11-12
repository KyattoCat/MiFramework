namespace MiFramework.AI.BT
{
    public class Repeat : Decorator
    {
        private int counter;
        private int limit;

        public Repeat(int limit)
        {
            this.limit = limit;
        }

        protected override void OnInitailize()
        {
            counter = 0;
        }

        protected override Status OnUpdate()
        {
            if (child == null)
                throw new BTException("Child node is null");
            
            while (true)
            {
                child.Tick();

                if (child.IsRunning) return Status.Running;
                if (child.IsFailure) return Status.Failure;
                if (child.IsSuccess) counter++;
                if (counter >= limit) return Status.Success;
            }
        }
    }

}
