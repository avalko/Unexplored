using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using System.Runtime.CompilerServices;

namespace Unexplored.Game.Components
{
    public struct SpriteAnimation
    {
        public double Timeout;
        public bool Once;
        public int[] Tiles;

        public SpriteAnimation(double timeout, bool once, params int[] tiles)
        {
            Timeout = timeout;
            Once = once;
            Tiles = tiles;
        }
    }

    public class SpriteAnimatorComponent : BehaviorComponent
    {
        private SpriteRendererComponent sprite;

        private int tilesCount;
        private int[] tiles;
        private int[] tilesCoordinateX;
        private int[] tilesCoordinateY;

        private bool oneTime;
        private double timeout;
        private double timeLeft;
        private int currentFrame;
        private int currentTile;

        public bool Completed;
        public bool FrameCompleted;

        public SpriteAnimatorComponent()
        {
            Enabled = false;
        }

        public void SetAnimation(SpriteAnimation animation) =>
            SetAnimation(animation.Timeout, animation.Once, animation.Tiles);

        public void SetAnimation(double timeout, bool oneTime, params int[] tiles)
        {
            this.tiles = tiles;
            this.oneTime = oneTime;
            this.timeout = timeout;
            tilesCount = tiles.Length;

            currentTile = tiles[currentFrame = 0];
            tilesCoordinateX = new int[tilesCount];
            tilesCoordinateY = new int[tilesCount];

            for (int index = 0; index < tilesCount; index++)
            {
                tilesCoordinateX[index] = tiles[index] % Map.TileSetWidth;
                tilesCoordinateY[index] = tiles[index] / Map.TileSetWidth;
            }
        }

        public override void Initialize()
        {
            sprite = GetComponent<SpriteRendererComponent>();
        }

        public override void Update(GameTime gameTime)
        {
            Completed = FrameCompleted = false;
            timeLeft += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeLeft >= timeout)
            {
                currentTile = tiles?[currentFrame = (currentFrame + 1) % tilesCount] ?? 0;

                // sprite.TileOffset.X = CurrentTile % Map.TileSetWidth;
                // sprite.TileOffset.Y = CurrentTile / Map.TileSetWidth;

                sprite.TileOffset.X = tilesCoordinateX?[currentFrame] ?? 0;
                sprite.TileOffset.Y = tilesCoordinateY?[currentFrame] ?? 0;

                timeLeft = 0;

                if (currentFrame == tilesCount - 1)
                {
                    Completed = true;

                    if (oneTime)
                    {
                        Enabled = false;
                    }
                }

                FrameCompleted = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            timeLeft = 0;
            currentTile = tiles?[currentFrame = 0] ?? 0;
            Enabled = true;
            Completed = false;
        }
    }
}
