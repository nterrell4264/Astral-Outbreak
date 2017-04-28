using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public delegate void MenuDelegate();
    class MenuContent
    {
        //Variables
        public Point Location;
        public string textureName;

        public bool Updatable { get; }
        public MenuDelegate SpecialCode { get; private set; } //Code specific to a menu item, like a button's press event or UI that uses game data.

        //Constructors
        public MenuContent(int x, int y, string texture, bool canUpdate = false)
        {
            Location = new Point(x, y);
            textureName = texture;
            Updatable = canUpdate;
        }
        public MenuContent(Point location, string texture, bool canUpdate = false)
        {
            Location = location;
            textureName = texture;
            Updatable = canUpdate;
        }

        /// <summary>
        /// Sets item-specific code. Can't be done in the constructor if it refers to any property of the item itself.
        /// </summary>
        /// <param name="code">Lambda expression of the code to put in.</param>
        public void SetSpecialCode(MenuDelegate code)
        {
            SpecialCode = code;
        }
    }
}
