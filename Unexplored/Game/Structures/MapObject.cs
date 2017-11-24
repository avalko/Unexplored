using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Unexplored.Core.Types;

namespace Unexplored.Game.Structures
{
    public struct MapText
    {
        public string Text;
        public Color Color;
    }

    public struct MapObject
    {
        public int Id;
        public FRect Position;
        public string Name;
        public string Type;
        public object Argument;
        public Dictionary<string, object> Properties;
    }
}
