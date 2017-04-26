using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    class MenuString : MenuContent
    {
        public string Text { get; private set; }
        public string SpriteFont { get; private set; }

        public MenuString(int x, int y, string text, string spriteFont) : base(x, y, null)
        {
            Text = text;
            SpriteFont = spriteFont;   
        }
        public MenuString(Point location, string text, string spriteFont) : base(location, null)
        {
            Text = text;
            SpriteFont = spriteFont;
        }
    }
}
