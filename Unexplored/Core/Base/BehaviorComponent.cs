using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;

namespace Unexplored.Core.Base
{
    public class BehaviorComponent : IBehaviorComponent
    {
        public bool Enabled = true;
        public Transform Transform;
        public GameObject GameObject;

        protected SpriteBatch spriteBatch;

        public void BeginInitialize(GameObject gameObject, SpriteBatch spriteBatch)
        {
            this.GameObject = gameObject;
            this.spriteBatch = spriteBatch;

            Transform = gameObject.Transform;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T GetComponent<T>() where T : BehaviorComponent
        {
            return GameObject.GetComponent<T>();
        }

        public virtual void Initialize()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw()
        {
        }

        public virtual void OnCollision(GameTime gameTime, Collision collision)
        {
        }
    }
}
