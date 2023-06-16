using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFramework.AI.GOAP
{
    public class GPlanner
    {
        private class Node
        {
            public Node? parent;
            public GAction? action;
            public Dictionary<string, StateItem> state;
            public int cost;

            public Node(Node? parent, GAction? action, Dictionary<string, StateItem> state, int cost)
            {
                this.parent = parent;
                this.action = action;
                this.state = state;
                this.cost = cost;
            }
        }

        public Queue<GAction>? Plan(List<GAction> usableActions, Dictionary<string, ConditionItem> goals, Dictionary<string, StateItem> currentStates)
        {
            Queue<GAction> result = new Queue<GAction>();

            List<Node> leaves = new List<Node>();

            Node root = new Node(null, null, currentStates, 0);

            bool success = BuildGraph(root, leaves, usableActions, goals);
            if (!success)
            {
                return null;
            }

            Node? cheapest = null;
            foreach (Node node in leaves)
            {
                if (cheapest == null)
                {
                    cheapest = node;
                }
                else if (cheapest.cost > node.cost)
                {
                    cheapest = node;
                }
                else if (cheapest.cost == node.cost)
                {
                    Random random = new Random();
                    int randomInt = random.Next(0, 2);
                    if (randomInt == 1) 
                        cheapest = node;
                }
            }

            if (cheapest == null)
                return null;

            List<GAction> resultList = new List<GAction>();
            Node? startNode = cheapest;
            while (startNode != null)
            {
                if (startNode.action == null)
                {
                    throw new Exception("[Planner] Action is null");
                }
                
                resultList.Add(startNode.action);
                startNode = startNode.parent;
            }

            foreach (GAction action in resultList)
            {
                result.Enqueue(action);
            }

            return result;
        }

        private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, ConditionItem> goals)
        {
            bool findOne = false;
            foreach (GAction action in usableActions)
            {
                if (action.IsMatch(parent.state))
                {
                    action.Effect(parent.state);

                    Node node = new Node(parent, action, parent.state, parent.cost + action.cost);
                    // 判断当前状态是否达到目标
                    if (IsReachGoals(goals, parent.state))
                    {
                        leaves.Add(node);
                        findOne = true;
                    }
                    else
                    {
                        var newUsableActions = new List<GAction>(usableActions);
                        newUsableActions.Remove(action);
                        bool isFind = BuildGraph(node, leaves, newUsableActions, goals);
                        if (isFind)
                            findOne = true;
                    }
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
