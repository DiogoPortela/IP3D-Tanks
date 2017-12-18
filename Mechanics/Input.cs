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
        internal static GamePadState PlayerOneGamePadState;
        internal static GamePadState PlayerOneLastFrameGamePadState;
        internal static GamePadState PlayerTwoGamePadState;
        internal static GamePadState PlayerTwoLastFrameGamePadState;

        internal static float xAxis;
        internal static float yAxis;
        private static Point lastMousePosition;

        //--------------------Functions--------------------//

        internal static void Start()
        {
            
            Mouse.SetPosition(Game1.graphics.PreferredBackBufferWidth / 2, Game1.graphics.PreferredBackBufferWidth / 2);

            KeyboardState = LastFrameKeyboardState = Keyboard.GetState();
            MouseState = LastFrameMouseState = Mouse.GetState();
            PlayerOneGamePadState = PlayerOneLastFrameGamePadState = GamePad.GetState(PlayerIndex.One);
            PlayerTwoGamePadState = PlayerTwoLastFrameGamePadState = GamePad.GetState(PlayerIndex.Two);
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
        internal static bool IsPressedDown(Buttons b, PlayerIndex index)
        {
            if(index == PlayerIndex.One)
            {
                if (PlayerOneGamePadState.IsButtonDown(b))
                    return true;
            }
            else if(index == PlayerIndex.Two)
            {
                if (PlayerTwoGamePadState.IsButtonDown(b))
                    return true;
            }
            return false;
        }

        internal static bool WasPressed(Keys k)
        {
            if (KeyboardState.IsKeyDown(k) && LastFrameKeyboardState.IsKeyUp(k))
                return true;
            return false;
        }

        internal static bool WasPressed(Buttons b, PlayerIndex index)
        {
            if (index == PlayerIndex.One)
            {
                if (PlayerOneGamePadState.IsButtonDown(b) && PlayerOneLastFrameGamePadState.IsButtonUp(b))
                    return true;
            }
            else if (index == PlayerIndex.Two)
            {
                if (PlayerTwoGamePadState.IsButtonDown(b) && PlayerTwoLastFrameGamePadState.IsButtonUp(b))
                    return true;
            }
            return false;
        }

        internal static bool WasReleased(Keys k)
        {
            if (LastFrameKeyboardState.IsKeyDown(k) && KeyboardState.IsKeyUp(k))
                return true;
            return false;
        }

        internal static bool WasReleased(Buttons b, PlayerIndex index)
        {
            if (index == PlayerIndex.One)
            {
                if (PlayerOneLastFrameGamePadState.IsButtonDown(b) && PlayerOneGamePadState.IsButtonUp(b))
                    return true;
            }
            else if (index == PlayerIndex.Two)
            {
                if (PlayerTwoLastFrameGamePadState.IsButtonDown(b) && PlayerTwoGamePadState.IsButtonUp(b))
                    return true;
            }
            return false;
        }

        internal static float MouseWheelValue()
        {
            return (MouseState.ScrollWheelValue - LastFrameMouseState.ScrollWheelValue);
        }
        internal static bool LeftMouseClick()
        {
            return (MouseState.LeftButton == ButtonState.Pressed && LastFrameMouseState.LeftButton == ButtonState.Released);
        }

        //--------------------Update&Draw--------------------//

        internal static void Update()
        {
            LastFrameKeyboardState = KeyboardState;
            LastFrameMouseState = MouseState;
            PlayerOneLastFrameGamePadState = PlayerOneGamePadState;
            PlayerTwoLastFrameGamePadState = PlayerTwoGamePadState;
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            PlayerOneGamePadState = GamePad.GetState(PlayerIndex.One);
            PlayerTwoGamePadState = GamePad.GetState(PlayerIndex.Two);

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
