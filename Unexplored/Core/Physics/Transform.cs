using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Types;

namespace Unexplored.Core.Physics
{
    public class Transform
    {
        public Vector2 Position;
        public Vector2 Size;
        public FRect Bounds => new FRect(Position, Size);
        public Vector2 CenterPosition => Position + Size / 2;

        public float Left => Position.X;
        public float Right => Position.X + Size.X;
        public float Top => Position.Y;
        public float Bottom => Position.Y + Size.Y;

        public Transform(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public bool Intersects(Transform other)
        {
            return other.Left < Right &&
                   Left < other.Right &&
                   other.Top < Bottom &&
                   Top < other.Bottom;
        }

        public bool Intersects(Rectangle other)
        {
            return other.Left < Right &&
                   Left < other.Right &&
                   other.Top < Bottom &&
                   Top < other.Bottom;
        }
    }
}
