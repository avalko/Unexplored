using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Unexplored.Core.Components.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Components
{
    [GameComponent(int.MinValue)]
    public class InputComponent : CoreComponent
    {
        public enum InputType
        {
            Vertical,
            Horizontal,
        }

        public InputComponent(MainGame game) : base(game)
        {
        }

        public MouseState MouseCurrentState { get; private set; }
        public MouseState MouseLastState { get; private set; }
        public KeyboardState KeyboardCurrentState { get; private set; }
        public KeyboardState KeyboardLastState { get; private set; }

        public bool CurrentKeyboardIsDown(Keys key) => KeyboardCurrentState.IsKeyDown(key);
        public bool LastKeyboardIsDown(Keys key) => KeyboardLastState.IsKeyDown(key);
        public bool OnceKeyboardIsDown(Keys key) => CurrentKeyboardIsDown(key) && KeyboardLastState.IsKeyUp(key);

        public bool CurrentMouseRightDown => MouseCurrentState.RightButton == ButtonState.Pressed;
        public bool CurrentMouseLeftDown => MouseCurrentState.LeftButton == ButtonState.Pressed;
        public bool LastMouseRightDown => MouseLastState.RightButton == ButtonState.Pressed;
        public bool LastMouseLeftDown => MouseLastState.LeftButton == ButtonState.Pressed;
        public bool OnceMouseRightDown => CurrentMouseRightDown && !LastMouseRightDown;
        public bool OnceMouseLeftDown => CurrentMouseLeftDown && !LastMouseLeftDown;

        private Vector2 currentMousePosition;
        public Vector2 CurrentMousePosition => currentMousePosition;

        private Dictionary<InputType, float> currentInput = new Dictionary<InputType, float>();
        private Dictionary<InputType, float> lastInput = new Dictionary<InputType, float>();

        public float Input(InputType inputType)
        {
            if (currentInput.ContainsKey(inputType))
                return currentInput[inputType];
            return 0;
        }

        public float LastInput(InputType inputType)
        {
            if (lastInput.ContainsKey(inputType))
                return lastInput[inputType];
            return 0;
        }

        public event Action OnClick;

        public override void Update(GameTime gameTime)
        {
            _AfterLastGameUpdate();
            _PreventGameUpdate();
        }

        private void _PreventGameUpdate()
        {
            MouseCurrentState = Mouse.GetState();
            KeyboardCurrentState = Keyboard.GetState();

            float vertical = 0;
            float horizontal = 0;

            // Up
            if (CurrentKeyboardIsDown(Keys.W) || CurrentKeyboardIsDown(Keys.Up))
                vertical = +1;
            // Down
            else if (CurrentKeyboardIsDown(Keys.S) || CurrentKeyboardIsDown(Keys.Down))
                vertical = -1;

            // Right
            if (CurrentKeyboardIsDown(Keys.D) || CurrentKeyboardIsDown(Keys.Right))
                horizontal = +1;
            // Left
            else if (CurrentKeyboardIsDown(Keys.A) || CurrentKeyboardIsDown(Keys.Left))
                horizontal = -1;

            currentInput[InputType.Vertical] = vertical;
            currentInput[InputType.Horizontal] = horizontal;

            currentMousePosition = MouseCurrentState.Position.ToVector2();
        }

        private void _AfterLastGameUpdate()
        {
            MouseLastState = MouseCurrentState;
            KeyboardLastState = KeyboardCurrentState;
            lastInput = new Dictionary<InputType, float>(currentInput);
        }
    }
}