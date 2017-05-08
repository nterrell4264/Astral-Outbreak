using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public enum ButtonStatus { Unpressed, Pressed, Held}
    public enum ActionButton { JumpButton, LeftButton, DownButton, RightButton, RollButton, DashButton, PauseButton}
    public class InputManager
    {
        private KeyboardState keyState; //Key state this frame
        private KeyboardState prevKeyState; //Key state last frame
        private MouseState mouseState; //Mouse state this frame
        private MouseState prevMouseState; //Mouse state last frame
        public bool checkUpdate = false;
        public ActionButton updateKey;

        private List<KeySet> keyList;
        public KeySet this[ActionButton button]
        {
            get { return keyList[(int)button]; }
        }
        public ButtonStatus ShootButtonState { get; private set; }

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
        
        //Constructor
        public InputManager() //Hard codes keys - Called on first boot or if JSON fails to load from file
        {
            keyList = new List<KeySet>();
            keyList.Add(new KeySet(ActionButton.JumpButton, Keys.W));
            keyList.Add(new KeySet(ActionButton.LeftButton, Keys.A));
            keyList.Add(new KeySet(ActionButton.DownButton, Keys.S));
            keyList.Add(new KeySet(ActionButton.RightButton, Keys.D));
            keyList.Add(new KeySet(ActionButton.RollButton, Keys.LeftShift));
            keyList.Add(new KeySet(ActionButton.DashButton, Keys.Space));
            keyList.Add(new KeySet(ActionButton.PauseButton, Keys.Escape));
        }

        //Methods
        public void Update() //Updates all buttons
        {
            prevKeyState = keyState;
            prevMouseState = mouseState;
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (Game1.CurrentState == GameState.OptionsMenu)
            {
                if (checkUpdate)
                {
                    foreach (Keys key in keyState.GetPressedKeys())
                    {
                        if (!prevKeyState.IsKeyDown(key))
                        {
                            keyList[(int)updateKey].Key = key;
                            checkUpdate = false;
                            Game1.menuManager.Reload();
                            break;
                        }
                    }
                }
            }
            if (Game1.CurrentState == GameState.Playing)
            {
                foreach (KeySet action in keyList) UpdateAction(action);
                ShootButtonState = M1State;
            }
            UpdateAction(keyList[(int)ActionButton.PauseButton]);
        }

        private void UpdateAction(KeySet action)
        {
            if (keyState.IsKeyDown(action.Key))
            {
                if (prevKeyState.IsKeyDown(action.Key)) action.Status = ButtonStatus.Held;
                else action.Status = ButtonStatus.Pressed;
            }
            else action.Status = ButtonStatus.Unpressed;
        }

        public void SaveToFile()
        {
            StreamWriter savefile = new StreamWriter("config.txt");
            savefile.WriteLine(JsonConvert.SerializeObject(this));
            savefile.Close();
        }
    }
}
