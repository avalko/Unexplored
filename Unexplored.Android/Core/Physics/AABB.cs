using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Physics
{
    public class AABB
    {
        private static int _Id;
        public int Id;
        public Vector2 Position, Size;
        public Vector2 Min, Max, MinOffset, MaxOffset;

        public AABB(Vector2 position, Vector2 size, Vector2 minOffset, Vector2 maxOffset)
        {
            Id = _Id++;

            Position = position;
            Size = size;

            Min = Vector2.Zero;
            Max = Vector2.Zero;
            MinOffset = minOffset;
            MaxOffset = maxOffset;
        }

        public AABB(Vector2 position, Vector2 size) 
            : this(position, size, Vector2.Zero, Vector2.Zero)
        {
        }

        public bool IsOverlapping(AABB other)
        {
            if (Max.X < other.Min.X || Min.X > other.Max.X)
                return false;
            else if (Max.Y < other.Min.Y || Min.Y > other.Max.Y)
                return false;
            return true;
        }

        public void UpdateBounds()
        {
            Min = Position + MinOffset;
            Max = Position + Size - MaxOffset;
        }
    }
}
