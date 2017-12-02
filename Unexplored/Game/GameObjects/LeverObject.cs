using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;
using Unexplored.Core.Types;
using Unexplored.Game.Components;

namespace Unexplored.Game.GameObjects
{
    public class LeverObject : GameObject
    {
        public LeverObject() : base()
        {
            SetComponents(
                new ObjectStateComponent(),
                new LeverControllerComponent(),
                new TriggerControllerComponent(),
                new ColliderComponent(false, MapCollider.Create(new Vector2(-5, 0), new Vector2(-5, 0))) { ForceNotify = true },
                new SpriteAnimatorComponent(),
                new SpriteRendererComponent(173)
                );
        }
    }
}
