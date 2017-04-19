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
   class MenuManager
    {
        public List<MenuContent> items { get; private set; } //List of things to draw
        private GameState prevState; //Holds last frame's GameState for automatic updating
        private Game1 main; //Used for Exit command

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
            items.Add(new MenuButton(32, 32, 150, 50, "mnuStart", () => { Game1.CurrentState = GameState.Playing; }));
            items.Add(new MenuButton(32, 182, 150, 50, "mnuQuit", () => { main.Exit(); }));
        }
        private void LoadOptions() //Loads options menu assets
        {

        }
        private void LoadPause() //Loads pause menu assets
        {
            items.Add(new MenuButton(32, 32, 150, 50, "mnuResume", () => { Game1.CurrentState = GameState.Playing; }));
            items.Add(new MenuButton(32, 107, 150, 50, "mnuOptions", () => { Game1.CurrentState = GameState.OptionsMenu; }));
            items.Add(new MenuButton(32, 182, 150, 50, "mnuQuit", () => { Game1.CurrentState = GameState.MainMenu; }));
        }
        private void LoadScreen() //Loads loading screen assets
        {

        }
        private void LoadSave() //Loads save menu assets
        {

        }
        private void LoadGameOver() //Loads game over menu assets
        {
            items.Add(new MenuButton(32, 182, 150, 50, "mnuQuit", () => { Game1.CurrentState = GameState.MainMenu; }));
        }
        private void LoadUI() //Loads GUI assets
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if(prevState == Game1.CurrentState) //Won't check buttons if something else has changed the GameState already.
                foreach (MenuContent button in items)
                {
                    if (button is MenuButton) (button as MenuButton).CheckClicked();
                }
            if (!(prevState == Game1.CurrentState)) //Menu only updates on state change.
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
