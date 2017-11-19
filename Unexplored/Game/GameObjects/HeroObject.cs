using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;
using Unexplored.Game.Attributes;
using Unexplored.Game.Components;

namespace Unexplored.Game.GameObjects
{
    public class HeroObject : GameObject
    {
        public HeroObject()
        {
            SetComponents(
                new HeroInputComponent(),
                new HeroControllerComponent(),
                new RigidbodyComponent(),
                new ColliderComponent(true, Collider.Create(new Vector2(2, 0), new Vector2(2, 0))),
                new SpriteAnimatorComponent(),
                new SpriteRendererComponent()
                );
        }
    }
}
