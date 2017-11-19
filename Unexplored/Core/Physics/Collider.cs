using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Attributes;

namespace Unexplored.Core.Physics
{
    public class Collider
    {
        public string Name;
        public string Type;

        public ColliderOffset Offset;

        [CustomProperty]
        public bool AllowJumpingOff;

        public static Collider Create(Vector2 offsetMin, Vector2 offsetMax)
        {
            return new Collider
            {
                Offset = new ColliderOffset(offsetMin, offsetMax)
            };
        }
    }
}
