using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Core.Components;

namespace Unexplored.Core.Physics
{
    public struct Trigger
    {
        public GameObject GameObject;
        public AABB Box;
        public string Type;

        public Trigger(GameObject gameObject, AABB box)
        {
            GameObject = gameObject;
            Box = box;
            Type = null;
        }

        public Trigger(string type, GameObject gameObject, AABB box) : this(gameObject, box)
        {
            Type = type;
        }

        public Trigger(string type, GameObject gameObject) : this(gameObject, null)
        {
            Type = type;
        }

        public Trigger(GameObject gameObject) : this(gameObject, null)
        {
        }
    }
}
