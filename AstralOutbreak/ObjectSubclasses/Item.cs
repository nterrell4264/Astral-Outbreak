using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public class Item : GameObject
    {
        public Item(Vector2 pos, float width, float height, bool mobile = false) : base(pos, width, height, mobile)
        {
        }
    }
}
