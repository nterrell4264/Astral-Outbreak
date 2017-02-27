using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;



namespace AstralOutbreak
{
    public class Player : Entity
    {
        public Player(Vector2 pos, float width, float height, float health, bool mobile = true) : base(pos, width, height, health, mobile)
        {
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);


        }
    }
}
