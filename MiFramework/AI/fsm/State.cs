using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiFramework.AI.FSM
{
    public abstract class State
    {
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate();
    }
}
