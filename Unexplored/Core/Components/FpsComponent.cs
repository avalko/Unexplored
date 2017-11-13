﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Unexplored;
using Unexplored.Core;
using Unexplored.Core.Components.Logic;

namespace Unexplored.Core.Components
{
    [GameComponent(int.MaxValue)]
    class FpsComponent : CoreComponent
    {
        public static int Value => fps;

        private static double fpsTime = 0;
        private static int counter = 0;
        private static int fps = 0;

        public FpsComponent(MainGame game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            fpsTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (fpsTime >= 1000)
            {
                fps = counter;
                counter = 0;
                fpsTime = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ++counter;
        }

        public static void DrawString(SpriteFont font, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Vector2 size = font.MeasureString(FpsComponent.Value.ToString());
            spriteBatch.DrawRect(Vector2.Zero, Color.Black, (int)size.X, (int)size.Y);
            spriteBatch.DrawString(font, Value.ToString(), Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}