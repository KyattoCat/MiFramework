using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFramework.AI.GOAP
{
    public class GAgent
    {
        private readonly GPlanner planner;
        private readonly List<GAction> actions;

        private Queue<GAction>? currentActionQueue;
        private GAction? currentAction;

        private readonly Dictionary<string, StateItem> agentState;
        private Dictionary<string, ConditionItem>? goal;

        public GAgent()
        {
            planner = new GPlanner();
            actions = new List<GAction>();
            currentActionQueue = new Queue<GAction>();
            agentState = new Dictionary<string, StateItem>();
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

        public void SetGoal(Dictionary<string, ConditionItem> goal)
        {
            this.goal = goal;
        }

        public void Update()
        {
            if (goal == null)
                return;
            
            if (currentAction == null)
            {
                currentActionQueue = planner.Plan(actions, goal, MergeState(agentState, GWorld.WorldState));

                if (currentActionQueue == null)
                    return;

                if (currentActionQueue.Count == 0)
                    return;

                currentAction = currentActionQueue.Dequeue();
            }

            if (currentAction == null)
                return;

            if (currentAction.actionState == GActionState.None)
            {
                currentAction.PreProcess();
            }
            else if (currentAction.actionState == GActionState.Running)
            {
                currentAction.Update();
            }
            else if (currentAction.actionState == GActionState.Success || currentAction.actionState == GActionState.Failed)
            {
                currentAction.PostProcess();
            }

            if (currentAction.actionState == GActionState.Success || currentAction.actionState == GActionState.Failed)
            {
                if (currentActionQueue != null && currentActionQueue.Count > 0)
                {
                    currentAction = currentActionQueue.Dequeue();
                }
                else // 队列执行完毕 待机
                {
                    goal = null;
                    currentAction = null;
                    currentActionQueue = null;
                }
            }
        }

        private Dictionary<string, StateItem> MergeState(Dictionary<string, StateItem> state1, Dictionary<string, StateItem> state2)
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
