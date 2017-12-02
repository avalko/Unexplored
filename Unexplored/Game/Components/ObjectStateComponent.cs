using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;

namespace Unexplored.Game.Components
{
    class ObjectStateComponent : BehaviorComponent
    {
        private bool state, oldState;
        public bool State
        {
            get => state;
            set
            {
                oldState = state;
                state = value;
            }
        }

        public bool StateOnce => state && !oldState;

        public ObjectStateComponent()
        {
            Drawable = false;
        }
    }
}
