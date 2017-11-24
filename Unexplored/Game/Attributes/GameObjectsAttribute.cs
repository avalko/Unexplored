using System;

namespace Unexplored.Game.Attributes
{
    public class GameObjectsAttribute : Attribute
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public GameObjectsAttribute(string name, string type = null)
        {
            Name = name;
            Type = type;
        }
    }
}
