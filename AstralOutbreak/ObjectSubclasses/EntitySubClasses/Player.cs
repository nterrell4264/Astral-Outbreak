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

        public Player(Vector2 pos, float width, float height, float health, bool mobile = true) : base(pos, width, height, health, mobile)
        {
            Gravity = true;
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            Acceleration.X = 0;
            Acceleration.Y = 0;
            if ((Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
            {
                Acceleration.X += -1;
            }
            if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
            {
                Acceleration.X += 1;
            }
            //Temporary Jump
            if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed && VelocityY.Y == 0))
            {
                Acceleration.Y -= 100;
            }

            if (Acceleration.X == 0)
            {
                VelocityX.X = 0;
            }
        }
    }
}
