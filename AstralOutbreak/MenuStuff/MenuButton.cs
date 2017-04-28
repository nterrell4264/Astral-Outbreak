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

        //Constructors
        public MenuButton(int x, int y, int width, int height, string texture, MenuDelegate clickAction) : base(x, y, texture, false)
        {
            hitbox = new Rectangle(base.Location, new Point(width, height));
            SetSpecialCode(clickAction);
        }
        public MenuButton(Point location, Point size, string texture, MenuDelegate clickAction) : base(location, texture, false)
        {
            hitbox = new Rectangle(base.Location, size);
            SetSpecialCode(clickAction);
        }
        public MenuButton(Rectangle casting, string texture, MenuDelegate clickAction) : base(casting.X, casting.Y, texture, false)
        {
            hitbox = casting;
            SetSpecialCode(clickAction);
        }

        public void CheckClicked()
        {
            //Makes the click check (mouse is down and over button)
            if (Game1.Inputs.M1State == ButtonStatus.Pressed && 
                Game1.Inputs.MouseX >= hitbox.X && Game1.Inputs.MouseX < (hitbox.X + hitbox.Width) && Game1.Inputs.MouseY >= hitbox.Y && Game1.Inputs.MouseY < (hitbox.Y + hitbox.Height))
            {
                SpecialCode();
            }
        }
    }
}
