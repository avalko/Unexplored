using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ComputeVelocity(GameTime gameTime, float speed)
        {
            NextPosition.X = Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            NextPosition.Y = MathHelper.Clamp(Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds, -15.0f, +15.0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyVelocity()
        {
            Transform.Position += NextPosition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableSlip()
        {
            Velocity.X = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyGravity(GameTime gameTime)
        {
            Velocity.Y += 20 * GravitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Math.Abs(Velocity.X) < 0.01f)
                Velocity.X = 0;
            else if (Velocity.X > 0)
                Velocity.X -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Velocity.X < 0)
                Velocity.X += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
