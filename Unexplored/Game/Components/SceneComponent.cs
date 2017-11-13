using Unexplored.Core;
using Unexplored.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Unexplored.Core.Components.Logic;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Unexplored.Game.Objects;
using Unexplored.Game.Controllers;
using Unexplored.Game.Structures;

namespace Unexplored.Game.Components
{
    [GameComponent(0)]
    class SceneComponent : CoreComponent
    {
        Level level;
        HeroObject mainHero;
        HeroController heroController;
        CameraController cameraController;

        public SceneComponent(MainGame game) : base(game)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            level = new Level(MapReader.ParseMap(File.ReadAllText("Content/map/map.json")));
            level.SetSpriteBatch(spriteBatch);

            level.Objects.MapAllObjects(MapObjectsCallback);
            void MapObjectsCallback(GameObject gameObject)
            {
                gameObject.Transform.Position.Y -= Tile.Size;
                gameObject.SetSpriteBatch(spriteBatch);
                gameObject.Initialized();
            }

            mainHero = level.Objects.HeroObjects.First(oneHero => oneHero.Type == "main");
            heroController = new HeroController(mainHero);
            cameraController = new CameraController(mainHero, level.CurrentMapSize);

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
            heroController.Update(gameTime);
            level.Update(gameTime);
            cameraController.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.TransformMatrix);
            level.Draw();
            mainHero.Draw();
            spriteBatch.End();
        }
    }
}
