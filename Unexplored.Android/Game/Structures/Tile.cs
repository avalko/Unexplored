using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Structures
{
    public struct Tile
    {
        public const int Size = 16;

        public Rectangle Position;
        public Rectangle Offset;

        public Tile(Rectangle position, Rectangle offset)
        {
            Position = position;
            Offset = offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Tile Create(int x, int y, int tile)
        {
            int tileX = tile % Map.TileSetWidth;
            int tileY = tile / Map.TileSetWidth;
            const float scale = Constants.ScaleFactor;
            return new Tile(
                new Rectangle((int)(x * Size * scale), (int)(y * Size * scale), (int)(Size * scale), (int)(Size * scale)),
                new Rectangle(tileX * Size, tileY * Size, Size, Size));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetTileOffset(int tile)
        {
            return new Point(tile % Map.TileSetWidth, tile / Map.TileSetWidth);
        }
    }
}
