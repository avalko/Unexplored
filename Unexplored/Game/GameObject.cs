using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Unexplored.Game.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Attributes;
using Unexplored.Game.Structures;

namespace Unexplored.Game
{
    public class GameObject
    {
        protected const float SpeedFactor = 16;
        protected SpriteBatch spriteBatch;

        public string Type;

        public Transform Transform;
        public Rigidbody Rigidbody;

        [CustomProperty]
        public bool IsRigidbody;
        [CustomProperty]
        public bool IsMovable;
        [CustomProperty]
        public float Speed;

        public GameObject()
        {
            Transform = new Transform(Vector2.Zero, Vector2.Zero);
            Rigidbody = new Rigidbody(Transform);
        }

        public virtual void Initialized()
        {
        }

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public bool CheckCollision(GameObject otherGameObject, out GameObjectCollision collision)
        {
            collision = new GameObjectCollision(this, otherGameObject);
            if (IsRigidbody && collision.Check())
            {
                OnCollision(otherGameObject);
                return true;
            }
            return false;
        }

        public void CheckCollision(Box box)
        {
            if (IsRigidbody && BoxCollision.Check(Transform.Box, box, out BoxCollision collision))
            {
                OnBoxColliderCollision(collision);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsRigidbody)
            {
                Rigidbody.ApplyGravity(gameTime);
                Rigidbody.ComputeVelocity(gameTime, Speed * SpeedFactor);

                Rigidbody.OnGround = false;
                Rigidbody.LeftWall = false;
                Rigidbody.RightWall = false;
            }
        }

        protected virtual void OnCollision(GameObject attacker)
        {
        }

        protected virtual void OnBoxColliderCollision(BoxCollision boxCollision)
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

        public virtual void UpdateEnd(GameTime gameTime)
        {
            if (IsRigidbody && IsMovable)
                Rigidbody.ApplyVelocity();
        }

        public virtual void Draw()
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void DrawSprite(int tileX, int tileY, Color color, Vector2 position, bool flipped)
        {
            int scaledSize = (int)(Tile.Size * Constants.ScaleFactor);
            position *= Constants.ScaleFactor;
            spriteBatch.Draw(StaticResources.Tileset,
                new Rectangle((int)(position.X), (int)(position.Y), scaledSize, scaledSize),
                new Rectangle(Tile.Size * tileX, Tile.Size * tileY, Tile.Size, Tile.Size), color, 0, Vector2.Zero, flipped ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}
