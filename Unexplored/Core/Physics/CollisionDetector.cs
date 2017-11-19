using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Components;

namespace Unexplored.Core.Physics
{
    public static class CollisionDetector
    {
        public static bool Check(ColliderOffset checkingBox, ColliderOffset verifiableBox, ref Collision collision)
        {
            Vector2 halfSizeA = checkingBox.HalfSize;
            Vector2 halfSizeB = verifiableBox.HalfSize;
            var positionA = checkingBox.CenterPosition;
            var positionB = verifiableBox.CenterPosition;
            Vector2 direction = positionB - positionA;
            Vector2 overlap = (halfSizeA + halfSizeB) - new Vector2(Math.Abs(direction.X), Math.Abs(direction.Y));

            if (overlap.X > 0 && overlap.Y > 0)
            {
                if (overlap.X < overlap.Y)
                {
                    if (direction.X < 0)
                        collision.Normal = new Vector2(-1, 0);
                    else
                        collision.Normal = new Vector2(1, 0);
                    collision.Direction = Vector2.UnitX;
                    collision.Penetration = overlap.X;
                    return true;
                }
                else
                {
                    if (direction.Y < 0)
                        collision.Normal = new Vector2(0, -1);
                    else
                        collision.Normal = new Vector2(0, 1);
                    collision.Direction = Vector2.UnitY;
                    collision.Penetration = overlap.Y;
                    return true;
                }
            }

            return false;
        }
    }
}
