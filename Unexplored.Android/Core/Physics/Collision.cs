using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Core.Components;

namespace Unexplored.Core.Physics
{
    public struct Collision
    {
        public Rigidbody OtherRigidbody;
        public GameObject GameObject;
        public Manifold Manifold;
    }
}
