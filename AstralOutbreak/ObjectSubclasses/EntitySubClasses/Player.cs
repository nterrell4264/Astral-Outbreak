using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;



namespace AstralOutbreak
{
    /// <summary>
    /// Unit controlled directly by the player
    /// </summary>
    public class Player : Entity
    {
        //Will be mad public after input manager is a public class
        InputManager Input { get; set; }

        public Player(Vector2 pos, float width, float height, float health, bool mobile = true) : base(pos, width, height, health, mobile)
        {
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);

        }
    }
}
