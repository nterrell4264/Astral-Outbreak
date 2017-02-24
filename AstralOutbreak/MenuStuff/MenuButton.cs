using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    class MenuButton : MenuContent
    {
        //Variables
        private Rectangle hitbox;
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }
        public new Point Location
        {
            get { return hitbox.Location; }
            private set { hitbox.Location = value; }
        }

        //Constructor
        public MenuButton(int x, int y, int width, int height, Texture2D texture) : base(x, y, texture)
        {
            hitbox = new Rectangle(base.Location, new Point(width, height));
        }

        
    }
}
