using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
   class MenuManager
    {
        public static List<MenuContent> items { get; private set; } //List of things to draw
        private GameState prevState; //Holds last frame's GameState for automatic updating
        private Game1 main; //Used for Exit command

        private MenuContent updateMenu; //Used for defining a MenuContent with update code.
        private MenuString updateString; //Used for defining a MenuString with update code.

        public MenuManager(Game1 game) //Automatically loads the main menu on instantiating.
        {
            items = new List<MenuContent>();
            main = game;
            LoadMain();
            prevState = GameState.MainMenu;
        }

        //Load options
        private void LoadMain() //Loads main menu assets
        {
            items.Add(new MenuButton(300, 250, 150, 75, "StartButton", () => { RoomManager.Active.ReloadRoom(); Game1.CurrentState = GameState.Playing;}));
            items.Add(new MenuButton(200, 350, 150, 75, "OptionsButton", () => {
                Game1.CurrentState = GameState.OptionsMenu;
                Game1.prevMenu = GameState.MainMenu;
            }));
            items.Add(new MenuButton(400, 350, 150, 75, "QuitButton", () => { main.Exit(); }));
        }
        private void LoadOptions() //Loads options menu assets
        {
            //Backgrounds
            items.Add(new MenuContent(475, 50, "SmallMenuBG"));
            items.Add(new MenuContent(475, 100, "SmallMenuBG"));
            items.Add(new MenuContent(475, 150, "SmallMenuBG"));
            items.Add(new MenuContent(475, 200, "SmallMenuBG"));
            items.Add(new MenuContent(475, 250, "SmallMenuBG"));
            items.Add(new MenuContent(475, 300, "SmallMenuBG"));
            items.Add(new MenuContent(475, 350, "SmallMenuBG"));
            items.Add(new MenuContent(50, 50, "LargeMenuBG"));
            items.Add(new MenuContent(50, 200, "LargeMenuBG"));
            //Text
            items.Add(new MenuString(500, 55, "Left = A", "font"));
            items.Add(new MenuString(500, 105, "Right = D", "font"));
            items.Add(new MenuString(500, 155, "Jump = W", "font"));
            items.Add(new MenuString(500, 205, "Dash = Space", "font"));
            items.Add(new MenuString(500, 255, "Roll = LShift", "font"));
            items.Add(new MenuString(500, 305, "Shoot = M1", "font"));
            items.Add(new MenuString(500, 355, "Pause = Escape", "font"));
            //Other
            items.Add(new MenuButton(50, 350, 150, 75, "ResumeButton", () => { Game1.CurrentState = Game1.prevMenu; }));
        }
        private void LoadPause() //Loads pause menu assets
        {
            items.Add(new MenuButton(300, 50, 150, 75, "ResumeButton", () => { Game1.CurrentState = GameState.Playing; }));
            items.Add(new MenuButton(300, 200, 150, 75, "OptionsButton", () => {
                Game1.CurrentState = GameState.OptionsMenu;
                Game1.prevMenu = GameState.PauseMenu;
            }));
            items.Add(new MenuButton(300, 350, 150, 75, "QuitButton", () => { Game1.ResetGame(); Game1.CurrentState = GameState.MainMenu; }));
        }
        private void LoadScreen() //Loads loading screen assets
        {

        }
        private void LoadSave() //Loads save menu assets
        {

        }
        private void LoadGameOver() //Loads game over menu assets
        {
            items.Add(new MenuButton(32, 182, 150, 75, "RetryButton", () => { RoomManager.Active.ReloadRoom();  Game1.CurrentState = GameState.Playing; })); 
            items.Add(new MenuButton(32, 282, 150, 75, "QuitButton", () => { Game1.ResetGame(); Game1.CurrentState = GameState.MainMenu; }));
        }
        private void LoadUI() //Loads GUI assets
        {
            //Health
            items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width / 2 - 84, main.GraphicsDevice.Viewport.Height - 50, "HudBG"));
            updateString = new MenuString(main.GraphicsDevice.Viewport.Width / 2 - 68, main.GraphicsDevice.Viewport.Height - 45, RoomManager.Active.PlayerOne.Health.ToString() + "/10 HP",
                "UIfont", true);
            updateString.SetSpecialCode(() => {
                updateString.text = RoomManager.Active.PlayerOne.Health.ToString() + "/10 HP";
                updateString.Location.X = (main.GraphicsDevice.Viewport.Width - 17 * updateString.text.Length) / 2;
            });
            items.Add(updateString);
            //Upgrades
            items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 61, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
            items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 47, main.GraphicsDevice.Viewport.Height - 49, "rollIcon"));
            if (RoomManager.Active.PlayerOne.MyUpgrades.HasFlag(Upgrades.Dash))
            {
                items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 122, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
                items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 108, main.GraphicsDevice.Viewport.Height - 49, "dashIcon"));
            }

        }

        public void Update()
        {
            if (prevState == Game1.CurrentState) {
                foreach (MenuContent content in items)
                {
                    if (content is MenuButton) (content as MenuButton).CheckClicked();
                    if (content.Updatable) content.SpecialCode();
                }
            }
            if(prevState != Game1.CurrentState) //Looks like an else, but the above statement can change the GameState.
            {
                items.Clear();
                switch (Game1.CurrentState)
                {
                    case (GameState.MainMenu):
                        {
                            LoadMain();
                            break;
                        }
                    case (GameState.OptionsMenu):
                        {
                            LoadOptions();
                            break;
                        }
                    case (GameState.PauseMenu):
                        {
                            LoadPause();
                            break;
                        }
                    case (GameState.LoadMenu):
                        {
                            LoadScreen();
                            break;
                        }
                    case (GameState.SaveMenu):
                        {
                            LoadSave();
                            break;
                        }
                    case (GameState.GameOverMenu):
                        {
                            LoadGameOver();
                            break;
                        }
                    default:
                        {
                            LoadUI();
                            break;
                        }
                }
            }
            prevState = Game1.CurrentState;                                                                                                                                                                                                                                                                                                                             
        }
    }
}
