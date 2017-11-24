﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Core.Components;
using Unexplored.Core;
using Unexplored.Game.Structures;
using Unexplored.Game.GameObjects;
using Unexplored.Game.Particles;

namespace Unexplored.Game.Components
{
    public class HeroControllerComponent : BehaviorComponent
    {
        #region Enums
        public enum HeroJumpingState
        {
            None,
            DefaultJump,
            SmallJump,
            LeftWallJump,
            SecondLeftWallJump,
            RightWallJump,
            SecondRightWallJump,
        }

        public enum HeroState
        {
            None,
            Idle,
            Walk,
            Landing,
            Shoot,
            Attack,
            Jumping,
            Fall,
        }
        public enum HorizontalView
        {
            Left,
            Right
        }
        #endregion

        private const double AnimationDurationDefault = 150;
        private const double AnimationDurationX1 = 50;
        private const double AnimationDurationX2 = 25;

        const float Speed = Tile.Size * 10.5f;
        const float FlySpeed = Tile.Size * 8.5f;

        InputComponent heroInput;
        SpriteRendererComponent renderer;
        SpriteAnimatorComponent animator;
        RigidbodyComponent rigidbody;
        ColliderComponent collider;

        public bool IsGrounded;
        public bool IsLeftWall;
        public bool IsRightWall;
        public bool IsAttacked;
        public bool AllowAttack;
        public double FallTimeout;

        private HeroJumpingState jumpingState;
        private HorizontalView direction;
        private HeroState heroState;
        private Vector2 inititalPosition;

        private BaseScene currentScene;

        private readonly Dictionary<HeroState, SpriteAnimation> animations = new Dictionary<HeroState, SpriteAnimation>
        {
            [HeroState.Idle] = new SpriteAnimation(AnimationDurationDefault, false, 0, 1, 2),
            [HeroState.Walk] = new SpriteAnimation(AnimationDurationX1, false, 26, 27, 28, 29, 30, 31, 32, 33),
            [HeroState.Landing] = new SpriteAnimation(AnimationDurationX2, true, 3, 4, 5, 6, 7),
            [HeroState.Shoot] = new SpriteAnimation(AnimationDurationX1, true, 56, 57, 58, 59),
            [HeroState.Attack] = new SpriteAnimation(AnimationDurationX1, true, 52, 53, 54, 55),
            [HeroState.Jumping] = new SpriteAnimation(AnimationDurationX1, true, 26, 27),
            [HeroState.Fall] = new SpriteAnimation(AnimationDurationX1, true, 28),
        };

        public override void Initialize()
        {
            renderer = GetComponent<SpriteRendererComponent>();
            animator = GetComponent<SpriteAnimatorComponent>();
            rigidbody = GetComponent<RigidbodyComponent>();
            heroInput = GetComponent<InputComponent>();
            collider = GetComponent<ColliderComponent>();

            animator.Enabled = true;
            SetState(HeroState.Idle);
            inititalPosition = Transform.Position;
            direction = HorizontalView.Right;
        }

        public void SetScene(BaseScene scene)
        {
            currentScene = scene;
        }

        public override void Update(GameTime gameTime)
        {
            ProcessMove();
            ProcessStateTree(gameTime);

            ResetGrounded();
            IsAttacked = false;
        }

