using Microsoft.Xna.Framework;
using Unexplored.Game.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Attributes;

namespace Unexplored.Game.Physics
{
    public struct Collider
    {
        public string Name;
        public string Type;

        public Box Box;

        [CustomProperty]
        public bool AllowJumpingOff;
    }
}
