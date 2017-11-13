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

namespace Unexplored.Game.Components
{
    //[GameComponent(0)]
    class MonitorComponent : CoreComponent
    {
        Texture2D glowScreen, mainScreen;

        public MonitorComponent(MainGame game) : base(game)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            glowScreen = game.Content.Load<Texture2D>("images/glow_screen");
            mainScreen = game.Content.Load<Texture2D>("images/main_screen");
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(
                   SpriteSortMode.Immediate,
                   BlendState.AlphaBlend,
                   SamplerState.LinearClamp,
                   DepthStencilState.Default,
                   RasterizerState.CullCounterClockwise,
                   transformMatrix: camera.TransformMatrix);
            spriteBatch.Draw(mainScreen, Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
