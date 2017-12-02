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
using Unexplored.Game.Attributes;
using Unexplored.Game.Components;

namespace Unexplored.Game.GameObjects
{
    public class HeroObject : GameObject
    {
        public HeroObject() : base()
        {
            SetComponents(
                new InputComponent(),
                new HeroControllerComponent(),
                new RigidbodyComponent(true, true),
                new ColliderComponent(true, MapCollider.Create(new Vector2(2, 0), new Vector2(2, 0))),
                new SpriteAnimatorComponent(),
                new SpriteRendererComponent()
                );
        }
    }
}
