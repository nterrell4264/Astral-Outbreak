﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
   public class MenuManager
    {
        public static List<MenuContent> items { get; private set; } //List of things to draw
        private GameState prevState; //Holds last frame's GameState for automatic updating
        private Game1 main; //Used to accommodate for window size changes, as well as the Exit button.

        private int windowWidth;
        private int windowHeight;

        private MenuContent updateMenu; //Used for defining a MenuContent with update code.
        private MenuString updateString; //Used for defining a MenuString with update code.

        public MenuManager(Game1 game) //Automatically loads the main menu on instantiating.
        {
            items = new List<MenuContent>();
            main = game;
            windowWidth = game.GraphicsDevice.Viewport.Width;
            windowHeight = game.GraphicsDevice.Viewport.Height;
            LoadMain();
            prevState = GameState.MainMenu;
        }

        //Load options
        private void LoadMain() //Loads main menu assets
        {
            items.Add(new MenuButton(windowWidth / 2 - 175, 300, 150, 75, "NewButton", () => { DialogueManager.Update(Triggers.Start); RoomManager.Active.ReloadRoom(); Game1.CurrentState = GameState.Playing;}));
            items.Add(new MenuButton(windowWidth / 2 + 25, 300, 150, 75, "ResumeButton", () => { RoomManager.LoadGame("SaveData.dat"); Game1.CurrentState = GameState.Playing; }));
            items.Add(new MenuButton(windowWidth / 2 - 175, 400, 150, 75, "OptionsButton", () => {
                Game1.CurrentState = GameState.OptionsMenu;
                Game1.prevMenu = GameState.MainMenu;
            }));
            items.Add(new MenuButton(windowWidth / 2 + 25, 400, 150, 75, "QuitButton", () => { main.Exit(); }));
        }
        private void LoadOptions() //Loads options menu assets
        {
            //Backgrounds, some fo which which also trigger key rebinding
            items.Add(new MenuButton(475, 50, 300, 25, "SmallMenuBG", () => {
                Game1.Inputs.checkUpdate = true;
                Game1.Inputs.updateKey = Game1.Inputs[ActionButton.JumpButton].Button;
            }, layer: .3f));
            items.Add(new MenuButton(475, 100, 300, 50, "SmallMenuBG", () => {
                Game1.Inputs.checkUpdate = true;
                Game1.Inputs.updateKey = Game1.Inputs[ActionButton.LeftButton].Button;
            }));
            items.Add(new MenuButton(475, 150, 300, 50, "SmallMenuBG", () => {
                Game1.Inputs.checkUpdate = true;
                Game1.Inputs.updateKey = Game1.Inputs[ActionButton.DownButton].Button;
            }));
            items.Add(new MenuButton(475, 200, 300, 50, "SmallMenuBG", () => {
                Game1.Inputs.checkUpdate = true;
                Game1.Inputs.updateKey = Game1.Inputs[ActionButton.RightButton].Button;
            }));
            items.Add(new MenuButton(475, 250, 300, 50, "SmallMenuBG", () => {
                Game1.Inputs.checkUpdate = true;
                Game1.Inputs.updateKey = Game1.Inputs[ActionButton.DashButton].Button;
            }));
            items.Add(new MenuButton(475, 300, 300, 50, "SmallMenuBG", () => {
                Game1.Inputs.checkUpdate = true;
                Game1.Inputs.updateKey = Game1.Inputs[ActionButton.RollButton].Button;
            }));
            items.Add(new MenuButton(475, 350, 300, 50, "SmallMenuBG", () => {
                Game1.Inputs.checkUpdate = true;
                Game1.Inputs.updateKey = Game1.Inputs[ActionButton.PauseButton].Button;
            }));
            items.Add(new MenuContent(475, 400, "SmallMenuBG"));
            items.Add(new MenuContent(50, 50, "LargeMenuBG"));
            items.Add(new MenuContent(50, 200, "LargeMenuBG"));
            //Text
            items.Add(new MenuString(500, 55, "Jump = " + Game1.Inputs[ActionButton.JumpButton].Key.ToString(), "font"));
            items.Add(new MenuString(500, 105, "Left = " + Game1.Inputs[ActionButton.LeftButton].Key.ToString(), "font"));
            items.Add(new MenuString(500, 155, "Down = " + Game1.Inputs[ActionButton.DownButton].Key.ToString(), "font"));
            items.Add(new MenuString(500, 205, "Right = " + Game1.Inputs[ActionButton.RightButton].Key.ToString(), "font"));
            items.Add(new MenuString(500, 255, "Dash = " + Game1.Inputs[ActionButton.DashButton].Key.ToString(), "font"));
            items.Add(new MenuString(500, 305, "Roll = " + Game1.Inputs[ActionButton.RollButton].Key.ToString(), "font"));
            items.Add(new MenuString(500, 355, "Pause = " + Game1.Inputs[ActionButton.PauseButton].Key.ToString(), "font"));
            items.Add(new MenuString(500, 405, "Shoot = M1", "font"));
            //Other
            items.Add(new MenuButton(50, 350, 150, 75, "ResumeButton", () => {
                Game1.Inputs.SaveToFile();
                Game1.CurrentState = Game1.prevMenu;
            }));
        }
        private void LoadPause() //Loads pause menu assets
        {
            items.Add(new MenuButton(windowWidth / 2 - 75, windowHeight / 2 - 188, 150, 75, "ResumeButton", () => { Game1.CurrentState = GameState.Playing; }));
            items.Add(new MenuButton(windowWidth / 2 - 175, windowHeight / 2 - 38, 150, 75, "SaveButton", () => { Game1.CurrentState = GameState.SaveMenu; }));
            items.Add(new MenuButton(windowWidth / 2 + 25, windowHeight / 2 - 38, 150, 75, "OptionsButton", () => {
                Game1.CurrentState = GameState.OptionsMenu;
                Game1.prevMenu = GameState.PauseMenu;
            }));
            items.Add(new MenuButton(windowWidth / 2 - 75, windowHeight / 2 + 112, 150, 75, "QuitButton", () => { Game1.ResetGame(); Game1.CurrentState = GameState.MainMenu; }));
        }
        private void LoadScreen() //Loads loading screen assets
        {

        }
        private void LoadSave() //Loads save menu assets
        {
            items.Add(new MenuButton(windowWidth / 2 - 175, windowHeight / 2 - 38, 150, 75, "SaveButton", () => { RoomManager.SaveGame("SaveData.dat"); }));
            items.Add(new MenuButton(windowWidth / 2 + 25, windowHeight / 2 - 38, 150, 75, "LoadButton", () => { RoomManager.LoadGame("SaveData.dat"); }));
            items.Add(new MenuButton(windowWidth / 2 - 75, windowHeight / 2 + 62, 150, 75, "ResumeButton", () => { Game1.CurrentState = GameState.PauseMenu; }));
        }
        private void LoadGameOver() //Loads game over menu assets
        {
            items.Add(new MenuButton(windowWidth / 2 - 75, 182, 150, 75, "RetryButton", () => { RoomManager.Active.ReloadRoom();  Game1.CurrentState = GameState.Playing; })); 
            items.Add(new MenuButton(windowWidth / 2 - 75, 282, 150, 75, "QuitButton", () => { Game1.ResetGame(); Game1.CurrentState = GameState.MainMenu; }));
        }
        private void LoadUI() //Loads GUI assets
        {
            //Health
            items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width / 2 - 84, main.GraphicsDevice.Viewport.Height - 50, "HudBG"));
            updateString = new MenuString(main.GraphicsDevice.Viewport.Width / 2 - 68, main.GraphicsDevice.Viewport.Height - 45, "10/10 HP",
                "UIfont", true, layer: .09f);
            updateString.SetUpdateCode(() => {
                updateString.text = RoomManager.Active.PlayerOne.Health.ToString() + "/" + RoomManager.Active.PlayerOne.MaxHealth.ToString() + " HP";
                updateString.Location.X = (main.GraphicsDevice.Viewport.Width - 17 * updateString.text.Length) / 2;
            });
            items.Add(updateString);
            //Upgrades
            items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 61, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
            items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 47, main.GraphicsDevice.Viewport.Height - 49, "rollIcon", layer: .09f));
            switch ((int)RoomManager.Active.PlayerOne.MyUpgrades)
            {
                case (1):
                    {
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 122, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 108, main.GraphicsDevice.Viewport.Height - 49, "dashIcon", layer: .09f));
                        break;
                    }
                case (2):
                    {
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 122, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 108, main.GraphicsDevice.Viewport.Height - 49, "batIcon", layer: .09f));
                        break;
                    }
                case (3):
                    {
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 122, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 108, main.GraphicsDevice.Viewport.Height - 49, "dashIcon", layer: .09f));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 61, main.GraphicsDevice.Viewport.Height - 100, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 47, main.GraphicsDevice.Viewport.Height - 99, "batIcon", layer: .09f));
                        break;
                    }
                case (4):
                    {
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 122, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 108, main.GraphicsDevice.Viewport.Height - 49, "spreadIcon", layer: .09f));
                        break;
                    }
                case (5):
                    {
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 122, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 108, main.GraphicsDevice.Viewport.Height - 49, "dashIcon", layer: .09f));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 61, main.GraphicsDevice.Viewport.Height - 100, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 47, main.GraphicsDevice.Viewport.Height - 99, "spreadIcon", layer: .09f));
                        break;
                    }
                case (6):
                    {
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 122, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 108, main.GraphicsDevice.Viewport.Height - 49, "batIcon", layer: .09f));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 61, main.GraphicsDevice.Viewport.Height - 100, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 47, main.GraphicsDevice.Viewport.Height - 99, "spreadIcon", layer: .09f));
                        break;
                    }
                case (7):
                    {

                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 122, main.GraphicsDevice.Viewport.Height - 50, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 108, main.GraphicsDevice.Viewport.Height - 49, "dashIcon", layer: .09f));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 61, main.GraphicsDevice.Viewport.Height - 100, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 47, main.GraphicsDevice.Viewport.Height - 99, "batIcon", layer: .09f));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 122, main.GraphicsDevice.Viewport.Height - 100, "UpgradeBG"));
                        items.Add(new MenuContent(main.GraphicsDevice.Viewport.Width - 108, main.GraphicsDevice.Viewport.Height - 99, "spreadIcon", layer: .09f));
                        break;
                    }
            }
            //Boss Health Bar
            updateMenu = new MenuContent(main.GraphicsDevice.Viewport.Width / 2 - 212, 0, "BossHealthBar", canUpdate: true, visible: false);
            updateMenu.SetUpdateCode(() => {
                updateMenu.IsVisible = RoomManager.Active.BossActive;
            });
            items.Add(updateMenu);
        }

        public void Update()
        {
            if (prevState == Game1.CurrentState) {
                foreach (MenuContent content in items) content.Update();
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

        public void Reload()
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
    }
}