        private void ProcessStateTree(GameTime gameTime)
        {
            AllowAttack = true;

            // Если стоим или бежим...
            if (heroState == HeroState.Idle || heroState == HeroState.Walk)
            {
                // ...не на земле (WAT?)...
                if (!IsGrounded)
                    // ...падаем!
                    SetState(HeroState.Fall);
            }

            if (heroState == HeroState.Walk)
            {
                if (animator.FrameCompleted && !(heroInput.Left || heroInput.Right))
                {
                    SetState(HeroState.Idle);
                }
            }
            else if (heroState == HeroState.Jumping)
            {
                if (rigidbody.Rigidbody.Velocity.Y > 0 || IsGrounded)
                    SetState(HeroState.Fall);
            }
            else if (heroState == HeroState.Fall)
            {
                if (IsGrounded)
                {
                    jumpingState = HeroJumpingState.None;

                    if (FallTimeout < 100)
                        SetState(HeroState.Idle);
                    else
                        SetState(HeroState.Landing);

                    FallTimeout = 0;
                }
                else
                    FallTimeout += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if (animator.Completed)
            {
                if (heroState == HeroState.Landing || heroState == HeroState.Attack)
                    SetState(HeroState.Idle);
            }

            if (heroState == HeroState.Landing)
                AllowAttack = false;
        }

        private void ProcessMove()
        {
            float speed = heroInput.Bottom || !IsGrounded ? FlySpeed : Speed;

            if (heroInput.Left)
                Move(-speed);
            if (heroInput.Right)
                Move(speed);
            if (heroInput.Jump)
                Jump();
            else if (heroInput.Bottom)
                rigidbody.Rigidbody.ResetVerticalSpeed();
            IsAttacked = heroInput.Attack;
            if (AllowAttack && IsAttacked)
                Attack();
        }

        private void Move(float speed)
        {
            collider.Rigidbody.ApplySpeedX(speed);

            if (speed > 0)
                direction = HorizontalView.Right;
            else
                direction = HorizontalView.Left;

            renderer.Flipped = direction == HorizontalView.Left;
            if (heroState == HeroState.Idle)
                SetState(HeroState.Walk);
        }

        private void Jump()
        {
            const float SmallJumpSpeed = 0.5f;
            const float DefaultJumpSpeed = 1.0f;
            float velocity = 0;

            if (IsGrounded)
            {
                IsGrounded = false;
                jumpingState = HeroJumpingState.DefaultJump;
                velocity = DefaultJumpSpeed;
            }
            else if (jumpingState == HeroJumpingState.DefaultJump)
            {
                if (IsLeftWall)
                {
                    jumpingState = HeroJumpingState.LeftWallJump;
                    velocity = DefaultJumpSpeed;
                }
                else if (IsRightWall)
                {
                    jumpingState = HeroJumpingState.RightWallJump;
                    velocity = DefaultJumpSpeed;
                }
                else
                {
                    jumpingState = HeroJumpingState.SmallJump;
                    velocity = SmallJumpSpeed;
                }
            }
            else if (jumpingState == HeroJumpingState.LeftWallJump)
            {
                if (IsRightWall)
                {
                    jumpingState = HeroJumpingState.RightWallJump;
                    velocity = DefaultJumpSpeed;
                }
                else
                {
                    jumpingState = HeroJumpingState.SmallJump;
                    velocity = SmallJumpSpeed;
                }
            }
            else if (jumpingState == HeroJumpingState.RightWallJump)
            {
                if (IsLeftWall)
                {
                    jumpingState = HeroJumpingState.LeftWallJump;
                    velocity = DefaultJumpSpeed;
                }
                else
                {
                    jumpingState = HeroJumpingState.SmallJump;
                    velocity = SmallJumpSpeed;
                }
            }

            if (velocity > 0)
            {
                SetState(HeroState.Jumping);
                collider.Rigidbody.Velocity.Y = -320 * velocity;
            }
        }

        private void Attack()
        {
            SetState(HeroState.Attack);
            ParticleRendererComponent.AddParticle(Transform.Position + new Vector2(direction == HorizontalView.Left ? -16 : 16, 0), new ParticleAttack(direction == HorizontalView.Left));
        }

        private void SetState(HeroState state)
        {
            if (heroState == state)
                return;

            heroState = state;
            animator.Reset();
            animator.SetAnimation(animations[state]);
        }

        private void ResetGrounded()
        {
            IsGrounded = false;
            IsLeftWall = false;
            IsRightWall = false;
        }

        public override void OnTriggerStay(Trigger trigger)
        {
            if (trigger.Type == null)
                return;

            if (heroInput.Attack)
            {
                trigger.GameObject.OnTriggerStay(new Trigger(trigger.Type, GameObject, trigger.Box));
            }
        }

        public override void OnTriggerEnter(Trigger trigger)
        {
            if (trigger.GameObject is SpikeObject)
            {
                currentScene.Blink();
                rigidbody.Box.Position = inititalPosition;
                return;
            }
        }

        public override void OnCollision(Collision collision)
        {
            if (collision.Manifold.Normal == -Vector2.UnitY)
            {
                IsGrounded = true;
            }
            else if (collision.Manifold.Normal == Vector2.UnitX)
            {
                if (rigidbody.Rigidbody.Velocity.X != 0)
                    IsRightWall = true;
            }
            else if (collision.Manifold.Normal == -Vector2.UnitX)
            {
                if (rigidbody.Rigidbody.Velocity.X != 0)
                    IsLeftWall = true;
            }
        }
    }
}