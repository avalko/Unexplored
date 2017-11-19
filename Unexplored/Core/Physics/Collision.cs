using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Components;

namespace Unexplored.Core.Physics
{
    public struct Collision
    {
        /// <summary>
        /// Глубина проникновения.
        /// </summary>
        public float Penetration;

        /// <summary>
        /// Вектор проникновения.
        /// </summary>
        public Vector2 Normal;
        
        public Vector2 Direction;

        public ColliderComponent OtherCollider;

        public Collision Copy(Vector2 normal, float value)
        {
            Collision collision = new Collision { OtherCollider = OtherCollider };
            collision.Normal = normal;
            collision.Penetration = value;
            collision.Direction = new Vector2(Math.Abs(normal.X), Math.Abs(normal.Y));
            return collision;
        }

        public Collision Copy(ColliderComponent collider)
        {
            Collision collision = new Collision
            {
                OtherCollider = collider,
                Direction = Direction,
                Normal = Normal,
                Penetration = Penetration
            };
            return collision;
        }
    }
}
