using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Physics
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
    }
}
