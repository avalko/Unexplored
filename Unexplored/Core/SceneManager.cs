using Unexplored.Core.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Unexplored.Core
{
    public class SceneManager
    {
        public static IGameCamera Camera { get; private set; }
        public static SceneManager Instance { get; private set; }

        private List<IGameScene> scenes;
        private IGameScene currentScene;

        public IGameScene CurrentScene
        {
            get => currentScene;
            set => currentScene = value;
        }

        public void SetCamera(IGameCamera camera)
        {
            Camera = camera;
            Camera.Update(new GameTime());
            Observer.NotifyAll("SceneCamera_Changed");
        }

        public SceneManager()
        {
            Instance = this;
        }

        public T Get<T>() where T : IGameScene
        {
            foreach (var scene in scenes)
                if (scene is T needScene)
                    return needScene;

            return default(T);
        }

        public void LoadContent(ContentManager manager)
        {
            scenes.ForEach(scene => scene.LoadContent(manager));

            SetCamera(currentScene.GetCamera());
        }

        public void Initialize(SpriteBatch spriteBatch)
        {
            scenes.ForEach(scene => scene.Initialize(spriteBatch));
        }

        public void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);
        }

        public void Draw()
        {
            currentScene.Draw();
        }

        public void FindAllScenes()
        {
            scenes = new List<IGameScene>();
            Reflection.ForEachTypesWithAttribute<IGameScene, GameSceneAttribute>((type, sceneAttribute) =>
            {
                var scene = (IGameScene)Activator.CreateInstance(type);
                if (sceneAttribute.Type == GameSceneType.Init)
                {
                    currentScene = scene;
                }
                scenes.Add(scene);
            });
        }
    }
}
