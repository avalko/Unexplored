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
using Unexplored.Core.Types;

namespace Unexplored.Core.Components
{
    public class RigidbodyComponent : BehaviorComponent, ICollider
    {
        private Rigidbody rigidbody;
        private ColliderComponent collider;
        private bool isKinematic;

        public AABB Box;

        public FRect Bounds => new FRect(Transform.Position, Transform.Size);
        public AABB AABB => Box;
        public Rigidbody Rigidbody => rigidbody;
        public bool IsTrigger => false;
        public bool IsCollision => true;
        public bool ResolveToCollide => Enabled;
        public GameObject OwnGameObject => GameObject;

        private static int _index;
        private int index;

        public RigidbodyComponent(bool isKinematic, bool forceMovable = false)
        {
            index = _index++;
            this.isKinematic = isKinematic;
            rigidbody = new Rigidbody(isKinematic, forceMovable);
        }

        public override void Initialize()
        {
            collider = GetComponent<ColliderComponent>();
            collider.Box.UpdateBounds();
            rigidbody.Box = Box = collider.Box;
        }

        public void SetKinematic(bool isKinematic)
        {
            this.isKinematic = isKinematic;
            rigidbody.IsKinematic = isKinematic;
        }

        public override void Update(GameTime gameTime)
        {
            Transform.Position = Rigidbody.Box.Position;
        }

        public override void Draw()
        {
            return;
            //Vector2 position = Transform.Position * Constants.ScaleFactor;
            //spriteBatch.DrawBoxedString(StaticResources.FontBase, $"({GameObject.ToString()}) Velocity: {Rigidbody.Velocity}", SceneManager.Camera.ToWorld(new Vector2(100, 50 + index * 40 - 20)));
            //spriteBatch.DrawBoxedString(StaticResources.FontBase, $"{new string(' ', 2*(GameObject.ToString().Length + 2))} Position: {Transform.Position}", SceneManager.Camera.ToWorld(new Vector2(100, 50 + index * 40)));
            //spriteBatch.DrawBoxedString(StaticResources.FontBase, $"{new string(' ', 2*(GameObject.ToString().Length + 2))} Position: {collider.Box.Velocity}", SceneManager.Camera.ToWorld(new Vector2(100, 50 + index * 40)));
        }
    }
}
