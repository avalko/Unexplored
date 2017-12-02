using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Core.Types;

namespace Unexplored.Core.Components
{
    public class ColliderComponent : BehaviorComponent, ICollider
    {
        public bool IsTriggired;
        public bool IsInter;
        public bool ForceNotify;
        public bool IsRigidbody;

        public AABB Box;
        public MapCollider Collider;

        public FRect Bounds => new FRect(Transform.Position, Transform.Size);
        public AABB AABB => Box;
        public Rigidbody Rigidbody { get; private set; }
        public bool IsTrigger => true;
        public bool IsCollision => false;
        public bool ResolveToCollide => Enabled && IsTriggired;
        public GameObject OwnGameObject => GameObject;

        public ColliderComponent(bool isTriggired, MapCollider collider)
        {
            Collider = collider;
            IsTriggired = isTriggired;
        }

        public override void SetGameObject(GameObject gameObject)
        {
            base.SetGameObject(gameObject);
        }

        public override void SingleInitialize()
        {
            Box = new AABB(Transform.Position, Transform.Size, Collider.OffsetMin, Collider.OffsetMax);
            Box.UpdateBounds();
        }

        public override void Initialize()
        {
            if (GetComponent<RigidbodyComponent>() is RigidbodyComponent rigidbody)
            {
                Rigidbody = rigidbody.Rigidbody;
            }
            else
            {
                Rigidbody = new Rigidbody(false, false);
                Rigidbody.Box = Box;
            }
        }

        public void NotifyCollision(Collision collision)
        {
            GameObject.OnCollision(collision);
        }

        public override void Draw()
        {
            return;
            Vector2 min = Box.Min * Constants.ScaleFactor;
            Vector2 max = Box.Max * Constants.ScaleFactor;
            Vector2 size = max - min;
            spriteBatch.DrawRect(min, (IsInter ? Color.Red : Color.Black) * 0.5f, (int)size.X, (int)size.Y);
            IsInter = false;
        }
    }
}
