using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using Unexplored.Game.Objects.Base;

namespace Unexplored.Game.Objects
{
    public enum HeroState
    {
        Idle,
        Walk,
        Landing,
        Shoot,
        Hit,
        Jumping,
        Fall,
    }

    public class HeroObject : RigidbodyGameObject
    {
        enum HorizontalView
        {
            Left,
            Right
        }

        private const double DefaultAnimationDuration = 200;
        private const double SpeedAnimationDuration = 100;
        private const double SlowAnimationDuration = 50;
        
        private bool wasMove, isJumping, isJumpingTwo;
        private bool lockDirectionLeft, lockDirectionRight;
        private HorizontalView direction;
        private HeroState heroState;
        private OneTileAnimation currentAnimation;
        private Dictionary<HeroState, OneTileAnimation> animations = new Dictionary<HeroState, OneTileAnimation>
        {
            [HeroState.Idle] = new OneTileAnimation(DefaultAnimationDuration, false, 0, 1, 2),
            [HeroState.Walk] = new OneTileAnimation(SpeedAnimationDuration, false, 26, 27, 28, 29, 30, 31, 32, 33),
            [HeroState.Landing] = new OneTileAnimation(DefaultAnimationDuration, true, 3, 4, 5, 6, 7),
            [HeroState.Shoot] = new OneTileAnimation(SpeedAnimationDuration, true, 56, 57, 58, 59),
            [HeroState.Hit] = new OneTileAnimation(SpeedAnimationDuration, true, 52, 53, 54, 55),
            [HeroState.Jumping] = new OneTileAnimation(SpeedAnimationDuration, true, 26, 27, 0),
            [HeroState.Fall] = new OneTileAnimation(SpeedAnimationDuration, true, 28),
        };

        public HeroObject()
            : base()
        {
            color = Color.White;
            isJumping = isJumpingTwo = wasMove = false;
            direction = HorizontalView.Right;
            currentAnimation = animations[heroState = HeroState.Idle];

            Rigidbody.OffsetMin = new Vector2(2, 0);
            Rigidbody.OffsetMax = new Vector2(2, 0);
        }

        public void MoveLeft()
        {
            Move(HorizontalView.Left, -Vector2.UnitX);
        }

        public void MoveRight()
        {
            Move(HorizontalView.Right, Vector2.UnitX);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Move(HorizontalView direction, Vector2 speed)
        {
            this.direction = direction;
            Rigidbody.Velocity += speed;
            if (heroState == HeroState.Idle)
                SetAnimation(HeroState.Walk);
            wasMove = true;
        }

        public void SetAnimation(HeroState state)
        {
            if (heroState == state)
                return;
            this.heroState = state;
            currentAnimation.Reset();
            currentAnimation = animations[state];
        }

        public override void Update(GameTime gameTime)
        {
            currentAnimation.Update(gameTime);

            Color wantColor = Color.White;
            if (heroState == HeroState.Idle || heroState == HeroState.Walk)
            {
                if (!Rigidbody.OnGround)
                    SetAnimation(HeroState.Fall);
            }
            
            if (heroState == HeroState.Walk)
            {
                if (currentAnimation.Completed && !wasMove)
                {
                    SetAnimation(HeroState.Idle);
                }
            }
            else if (heroState == HeroState.Jumping)
            {
                if (Rigidbody.Velocity.Y > 0 || Rigidbody.OnGround)
                    SetAnimation(HeroState.Fall);
            }
            else if (heroState == HeroState.Fall)
            {
                if (Rigidbody.OnGround)
                {
                    isJumping = isJumpingTwo = false;
                    SetAnimation(HeroState.Landing);
                }
            }
            else if (heroState == HeroState.Landing && currentAnimation.Completed)
            {
                SetAnimation(HeroState.Idle);
            }

            wasMove = false;

            base.Update(gameTime);
        }

        public void UnlockLeft()
        {
            lockDirectionLeft = false;
        }

        public void UnlockRight()
        {
            lockDirectionRight = false;
        }

        public void Jump()
        {
            float velocity = 0;
            if (Rigidbody.OnGround || (isJumping && !isJumpingTwo))
            {
                SetAnimation(HeroState.Jumping);
                Rigidbody.OnGround = false;
                velocity = isJumping ? -0.7f : -1;
                isJumpingTwo = isJumping;
                isJumping = true;
                if (isJumpingTwo)
                {
                    if (Rigidbody.LeftWall && !lockDirectionLeft)
                    {
                        velocity = -1;
                        lockDirectionLeft = true;
                        isJumpingTwo = false;
                    }
                    else if (Rigidbody.RightWall && !lockDirectionRight)
                    {
                        velocity = -1;
                        lockDirectionRight = true;
                        isJumpingTwo = false;
                    }
                }
            }
            /*else if (lockDirectionLeft)
            {
                if (Rigidbody.RightWall)
                {
                    velocity = -1;
                    MoveLeft();
                    lockDirectionLeft = false;
                    lockDirectionRight = true;
                }
            }
            else if (lockDirectionRight)
            {
                if (Rigidbody.LeftWall)
                {
                    velocity = -1;
                    MoveRight();
                    lockDirectionRight = false;
                    lockDirectionLeft = true;
                }
            }*/

            if (velocity != 0)
                Rigidbody.Velocity.Y = velocity * 16 * 12;
        }

        public void Stop()
        {
            Rigidbody.Velocity = Vector2.Zero;
            SetAnimation(HeroState.Idle);
            wasMove = false;
        }

        public override void Draw()
        {
            DrawSprite(currentAnimation.CurrentTileX, currentAnimation.CurrentTileY, color,
                Transform.Position, direction == HorizontalView.Right ? true : false);
        }
    }
}
