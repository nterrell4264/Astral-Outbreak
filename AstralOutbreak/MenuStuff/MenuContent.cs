using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    class MenuContent
    {
        //Variables
        public Point Location { get; private set; }
        public string TextureName { get; set; }

        //Constructor
        public MenuContent(int x, int y, string texture)
        {
            Location = new Point(x, y);
            TextureName = texture;
        }
        public MenuContent(Point location, string texture)
        {
            Location = location;
            TextureName = texture;
        }
    }
}
