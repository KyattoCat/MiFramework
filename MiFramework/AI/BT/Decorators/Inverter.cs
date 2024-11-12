namespace MiFramework.AI.BT
{
    public class Inverter : Decorator
    {
        protected override Status OnUpdate()
        {
            if (child == null) throw new BTException("Child node is null");
            
            child.Tick();

            if (child.IsFailure)
                return Status.Success;
            
            if (child.IsSuccess)
                return Status.Failure;
            
            return Status.Running;
        }
    }
}