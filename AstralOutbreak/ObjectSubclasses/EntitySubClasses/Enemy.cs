using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    class Enemy : Entity
    {
        public Enemy(Vector pos, float width, float height, bool mobile = false) : base(pos, width, height, mobile)
        {
        }
    }
}
