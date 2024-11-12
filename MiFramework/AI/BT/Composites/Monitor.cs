namespace MiFramework.AI.BT
{
    public class Monitor : Parallel
    {
        public Monitor(Policy mSuccessPolicy, Policy mFailurePolicy) : base(mSuccessPolicy, mFailurePolicy) {}

        public void AddCondition(Behaviour condition)
        {
            children.AddFirst(condition);
        }

        public void AddAction(Behaviour action)
        {
            children.AddLast(action);
        }
    }
}