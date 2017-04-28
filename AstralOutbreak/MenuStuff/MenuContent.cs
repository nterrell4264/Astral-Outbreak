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
        public Point Location { get; private set; }
        public string textureName;
        public bool Updatable { get; }
        public MenuDelegate SpecialCode { get; private set; }

        //Constructors
        public MenuContent(int x, int y, string texture, bool canUpdate = false, MenuDelegate updateCode = null)
        {
            Location = new Point(x, y);
            textureName = texture;
            Updatable = canUpdate;
        }
        public MenuContent(Point location, string texture, bool canUpdate = false, MenuDelegate updateCode = null)
        {
            Location = location;
            textureName = texture;
            Updatable = canUpdate;
        }
        public void UpdateTexture(string newTextureName)
        {
            textureName = newTextureName;
        }
    }
}
