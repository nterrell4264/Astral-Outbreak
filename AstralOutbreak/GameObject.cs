using FarseerPhysics.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    class GameObject : Body
    {
        public GameObject(World world, Vector2? position = default(Vector2?), float rotation = 0, object userdata = null) : base(world, position, rotation, userdata)
        {
        }
    }
}
