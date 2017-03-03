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
        private GameState currentState;

        public Menu() //Automatically loads the main menu on instantiating.
        {
            Load(GameState.MainMenu);
        }

        public void Load(GameState type) //Loads the appropriate menu type.
        {
            if (type == currentState) return; //Prevents loading the same menu more than once in a row.
            switch (type)
            {
                case (GameState.MainMenu):
                    {
                        LoadMain();
                        break;
                    }
                case (GameState.PauseMenu):
                    {
                        LoadPause();
                        break;
                    }

                case (GameState.Options):
                    {
                        LoadOptions();
                        break;
                    }
                case (GameState.GameOver):
                    {
                        LoadGameOver();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException();
                    }
            }
            currentState = type;
        }

        //Load options
        private void LoadMain() //Loads main menu assets
        {

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
