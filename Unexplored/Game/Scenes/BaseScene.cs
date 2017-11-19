using Unexplored.Core;
using Unexplored.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Unexplored.Game.Structures;
using Unexplored.Core.Base;
using Microsoft.Xna.Framework.Content;
using Unexplored.Game.GameObjects;

namespace Unexplored.Game.Components
{
    [GameScene(GameSceneType.Init)]
    public class BaseScene : GameScene
    {
        private const double Timeout = 100;
        private Level level;
        private double timeout;

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            level = new Level(MapReader.ParseMap(File.ReadAllText("Content/map/map.json")));

            Physics2D.Clear();

            level.Objects.MapAllObjects(MapObjectsCallback, (nonCollider) => nonCollider.Transform.Position.Y -= Tile.Size);
            void MapObjectsCallback(GameObject gameObject)
            {              
                if (gameObject.GetComponent<ColliderComponent>() is ColliderComponent collider)
                {
                    Physics2D.Add(collider);
                }
            }

            var hero = level.Objects.HeroObjects.Single(obj => obj.Tag == "main");
            var camera = new CameraObject();
            //camera.GetComponent<CameraControllerComponent>().SetMapSize(level.CurrentMapSize);
            camera.GetComponent<CameraControllerComponent>().SetOtherObject(hero);

            var objects = new List<GameObject>();
            objects.Add(camera);
            objects.AddRange(level.Objects.AllObjects);
            gameObjects = objects.ToArray();
        }

        public override void Initialize(SpriteBatch spriteBatch)
        {
            base.Initialize(spriteBatch);

            level.SetSpriteBatch(spriteBatch);
            Observer.Subscribe("Camera2D_Update", OnViewSettingsChanged);
            Constants.OnSizeChanged += OnViewSettingsChanged;
            OnViewSettingsChanged();
        }

        public void OnViewSettingsChanged()
        {
            Vector2 screenBeginPoint = camera.ToWorld(Vector2.Zero);
            Vector2 screenEndPoint = camera.ToWorld(Constants.SceneSize);
            level.UpdateTilesFrustum(screenBeginPoint, screenEndPoint);
        }

        public override void Update(GameTime gameTime)
        {
            if (timeout < Timeout)
            {
                timeout += gameTime.ElapsedGameTime.TotalMilliseconds;
                return;
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.TransformMatrix);
            level.Draw();
            base.Draw();
            spriteBatch.End();
        }
    }
}
