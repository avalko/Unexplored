using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Unexplored.Core.Components;
using Unexplored.Core;
using Unexplored.Game;
using System;
using Unexplored.Core.Base;

namespace Unexplored
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public static MainGame Instance;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Fps fps;
        private Rectangle gameRectangle, sourceRectangle;

        public SceneManager SceneManager { get; private set; }
        public RenderTarget2D GameSceneTarget { get; private set; }

        public MainGame()
        {
            Instance = this;
            SceneManager = new SceneManager();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
#if !DEBUG
            graphics.IsFullScreen = true;
#endif
            //graphics.IsFullScreen = true;
            IsMouseVisible = true;
            //graphics.PreferMultiSampling = true;
            //graphics.HardwareModeSwitch = true;
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
            Window.ClientSizeChanged += ClientSizeChanged;
            Window.Position = (new Vector2(GraphicsDevice.DisplayMode.Width - Constants.SceneWidth, GraphicsDevice.DisplayMode.Height - Constants.SceneHeight + 100) / 2.0f).ToPoint();
            Window.AllowUserResizing = true;
            sourceRectangle = new Rectangle(0, 0, Constants.SceneWidth, Constants.SceneHeight);
            
            base.Initialize();
            Observer.NotifyAll("Core_Initialized");
        }

        private void ClientSizeChanged(object sender, System.EventArgs e)
        {
            float realWidth = Window.ClientBounds.Width;
            float realHeight = Window.ClientBounds.Height;
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

            Constants.SetSize(Window.ClientBounds.Width, Window.ClientBounds.Height);
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
            return new RenderTarget2D(GraphicsDevice, Constants.SceneWidth, Constants.SceneHeight, false,
                SurfaceFormat.HalfVector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        public RenderTarget2D CreateSmallRenderTarget()
        {
            return new RenderTarget2D(GraphicsDevice, Constants.SceneWidth / 2, Constants.SceneHeight / 2, false,
                SurfaceFormat.HalfVector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
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
