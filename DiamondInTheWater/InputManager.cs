using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondInTheWater
{
    /// <summary>
    /// The mouse buttons on a mouse.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// The left mouse button.
        /// </summary>
        LEFT,

        /// <summary>
        /// The right mouse button.
        /// </summary>
        RIGHT,

        /// <summary>
        /// The middle mouse button.
        /// </summary>
        MIDDLE,
    }

    /// <summary>
    /// Determines different states of input from the user.
    /// </summary>
    public class InputManager
    {
        private KeyboardState currentKeyState, prevKeyState;
        private MouseState currentMouseState, prevMouseState;

        private static InputManager instance;

        /// <summary>
        /// Singleton instance of the inputmanager.
        /// </summary>
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputManager();

                return instance;
            }
        }

        /// <summary>
        /// Updates the instance of the inputmanager.
        /// </summary>
        public void Update()
        {
            prevKeyState = currentKeyState;
            prevMouseState = currentMouseState;

            //if (GameManager.Instance.isTransitioning)
            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Determines whether the mouse is being pressed down.
        /// </summary>
        /// <param name="button">The button checked.</param>
        /// <returns></returns>
        public bool IsMouseDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LEFT:
                    return currentMouseState.LeftButton.Equals(ButtonState.Pressed);
                case MouseButton.RIGHT:
                    return currentMouseState.RightButton.Equals(ButtonState.Pressed);
                case MouseButton.MIDDLE:
                    return currentMouseState.MiddleButton.Equals(ButtonState.Pressed);
                default:
                    // placeholder
                    return currentMouseState.LeftButton.Equals(ButtonState.Pressed);
            }
        }

        /// <summary>
        /// Determines whether the mouse is not being pressed down.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsMouseUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LEFT:
                    return currentMouseState.LeftButton.Equals(ButtonState.Released);
                case MouseButton.RIGHT:
                    return currentMouseState.RightButton.Equals(ButtonState.Released);
                case MouseButton.MIDDLE:
                    return currentMouseState.MiddleButton.Equals(ButtonState.Released);
                default:
                    // placeholder
                    return currentMouseState.LeftButton.Equals(ButtonState.Released);
            }
        }

        /// <summary>
        /// Determines whether the mouse has been clicked.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsMouseClicked(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LEFT:
                    return currentMouseState.LeftButton.Equals(ButtonState.Pressed) &&
                        prevMouseState.LeftButton.Equals(ButtonState.Released);
                case MouseButton.RIGHT:
                    return currentMouseState.RightButton.Equals(ButtonState.Pressed) &&
                        prevMouseState.RightButton.Equals(ButtonState.Released);
                case MouseButton.MIDDLE:
                    return currentMouseState.MiddleButton.Equals(ButtonState.Pressed) &&
                        prevMouseState.MiddleButton.Equals(ButtonState.Released);
                default:
                    // placeholder
                    return currentMouseState.LeftButton.Equals(ButtonState.Pressed) &&
                        prevMouseState.LeftButton.Equals(ButtonState.Released);
            }
        }

        /// <summary>
        /// Determines whether the key(s) have been pressed.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the key(s) are released.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the key(s) are down.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }
    }
}
