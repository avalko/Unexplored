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
    public class InputComponent : BehaviorComponent
    {
        public bool Right;
        public bool Left;
        public bool Up;
        public bool Down, Bottom;
        public bool Jump;
        public bool Attack;

        public override void Update(GameTime gameTime)
        {
            Right = Input.Is(Input.InputType.Right) || Input.Is(Input.InputType.SecondLeft);
            Left = Input.Is(Input.InputType.Left) || Input.Is(Input.InputType.FirstLeft);
            Up = Input.Is(Input.InputType.Up);
            Down = Input.Is(Input.InputType.Down);
            Bottom = Input.Is(Input.InputType.Bottom);
            Jump = Input.IsOnce(Input.InputType.Jump) || Input.OnceKeyboardIsDown(Microsoft.Xna.Framework.Input.Keys.Up)
                || Input.OnceKeyboardIsDown(Microsoft.Xna.Framework.Input.Keys.W) || Input.IsOnce(Input.InputType.SecondRight);
            Attack = Input.IsOnce(Input.InputType.Attack) || Input.OnceKeyboardIsDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) || Input.IsOnce(Input.InputType.FirstRight);
        }
    }
}
