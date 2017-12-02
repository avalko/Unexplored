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

    public class WarpObject : GameObject
    {
        public WarpObject() : base()
        {
            SetComponents(
                new WarpControllerComponent(),
                new ColliderComponent(false, MapCollider.Create(Vector2.Zero, Vector2.Zero)),
                new SpriteRendererComponent(335)
            );
        }
    }
}
