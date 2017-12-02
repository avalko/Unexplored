using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Game.Structures;

namespace Unexplored.Game.Components
{
    public class WarpControllerComponent : BehaviorComponent
    {
        const int STATES_COUNT = 6;
        const int START_SPRITE_INDEX = 335;

        private SpriteRendererComponent renderer;
        private int currentState;
        public bool Avaliable;
        public WarpPoint Point;

        public override void Initialize()
        {
            renderer = GetComponent<SpriteRendererComponent>();
            currentState = 0;
            Point = new WarpPoint(STATES_COUNT, OnUsed) { Position = Transform.Position };
            Avaliable = Point.Avaliable;
        }

        private void OnUsed()
        {
            if (Point.Avaliable)
            {
                ++Point.State;
                ++currentState;
                renderer.TileOffset = Tile.GetTileOffset(START_SPRITE_INDEX - currentState);

                Avaliable = Point.Avaliable;
            }
        }
    }
}
