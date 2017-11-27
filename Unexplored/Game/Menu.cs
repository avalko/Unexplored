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
        private static Texture2D topLeft;
        private static Texture2D topRight;
        private static Texture2D bottomLeft;
        private static Texture2D bottomRight;
        private static Texture2D rightSide;
        private static Texture2D leftSide;
        private static Texture2D topSide;
        private static Texture2D bottomSide;

        const int size = 24;

        public static void Init(GraphicsDevice GraphicsDevice)
        {
            topLeft = new Texture2D(GraphicsDevice, size, size);
            topRight = new Texture2D(GraphicsDevice, size, size);
            bottomLeft = new Texture2D(GraphicsDevice, size, size);
            bottomRight = new Texture2D(GraphicsDevice, size, size);
            rightSide = new Texture2D(GraphicsDevice, size, 1);
            leftSide = new Texture2D(GraphicsDevice, size, 1);
            topSide = new Texture2D(GraphicsDevice, 1, size);
            bottomSide = new Texture2D(GraphicsDevice, 1, size);

            Color[] array = new Color[size * size];

            StaticResources.MenuBackground.GetData(0, new Rectangle(0, 0, size, size), array, 0, array.Length);
            topLeft.SetData(array);
            StaticResources.MenuBackground.GetData(0, new Rectangle(size, 0, size, size), array, 0, array.Length);
            topRight.SetData(array);
            StaticResources.MenuBackground.GetData(0, new Rectangle(0, size, size, size), array, 0, array.Length);
            bottomLeft.SetData(array);
            StaticResources.MenuBackground.GetData(0, new Rectangle(size, size, size, size), array, 0, array.Length);
            bottomRight.SetData(array);


            Color[] array2 = new Color[size];
            StaticResources.MenuBackground.GetData(0, new Rectangle(size, size, size, 1), array2, 0, array2.Length);
            rightSide.SetData(array2);
            StaticResources.MenuBackground.GetData(0, new Rectangle(0, size, size, 1), array2, 0, array2.Length);
            leftSide.SetData(array2);
            StaticResources.MenuBackground.GetData(0, new Rectangle(size, 0, 1, size), array2, 0, array2.Length);
            topSide.SetData(array2);
            StaticResources.MenuBackground.GetData(0, new Rectangle(size, size, 1, size), array2, 0, array2.Length);
            bottomSide.SetData(array2);
        }

        public static void Draw(SpriteBatch spriteBatch, Vector2 position, int width, int height)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
            spriteBatch.Draw(topLeft, position * Constants.ScaleFactor, null, Color.White, 0, Vector2.Zero, Constants.ScaleFactor, SpriteEffects.None, 0);
            spriteBatch.Draw(topSide, Constants.ScaleFactor * (position + new Vector2(size, 0)), new Rectangle(0, 0, size, size), Color.White, 0, Vector2.Zero, Constants.ScaleFactor, SpriteEffects.None, 0);
            spriteBatch.Draw(topRight, Constants.ScaleFactor * (position + new Vector2(size * 2, 0)), null, Color.White, 0, Vector2.Zero, Constants.ScaleFactor, SpriteEffects.None, 0);

            spriteBatch.Draw(leftSide, Constants.ScaleFactor * (position + new Vector2(0, size)), new Rectangle(0, 0, size, size), Color.White, 0, Vector2.Zero, Constants.ScaleFactor, SpriteEffects.None, 0);
            spriteBatch.Draw(rightSide, Constants.ScaleFactor * (position + new Vector2(size * 2, size)), new Rectangle(0, 0, size, size), Color.White, 0, Vector2.Zero, Constants.ScaleFactor, SpriteEffects.None, 0);

            spriteBatch.Draw(bottomLeft, (position + new Vector2(0, size * 2)) * Constants.ScaleFactor, null, Color.White, 0, Vector2.Zero, Constants.ScaleFactor, SpriteEffects.None, 0);
            spriteBatch.Draw(bottomSide, Constants.ScaleFactor * (position + new Vector2(size, size * 2)), new Rectangle(0, 0, size, size), Color.White, 0, Vector2.Zero, Constants.ScaleFactor, SpriteEffects.None, 0);
            spriteBatch.Draw(bottomRight, Constants.ScaleFactor * (position + new Vector2(size * 2, size * 2)), null, Color.White, 0, Vector2.Zero, Constants.ScaleFactor, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
