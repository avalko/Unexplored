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
        public bool State;

        public ObjectStateComponent()
        {
            Drawable = false;
        }
    }
}
