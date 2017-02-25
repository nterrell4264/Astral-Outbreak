using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    class Entity : GameObject
    {
        public Entity(Vector pos, float width, float height, bool mobile = false) : base(pos, width, height, mobile)
        {
        }
    }
}
