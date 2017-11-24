using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Game.Components;
using Unexplored.Game.Structures;

namespace Unexplored.Game.GameObjects
{
    public class TextObject : GameObject
    {
        public TextObject(MapText text)
        {
            SetComponents(
                new TextRendererComponent(text)
                );
        }
    }
}
