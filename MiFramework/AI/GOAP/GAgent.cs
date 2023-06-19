namespace MiFramework.AI.GOAP
{
    public class GAgent
    {
        private readonly GPlanner planner;
        private readonly List<GAction> actions;

        private Queue<GAction>? currentActionQueue;
        private GAction? currentAction;
        private GActionState currentActionState;

        private readonly Dictionary<string, StateItem> agentState;
        private GGoal? goal;

        public GAgent()
        {
            planner = new GPlanner();
            actions = new List<GAction>();
            currentActionQueue = new Queue<GAction>();
            agentState = new Dictionary<string, StateItem>();
        }

        public void AddAction<T>() where T : GAction, new()
        {
            actions.Add(new T());
        }

        public void AddAction(GAction action)
        {
            actions.Add(action);
        }

        public void AddActions(List<GAction> actions)
        {
            foreach (GAction action in actions)
            {
                this.actions.Add(action);
            }
        }

        public void SetGoal(GGoal goal)
        {
            this.goal = goal;
        }

        public void SetState(string name, int state)
        {
            agentState[name] = new StateItem(state);
        }

        public void SetState(string name, StateItem state)
        {
            agentState[name] = state;
        }

        public void Update()
        {
            if (goal == null)
                return;

            var goalDic = goal.GetGoal();
            if (goalDic == null)
                return;

            if (currentAction == null)
            {
                currentActionQueue = planner.Plan(actions, goalDic, MergeState(agentState, GWorld.WorldState));

                if (currentActionQueue == null)
                {
#if DEBUG
                    Console.WriteLine("Plan Failed");
#endif
                    goal = null;
                    return;
                }

                if (currentActionQueue.Count == 0)
                    return;

#if DEBUG
                Console.Write("Plan Success: ");
                foreach (GAction action in currentActionQueue)
                {
                    Console.Write(action.GetType().ToString() + "->");
                }
                Console.Write("[EOQ]\n");
#endif
                currentAction = currentActionQueue.Dequeue();
            }

            if (currentAction == null)
                return;

            if (currentActionState == GActionState.None)
            {
                currentAction.PreProcess(this);
                currentActionState = GActionState.Running;
            }
            
            if (currentActionState == GActionState.Running)
            {
                currentActionState = currentAction.Process(this);
            }
            
            if (currentActionState == GActionState.Success)
            {
                currentAction.Effect(agentState);
            }

            if (currentActionState == GActionState.Success || currentActionState == GActionState.Failed)
            {
                currentAction.PostProcess(this);

                currentActionState = GActionState.None;

                if (currentActionQueue != null && currentActionQueue.Count > 0)
                {
                    currentAction = currentActionQueue.Dequeue();
                }
                else // 队列执行完毕 待机
                {
                    if (!goal.SupportLopp)
                        goal = null;
                    currentAction = null;
                    currentActionQueue = null;
#if DEBUG
                    Console.WriteLine("ReachGoal");
#endif
                }
            }
        }

        private static Dictionary<string, StateItem> MergeState(Dictionary<string, StateItem> state1, Dictionary<string, StateItem> state2)
        {
            Dictionary<string, StateItem> merged = new Dictionary<string, StateItem>(state1.Count + state2.Count);

            foreach (var kvp in state1)
            {
                merged[kvp.Key] = kvp.Value;
            }

            foreach (var kvp in state2)
            {
                if (merged.ContainsKey(kvp.Key))
                {
                    continue;
                }

                merged[kvp.Key] = kvp.Value;
            }

            return merged;
        }
    }
}
