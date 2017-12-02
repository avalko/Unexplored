using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Attributes;
using Unexplored.Core.Components;
using Unexplored.Game.Structures;
using Unexplored.Core.Physics;
using Unexplored.Core;

namespace Unexplored.Game.Components
{
    public class EnemyControllerComponent : BehaviorComponent
    {
        public enum HorizontalView
        {
            Left,
            Right
        }

        public enum EnemyJumpingState
        {
            None,
            DefaultJumping,
            SmallJumping
        }

        public enum EnemyState
        {
            Idle,
            Running,
            Jumping,
            Fall,
        }

        private const double AnimationDurationDefault = 150;
        private const float SpeedFactor = Tile.Size;

        [CustomProperty]
        public float Speed = 10;
        [CustomProperty]
        public float WaitTimeout = 350;
        [CustomProperty]
        public bool DisableFall = false;

        SpriteRendererComponent renderer;
        SpriteAnimatorComponent animator;
        RigidbodyComponent rigidbody;
        ColliderComponent collider;

        HorizontalView direction;
        EnemyJumpingState jumpingState;
        EnemyState state;
        double currentBaseTimeout, currentFallTimeout, currentWaitChangeModeTimeout;
        bool waitChangeMode, waitFall;
        bool isSpeedMode;
        float preventJumpingPositionX;
        float minX = int.MinValue, maxX = int.MaxValue;

        public bool IsGrounded;
        public bool IsLeftWall;
        public bool IsRightWall;

        public override void Initialize()
        {
            renderer = GetComponent<SpriteRendererComponent>();
            animator = GetComponent<SpriteAnimatorComponent>();
            rigidbody = GetComponent<RigidbodyComponent>();
            collider =  GameObject.GetComponent<ColliderComponent>(1);

            animator.Enabled = true;
            animator.Reset();
            animator.SetAnimation(new SpriteAnimation(AnimationDurationDefault, false, 410, 411, 412, 413, 414));
            direction = HorizontalView.Right;

            state = EnemyState.Idle;
        }

        public void Shoot()
        {
            if (isSpeedMode)
            {
                GameObject.Enabled = false;
                Physics2D.RemoveCollider(collider);
                Physics2D.RemoveCollider(rigidbody);
                Physics2D.RemoveRigidbody(rigidbody.Rigidbody);
                return;
            }

            waitChangeMode = true;
            isSpeedMode = true;
            state = EnemyState.Idle;
            animator.SetAnimation(new SpriteAnimation(AnimationDurationDefault, false, 384, 385, 386, 387, 388));
            Speed *= 2;
        }

        public void ChangeDirection(HorizontalView direction)
        {
            this.direction = direction;
            renderer.Flipped = direction == HorizontalView.Left;
        }

        public void ChangeDirection()
        {
            if (direction == HorizontalView.Left)
                ChangeDirection(HorizontalView.Right);
            else
                ChangeDirection(HorizontalView.Left);
        }

        public override void Update(GameTime gameTime)
        {
            collider.Box.Position = GameObject.Transform.Position;
            collider.Box.UpdateBounds();

            if (waitChangeMode)
            {
                currentWaitChangeModeTimeout += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (currentWaitChangeModeTimeout >= WaitTimeout * 3)
                {
                    waitChangeMode = false;
                    currentWaitChangeModeTimeout = 0;
                }
                else return;
            }

            currentBaseTimeout += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (currentBaseTimeout >= WaitTimeout)
            {
                currentBaseTimeout = 0;
                ProcessState();
            }

            if (waitFall)
            {
                currentFallTimeout += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (currentFallTimeout >= 50)
                {
                    currentFallTimeout = 0;
                    waitFall = false;
                }
            }

            Tick();

            ResetGrounded();
        }

        private void Tick()
        {
            if (IsGrounded)
            {
                jumpingState = EnemyJumpingState.None;
            }

            if (state == EnemyState.Running)
            {
                if (direction == HorizontalView.Left && IsRightWall ||
                    direction == HorizontalView.Right && IsLeftWall)
                {
                    preventJumpingPositionX = rigidbody.Box.Position.X;
                    Jump();
                }
            }
            else if (state == EnemyState.Jumping)
            {
                if (jumpingState == EnemyJumpingState.DefaultJumping)
                {
                    if (rigidbody.Rigidbody.Velocity.Y > 0)
                        Jump();
                }
                else if (rigidbody.Rigidbody.Velocity.Y > 0)
                {
                    waitFall = true;
                    state = EnemyState.Fall;
                }
            }
            else if (state == EnemyState.Fall)
            {
                if (!waitFall && IsGrounded)
                {
                    if (Math.Abs(rigidbody.Box.Position.X - preventJumpingPositionX) < 10f)
                    {
                        ChangeDirection();
                    }

                    state = EnemyState.Idle;
                }
            }

            if (state != EnemyState.Idle)
                Move();
        }

        private void Move()
        {
            if (DisableFall)
            {
                if (direction == HorizontalView.Left && rigidbody.Box.Min.X < minX ||
                    direction == HorizontalView.Right && rigidbody.Box.Max.X > maxX)
                {
                    ChangeDirection();
                }
            }

            float speed = Speed * SpeedFactor * (direction == HorizontalView.Left ? -1 : 1);
            rigidbody.Rigidbody.ApplySpeedX(speed);
        }

        private void ProcessState()
        {
            if (state == EnemyState.Idle)
            {
                state = EnemyState.Running;
            }
            else if (state == EnemyState.Running && jumpingState == EnemyJumpingState.None)
            {
                state = EnemyState.Idle;
            }
        }

        private void ResetGrounded()
        {
            IsGrounded = false;
            IsRightWall = false;
            IsLeftWall = false;
        }

        private void Jump()
        {
            if (!IsGrounded)
            {
                return;
            }

            const float SmallJumpSpeed = 0.5f;
            const float DefaultJumpSpeed = 1.0f;
            float velocity = 0;

            if (jumpingState == EnemyJumpingState.None)
            {
                jumpingState = EnemyJumpingState.DefaultJumping;
                velocity = DefaultJumpSpeed;
            }
            else if (jumpingState == EnemyJumpingState.DefaultJumping)
            {
                jumpingState = EnemyJumpingState.SmallJumping;
                velocity = SmallJumpSpeed;
            }            

            if (velocity > 0)
            {
                state = EnemyState.Jumping;
                IsGrounded = false;
                collider.Rigidbody.Velocity.Y = -320 * velocity;
            }
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.Manifold.Normal == -Vector2.UnitY)
            {
                if (DisableFall)
                {
                    minX = collision.OtherRigidbody.Box.Min.X;
                    maxX = collision.OtherRigidbody.Box.Max.X;
                }
                IsGrounded = true;
            }
            else if (collision.Manifold.Normal == Vector2.UnitX)
            {
                if (rigidbody.Rigidbody.Speed.X != 0)
                    IsRightWall = true;
            }
            else if (collision.Manifold.Normal == -Vector2.UnitX)
            {
                if (rigidbody.Rigidbody.Speed.X != 0)
                    IsLeftWall = true;
            }
        }
    }
}
