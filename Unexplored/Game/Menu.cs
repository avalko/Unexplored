using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game
{
    public static class Menu
    {
        private static Texture2D panel, item;

        const int size = 24;

        public static void Init(GraphicsDevice GraphicsDevice)
        {
            panel = StaticResources.MenuBackground;
            item = StaticResources.MenuItem;
        }

        private static Vector2 Scaled(Vector2 normal) => normal * Constants.ScaleFactor;

        private static void Draw(Texture2D texture, SpriteBatch spriteBatch, Vector2 position, Rectangle offset, float scale) => spriteBatch.Draw(texture, Scaled(position), offset, Color.White, 0, Vector2.Zero, Constants.ScaleFactor * scale, SpriteEffects.None, 0);
        private static void Draw(Texture2D texture, SpriteBatch spriteBatch, Vector2 position, Rectangle offset, float scale, Vector2 size) => spriteBatch.Draw(texture, new Rectangle(Scaled(position).ToPoint(), Scaled(scale * size).ToPoint()), offset, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        private static void DrawPanel(SpriteBatch spriteBatch, Vector2 position, Rectangle offset) => Draw(panel, spriteBatch, position, offset, 1);
        private static void DrawPanel(SpriteBatch spriteBatch, Vector2 position, Rectangle offset, Vector2 size) => Draw(panel, spriteBatch, position, offset, 1, size);
        private static void DrawItem(SpriteBatch spriteBatch, Vector2 position, Rectangle offset) => Draw(item, spriteBatch, position, offset, 0.5f);
        private static void DrawItem(SpriteBatch spriteBatch, Vector2 position, Rectangle offset, Vector2 size) => Draw(item, spriteBatch, position, offset, 0.5f, size);

        public static void DrawPanel(SpriteBatch spriteBatch, Vector2 position, int width, int height)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);

            //Vector2 subVector = 

            DrawPanel(spriteBatch, position, new Rectangle(0, 0, size, size));
            DrawPanel(spriteBatch, position + new Vector2(size * 2, 0), new Rectangle(size, 0, size, size));

            DrawPanel(spriteBatch, position + new Vector2(0, size * 2), new Rectangle(0, size, size, size));
            DrawPanel(spriteBatch, position + new Vector2(size * 2, size * 2), new Rectangle(size, size, size, size));


            DrawPanel(spriteBatch, position + new Vector2(size, 0), new Rectangle(size, 0, 1, size), new Vector2(size, size));
            DrawPanel(spriteBatch, position + new Vector2(size, size * 2), new Rectangle(size, size, 1, size), new Vector2(size, size));

            DrawPanel(spriteBatch, position + new Vector2(0, size), new Rectangle(0, size, size, 1), new Vector2(size, size));
            DrawPanel(spriteBatch, position + new Vector2(size * 2, size), new Rectangle(size, size - 1, size, 1), new Vector2(size, size));

            DrawPanel(spriteBatch, position + new Vector2(size, size), new Rectangle(size / 2, size / 2, 1, 1), new Vector2(size, size));

            DrawItem(spriteBatch, position + new Vector2(size) / 2, new Rectangle(0, 0, size, size));

            spriteBatch.End();
        }

        public static void DrawItems()
        {
        }
    }
}
