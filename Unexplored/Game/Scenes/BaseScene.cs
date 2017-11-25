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
using System.Runtime.CompilerServices;

namespace Unexplored.Game.Components
{
    [GameScene(GameSceneType.Init)]
    public class BaseScene : GameScene
    {
        private Level level;
        private Ligtings lightings;
        private HeroObject hero;
        private ParticlesObject particles;

        private const double BLINK_TIMEOUT = 500;
        private double currentBlinkTimeout;
        private bool isBlink;
        private double blinkTimeout;
        private Color blinkColor;

        public BaseScene()
        {
            DoInit();
        }

        public void Blink(Color color, double timeout = BLINK_TIMEOUT)
        {
            currentBlinkTimeout = timeout;
            blinkColor = color;
            isBlink = true;
        }

        public void Blink(double timeout = BLINK_TIMEOUT)
        {
            currentBlinkTimeout = timeout;
            blinkColor = Color.Black;
            isBlink = true;
        }

        public override void Reset()
        {
            base.Reset();
            level?.Dispose();
            level = null;

            Observer.UnSubscribe("Camera2D_Update", OnViewSettingsChanged);
            Constants.OnSizeChanged -= OnViewSettingsChanged;
        }

        public void DoInit()
        {
            level = new Level(Levels.Level1);

            Physics2D.Clear();

            level.Objects.MapAllObjects(MapObjectsCallback, (nonCollider) => nonCollider.Transform.Position.Y -= Tile.Size);
            void MapObjectsCallback(GameObject gameObject)
            {
                gameObject.SingleInitialize();

                if (gameObject.GetComponent<RigidbodyComponent>() is RigidbodyComponent rigidbody)
                {
                    Physics2D.AddCollider(rigidbody);
                    Physics2D.AddRigidbody(rigidbody.Rigidbody);
                }
                else if (gameObject.GetComponent<ColliderComponent>() is ColliderComponent collider)
                {
                    Physics2D.AddCollider(collider);
                }

                if (gameObject.GetComponent<TriggerControllerComponent>() is TriggerControllerComponent trigger)
                {
                    var all = from obj in level.Objects.AllObjects
                              from triggerItem in trigger.Triggers
                              where triggerItem.Id == obj.Id
                              select triggerItem.CopyWithObject(obj);

                    trigger.Triggers = all.ToArray();
                }
            }

            hero = level.Objects.HeroObjects.Single(obj => obj.Tag == "main");
            hero.GetComponent<HeroControllerComponent>().SetScene(this);
            var camera = new CameraObject();
            camera.GetComponent<CameraControllerComponent>().SetOtherObject(hero);
            SceneManager.Camera = camera.GetComponent<Camera2DComponent>();

            var objects = new List<GameObject>();
            objects.Add(camera);
            objects.AddRange(level.Objects.AllObjects);
            gameObjects = objects.ToArray();
        }

        public override void Initialize(SpriteBatch spriteBatch)
        {
            base.Initialize(spriteBatch);

            particles = new ParticlesObject();
            particles.Initialize(spriteBatch);

            lightings = new Ligtings();
            lightings.Initialize();

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
            if (Input.CurrentKeyboardIsDown(Keys.R))
            {
                SceneManager.Instance.Reset();
                return; // Good bye.
            }

            if (isBlink)
            {
                blinkTimeout += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (blinkTimeout >= currentBlinkTimeout)
                {
                    blinkTimeout = 0;
                    isBlink = false;
                }
            }

            lightings.Reset();
            base.Update(gameTime);
            particles.Update(gameTime);
            UpdateLightings(gameTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateLightings(GameTime gameTime)
        {
            lightings.SetFirstLight(SceneManager.Camera.ToScreen(hero.Transform.CenterPosition * Constants.ScaleFactor), gameTime);

            int index = level.Objects.LightingsCount;
            while (--index >= 0)
            {
                if (InBounds(level.Objects.Lightings[index].Position, 150, out level.Objects.Lightings[index].RealPosition))
                {
                    lightings.Add(level.Objects.Lightings[index]);
                }
            }
        }

        public override void Draw()
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.TransformMatrix);
            level.Draw();
            base.Draw();
            particles.Draw();
            spriteBatch.End();
            
            if (isBlink)
            {
                spriteBatch.Begin();
                spriteBatch.DrawRect(Vector2.Zero, blinkColor * (float)(Math.Sin((blinkTimeout * 2 * Math.PI) / currentBlinkTimeout)), Constants.SceneWidth, Constants.SceneHeight);
                spriteBatch.End();
            }

            lightings.Draw(spriteBatch);
        }
    }
}
