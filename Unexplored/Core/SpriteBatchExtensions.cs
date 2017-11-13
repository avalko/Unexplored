using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Contains extension methods of the spritebatch class to draw lines, rectangles, etc.
/// </summary>
static class SpriteBatchExtensions
{
    /// <summary>
    /// Draws a single line. 
    /// Require SpriteBatch.Begin() and SpriteBatch.End()
    /// </summary>
    public static void DrawLine(this SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
    {
        Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length(), width);
        Vector2 v = Vector2.Normalize(begin - end);
        float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
        if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
        spriteBatch.Draw(TextureGenerator.WhiteLinePart, r, null, color, angle, new Vector2(0, 5), SpriteEffects.None, 0);
    }

    /// <summary>
    /// Draws a point.
    /// Require SpriteBatch.Begin() and SpriteBatch.End()
    /// </summary>
    public static void DrawPoint(this SpriteBatch spriteBatch, Vector2 position, Color color, int width = 1)
    {
        spriteBatch.Draw(TextureGenerator.White, position, new Rectangle(0, 0, width, width), color);
    }
    
    /// <summary>
    /// Draws a rectangle.
    /// Require SpriteBatch.Begin() and SpriteBatch.End()
    /// </summary>
    public static void DrawRect(this SpriteBatch spriteBatch, Vector2 position, Color color, int width, int height)
    {
        spriteBatch.Draw(TextureGenerator.White, position, new Rectangle(0, 0, width, height), color);
    }

    /// Draws a rectangle.
    /// Require SpriteBatch.Begin() and SpriteBatch.End()
    /// </summary>
    public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        spriteBatch.Draw(TextureGenerator.White, rectangle, color);
    }

    public static void DrawCenteredRect(this SpriteBatch spriteBatch, Vector2 position, Vector2 realSize, Color color, float width, float height)
    {
        Vector2 offset = (realSize - new Vector2(width, height)) / 2;
        spriteBatch.Draw(TextureGenerator.White, position + offset, new Rectangle(0, 0, (int)width, (int)height), color);
    }

    /// <summary>
    /// Draws a single line. 
    /// Doesn't require SpriteBatch.Begin() or SpriteBatch.End()
    /// </summary>
    public static void DrawSingleLine(this SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        spriteBatch.DrawLine(begin, end, color, width);
        spriteBatch.End();
    }

    /// <summary>
    /// Draws a poly line.
    /// Doesn't require SpriteBatch.Begin() or SpriteBatch.End()
    /// </summary>
    public static void DrawPolyLine(this SpriteBatch spriteBatch, Vector2[] points, Color color, int width = 1, bool closed = false)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        for (int i = 0; i < points.Length - 1; i++)
            spriteBatch.DrawLine(points[i], points[i + 1], color, width);
        if (closed)
            spriteBatch.DrawLine(points[points.Length - 1], points[0], color, width);
        spriteBatch.End();
    }

    /// <summary>
    /// The graphics device, set this before drawing.
    /// </summary>
    public static GraphicsDevice GraphicsDevice;

    /// <summary>
    /// Generates textures.
    /// </summary>
    static class TextureGenerator
    {
        static Texture2D whiteLinePart = null, white = null;

        public static Texture2D WhiteLinePart
        {
            get
            {
                if (whiteLinePart == null)
                {
                    if (GraphicsDevice == null)
                        throw new Exception($"Please set the {nameof(SpriteBatchExtensions)}.{nameof(GraphicsDevice)} to your GraphicsDevice before drawing.");

                    whiteLinePart = new Texture2D(GraphicsDevice, 1, 10);
                    Color[] color = new Color[10];

                    color[0] = Color.FromNonPremultiplied(1, 1, 1, 1);
                    color[1] = Color.FromNonPremultiplied(10, 10, 10, 10);
                    color[2] = Color.FromNonPremultiplied(50, 50, 50, 50);
                    color[3] = Color.White;
                    color[4] = Color.White;
                    color[5] = Color.White;
                    color[6] = Color.White;
                    color[7] = Color.FromNonPremultiplied(50, 50, 50, 50);
                    color[8] = Color.FromNonPremultiplied(10, 10, 10, 10);
                    color[9] = Color.FromNonPremultiplied(1, 1, 1, 1);

                    whiteLinePart.SetData(color);
                }
                return whiteLinePart;
            }
        }

        public static Texture2D White
        {
            get
            {
                if (white == null)
                {
                    if (GraphicsDevice == null)
                        throw new Exception($"Please set the {nameof(SpriteBatchExtensions)}.{nameof(GraphicsDevice)} to your GraphicsDevice before drawing.");

                    white = new Texture2D(GraphicsDevice, 1, 1);
                    Color[] color = new Color[1];
                    color[0] = Color.White;
                    white.SetData<Color>(color);
                }

                return white;
            }
        }
    }
}
