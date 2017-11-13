using System.Collections.Generic;
using Unexplored.Core.Types;

namespace Unexplored.Game.Structures
{
    public struct MapObject
    {
        public FRect Position;
        public string Name;
        public string Type;
        public Dictionary<string, object> Properties;
    }
}
