using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Unexplored.Core;
using Unexplored.Core.Components;
using Unexplored.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Controllers
{
    class HeroController : BasicController<HeroObject>
    {
        InputComponent input;
        HeroObject hero;
        double timeout = 0;
        bool dir = false;

        public HeroController(HeroObject hero)
            : base(hero)
        {
            input = ComponentManager.Instance.Get<InputComponent>();
            this.hero = hero;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 speed = Vector2.Zero;

            if (input.KeyboardLastState.IsKeyDown(Keys.A) &&
                input.KeyboardCurrentState.IsKeyUp(Keys.A))
                ;

            if (input.CurrentKeyboardIsDown(Keys.A) /*&& !hero.Rigidbody.LeftWall*/)
            {
                hero.MoveLeft();
            }
            else
                hero.UnlockLeft();

            if (input.CurrentKeyboardIsDown(Keys.D) /* && !hero.Rigidbody.RightWall*/)
            {
                hero.MoveRight();
            }
            else
                hero.UnlockRight();

            if (input.OnceKeyboardIsDown(Keys.Space))
                hero.Jump();

            /*if (input.OnceKeyboardIsDown(Keys.R)) Constants.BackgroundColor.R++;
            if (input.OnceKeyboardIsDown(Keys.G)) Constants.BackgroundColor.G++;
            if (input.OnceKeyboardIsDown(Keys.B)) Constants.BackgroundColor.B++;
            if (input.OnceKeyboardIsDown(Keys.T)) Constants.BackgroundColor.R--;
            if (input.OnceKeyboardIsDown(Keys.H)) Constants.BackgroundColor.G--;
            if (input.OnceKeyboardIsDown(Keys.N)) Constants.BackgroundColor.B--;*/
        }
    }
}
