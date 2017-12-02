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
    struct Chunks
    {
        private Tile[] visibleTiles;
        private int visibleTilesCount;
        private Tile[,,] levelChunks;
        private int[,] tilesCountInChunk;

        private int chunkHorizontallyCount;
        private int chunkVerticallyCount;

        private int chunkStartX, chunkStartY;
        private int chunkEndX, chunkEndY;

        public Chunks(int mapWidth, int mapHeight, int tilesPerChunk, int maxTilesCountInChunk)
        {
            chunkHorizontallyCount = mapWidth / tilesPerChunk;
            chunkVerticallyCount = mapHeight / tilesPerChunk;
            tilesCountInChunk = new int[chunkHorizontallyCount, chunkVerticallyCount];
            levelChunks = new Tile[chunkHorizontallyCount, chunkVerticallyCount, maxTilesCountInChunk];
            visibleTilesCount = 0;
            visibleTiles = new Tile[mapWidth * mapHeight];

            chunkStartX = chunkStartY = 0;
            chunkEndX = chunkEndY = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetTile(int chunkX, int chunkY, Tile tile)
        {
            levelChunks[chunkY, chunkX, tilesCountInChunk[chunkY, chunkX]++] = tile;
        }

        public void Update(Vector2 startView, Vector2 endView)
        {
            int chunkStartX = (int)(startView.X / chunkHorizontallyCount);
            int chunkStartY = (int)(startView.Y / chunkVerticallyCount);
            int chunkEndX = (int)Math.Floor(endView.X / chunkHorizontallyCount);
            int chunkEndY = (int)Math.Floor(endView.Y / chunkVerticallyCount);

            chunkStartX = chunkStartX < 0 ? 0 : chunkStartX;
            chunkStartY = chunkStartY < 0 ? 0 : chunkStartY;
            chunkEndX = chunkEndX > chunkHorizontallyCount ? chunkHorizontallyCount : chunkEndX;
            chunkEndY = chunkEndY > chunkVerticallyCount ? chunkVerticallyCount : chunkEndY;

            if (!(this.chunkStartX == chunkStartX &&
                this.chunkStartY == chunkStartY &&
                this.chunkEndX == chunkEndX &&
                this.chunkEndY == chunkEndY))
            {
                this.chunkStartX = chunkStartX;
                this.chunkStartY = chunkStartY;
                this.chunkEndX = chunkEndX;
                this.chunkEndY = chunkEndY;

                visibleTilesCount = 0;
                for (int chunkY = 0; chunkY < chunkEndY; ++chunkY)
                {
                    for (int chunkX = 0; chunkX < chunkEndX; ++chunkX)
                    {
                        int tilesCount = tilesCountInChunk[chunkY, chunkX];
                        for (int tileIndex = 0; tileIndex < tilesCount; ++tileIndex)
                        {
                            visibleTiles[visibleTilesCount++] = levelChunks[chunkY, chunkX, tileIndex];
                        }
                    }
                }
            }
        }

        public void DrawVisibleTiles(SpriteBatch spriteBatch)
        {
            int tileIndex = visibleTilesCount;
            while (--tileIndex >= 0)
            {
                var tile = visibleTiles[tileIndex];
                spriteBatch.Draw(StaticResources.Tileset, tile.Position, tile.Offset, Constants.BackgroundColor);
            }
        }
    }
}
