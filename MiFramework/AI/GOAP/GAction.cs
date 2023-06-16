using System.Diagnostics.CodeAnalysis;

namespace MiFramework.AI.GOAP
{
    public enum ComparisonOp
    {
        GT,
        GE,
        LT,
        LE,
        EQ,
        NEQ,
    }

    public enum Op
    {
        ADD,
        SUB,
        MUL,
        DIV,
        SET,
    }

    public struct StateItem
    {
        public int value;

        public StateItem(int value)
        {
            this.value = value;
        }

        public bool IsMatch(ConditionItem condition)
        {
            return condition.comparisonOp switch
            {
                ComparisonOp.GT => value > condition.value,
                ComparisonOp.GE => value >= condition.value,
                ComparisonOp.LT => value < condition.value,
                ComparisonOp.LE => value <= condition.value,
                ComparisonOp.EQ => value == condition.value,
                ComparisonOp.NEQ => value != condition.value,
                _ => false,
            };
        }

        public void Affected(EffectItem effect)
        {
            switch (effect.op)
            {
                case Op.ADD: value += effect.value; break;
                case Op.SUB: value -= effect.value; break;
                case Op.MUL: value *= effect.value; break;
                case Op.DIV: value /= effect.value; break;
                case Op.SET: value = effect.value; break;
            }
        }
    }

    public struct ConditionItem
    {
        public ComparisonOp comparisonOp;
        public int value;

        public ConditionItem(ComparisonOp comparisonOp, int value)
        {
            this.comparisonOp = comparisonOp;
            this.value = value;
        }
    }

    public struct EffectItem
    {
        public Op op;
        public int value;

        public EffectItem(Op op, int value)
        {
            this.op = op;
            this.value = value;
        }
    }
    
    public enum GActionState
    {
        None,
        Running,
        Success,
        Failed,
    }

    public class GAction
    {
        public Dictionary<string, ConditionItem> conditions;
        public Dictionary<string, EffectItem> effects;
        public int cost;

        public virtual bool SupportLoop => false;

        public GAction()
        {
            conditions = new Dictionary<string, ConditionItem>();
            effects = new Dictionary<string, EffectItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="effects"></param>
        /// <param name="cost"></param>
        /// <param name="duration">this is in millisecond</param>
        public GAction(Dictionary<string, ConditionItem> conditions,  Dictionary<string, EffectItem> effects, int cost)
        {
            this.conditions = conditions;
            this.effects = effects;
            this.cost = cost;
        }

        public bool IsMatch(Dictionary<string, StateItem> states)
        {
            foreach (var condition in conditions)
            {
                if (!states.TryGetValue(condition.Key, out var state))
                    return false;
                if (!state.IsMatch(condition.Value))
                    return false;
            }
            return true;
        }

        public void Effect(Dictionary<string, StateItem> states)
        {
            foreach (var effect in effects)
            {
                if (!states.TryGetValue(effect.Key, out var state))
                {
                    state = new StateItem();
                }
                state.Affected(effect.Value);
                states[effect.Key] = state;
            }
        }

        public virtual void PreProcess(GAgent agent)
        {

        }

        public virtual void PostProcess(GAgent agent)
        {

        }

        public virtual GActionState Process(GAgent agent)
        {
            return GActionState.Success;
        }
    }
}
