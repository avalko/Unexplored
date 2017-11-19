using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Base
{
    public class GameScene : IGameScene
    {
        protected IGameCamera camera;
        protected SpriteBatch spriteBatch;
        protected GameObject[] gameObjects;
        protected int gameObjectsCount;

        public IGameCamera GetCamera()
        {
            foreach (var obj in gameObjects)
            {
                if (obj.GetComponentByInterface<IGameCamera>() is IGameCamera camera)
                {
                    camera?.Update(new GameTime());
                    return camera;
                }
            }

            return null;
        }

        public virtual void LoadContent(ContentManager content)
        {
            OnCameraChanged();
            Observer.Subscribe("SceneCamera_Changed", OnCameraChanged);
        }

        private void OnCameraChanged()
        {
            camera = SceneManager.Camera;
        }

        public virtual void Initialize(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            gameObjectsCount = gameObjects?.Length ?? 0;
            InitializeObject(spriteBatch);
        }

        public void InitializeObject(SpriteBatch spriteBatch)
        {
            int objectIndex = this.gameObjectsCount;
            while (--objectIndex >= 0)
            {
                gameObjects[objectIndex].Initialize(spriteBatch);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            int objectIndex = this.gameObjectsCount;
            while (--objectIndex >= 0)
            {
                gameObjects[objectIndex].Update(gameTime);
            }
        }

        public virtual void Draw()
        {
            int objectIndex = this.gameObjectsCount;
            while (--objectIndex >= 0)
            {
                gameObjects[objectIndex].Draw();
            }
        }
    }
}
