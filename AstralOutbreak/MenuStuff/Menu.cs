using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    enum ActiveMenu { MainMenu, PauseMenu, OptionsMenu, GameOverMenu} //Passed in during Menu's constructor to determine what gets loaded

    class Menu
    {
        public List<MenuContent> items { get; private set; } //List of things to draw 

        public Menu()
        {
            Load(ActiveMenu.MainMenu);
        }

        public void Load(ActiveMenu type)
        {
            switch (type)
            {
                case (ActiveMenu.MainMenu):
                    {
                        LoadMain();
                        break;
                    }
                case (ActiveMenu.PauseMenu):
                    {
                        LoadPause();
                        break;
                    }

                case (ActiveMenu.OptionsMenu):
                    {
                        LoadOptions();
                        break;
                    }
                case (ActiveMenu.GameOverMenu):
                    {
                        LoadGameOver();
                        break;
                    }
            }
        }

        //Load options
        private void LoadMain() //Loads main menu assets
        {

        }
        private void LoadPause() //Loads main menu assets
        {

        }
        private void LoadOptions() //Loads main menu assets
        {

        }
        private void LoadGameOver() //Loads main menu assets
        {

        }
    }
}
