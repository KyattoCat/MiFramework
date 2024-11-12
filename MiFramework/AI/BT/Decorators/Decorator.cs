namespace MiFramework.AI.BT
{
    public abstract class Decorator : Behaviour
    {
        protected Behaviour? child;
        public override void AddChild(Behaviour child)
        {
            this.child = child;
        }
    }
}