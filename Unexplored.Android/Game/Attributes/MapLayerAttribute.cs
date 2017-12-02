using System;
using Unexplored.Game.Structures;

namespace Unexplored.Game.Attributes
{
    class MapLayerAttribute : Attribute
    {
        public LayerType LayerType { get; set; }
        public string Tag { get; set; }

        public MapLayerAttribute(string tag, LayerType layerType = LayerType.Tiles)
        {
            Tag = tag;
            LayerType = layerType;
        }
    }
}
