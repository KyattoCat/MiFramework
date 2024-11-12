namespace MiFramework.AI.BT
{
    public class ActiveSelector : Selector
    {
        protected override Status OnUpdate()
        {
            var prev = currentChild ?? throw new BTException("ActiveSelector has no children");
            base.OnInitailize();
            var result = base.OnUpdate();
            if (prev != children.Last && currentChild != prev)
                prev.Value.Abort();
            return result;
        }
    }
}