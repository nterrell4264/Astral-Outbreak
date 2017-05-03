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
        public Point Location;
        public string textureName;
        public float depth;
        private bool updatable;
        private MenuDelegate UpdateCode;

        //Constructors
        public MenuContent(int x, int y, string texture, bool canUpdate = false, float layer = .1f)
        {
            Location = new Point(x, y);
            textureName = texture;
            updatable = canUpdate;
            depth = layer;
        }
        public MenuContent(Point location, string texture, bool canUpdate = false, float layer = .1f)
        {
            Location = location;
            textureName = texture;
            updatable = canUpdate;
            depth = layer;
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
