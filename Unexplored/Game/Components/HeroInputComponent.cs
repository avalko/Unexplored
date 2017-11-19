using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core;

namespace Unexplored.Game.Components
{
    public class HeroInputComponent : BehaviorComponent
    {
        public bool Right;
        public bool Left;
        public bool Up;
        public bool Down;
        public bool Jump;
        public bool Attack;

        public override void Update(GameTime gameTime)
        {
            Right = Input.Is(Input.InputType.Right);
            Left = Input.Is(Input.InputType.Left);
            Up = Input.Is(Input.InputType.Up);
            Down = Input.Is(Input.InputType.Down);
            Jump = Input.IsOnce(Input.InputType.Jump);
            Attack = Input.IsOnce(Input.InputType.Attack);
        }
    }
}
