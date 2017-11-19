using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;

namespace Unexplored.Core.Components
{
    public class ColliderComponent : BehaviorComponent
    {
        public bool IsTriggired;
        public Collider Collider;

        public ColliderComponent(bool isTriggired, Collider collider)
        {
            Collider = collider;
            IsTriggired = isTriggired;
        }

        public override void Update(GameTime gameTime)
        {
            Collider.Offset.Update(GameObject.Transform);
            if (IsTriggired)
            {
                Physics2D.Check(gameTime, this);
            }
        }

        public void NotifyCollision(GameTime gameTime, Collision collision)
        {
            GameObject.OnCollision(gameTime, collision);
        }
    }
}
