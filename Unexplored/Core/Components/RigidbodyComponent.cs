using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using System.Runtime.CompilerServices;
using Unexplored.Game.Structures;
using Unexplored.Game;
using Unexplored.Game.Components;

namespace Unexplored.Core.Components
{
    public class RigidbodyComponent : BehaviorComponent
    {
        public const float FRICTION_FORCE = 10.0f;
        public const float GRAVITY_FORCE = Tile.Size * 50;

        private ColliderComponent collider;
        private float speed;
        private Vector2 acceleration;

        public Vector2 Velocity;

        public override void Initialize()
        {
            collider = GetComponent<ColliderComponent>();
            acceleration = new Vector2(0, GRAVITY_FORCE);
        }

        public void SetHorizontalSpeed(float speed)
        {
            this.speed = speed;
            Velocity.X = speed;
        }

        public void SetVerticalSpeed(float speed)
        {
            Velocity.Y = speed;
        }

        public void ResetGravity() => Velocity.Y = 0;
        public void ResetSlip() => Velocity.X = 0;

        public override void Update(GameTime gameTime)
        {
            acceleration = new Vector2(-Math.Sign(speed) * speed * Velocity.X * FRICTION_FORCE * (float)(gameTime.ElapsedGameTime.TotalSeconds * Constants.FrameScale), GRAVITY_FORCE);
            Velocity += DeltaAcceleration(gameTime);
            if (Math.Abs(Velocity.X) < 10)
                Velocity.X = 0;
            GameObject.Transform.Position += DeltaVelocity(gameTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 DeltaVelocity(GameTime gameTime)
        {
            return Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds * Constants.FrameScale);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 DeltaAcceleration(GameTime gameTime)
        {
            return acceleration * (float)(gameTime.ElapsedGameTime.TotalSeconds * Constants.FrameScale);
        }

        public override void OnCollision(GameTime gameTime, Collision collision)
        {
            if (collision.OtherCollider.Collider.Type == "collider")
                GameObject.Transform.Position -= collision.Normal * collision.Penetration;
            else //if (collision.Direction.X > 0)
            {
                GameObject.Transform.Position.X -= collision.Normal.X * collision.Penetration * 0.05F;
            }
        }

        public override void Draw()
        {
            Vector2 position = Transform.Position * Constants.ScaleFactor;
            //spriteBatch.DrawBoxedString(StaticResources.Font, $"Velocity: {Velocity}", SceneManager.Camera.ToWorld(new Vector2(100, 50 + (gameObject.Tag == "main" ? 50 : 0))));
        }
    }
}
