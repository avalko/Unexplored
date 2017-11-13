using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Structures;

namespace Unexplored.Game
{
    class OneTileAnimation
    {
        public int CurrentTileX;
        public int CurrentTileY;

        public int CurrentTile;
        public bool Enabled;
        public bool Completed;

        private bool oneTime;
        private int tilesCount;
        private int currentTileIndex;

        private int[] tiles;
        private int[] tilesCoordinateX;
        private int[] tilesCoordinateY;
        private double timeout;
        private double timeLeft;

        public OneTileAnimation(double timeout, bool oneTime, params int[] tiles)
        {
            this.tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));

            if (tiles.Length == 0)
                throw new ArgumentException("Array is empty!", nameof(tiles));

            this.oneTime = oneTime;
            this.timeout = timeout;
            tilesCount = tiles.Length;

            Enabled = true;
            CurrentTile = tiles[currentTileIndex = 0];
            tilesCoordinateX = new int[tilesCount];
            tilesCoordinateY = new int[tilesCount];

            for (int index = 0; index < tilesCount; index++)
            {
                tilesCoordinateX[index] = tiles[index] % Map.TileSetWidth;
                tilesCoordinateY[index] = tiles[index] / Map.TileSetWidth;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                Completed = false;
                timeLeft += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeLeft >= timeout)
                {
                    CurrentTile = tiles[currentTileIndex = (currentTileIndex + 1) % tilesCount];

                    //CurrentTileX = CurrentTile % Map.TileSetWidth;
                    //CurrentTileY = CurrentTile / Map.TileSetWidth;

                    CurrentTileX = tilesCoordinateX[currentTileIndex];
                    CurrentTileY = tilesCoordinateY[currentTileIndex];

                    timeLeft = 0;

                    if (oneTime)
                    {
                        Enabled = false;
                    }

                    Completed = true;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            timeLeft = 0;
            CurrentTile = tiles[currentTileIndex = 0];
            Enabled = true;
            Completed = false;
        }
    }
}
