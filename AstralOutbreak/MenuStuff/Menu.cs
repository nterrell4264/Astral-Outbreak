using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
   class Menu
    {
        public List<MenuContent> items { get; private set; } //List of things to draw

        public Menu() //Automatically loads the main menu on instantiating.
        {
            items = new List<MenuContent>();
            Load(GameState.MainMenu);
        }

        public void Load(GameState type) //Loads the appropriate menu type.
        {
            if (type == Game1.CurrentState) return; //Prevents loading the same menu more than once in a row.
            switch (type)
            {
                case (GameState.MainMenu):
                    {
                        items.Clear();
                        LoadMain();
                        break;
                    }
                case (GameState.PauseMenu):
                    {
                        items.Clear();
                        LoadPause();
                        break;
                    }

                case (GameState.OptionsMenu):
                    {
                        items.Clear();
                        LoadOptions();
                        break;
                    }
                case (GameState.GameOverMenu):
                    {
                        items.Clear();
                        LoadGameOver();
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            Game1.CurrentState = type;
        }

        //Load options
        private void LoadMain() //Loads main menu assets
        {
            items.Add(new MenuContent(0, 0));
            items.Add(new MenuContent(0, 128));
            items.Add(new MenuButton(64, 64, 64, 128, null));
        }
        private void LoadPause() //Loads pause menu assets
        {

        }
        private void LoadOptions() //Loads options menu assets
        {

        }
        private void LoadGameOver() //Loads game over menu assets
        {

        }
    }
}
