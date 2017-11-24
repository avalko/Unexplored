using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Attributes;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;
using Unexplored.Game.Attributes;

namespace Unexplored.Core.Base
{
    public class BehaviorComponent : IBehaviorComponent
    {
        [CustomProperty]
        public bool Enabled = true;
        public bool Drawable = true;
        public Transform Transform;
        public GameObject GameObject;

        protected SpriteBatch spriteBatch;

        public virtual void SetGameObject(GameObject gameObject)
        {
            this.GameObject = gameObject;

            Transform = gameObject.Transform;
        }

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T GetComponent<T>() where T : BehaviorComponent
        {
            return GameObject.GetComponent<T>();
        }

        public virtual void SingleInitialize()
        {
        }

        public virtual void Initialize()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void AfterUpdate(GameTime gameTime)
        {
        }

        public virtual void Draw()
        {
        }

        public virtual void OnCollision(Collision collision)
        {
        }

        public virtual void Reset()
        {
        }

        public virtual void OnTriggerEnter(Trigger trigger)
        {
        }

        public virtual void OnTriggerExit(Trigger trigger)
        {
        }

        public virtual void OnTriggerStay(Trigger trigger)
        {
        }
    }
}
