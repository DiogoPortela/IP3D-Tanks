using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TankProject
{
    /// <summary>
    /// Input class to handle all user I/O.
    /// </summary>
    class Input
    {
        internal static KeyboardState KeyboardState;
        internal static KeyboardState LastFrameKeyboardState;
        internal static MouseState MouseState;
        internal static MouseState LastFrameMouseState;

        internal static float xAxis;
        internal static float yAxis;
        private static Point lastMousePosition;

        //--------------------Functions--------------------//

        internal static void Start()
        {
            Mouse.SetPosition(Game1.graphics.PreferredBackBufferWidth / 2, Game1.graphics.PreferredBackBufferWidth / 2);

            KeyboardState = LastFrameKeyboardState = Keyboard.GetState();
            MouseState = LastFrameMouseState = Mouse.GetState();
            lastMousePosition = MouseState.Position;

            xAxis = 0.0f;
            yAxis = 0.0f;
        }
        internal static bool IsPressedDown(Keys k)
        {
            if (KeyboardState.IsKeyDown(k))
                return true;
            return false;
        }
        internal static bool WasPressed(Keys k)
        {
            if (KeyboardState.IsKeyDown(k) && LastFrameKeyboardState.IsKeyUp(k))
                return true;
            return false;
        }
        internal static bool WasReleased(Keys k)
        {
            if (LastFrameKeyboardState.IsKeyDown(k) && KeyboardState.IsKeyUp(k))
                return true;
            return false;
        }
        internal static float MouseWheelValue()
        {
            return (MouseState.ScrollWheelValue - LastFrameMouseState.ScrollWheelValue);
        }

        //--------------------Update&Draw--------------------//

        internal static void Update()
        {
            LastFrameKeyboardState = KeyboardState;
            LastFrameMouseState = MouseState;
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();

            yAxis = MouseState.Position.Y - lastMousePosition.Y;
            xAxis = MouseState.Position.X - lastMousePosition.X;

            lastMousePosition = MouseState.Position;
        }
        internal static void LateUpdate()
        {
            Mouse.SetPosition(Game1.graphics.PreferredBackBufferWidth / 2, Game1.graphics.PreferredBackBufferHeight / 2);
        }
    }
}
