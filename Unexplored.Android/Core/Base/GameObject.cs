using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Android.Core.Types;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;
using Unexplored.Core.Types;

namespace Unexplored.Core.Base
{
    public class GameObject
    {
        class TriggerMap
        {
            public bool IsEntered;
            public Trigger Trigger;
        }

        public int Id;
        public string Tag;
        public Transform Transform;
        public bool Drawable;

        public BehaviorComponent[] Components => components;

        private Dictionary<string, MethodInfo> methods;
        private BehaviorComponent[] components;
        private int componentsCount;

        private List<TriggerMap> triggerMaps;
        private Dictionary<GameObject, TriggerMap> triggersDict;

        public bool Enabled = true;

        public GameObject()
        {
            Drawable = true;
            Transform = new Transform(Vector2.Zero, Vector2.Zero);
            triggerMaps = new List<TriggerMap>();
            triggersDict = new Dictionary<GameObject, TriggerMap>();
            /*methods = this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ToDictionary(x => x.Name, y => y);*/
        }


        public void PreventInitialization()
        {
            componentsCount = components?.Length ?? 0;
            if (componentsCount > 0)
            {
                foreach (var component in components)
                {
                    component.SetGameObject(this);
                }
            }
        }

        public void SingleInitialize()
        {
            componentsCount = components?.Length ?? 0;
            if (componentsCount > 0)
            {
                foreach (var component in components)
                {
                    component.SingleInitialize();
                }
            }
        }

        public void Initialize(SpriteBatch spriteBatch)
        {
            componentsCount = components?.Length ?? 0;
            if (componentsCount > 0)
            {
                foreach (var component in components)
                {
                    component.SetSpriteBatch(spriteBatch);
                    component.Initialize();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponent<T>() where T : BehaviorComponent
        {
            return (T)components?.FirstOrDefault(x => x is T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponent<T>(int index) where T : BehaviorComponent
        {
            int i = 0;
            return (T)components?.FirstOrDefault(x => x is T && i++ == index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] GetComponents<T>() where T : BehaviorComponent
        {
            return (T[])components?.Where(x => x is T).Select(x => (T)x).ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponentByInterface<T>() where T : IBehaviorComponent
        {
            return (dynamic)components?.SingleOrDefault(x => x is T);
        }

        public void Update(GameTime gameTime)
        {
            int triggerIndex = triggerMaps.Count;
            while (--triggerIndex >= 0)
            {
                var trigger = triggerMaps[triggerIndex];
                if (!trigger.IsEntered)
                {
                    OnTriggerExit(trigger.Trigger);
                    triggerMaps.Remove(trigger);
                    triggersDict.Remove(trigger.Trigger.GameObject);
                }
                else
                    trigger.IsEntered = false;
            }

            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].Update(gameTime);
            }
        }

        public void Draw()
        {
            if (!Drawable)
                return;

            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].Draw();
            }
        }

        public virtual void Reset()
        {
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                components[componentIndex].Reset();
            }
        }

        public void SetComponents(params BehaviorComponent[] components)
        {
            this.components = components;
        }

        public void OnCollision(Collision collision)
        {
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].OnCollision(collision);
            }
        }

        public void OnTriggerEnter(Trigger trigger, bool forcePush = false)
        {
            if (!forcePush)
            {
                if (triggersDict.ContainsKey(trigger.GameObject))
                {
                    OnTriggerStay(trigger);
                    return;
                }
            }

            triggersDict[trigger.GameObject] = new TriggerMap { Trigger = trigger, IsEntered = true };
            triggerMaps.Add(triggersDict[trigger.GameObject]);

            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].OnTriggerEnter(trigger);
            }
        }

        public void OnTriggerStay(Trigger trigger)
        {
            triggersDict[trigger.GameObject].IsEntered = true;
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].OnTriggerStay(trigger);
            }
        }
        
        public void OnTriggerExit(Trigger trigger)
        {
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].OnTriggerExit(trigger);
            }
        }

        public void OnEventBegin(GameEvent gameEvent)
        {
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].OnEventBegin(gameEvent);
            }
        }

        public void OnEventStay(GameEvent gameEvent)
        {
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].OnEventStay(gameEvent);
            }
        }

        public void OnEventEnd(GameEvent gameEvent)
        {
            int componentIndex = this.componentsCount;
            while (--componentIndex >= 0)
            {
                if (components[componentIndex].Enabled)
                    components[componentIndex].OnEventEnd(gameEvent);
            }
        }

        public void SendMessage(string method, params object[] arguments)
        {
            if (methods.ContainsKey(method))
                methods[method].Invoke(this, arguments);
        }

        public static GameObject Create(string tag, bool drawable, Transform transform, params BehaviorComponent[] components)
        {
            return new GameObject
            {
                Drawable = drawable,
                Tag = tag,
                Transform = transform,
                components = components
            };
        }
    }
}
