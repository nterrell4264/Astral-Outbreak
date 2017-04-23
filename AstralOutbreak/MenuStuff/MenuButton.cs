using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    delegate void ButtonPressDelegate();
    class MenuButton : MenuContent
    {
        //Variables
        private Rectangle hitbox;

        //Properties
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }
        public new Point Location
        {
            get { return hitbox.Location; }
            private set { hitbox.Location = value; }
        }
        public ButtonPressDelegate ClickEvent { get; private set; }

        //Constructors
        public MenuButton(int x, int y, int width, int height, string texture, ButtonPressDelegate clickAction) : base(x, y, texture)
        {
            hitbox = new Rectangle(base.Location, new Point(width, height));
            ClickEvent = clickAction;
        }
        public MenuButton(Point location, Point size, string texture, ButtonPressDelegate clickAction) : base(location, texture)
        {
            hitbox = new Rectangle(base.Location, size);
            ClickEvent = clickAction;
        }
        public MenuButton(Rectangle casting, string texture, ButtonPressDelegate clickAction) : base(casting.X, casting.Y, texture)
        {
            hitbox = casting;
            ClickEvent = clickAction;
        }

        public void CheckClicked() //Checks if it has been pressed and takes appropriate action
        {
            //Makes the click check (mouse is down and over button)
            if (Game1.Inputs.M1State == ButtonStatus.Pressed && 
                Game1.Inputs.MouseX >= hitbox.X && Game1.Inputs.MouseX < (hitbox.X + hitbox.Width) && Game1.Inputs.MouseY >= hitbox.Y && Game1.Inputs.MouseY < (hitbox.Y + hitbox.Height))
            {
                ClickEvent();
            }
        }
    }
}
