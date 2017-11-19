using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Game.Structures;

namespace Unexplored.Game.Components
{
    public class SpriteRendererComponent : BehaviorComponent
    {
        public bool Flipped;
        public Color Color;
        public Point TileOffset;

        public SpriteRendererComponent()
        {
            Color = Color.White;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Draw()
        {
            int scaledSize = (int)Constants.ScaledTile;
            var position = GameObject.Transform.Position * Constants.ScaleFactor;
            spriteBatch.Draw(StaticResources.Tileset,
                new Rectangle((int)(position.X), (int)(position.Y), scaledSize, scaledSize),
                new Rectangle(Tile.Size * TileOffset.X, Tile.Size * TileOffset.Y, Tile.Size, Tile.Size), Color, 0, Vector2.Zero, Flipped ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}
