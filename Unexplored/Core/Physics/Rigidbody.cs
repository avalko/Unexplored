using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Physics
{
    public class Rigidbody
    {
        public const float FRICTION_FORCE = 800;

        private static int _Id;

        public int Id;
        public AABB Box;

        private bool WasAppliedSpeedX;
        private bool WasAppliedSpeedY;
        public Vector2 Velocity;
        public Vector2 Force;
        public Vector2 Speed;
        public float InverseMass;
        public bool IsKinematic, IsMovable;

        public Rigidbody(AABB box, bool isKinematic, bool isMovable)
        {
            Id = _Id++;

            Box = box;
            IsKinematic = isKinematic;
            IsMovable = isMovable;
            InverseMass = isKinematic ? 1 : 0;
            Velocity = Vector2.Zero;
            Force = Vector2.Zero;
        }

        public Rigidbody(bool isKinematic, bool isMovable)
        {
            Id = _Id++;

            IsMovable = isMovable;
            IsKinematic = isKinematic;
            InverseMass = isKinematic ? 1 : 0;
            Velocity = Vector2.Zero;
            Force = Vector2.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IntegrateForces(float x, float y, float deltaTime) => IntegrateForces(new Vector2(x, y), deltaTime);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IntegrateForces(Vector2 gravity, float deltaTime)
        {
            if (WasAppliedSpeedX)
            {
                if (Speed.X > 0)
                {
                    if (Velocity.X < Speed.X)
                        Velocity.X += FRICTION_FORCE * deltaTime;
                    else
                        Velocity.X = Speed.X;
                }
                else if (Speed.X < 0)
                {
                    if (Velocity.X > Speed.X)
                        Velocity.X -= FRICTION_FORCE * deltaTime;
                    else
                        Velocity.X = Speed.X;
                }
            }
            else if (Velocity.X != 0)
            {
                if (Math.Abs(Velocity.X) > 11)
                {
                    if (Velocity.X > 0)
                        Velocity.X -= FRICTION_FORCE * deltaTime;
                    else
                        Velocity.X += FRICTION_FORCE * deltaTime;
                }
                else
                    Velocity.X = 0;
            }

            if (IsKinematic)
            {
                Velocity += (Force + gravity * deltaTime) * 0.5f;
            }
            else
            {
                if (WasAppliedSpeedY)
                {
                    if (Speed.Y > 0)
                    {
                        if (Velocity.Y < Speed.Y)
                            Velocity.Y += FRICTION_FORCE * deltaTime;
                        else
                            Velocity.Y = Speed.Y;
                    }
                    else if (Speed.Y < 0)
                    {
                        if (Velocity.Y > Speed.Y)
                            Velocity.Y -= FRICTION_FORCE * deltaTime;
                        else
                            Velocity.Y = Speed.Y;
                    }
                }
                else
                {
                    if (Math.Abs(Velocity.Y) > 11)
                    {
                        if (Velocity.Y > 0)
                            Velocity.Y -= FRICTION_FORCE * deltaTime;
                        else
                            Velocity.Y += FRICTION_FORCE * deltaTime;
                    }
                    else
                        Velocity.Y = 0;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IntegrateVelocity(float x, float y, float deltaTime) => IntegrateVelocity(new Vector2(x, y), deltaTime);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IntegrateVelocity(Vector2 gravity, float deltaTime)
        {
            if (this.IsMovable)
            {
                Box.Position += Velocity * deltaTime;
                IntegrateForces(gravity, deltaTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyImpulse(float x, float y) => ApplyImpulse(new Vector2(x, y));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyImpulse(Vector2 impulse)
        {
            Velocity += impulse * InverseMass;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplySpeedX(float speed)
        {
            WasAppliedSpeedX = true;
            Speed.X = speed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplySpeedY(float speed)
        {
            WasAppliedSpeedY = true;
            Speed.Y = speed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyForce(float x, float y) => ApplyForce(new Vector2(x, y));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyForce(Vector2 force)
        {
            Force += force;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearForces()
        {
            Force = Vector2.Zero;

            WasAppliedSpeedX = false;
            WasAppliedSpeedY = false;
        }

        internal void ResetVerticalSpeed()
        {
            if (Velocity.Y < 0 || !IsKinematic)
            {
                Velocity.Y = 0;
                WasAppliedSpeedY = false;
            }
        }
    }
}
