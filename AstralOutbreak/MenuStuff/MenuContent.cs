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
        public bool IsText { get; private set; }

        //Constructor
        public MenuContent(int x, int y, string texture, bool text = false)
        {
            Location = new Point(x, y);
            TextureName = texture;
            IsText = text;
        }
        public MenuContent(Point location, string texture, bool text = false)
        {
            Location = location;
            TextureName = texture;
            IsText = text;
        }
    }
}
