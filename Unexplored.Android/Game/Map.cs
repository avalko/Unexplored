using System.Collections.Generic;
using Unexplored.Game.Attributes;
using Unexplored.Game.Structures;

namespace Unexplored.Game
{
    enum LayerType
    {
        Tiles,
        Objects
    }

    public class Map
    {
        public const int TileSetWidth = 26;

        const string GhostlyTag = "ghostly";
        const string BasicTag = "basic";
        const string ItemsTag = "items";
        const string ObjectsTag = "objects";
        const string CollidersTag = "colliders";
        const string LightingsTag = "lightings";

        public int Width;
        public int Height;

        [MapLayer(GhostlyTag)]
        public int[,] LevelGhostly;
        [MapLayer(BasicTag)]
        public int[,] LevelBasic;
        [MapLayer(ItemsTag)]
        public int[,] LevelItems;

        [MapLayer(CollidersTag, LayerType.Objects)]
        public MapObject[] LevelColliders;
        [MapLayer(ObjectsTag, LayerType.Objects)]
        public MapObject[] LevelObjects;
        [MapLayer(LightingsTag, LayerType.Objects)]
        public MapObject[] LevelLightings;
    }
}
