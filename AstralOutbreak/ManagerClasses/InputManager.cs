using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Keys downButton { get; set; }
        public Keys rollButton { get; set; }
        public Keys dashButton { get; set; }
        public Keys pauseButton { get; set; }

        //Button Statuses
        public ButtonStatus JumpButtonState { get; private set; }
        public ButtonStatus LeftButtonState { get; private set; }
        public ButtonStatus RightButtonState { get; private set; }
        public ButtonStatus DownButtonState { get; private set; }
        public ButtonStatus RollButtonState { get; private set; }
        public ButtonStatus DashButtonState { get; private set; }
        public ButtonStatus ShootButtonState { get; private set; }
        public ButtonStatus PauseButtonState { get; private set; }

        //Mouse stuff
        public int MouseX
        {
            get
            {
                if (Game1.Graphics.IsFullScreen)
                {
                    return mouseState.Position.X * Game1.Graphics.PreferredBackBufferWidth / Game1.Graphics.GraphicsDevice.DisplayMode.Width;
                }
                else
                    return mouseState.Position.X;
            }
        }
        public int MouseY
        {
            get
            {
                if (Game1.Graphics.IsFullScreen)
                {
                    return mouseState.Position.Y * Game1.Graphics.PreferredBackBufferHeight / Game1.Graphics.GraphicsDevice.DisplayMode.Height;
                }
                else
                    return mouseState.Position.Y;
            }
        }
        public ButtonStatus M1State
        {
            get
            {
                if (mouseState.LeftButton.Equals(ButtonState.Pressed))
                {
                    if (prevMouseState.LeftButton.Equals(ButtonState.Released)) return ButtonStatus.Pressed;
                    else return ButtonStatus.Held;
                }
                else return ButtonStatus.Unpressed;
            }
        }
        public ButtonStatus M2State
        {
            get
            {
                if (mouseState.RightButton.Equals(ButtonState.Pressed))
                {
                    if (prevMouseState.RightButton.Equals(ButtonState.Released)) return ButtonStatus.Pressed;
                    else return ButtonStatus.Held;
                }
                else return ButtonStatus.Unpressed;
            }
        }
        
        public InputManager() //Hard codes keys - Called on first boot or if JSON fails to load from file
        {
            jumpButton = Keys.W;
            leftButton = Keys.A;
            rightButton = Keys.D;
            rollButton = Keys.LeftShift;
            dashButton = Keys.Space;
            pauseButton = Keys.Escape;
            downButton = Keys.S;
        }

        public void Update() //Updates all buttons
        {
            prevKeyState = keyState;
            prevMouseState = mouseState;
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            JumpButtonState = UpdateKey(jumpButton);
            LeftButtonState = UpdateKey(leftButton);
            RightButtonState = UpdateKey(rightButton);
            RollButtonState = UpdateKey(rollButton);
            DashButtonState = UpdateKey(dashButton);
            PauseButtonState = UpdateKey(pauseButton);
            DownButtonState = UpdateKey(downButton);
            ShootButtonState = M1State;
        }

        /// <summary>
        /// Updates a key state based on a key's status
        /// </summary>
        /// <param name="state">The button state to update</param>
        /// <param name="button">The key used to determine the state</param>
        private ButtonStatus UpdateKey(Keys key)
        {
            if (keyState.IsKeyDown(key))
            {
                if (prevKeyState.IsKeyDown(key)) return ButtonStatus.Held;
                else return ButtonStatus.Pressed;
            }
            else return ButtonStatus.Unpressed;
        }



        public void SaveToFile()
        {
            StreamWriter savefile = new StreamWriter("config.txt");
            savefile.WriteLine(JsonConvert.SerializeObject(this));
            savefile.Close();
        }
    }
}
