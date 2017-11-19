using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Types;

namespace Unexplored.Core.Physics
{
    public class ColliderOffset
    {
        public FRect Bounds => bounds;
        public Vector2 CenterPosition;
        public Vector2 HalfSize;

        private FRect bounds;
        private Vector2 OffsetTopLeft;
        private Vector2 OffsetBottomRight;

        public ColliderOffset(Vector2 position, Vector2 size, Vector2 offsetMin, Vector2 offsetMax)
        {
            OffsetTopLeft = offsetMin;
            OffsetBottomRight = offsetMax;
            HalfSize = size / 2;
            CenterPosition = position + HalfSize;
            bounds = new FRect(position.X, position.Y, size.X, size.Y);
        }

        public ColliderOffset(Vector2 offsetMin, Vector2 offsetMax)
        {
            OffsetTopLeft = offsetMin;
            OffsetBottomRight = offsetMin + offsetMax;
            HalfSize = CenterPosition = Vector2.Zero;
            bounds = new FRect();
        }

        public void Update(Transform transform)
        {
            HalfSize = (transform.Size - OffsetBottomRight) / 2;
            CenterPosition = (transform.Position + OffsetTopLeft) + HalfSize;
            bounds = new FRect(transform.Position.X, transform.Position.Y, transform.Size.X, transform.Size.Y);
        }
    }
}
