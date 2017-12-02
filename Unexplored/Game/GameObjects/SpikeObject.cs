using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Attributes;
using Unexplored.Core.Base;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;
using Unexplored.Core.Types;
using Unexplored.Game.Components;

namespace Unexplored.Game.GameObjects
{

    public class SpikeObject : GameObject
    {
        public SpikeObject()
        {
            SetComponents(
                new ColliderComponent(false, MapCollider.Create(new Vector2(2, 13), new Vector2(2, 0))),
                new SpriteRendererComponent(10)
            );
        }
    }
}
