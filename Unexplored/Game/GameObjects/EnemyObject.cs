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
    public class EnemyObject : GameObject
    {
        public EnemyObject() : base()
        {
            SetComponents(
                new EnemyControllerComponent(),
                new RigidbodyComponent(true, true),
                new ColliderComponent(true, MapCollider.Create(new Vector2(3, 0), new Vector2(3, 0))),
                new ColliderComponent(false, MapCollider.Create(new Vector2(-9, -4), new Vector2(-9, -4))),
                new SpriteAnimatorComponent(),
                new SpriteRendererComponent()
            );
        }
    }
}
