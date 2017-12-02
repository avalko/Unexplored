using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Base
{
    public enum GameSceneType
    {
        Init,
        Base
    }

    public class GameSceneAttribute : Attribute
    {
        public GameSceneType Type { get; private set; }

        public GameSceneAttribute(GameSceneType type = GameSceneType.Base)
        {
            Type = type;
        }
    }
}
