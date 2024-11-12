namespace MiFramework.AI.BT
{
    public class Filter : Sequence
    {
        public void AddContition(Behaviour condition)
        {
            children.AddFirst(condition);
        }

        public void AddAction(Behaviour action)
        {
            children.AddLast(action);
        }
    }
}