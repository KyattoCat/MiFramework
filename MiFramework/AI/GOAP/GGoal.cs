using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFramework.AI.GOAP
{
    public class GGoal
    {
        public virtual bool SupportLopp => false;

        private Dictionary<string, ConditionItem>? goal;

        public GGoal()
        {
            goal = new Dictionary<string, ConditionItem>();
        }

        public GGoal(Dictionary<string, ConditionItem>? goal)
        {
            this.goal = goal;
        }

        public GGoal(string name, ComparisonOp comparisonOp, int value)
        {
            goal ??= new Dictionary<string, ConditionItem>();
            goal.Add(name, new ConditionItem(comparisonOp, value));
        }

        public GGoal(string name, ConditionItem condition)
        {
            goal ??= new Dictionary<string, ConditionItem>();
            goal.Add(name, condition);
        }

        public void SetGoal(Dictionary<string, ConditionItem> goal)
        {
            this.goal = goal;
        }

        public void RemoveGoal(string name)
        {
            goal?.Remove(name);
        }

        public void RemoveAllGoal()
        {
            goal?.Clear();
        }

        public Dictionary<string, ConditionItem>? GetGoal()
        {
            return goal;
        }
    }
}
