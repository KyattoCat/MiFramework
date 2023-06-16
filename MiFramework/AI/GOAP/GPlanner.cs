namespace MiFramework.AI.GOAP
{
    public class GPlanner
    {
        private readonly static Random random = new Random();

        private class Node
        {
            public Node? parent;
            public GAction? action;
            public int cost;
            public Dictionary<string, StateItem> cachedState;

            public Node(Node? parent, GAction? action, int cost, Dictionary<string, StateItem> cachedState)
            {
                this.parent = parent;
                this.action = action;
                this.cost = cost;
                this.cachedState = cachedState;
            }
        }

        public Queue<GAction>? Plan(List<GAction> usableActions, Dictionary<string, ConditionItem> goals, Dictionary<string, StateItem> currentStates)
        {
            Queue<GAction> result = new Queue<GAction>();

            List<Node> leaves = new List<Node>();

            Node root = new Node(null, null, 0, currentStates);

            if (!BuildGraph(root, leaves, usableActions, goals))
                return null;

            if (leaves.Count == 0)
                return null;

            // 获取代价最低的行为队列
            Node cheapest = leaves[0];
            for (int i = 1; i < leaves.Count; i++)
            {
                if (cheapest.cost > leaves[i].cost)
                {
                    cheapest = leaves[i];
                }
                else if (cheapest.cost == leaves[i].cost)
                {
                    if (random.NextDouble() >= 0.5f)
                    {
                        cheapest = leaves[i];
                    }
                }
            }

            // 得到的结果是倒序 需要逆转一下
            Stack<GAction> tempStack = new Stack<GAction>();
            // 遍历节点获取行为队列
            Node node = cheapest;
            while (node.parent != null)
            {
                if (node.action == null)
                    return null;

                tempStack.Push(node.action);
                node = node.parent;
            }

            int count = tempStack.Count;
            for (int i = 0; i < count; i++)
            {
                result.Enqueue(tempStack.Pop());
            }

            return result;
        }

        private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, ConditionItem> goals)
        {
            bool findOne = false;
            // 遍历可用行为
            foreach (GAction action in usableActions)
            {
                // 判断该行为是否可以执行
                if (!action.IsMatch(parent.cachedState))
                    continue;

                // 缓存当前行为带来的影响
                var newState = new Dictionary<string, StateItem>(parent.cachedState);
                action.Effect(newState);
                // 创建新节点
                Node newNode = new Node(parent, action, parent.cost + action.cost, newState);

                // 判断当前节点是否满足目标
                if (IsReachGoals(goals, newState))
                {
                    leaves.Add(newNode);
                    findOne = true;
                }
                else
                {
                    // 否则剔除该action 开始递归
                    var newUsableActionList = new List<GAction>(usableActions);
                    if (!action.SupportLoop) newUsableActionList.Remove(action);
                    findOne |= BuildGraph(newNode, leaves, newUsableActionList, goals);
                }
            }
            return findOne;
        }

        public bool IsReachGoals(Dictionary<string, ConditionItem> goals, Dictionary<string, StateItem> states)
        {
            foreach (var condition in goals)
            {
                if (!states.TryGetValue(condition.Key, out var state))
                    return false;
                if (!state.IsMatch(condition.Value))
                    return false;
            }
            return true;
        }
    }
}
