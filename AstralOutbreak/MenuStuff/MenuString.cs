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
        public string text;
        public string SpriteFont { get; private set; }

        public MenuString(int x, int y, string text, string spriteFont, bool canUpdate = false) : base(x, y, null, canUpdate)
        {
            this.text = text;
            SpriteFont = spriteFont;   
        }
        public MenuString(Point location, string text, string spriteFont, bool canUpdate = false) : base(location, null, canUpdate)
        {
            this.text = text;
            SpriteFont = spriteFont;
        }
        public void UpdateText(string newText)
        {
            text = newText;
        }
    }
}
