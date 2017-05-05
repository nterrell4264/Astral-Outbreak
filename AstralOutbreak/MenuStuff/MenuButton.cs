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
        private MenuDelegate ClickEvent;

        //Constructors
        public MenuButton(int x, int y, int width, int height, string texture, MenuDelegate clickAction, bool visible = true, float layer = .1f) : base(x, y, texture, true, visible, layer)
        {
            hitbox = new Rectangle(base.Location, new Point(width, height));
            ClickEvent = clickAction;
        }
        public MenuButton(Point location, Point size, string texture, MenuDelegate clickAction, bool visible = true, float layer = .1f) : base(location, texture, true, visible, layer)
        {
            hitbox = new Rectangle(base.Location, size);
            ClickEvent = clickAction;
        }
        public MenuButton(Rectangle casting, string texture, MenuDelegate clickAction, bool visible = true, float layer = .1f) : base(casting.X, casting.Y, texture, true, visible, layer)
        {
            hitbox = casting;
            ClickEvent = clickAction;
        }

        public override void Update()
        {
            if (Game1.Inputs.M1State == ButtonStatus.Pressed && 
                Game1.Inputs.MouseX >= hitbox.X && Game1.Inputs.MouseX < (hitbox.X + hitbox.Width) && Game1.Inputs.MouseY >= hitbox.Y && Game1.Inputs.MouseY < (hitbox.Y + hitbox.Height))
            {
                ClickEvent();
            }
        }
    }
}
