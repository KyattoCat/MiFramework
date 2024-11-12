namespace MiFramework.AI.BT
{
    public class BehaviourTreeBuilder
    {
        private readonly Stack<Behaviour> nodeStack;
        private readonly BehaviourTree behaviourTree;

        public BehaviourTreeBuilder()
        {
            nodeStack = new Stack<Behaviour>();
            behaviourTree = new BehaviourTree(null);
        }

        private void AddBehaviour(Behaviour behaviour)
        {
            if (behaviourTree.HaveRoot)
            {
                nodeStack.Peek().AddChild(behaviour);
            }
            else
            {
                behaviourTree.SetRoot(behaviour);
            }

            if (behaviour is Composite || behaviour is Decorator)
            {
                nodeStack.Push(behaviour);
            }
        }

        public void TreeTick()
        {
            behaviourTree.Tick();
        }

        public BehaviourTreeBuilder Back()
        {
            nodeStack.Pop();
            return this;
        }

        public BehaviourTree End()
        {
            nodeStack.Clear();
            return behaviourTree;
        }
    }
}