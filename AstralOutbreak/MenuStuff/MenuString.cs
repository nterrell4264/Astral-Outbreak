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

        public MenuString(int x, int y, string text, string spriteFont, bool canUpdate = false, bool visible = true, float layer = .2f) : base(x, y, null, canUpdate, visible, layer)
        {
            this.text = text;
            SpriteFont = spriteFont;   
        }
        public MenuString(Point location, string text, string spriteFont, bool canUpdate = false, bool visible = true, float layer = .2f) : base(location, null, canUpdate, visible, layer)
        {
            this.text = text;
            SpriteFont = spriteFont;
        }
    }
}
