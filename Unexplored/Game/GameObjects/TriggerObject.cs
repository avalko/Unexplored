using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core.Base;
using Unexplored.Core.Components;
using Unexplored.Core.Physics;
using Unexplored.Game.Components;

namespace Unexplored.Game.GameObjects
{
    public class TriggerObject : GameObject
    {
        public TriggerObject()
        {
            SetComponents(
                new TriggerControllerComponent(),
                new ColliderComponent(false, MapCollider.Create(Vector2.Zero, Vector2.Zero, "trigger")) {  ForceNotify = true }
                );
        }
    }
}
