using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Physics
{
    public struct Box
    {
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Min;
        public Vector2 Max;

        public Box(Vector2 position, Vector2 size)
        {
            Min = Position = position;
            Size = size;
            Max = Position + Size;
        }
    }
}
