using Unexplored.Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Components.Logic;

namespace Unexplored.Core
{
    public class ComponentManager
    {
        struct ComponentItem
        {
            public int Level;
            public ICoreComponent Component;
        }

        public static ComponentManager Instance { get; private set; }
        private ICoreComponent[] components;
        private int componentsCount;
        private MainGame game;

        public ComponentManager(MainGame game)
        {
            this.game = game;
            Instance = this;
        }

        public T Get<T>() where T : ICoreComponent
        {
            foreach (var component in components)
                if (component is T retComponent)
                    return retComponent;

            return default(T);
        }

        public void Initialize()
        {
            int index = componentsCount;
            while (index-- > 0)
                components[index].Initialize();
        }

        public void LoadContent()
        {
            int index = componentsCount;
            while (index-- > 0)
                components[index].LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            int index = componentsCount;
            while (index-- > 0)
            {
                if (components[index].Enabled)
                    components[index].Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            int index = componentsCount;
            while (index-- > 0)
            {
                if (components[index].DrawEnabled && components[index].Enabled)
                    components[index].Draw(gameTime);
            }
        }

        public void FindAllComponents()
        {
            var types = typeof(ComponentManager).Assembly.GetTypes();
            List<ICoreComponent> components = new List<ICoreComponent>();
            List<ComponentItem> componentCollection = new List<ComponentItem>();

            foreach (var type in types)
            {
                if (type.GetInterfaces().Contains(typeof(ICoreComponent)))
                {
                    var attributes = type.GetCustomAttributes(false);
                    foreach (var attr in attributes)
                    {
                        if (attr is GameComponentAttribute componentAttribute)
                        {
                            var component = (ICoreComponent)Activator.CreateInstance(type, new [] { game });
                            components.Add(component);
                            componentCollection.Add(new ComponentItem
                            {
                                Level = componentAttribute.OrderLevel,
                                Component = component
                            });
                        }
                    }
                }
            }

            this.components = (from pair in componentCollection
                                 orderby pair.Level
                                 select pair.Component).ToArray();
            componentsCount = components.Count;
        }
    }
}
