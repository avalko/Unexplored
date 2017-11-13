using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Physics
{
    public class Transform
    {
        public Vector2 Position;
        public Vector2 Size;

        public Transform(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public Box Box => new Box(Position, Size);
    }
}
