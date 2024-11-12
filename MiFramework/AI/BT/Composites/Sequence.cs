namespace MiFramework.AI.BT
{
    public class Sequence : Composite
    {
        protected LinkedListNode<Behaviour>? currentChild;

        protected override void OnInitailize()
        {
            currentChild = children.First;
        }

        protected override Status OnUpdate()
        {
            if (currentChild == null)
                return Status.Failure;
            
            while (true)
            {
                Status status = currentChild.Value.Tick();
                if (status != Status.Success)
                    return status;
                currentChild = currentChild.Next;
                if (currentChild == null)
                    return Status.Success;
            }
        }
    }
}