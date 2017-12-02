using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;

namespace Unexplored.Core.Types
{
    public struct Trigger
    {
        public GameObject GameObject;
        public AABB Box;

        public Trigger(GameObject gameObject, AABB box)
        {
            GameObject = gameObject;
            Box = box;
        }

        public Trigger(GameObject gameObject) : this(gameObject, null)
        {
        }
    }
}
