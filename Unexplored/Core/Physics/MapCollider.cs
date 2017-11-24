using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Attributes;

namespace Unexplored.Core.Physics
{
    public struct MapCollider
    {
        public string Name;
        public string Type;

        public Vector2 OffsetMin;
        public Vector2 OffsetMax;

        [CustomProperty]
        public bool AllowJumpingOff;

        public static MapCollider Create(Vector2 offsetMin, Vector2 offsetMax, string type = null)
        {
            return new MapCollider
            {
                OffsetMin = offsetMin,
                OffsetMax = offsetMax,
                Type = type
            };
        }
    }
}
