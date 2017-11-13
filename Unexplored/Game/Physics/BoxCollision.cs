using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Physics
{
    public struct BoxCollision
    {
        public Box CheckingBox;
        public Box VerifiableBox;
        public Collision Collision;

        public BoxCollision(Box checkingBox, Box verifiableBox) : this()
        {
            CheckingBox = checkingBox;
            VerifiableBox = verifiableBox;
        }

        public static bool Check(Box checkingBox, Box verifiableBox, out BoxCollision boxCollision)
        {
            boxCollision = new BoxCollision(checkingBox, verifiableBox);

            var sizeA = checkingBox.Size;
            var sizeB = verifiableBox.Size;
            var positionA = checkingBox.Position + sizeA / 2;
            var positionB = verifiableBox.Position + sizeB / 2;

            Vector2 direction = positionB - positionA;
            Vector2 halfSizeA = sizeA / 2;
            Vector2 halfSizeB = sizeB / 2;
            Vector2 overlap = (halfSizeA + halfSizeB) - new Vector2(Math.Abs(direction.X), Math.Abs(direction.Y));

            if (overlap.X > 0 && overlap.Y > 0)
            {
                if (overlap.X < overlap.Y)
                {
                    if (direction.X < 0)
                        boxCollision.Collision.Normal = new Vector2(-1, 0);
                    else
                        boxCollision.Collision.Normal = new Vector2(1, 0);
                    boxCollision.Collision.Direction = Vector2.UnitX;
                    boxCollision.Collision.Penetration = overlap.X;
                    return true;
                }
                else
                {
                    if (direction.Y < 0)
                        boxCollision.Collision.Normal = new Vector2(0, -1);
                    else
                        boxCollision.Collision.Normal = new Vector2(0, 1);
                    boxCollision.Collision.Direction = Vector2.UnitY;
                    boxCollision.Collision.Penetration = overlap.Y;
                    return true;
                }
            }

            return false;
        }
    }
}
