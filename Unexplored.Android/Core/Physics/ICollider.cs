using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;

namespace Unexplored.Core.Physics
{
    public interface ICollider : IQuadItem
    {
        bool ResolveToCollide { get; }
        bool IsTrigger { get; }
        bool IsCollision { get; }
        AABB AABB { get; }
        Rigidbody Rigidbody { get; }
        GameObject OwnGameObject { get; }
    }
}
