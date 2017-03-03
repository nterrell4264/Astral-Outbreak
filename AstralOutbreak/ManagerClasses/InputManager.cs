using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public enum ButtonStatus { Unpressed, Pressed, Held}
    public class InputManager
    {
        private KeyboardState keyState; //Key state this frame
        private KeyboardState prevKeyState; //Key state last frame
        private MouseState mouseState;
        private MouseState prevMouseState;

        //Button Bindings
        public Keys jumpButton { get; set; }
        public Keys leftButton { get; set; }
        public Keys rightButton { get; set; }
        public Keys rollButton { get; set; }
        public Keys dashButton { get; set; }

        //Button Statuses
        public ButtonStatus JumpButtonState { get; private set; }
        public ButtonStatus LeftButtonState { get; private set; }
        public ButtonStatus RightButtonState { get; private set; }
        public ButtonStatus RollButtonState { get; private set; }
        public ButtonStatus DashButtonState { get; private set; }
        public ButtonStatus ShootButtonState { get; private set; }


        //Default constructor
        public InputManager()
        {
            //Hard codes keys - REMOVE when we get config working
            jumpButton = Keys.W;
            leftButton = Keys.A;
            rightButton = Keys.D;
            rollButton = Keys.LeftShift;
            dashButton = Keys.Space;
        }

        public void Update() //Updates all buttons
        {
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            UpdateKey(JumpButtonState, jumpButton);
            UpdateKey(LeftButtonState, leftButton);
            UpdateKey(RightButtonState, rightButton);
            UpdateKey(RollButtonState, rollButton);
            UpdateKey(DashButtonState, dashButton);
            //Updates mouse separately because it's a special snowflake
            if (mouseState.LeftButton.Equals(ButtonState.Pressed))
            {
                if (prevMouseState.LeftButton.Equals(ButtonState.Released)) ShootButtonState = ButtonStatus.Held;
                else ShootButtonState = ButtonStatus.Pressed;
            }
            else ShootButtonState = ButtonStatus.Unpressed;

            prevKeyState = keyState;
            prevMouseState = mouseState;
        }

        /// <summary>
        /// Updates a key state based on a key's status
        /// </summary>
        /// <param name="state">The button state to update</param>
        /// <param name="button">The key used to determine the state</param>
        private void UpdateKey(ButtonStatus state, Keys key)
        {
            if (keyState.IsKeyDown(key))
            {
                if (!prevKeyState.IsKeyDown(key)) state = ButtonStatus.Held;
                else state = ButtonStatus.Pressed;
            }
            else state = ButtonStatus.Unpressed;
        }

        private void ReConfig() { } //To be used for config files
    }
}
