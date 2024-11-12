namespace MiFramework.AI.BT
{
    public class BehaviourTree
    {
        public bool HaveRoot => root != null;
        private Behaviour? root;

        public BehaviourTree(Behaviour? root)
        {
            this.root = root;
        }

        public void Tick()
        {
            root?.Tick();
        }

        public void SetRoot(Behaviour? newRoot)
        {
            root = newRoot;
        }
    }
}