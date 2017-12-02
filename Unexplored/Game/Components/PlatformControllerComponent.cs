using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Core.Components;
using Unexplored.Core;
using Unexplored.Game.Structures;
using Unexplored.Game.GameObjects;
using Unexplored.Game.Attributes;
using System;
using Unexplored.Core.Attributes;
using Unexplored.Android.Core.Types;

namespace Unexplored.Game.Components
{
    public enum PlatformMovementType
    {
        None,
        Horizontal,
        Vertical
    }

    class PlatformControllerComponent : BehaviorComponent
    {
        const float SPEED = 90;

        RigidbodyComponent rigidbody;
        ColliderComponent collider;
        PlatformMovementType platformType;
        bool waitCooldown;
        double cooldownTimeout;
        bool fromBiggerThanTo;

        [CustomProperty]
        public float FromX, ToX;
        [CustomProperty]
        public float FromY, ToY;
        [CustomProperty]
        public float PeriodicallyCooldown;
        [CustomProperty]
        public bool InverseDirection;
        [CustomProperty]
        public bool NotInfluential;
        [CustomProperty]
        public bool IsActive;

        private float minPosition, maxPosition;
        private bool currentDirection;

        private ObjectStateComponent parentState;

        public override void Initialize()
        {
            base.Initialize();

            rigidbody = GetComponent<RigidbodyComponent>();
            collider = GetComponent<ColliderComponent>();

            // Only for Tiles maps!
            FromY -= 16;
            ToY -= 16;

            if (Math.Abs(FromX - ToX) >= 0.01f)
            {
                if (fromBiggerThanTo = FromX > ToX)
                {
                    minPosition = ToX;
                    maxPosition = FromX;
                }
                else
                {
                    minPosition = FromX;
                    maxPosition = ToX;
                }
                platformType = PlatformMovementType.Horizontal;
            }
            else if (Math.Abs(FromY - ToY) >= 0.01f)
            {
                if (fromBiggerThanTo = FromY > ToY)
                {
                    minPosition = ToY;
                    maxPosition = FromY;
                }
                else
                {
                    minPosition = FromY;
                    maxPosition = ToY;
                }
                platformType = PlatformMovementType.Vertical;
            }
            // else: None
            else
            {
                Enabled = false;
            }
        }

        public override void OnEventBegin(GameEvent gameEvent)
        {
            if (gameEvent.GameObject.GetComponent<ObjectStateComponent>() is var state)
                parentState = state;
        }

        public override void Update(GameTime gameTime)
        {
            if (parentState != null)
                IsActive = parentState.State;

            if (!IsActive)
                return;

            currentDirection = (fromBiggerThanTo && !InverseDirection || (!fromBiggerThanTo && InverseDirection));
            if ((platformType == PlatformMovementType.Horizontal &&
                 ((currentDirection && Transform.Position.X < minPosition) || (!currentDirection && Transform.Position.X > maxPosition))) ||
                (platformType == PlatformMovementType.Vertical &&
                 ((currentDirection && Transform.Position.Y < minPosition) || (!currentDirection && Transform.Position.Y > maxPosition))))
            {
                InverseDirection = !InverseDirection;
                waitCooldown = true;
            }

            if (waitCooldown)
            {
                cooldownTimeout += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (cooldownTimeout >= PeriodicallyCooldown)
                {
                    cooldownTimeout = 0;
                    waitCooldown = false;
                }
                else
                    return;
            }

            float speed = currentDirection ? -SPEED : SPEED;
            if (platformType == PlatformMovementType.Horizontal)
                rigidbody.Rigidbody.ApplySpeedX(speed);
            else
                rigidbody.Rigidbody.ApplySpeedY(speed);

            rigidbody.Rigidbody.RemoveChild();
        }

        public override void OnCollision(Collision collision)
        {
            if (!IsActive || NotInfluential)
                return;

            if (collision.GameObject is HeroObject hero)
            {
                if (collision.Manifold.Normal == -Vector2.UnitY)
                {
                    var r = hero.GetComponent<RigidbodyComponent>();
                    rigidbody.Rigidbody.Child = r.Rigidbody;
                    //if (platformType == PlatformMovementType.Vertical)
                    {
                        /*if (fromBiggerThanTo && !InverseDirection || (!fromBiggerThanTo && InverseDirection))
                            collider.Collider.Offset = new ColliderOffset(new Vector2(0, waitCooldown ? 0 : -1.25f), new Vector2(0, 11));
                        else
                            collider.Collider.Offset = new ColliderOffset(new Vector2(0, waitCooldown ? 0 : 1f), new Vector2(0, 11));*/
                    }
                }
            }
        }
    }
}
