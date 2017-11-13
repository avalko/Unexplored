using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Unexplored.Core.Components.Logic
{
    public abstract class CoreComponent : ICoreComponent
    {
        public bool Enabled { get; set; }
        public bool DrawEnabled { get; set; }

        protected MainGame game;
        protected SpriteBatch spriteBatch;
        protected InputComponent input;
        protected Camera2DComponent camera;
        protected ComponentManager components;
        protected ContentManager content;
        protected GraphicsDevice GraphicsDevice;
        protected Observer observer;

        public CoreComponent(MainGame game, bool drawable = true, string objectName = null)
        {
            Enabled = true;
            DrawEnabled = drawable;
            
            if (objectName == null)
                objectName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().ReflectedType.Name;
            observer = new Observer(objectName);
            components = ComponentManager.Instance;
            content = game.Content;
            this.game = game;
        }

        public virtual void Initialize()
        {
            GraphicsDevice = game.GraphicsDevice;
        }

        public virtual void LoadContent()
        {
            input = components.Get<InputComponent>();
            camera = components.Get<Camera2DComponent>();
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(GameTime gameTime)
        {
        }
    }
}
