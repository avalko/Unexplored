using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Game.Components;
using Unexplored.Game.Structures;
using Microsoft.Xna.Framework.Graphics;

namespace Unexplored.Game.Particles
{
    public class ParticleAttack : Particle
    {
        private const double CHANGE_TIMEOUT = 50;

        private double timeout;
        private int currentFrame;

        private Point[] tiles;
        private bool flipped;

        public ParticleAttack(bool flipped)
        {
            this.flipped = flipped; 
            tiles = new Point[] {
                Tile.GetTileOffset(78), Tile.GetTileOffset(79),
                Tile.GetTileOffset(104), Tile.GetTileOffset(105),
                Tile.GetTileOffset(130), Tile.GetTileOffset(131),
                Tile.GetTileOffset(156), Tile.GetTileOffset(157),
            };
        }

        public override void Update(GameTime gameTime)
        {
            timeout += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeout >= CHANGE_TIMEOUT)
            {
                timeout = 0;
                ++currentFrame;
            }

            if (currentFrame == 4)
            {
                Die = true;
                return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int scaledSize = (int)Constants.ScaledTile;
            var position = Position * Constants.ScaleFactor;
            spriteBatch.Draw(StaticResources.Tileset,
                new Rectangle((int)(position.X), (int)(position.Y), scaledSize, scaledSize),
                new Rectangle(Tile.Size * tiles[currentFrame * 2].X, Tile.Size * tiles[currentFrame * 2].Y, Tile.Size, Tile.Size), Color.White, 0, Vector2.Zero, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            spriteBatch.Draw(StaticResources.Tileset,
                new Rectangle((int)(position.X + (flipped ? -Constants.ScaledTile : Constants.ScaledTile)), (int)(position.Y), scaledSize, scaledSize),
                new Rectangle(Tile.Size * tiles[currentFrame * 2 + 1].X, Tile.Size * tiles[currentFrame * 2 + 1].Y, Tile.Size, Tile.Size), Color.White, 0, Vector2.Zero, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }
}
