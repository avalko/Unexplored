using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Types;

namespace Unexplored.Core.Base
{
    public class GameScene : IGameScene
    {
        protected IGameCamera camera;
        protected SpriteBatch spriteBatch;
        protected GameObject[] gameObjects;
        protected int gameObjectsCount;
        protected bool disposed;

        public virtual void Initialize(SpriteBatch spriteBatch)
        {
            OnCameraChanged();
            Observer.Subscribe("SceneCamera_Changed", OnCameraChanged);

            this.spriteBatch = spriteBatch;
            gameObjectsCount = gameObjects?.Length ?? 0;
            InitializeObject(spriteBatch);
        }

        private void OnCameraChanged()
        {
            camera = SceneManager.Camera;
        }

        public void InitializeObject(SpriteBatch spriteBatch)
        {
            int objectIndex = this.gameObjectsCount;
            while (--objectIndex >= 0)
            {
                gameObjects[objectIndex].Initialize(spriteBatch);
            }

            Physics2D.Initialize();
        }

        double timeout = 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Update(GameTime gameTime)
        {
            Physics2D.Update();

            int objectIndex = this.gameObjectsCount;
            while (--objectIndex >= 0)
            {
                gameObjects[objectIndex].Update(gameTime);
            }

            timeout += gameTime.ElapsedGameTime.TotalMilliseconds;
            Physics2D.Tick(gameTime);
            /*objectIndex = this.gameObjectsCount;
            while (--objectIndex >= 0)
            {
                gameObjects[objectIndex].AfterUpdate(gameTime);
            }*/
        }

        public virtual void Reset()
        {
            disposed = true;
            Physics2D.Clear();

            int objectIndex = this.gameObjectsCount;
            while (--objectIndex >= 0)
            {
                gameObjects[objectIndex].Reset();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InBounds(Vector2 position, Vector2 size)
        {
            var realPos = SceneManager.Camera.ToScreen(position * Constants.ScaleFactor);
            return SceneManager.Camera.InBounds(new FRect(realPos, size * Constants.ScaleFactor));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InBounds(Vector2 position, float radius)
        {
            Vector2 r = new Vector2(radius);
            position += r;
            var realPos = SceneManager.Camera.ToScreen(position * Constants.ScaleFactor);
            return SceneManager.Camera.InBounds(new FRect(realPos, 2 * r * Constants.ScaleFactor));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InBounds(Vector2 position, float radius, out Vector2 realPosition)
        {
            Vector2 r = new Vector2(radius * Constants.ScaleFactor);
            realPosition = SceneManager.Camera.ToScreen(position * Constants.ScaleFactor);
            return SceneManager.Camera.InBounds(new FRect(realPosition - r, 2 * r));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Draw()
        {
            int objectIndex = this.gameObjectsCount;
            while (--objectIndex >= 0)
            {
                if (InBounds(gameObjects[objectIndex].Transform.Position, gameObjects[objectIndex].Transform.Size))
                    gameObjects[objectIndex].Draw();
            }
        }
    }
}
