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

        //Constructor
        public MenuContent(int x, int y)
        {
            Location = new Point(x, y);
        }
        public MenuContent(Point location)
        {
            Location = location;
        }
    }
}
