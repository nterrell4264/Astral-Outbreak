using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public class Projectile : Entity
    {
        public Projectile(Vector2 pos, float width, float height, bool mobile = true) : base(pos, width, height, mobile)
        {
        }

        public override void Step(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
