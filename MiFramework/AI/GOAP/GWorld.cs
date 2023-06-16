using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFramework.AI.GOAP
{
    public class GWorld
    {
        private static readonly Dictionary<string, StateItem> worldState = new Dictionary<string, StateItem>();
        public static Dictionary<string, StateItem> WorldState => worldState;
        public static void Set(string name, StateItem item)
        {
            worldState[name] = item;
        }
        public static void Set(string name, int value)
        {
            worldState[name] = new StateItem { value = value};
        }
        public static bool TryGet(string name, out StateItem state)
        {
            return worldState.TryGetValue(name, out state);
        }
    }
}
