using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core
{
    public static class Input
    {
        public enum InputType
        {
            Vertical,
            Horizontal,
            Up,
            Down,
            Left,
            Right,
            Jump,
            Attack,
            Enter,
            Back,
            Bottom,
        }

        public static MouseState MouseCurrentState { get; private set; }
        public static MouseState MouseLastState { get; private set; }
        public static KeyboardState KeyboardCurrentState { get; private set; }
        public static KeyboardState KeyboardLastState { get; private set; }
        public static JoystickState JoystickCurrentState { get; private set; }
        public static JoystickState JoystickLastState { get; private set; }

        public static bool CurrentKeyboardIsDown(Keys key) => KeyboardCurrentState.IsKeyDown(key);
        public static bool LastKeyboardIsDown(Keys key) => KeyboardLastState.IsKeyDown(key);
        public static bool OnceKeyboardIsDown(Keys key) => CurrentKeyboardIsDown(key) && KeyboardLastState.IsKeyUp(key);

        public static bool CurrentMouseRightDown => MouseCurrentState.RightButton == ButtonState.Pressed;
        public static bool CurrentMouseLeftDown => MouseCurrentState.LeftButton == ButtonState.Pressed;
        public static bool LastMouseRightDown => MouseLastState.RightButton == ButtonState.Pressed;
        public static bool LastMouseLeftDown => MouseLastState.LeftButton == ButtonState.Pressed;
        public static bool OnceMouseRightDown => CurrentMouseRightDown && !LastMouseRightDown;
        public static bool OnceMouseLeftDown => CurrentMouseLeftDown && !LastMouseLeftDown;

        private static Vector2 currentMousePosition;
        public static Vector2 CurrentMousePosition => currentMousePosition;

        private static Dictionary<InputType, float> currentInputValues = new Dictionary<InputType, float>();
        private static Dictionary<InputType, float> lastInputValues = new Dictionary<InputType, float>();
        private static Dictionary<InputType, bool> currentInputStatus = new Dictionary<InputType, bool>();
        private static Dictionary<InputType, bool> lastInputStatus = new Dictionary<InputType, bool>();

        public static float Get(InputType inputType)
        {
            if (currentInputValues.ContainsKey(inputType))
                return currentInputValues[inputType];
            return 0;
        }

        public static float GetLast(InputType inputType)
        {
            if (lastInputValues.ContainsKey(inputType))
                return lastInputValues[inputType];
            return 0;
        }

        public static bool Is(InputType inputType)
        {
            if (currentInputStatus.ContainsKey(inputType))
                return currentInputStatus[inputType];
            return false;
        }

        public static bool IsLast(InputType inputType)
        {
            if (lastInputStatus.ContainsKey(inputType))
                return lastInputStatus[inputType];
            return false;
        }

        public static bool IsOnce(InputType inputType)
        {
            if (currentInputStatus.ContainsKey(inputType))
                return currentInputStatus[inputType] && !lastInputStatus[inputType];
            return false;
        }

        public static event Action OnClick;

        public static void Update(GameTime gameTime)
        {
            _AfterLastGameUpdate();
            _PreventGameUpdate();
        }

        private static bool CheckJoystickAxeBiggerZero(int axe)
        {
            return JoystickCurrentState.IsConnected && JoystickCurrentState.Axes[axe] > 0;
        }

        private static bool CheckJoystickAxeLessZero(int axe)
        {
            return JoystickCurrentState.IsConnected && JoystickCurrentState.Axes[axe] < 0;
        }

        private static bool CheckJoystickButton(int button)
        {
            return JoystickCurrentState.IsConnected && JoystickCurrentState.Buttons[button] == ButtonState.Pressed;
        }

        private static void _PreventGameUpdate()
        {
            MouseCurrentState = Mouse.GetState();
            KeyboardCurrentState = Keyboard.GetState();
            JoystickCurrentState = Joystick.GetState(0);
            bool isConnectedJoystick = JoystickCurrentState.IsConnected;

            float vertical = 0;
            float horizontal = 0;
            bool left = false, right = false;
            bool up = false, down = false;
            bool jump = false, attack = false;
            bool bottom = false;

            // Up
            if (CurrentKeyboardIsDown(Keys.W) || CurrentKeyboardIsDown(Keys.Up) ||
                CheckJoystickAxeLessZero(1))
            { vertical = +1; up = true; }
            // Down
            else if (CurrentKeyboardIsDown(Keys.S) || CurrentKeyboardIsDown(Keys.Down) ||
                CheckJoystickAxeBiggerZero(1))
            { vertical = -1; down = true; }

            // Right
            if (CurrentKeyboardIsDown(Keys.D) || CurrentKeyboardIsDown(Keys.Right) ||
                CheckJoystickAxeBiggerZero(0))
            { horizontal = +1; right = true; }
            // Left
            else if (CurrentKeyboardIsDown(Keys.A) || CurrentKeyboardIsDown(Keys.Left) ||
                CheckJoystickAxeLessZero(0))
            { horizontal = -1; left = true; }

            // Jump
            if (CurrentKeyboardIsDown(Keys.Space)
                || CheckJoystickButton(0))
                jump = true;

            // Attack
            if (CurrentKeyboardIsDown(Keys.Z)
                || CheckJoystickButton(1))
                attack = true;

            // Bottom
            if (CurrentKeyboardIsDown(Keys.S) || CurrentKeyboardIsDown(Keys.Down) ||
                CheckJoystickButton(4))
                bottom = true;

            currentInputValues[InputType.Vertical] = vertical;
            currentInputValues[InputType.Horizontal] = horizontal;
            currentInputStatus[InputType.Up] = up;
            currentInputStatus[InputType.Down] = down;
            currentInputStatus[InputType.Bottom] = bottom;
            currentInputStatus[InputType.Left] = left;
            currentInputStatus[InputType.Right] = right;
            currentInputStatus[InputType.Jump] = jump;
            currentInputStatus[InputType.Attack] = attack;

            currentMousePosition = MouseCurrentState.Position.ToVector2();
        }

        private static void _AfterLastGameUpdate()
        {
            MouseLastState = MouseCurrentState;
            KeyboardLastState = KeyboardCurrentState;
            JoystickLastState = JoystickCurrentState;

            lastInputValues?.Clear(); lastInputValues = null;
            lastInputStatus?.Clear(); lastInputStatus = null;

            lastInputValues = new Dictionary<InputType, float>(currentInputValues);
            lastInputStatus = new Dictionary<InputType, bool>(currentInputStatus);
        }
    }
}