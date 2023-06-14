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
        public string name;
        public int value;

        public bool IsMatch(ConditionItem condition)
        {
            if (!name.Equals(condition.name))
                return false;

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
            if (!name.Equals(effect.name))
                return;
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
        public string name;
        public int value;
    }

    public struct EffectItem
    {
        public Op op;
        public string name;
        public int value;
    }
}
