using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public delegate void MenuDelegate();
    public class MenuContent
    {
        //Variables
        public string textureName;
        public Point Location;
        public float depth;
        public bool IsVisible;

        private bool updatable; //if UI should update every frame
        private MenuDelegate UpdateCode; //Code if UI updates.

        //Constructors
        public MenuContent(int x, int y, string texture, bool canUpdate = false, bool visible = true, float layer = .1f)
        {
            Location = new Point(x, y);
            textureName = texture;
            updatable = canUpdate;
            depth = layer;
            IsVisible = visible;
        }
        public MenuContent(Point location, string texture, bool canUpdate = false, bool visible = true, float layer = .1f)
        {
            Location = location;
            textureName = texture;
            updatable = canUpdate;
            depth = layer;
            IsVisible = visible;
        }

        //Methods
        public virtual void Update()
        {
            if(updatable) UpdateCode();
        }

        public void SetUpdateCode(MenuDelegate code)
        {
            UpdateCode = code;
        }
    }
}
