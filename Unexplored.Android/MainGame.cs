using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Unexplored.Core.Components;
using Unexplored.Core;
using Unexplored.Game;
using System;
using Unexplored.Core.Base;
using System.IO;

namespace Unexplored
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Fps fps;
        private Rectangle gameRectangle, sourceRectangle;

        public SceneManager SceneManager { get; private set; }
        public RenderTarget2D GameSceneTarget { get; private set; }

        public MainGame()
        {
            SceneManager = new SceneManager(this);
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            TargetElapsedTime = TimeSpan.FromSeconds(Constants.FrameRate);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();

            SpriteBatchExtensions.GraphicsDevice = GraphicsDevice;

            graphics.PreparingDeviceSettings += PreparingDeviceSettings;
            ClientSizeChanged();
            sourceRectangle = new Rectangle(0, 0, Constants.SceneWidth, Constants.SceneHeight);
            
            base.Initialize();
            Observer.NotifyAll("Core_Initialized");

            Menu.Init(GraphicsDevice);
        }

        private void ClientSizeChanged()
        {
            float realWidth = GraphicsDevice.DisplayMode.Width;
            float realHeight = GraphicsDevice.DisplayMode.Height;
            float yScale = realHeight / Constants.SceneHeight;
            float xScale = realWidth / Constants.SceneWidth;
            float scale = MathHelper.Min(yScale, xScale);
            Vector2 offset = Vector2.Zero;
            if (xScale > yScale)
                offset = new Vector2((realWidth - Constants.SceneWidth * scale) / 2, 0);
            else if (yScale > xScale)
                offset = new Vector2(0, (realHeight - Constants.SceneHeight * scale) / 2);
            var size = new Vector2(Constants.SceneWidth, Constants.SceneHeight) * scale;

            gameRectangle = new Rectangle(offset.ToPoint(), size.ToPoint());

            Constants.SetSize(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
            Constants.AspectRatio = GraphicsDevice.DisplayMode.AspectRatio;
        }

        private void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }
        
        SpriteFont baseFont;

        protected override void LoadContent()
        {
            base.LoadContent();
            
            baseFont = Content.Load<SpriteFont>("fonts/baseFont");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameSceneTarget = CreateRenderTarget();
            
            StaticResources.LoadContent(Content);
            Levels.Load();

            SceneManager.InitializeAllScenes(spriteBatch);

            fps = new Fps();
        }

        public RenderTarget2D CreateRenderTarget()
        {
            try
            {
                return new RenderTarget2D(GraphicsDevice, Constants.SceneWidth, Constants.SceneHeight);
            }
            catch (Exception e)
            {
                string message = e.Message;
                return null;
            }
        }

        public RenderTarget2D CreateSmallRenderTarget()
        {
            return new RenderTarget2D(GraphicsDevice, Constants.SceneWidth / 2, Constants.SceneHeight / 2);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);

            fps.Update(gameTime);
            SceneManager.Update(gameTime);

            if (Input.CurrentKeyboardIsDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            fps.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(GameSceneTarget);
            GraphicsDevice.Clear(Color.FromNonPremultiplied(29, 33, 45, 255));
            SceneManager.Draw();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();
            spriteBatch.Draw(GameSceneTarget, gameRectangle, sourceRectangle, Color.White);
            spriteBatch.End();

            Fps.DrawString(baseFont, spriteBatch);
        }
    }
}
