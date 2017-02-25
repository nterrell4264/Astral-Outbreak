using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    class Projectile : Entity
    {
        public Projectile(Vector pos, float width, float height, bool mobile = false) : base(pos, width, height, mobile)
        {
        }
    }
}
