using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Physics
{
    public class Rigidbody
    {
        public const float GravitySpeed = 16.0f;
        public const float MoveSpeed = 16.0f * 6.0f;

        public Vector2 Acceleration;
        public Vector2 Velocity;
        public Vector2 NextPosition;
        public Transform Transform;
        public Vector2 OffsetMin;
        public Vector2 OffsetMax;

        public bool OnGround, LeftWall, RightWall;

        public Rigidbody(Transform transform)
        {
            Transform = transform;
            OnGround = false;
        }

        public void ComputeVelocity(GameTime gameTime, float speed)
        {
            NextPosition.X = Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            NextPosition.Y = MathHelper.Clamp(Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds, -15.0f, +15.0f);
        }

        public void ApplyVelocity()
        {
            Transform.Position += NextPosition;
            Velocity.X = 0;
        }

        public void ApplyGravity(GameTime gameTime)
        {
            Velocity.Y += 20 * GravitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
