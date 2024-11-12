namespace MiFramework.AI.BT
{
    public abstract class Composite : Behaviour
    {
        protected LinkedList<Behaviour> children = new LinkedList<Behaviour>();

        public virtual void RemoveChild(Behaviour child)
        {
            children.Remove(child);
        }
        
        public void ClearChildren()
        {
            children.Clear();
        }

        public override void AddChild(Behaviour child)
        {
            children.AddLast(child);
        }
    }
}