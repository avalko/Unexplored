using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;

namespace Unexplored.Core
{
    public static class Physics2D
    {
        //private static List<ColliderComponent> colliders;
        private static ColliderComponent[] colliders;
        private static int collidersCount;

        static Physics2D()
        {
            //colliders = new List<ColliderComponent>();
            colliders = new ColliderComponent[65536];
            collidersCount = 0;
        }

        public static void Add(ColliderComponent collider)
        {
            //colliders.Add(collider);
            colliders[collidersCount++] = collider;
        }

        public static void Remove(ColliderComponent collider)
        {
            //colliders.Remove(collider);
        }

        public static void Update()
        {

        }

        public static bool Check(GameTime gameTime, ColliderComponent colliderComponent)
        {
            var index = collidersCount;
            Collision collision = new Collision();

            while (--index >= 0)
            {
                var otherCollider = colliders[index];
                if (colliderComponent == otherCollider)
                    continue;

                if (CollisionDetector.Check(colliderComponent.Collider.Offset, otherCollider.Collider.Offset, ref collision))
                {
                    collision.OtherCollider = otherCollider;
                    colliderComponent.NotifyCollision(gameTime, collision);

                    if (otherCollider.IsTriggired)
                    {
                        collision.OtherCollider = colliderComponent;
                        collision.Normal = -collision.Normal;
                        otherCollider.NotifyCollision(gameTime, collision);
                    }
                }
            }

            return false;
        }

        internal static void Clear()
        {
            //colliders.Clear();
            collidersCount = 0;
        }
    }
}
