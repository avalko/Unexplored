using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Physics;

namespace Unexplored.Game.Objects.Base
{
    public class RigidbodyGameObject : GameObject
    {
        public override void Update(GameTime gameTime)
        {
            if (IsRigidbody)
            {
                Rigidbody.ApplyGravity(gameTime);
                Rigidbody.ComputeVelocity(gameTime, Speed * SpeedFactor);

                Rigidbody.OnGround = false;
                Rigidbody.LeftWall = false;
                Rigidbody.RightWall = false;
            }

            base.Update(gameTime);
        }

        protected override void OnBoxColliderCollision(BoxCollision boxCollision)
        {
            var collision = boxCollision.Collision;

            if (collision.Normal == Vector2.UnitY)
            {
                if (Rigidbody.Velocity.Y < 0)
                    return;

                Rigidbody.Velocity.Y = 0;
                Rigidbody.OnGround = true;
            }
            else if (collision.Normal == -Vector2.UnitY)
            {
                Rigidbody.Velocity.Y = 0;
            }
            else if (collision.Normal == Vector2.UnitX)
            {
                Rigidbody.RightWall = true;
            }
            else if (collision.Normal == -Vector2.UnitX)
            {
                Rigidbody.LeftWall = true;
            }

            Rigidbody.NextPosition -= collision.Normal * collision.Penetration;
        }

        public override void UpdateEnd(GameTime gameTime)
        {
            if (IsRigidbody && IsMovable)
            {
                Rigidbody.ApplyVelocity();
                Rigidbody.DisableSlip();
            }

            base.UpdateEnd(gameTime);
        }
    }
}
