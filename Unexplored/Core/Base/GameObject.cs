using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;

namespace Unexplored.Core.Base
{
    public class GameObject
    {
        public string Tag;
        public Transform Transform;

        private Dictionary<string, MethodInfo> methods;
        private BehaviorComponent[] components;
        private int componentsCount;

        public GameObject()
        {
            Transform = new Transform(Vector2.Zero, Vector2.Zero);
            methods = this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToDictionary(x => x.Name, y => y);
        }

        public void Initialize(SpriteBatch spriteBatch)
        {
            componentsCount = components?.Length ?? 0;
            if (componentsCount > 0)
            {
                foreach (var component in components)
                {
                    component.BeginInitialize(this, spriteBatch);
                    component.Initialize();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponent<T>() where T : BehaviorComponent
        {
            return (T)components?.SingleOrDefault(x => x is T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponentByInterface<T>() where T : IBehaviorComponent
        {
            return (dynamic)components?.SingleOrDefault(x => x is T);
        }

        public void Update(GameTime gameTime)
        {
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].Update(gameTime);
            }
        }

        public void Draw()
        {
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].Draw();
            }
        }

        public void SetComponents(params BehaviorComponent[] components)
        {
            this.components = components;
        }

        public void OnCollision(GameTime gameTime, Collision collision)
        {
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].OnCollision(gameTime, collision);
            }
        }

        public void SendMessage(string method, params object[] arguments)
        {
            if (methods.ContainsKey(method))
                methods[method].Invoke(this, arguments);
        }

        public static GameObject Create(string tag, Transform transform, params BehaviorComponent[] components)
        {
            return new GameObject
            {
                Tag = tag,
                Transform = transform,
                components = components
            };
        }
    }
}
