using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Unexplored.Core.Components;
using Unexplored.Core;
using Unexplored.Game;
using System;

namespace Unexplored
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private InputComponent inputComponent;
        private Camera2DComponent camera;
        private FpsComponent fps;
        private Rectangle gameRectangle, sourceRectangle;
        BloomComponent bloom;

        public ComponentManager ComponentManager { get; private set; }
        public RenderTarget2D GameSceneTarget { get; private set; }
        public RenderTarget2D GameSceneTarget2 { get; private set; }

        public MainGame()
        {
            ComponentManager = new ComponentManager(this);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
#if !DEBUG
            graphics.IsFullScreen = true;
#endif
            //graphics.IsFullScreen = true;
            IsMouseVisible = true;
            //graphics.PreferMultiSampling = false;
            //graphics.HardwareModeSwitch = true;
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;

            ComponentManager.FindAllComponents();
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();


            inputComponent = ComponentManager.Get<InputComponent>();

            bloom = ComponentManager.Get<BloomComponent>();
            bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);
            fps = ComponentManager.Get<FpsComponent>();

            camera = ComponentManager.Get<Camera2DComponent>();
            camera.SetViewport(GraphicsDevice.Viewport);
            camera.Update(new GameTime());

            SpriteBatchExtensions.GraphicsDevice = GraphicsDevice;

            graphics.PreparingDeviceSettings += PreparingDeviceSettings;
            Window.ClientSizeChanged += ClientSizeChanged;
            Window.Position = (new Vector2(GraphicsDevice.DisplayMode.Width - Constants.SceneWidth, GraphicsDevice.DisplayMode.Height - Constants.SceneHeight + 100) / 2.0f).ToPoint();
            Window.AllowUserResizing = true;
            sourceRectangle = new Rectangle(0, 0, Constants.SceneWidth, Constants.SceneHeight);

            ComponentManager.Initialize();
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

        Effect effect;
        SpriteFont baseFont;

        protected override void LoadContent()
        {
            base.LoadContent();

            effect = Content.Load<Effect>("effects/pixel");
            baseFont = Content.Load<SpriteFont>("fonts/baseFont");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameSceneTarget = CreateRenderTarget();
            GameSceneTarget2 = CreateRenderTarget();

            ComponentManager.LoadContent();
            StaticResources.LoadContent(Content);
        }

        public RenderTarget2D CreateRenderTarget()
        {
            return new RenderTarget2D(GraphicsDevice, Constants.SceneWidth, Constants.SceneHeight, false,
                SurfaceFormat.HalfVector4, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }

        protected override void Update(GameTime gameTime)
        {
            ComponentManager.Update(gameTime);

            if (inputComponent.CurrentKeyboardIsDown(Keys.Escape))
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            fps.Draw(gameTime);
            //bloom.ResultTarget = GameSceneTarget;
            GraphicsDevice.SetRenderTarget(GameSceneTarget);
            //GraphicsDevice.Clear(Color.Transparent);
            //GraphicsDevice.Clear(Color.Transparent);
            //bloom.BeginDraw();
            //GraphicsDevice.Clear(Color.Transparent);
            GraphicsDevice.Clear(Color.FromNonPremultiplied(29, 33, 45, 255));
            ComponentManager.Draw(gameTime);
            //bloom.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);
            //GraphicsDevice.Clear(Color.FromNonPremultiplied(29, 33, 45, 255));
            //bloom.BeginDraw();
            spriteBatch.Begin();
            spriteBatch.Draw(GameSceneTarget, gameRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
            //bloom.Draw(gameTime);


            //bloom.BeginDraw();
            /*GraphicsDevice.SetRenderTarget(null);
            //GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(GameSceneTarget2, Vector2.Zero, Color.White);
            spriteBatch.End();
            //*/
            //bloom.Draw(gameTime);

            FpsComponent.DrawString(baseFont, spriteBatch);
        }
    }
}
