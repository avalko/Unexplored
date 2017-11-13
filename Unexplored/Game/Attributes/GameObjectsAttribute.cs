using System;

namespace Unexplored.Game.Attributes
{
    public class GameObjectsAttribute : Attribute
    {
        public string Name { get; set; }

        public GameObjectsAttribute(string name)
        {
            Name = name;
        }
    }
}
