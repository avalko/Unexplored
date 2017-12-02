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
using System.Runtime.CompilerServices;
using System.Threading;
using Unexplored.Game;

namespace Unexplored.Core
{
    public class SceneManager
    {
        public static IGameCamera CurrentCamera { get; private set; }

        private IGameCamera camera;
        public IGameCamera Camera
        {
            get => camera;
            set
            {
                camera = value;
                camera.Update(new GameTime());
                camera.SetViewport(Game.GraphicsDevice.Viewport);
                CurrentCamera = camera;
                Observer.NotifyAll("SceneCamera_Changed");
            }
        }

        public MainGame Game { get; private set; }

        private List<IGameScene> scenes;
        private IGameScene currentScene;
        private bool sceneIsLoaded;
        private SpriteBatch spriteBatch;
        private Task sceneLoadedTaskDelay;

        public IGameScene CurrentScene
        {
            get => currentScene;
            set => currentScene = value;
        }

        public SceneManager(MainGame mainGame)
        {
            Game = mainGame;
            sceneIsLoaded = false;
        }

        public T Get<T>() where T : IGameScene
        {
            foreach (var scene in scenes)
                if (scene is T needScene)
                    return needScene;

            return default(T);
        }

        public void Reset()
        {
            sceneIsLoaded = false;
            new Thread(async () =>
            {
                currentScene.Reset();
                var scene = (IGameScene)Activator.CreateInstance(currentScene.GetType());
                scene.Initialize(spriteBatch, this);
                scenes.Remove(currentScene);
                scenes.Add(scene);
                currentScene = null;
                GC.Collect();
                currentScene = scene;
                if (sceneLoadedTaskDelay?.IsCompleted ?? true)
                {
                    sceneLoadedTaskDelay = Task.Delay(300);
                    await sceneLoadedTaskDelay;
                    sceneIsLoaded = true;
                }
            }).Start();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(GameTime gameTime)
        {
            if (sceneIsLoaded)
                currentScene.Update(gameTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw()
        {
            if (sceneIsLoaded)
                currentScene.Draw();
            else
            {
                spriteBatch.Begin();
                string message = "Loading...";
                spriteBatch.DrawRect(Vector2.Zero, Color.Black, Constants.SceneWidth, Constants.SceneHeight);
                spriteBatch.DrawString(StaticResources.FontUI,
                    message, (Constants.SceneSize - StaticResources.FontUI.MeasureString(message)) / 2, Color.White);
                spriteBatch.End();
            }
        }

        public void InitializeAllScenes(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            scenes = new List<IGameScene>();
            Reflection.ForEachTypesWithAttribute<IGameScene, GameSceneAttribute>((type, sceneAttribute) =>
            {
                var scene = (IGameScene)Activator.CreateInstance(type);

                scene.Initialize(spriteBatch, this);

                if (sceneAttribute.Type == GameSceneType.Init)
                {
                    currentScene = scene;
                    sceneIsLoaded = true;
                }
                scenes.Add(scene);
            });
        }
    }
}
