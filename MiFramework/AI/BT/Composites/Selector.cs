namespace MiFramework.AI.BT
{
    public class Selector : Sequence
    {
        protected override Status OnUpdate()
        {
            while (true)
            {
                if (currentChild == null) throw new BTException("Selector has no children");
                Status status = currentChild.Value.Tick();
                if (status != Status.Failure)
                    return status;
                currentChild = currentChild.Next;
                if (currentChild == null)
                    return Status.Failure;
            }
        }
    }
}