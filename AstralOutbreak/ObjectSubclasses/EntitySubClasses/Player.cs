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
            //Order of Movement Reading: Rolling/Dashing -> Left Right -> Jump -> Smooth Direction Changes

            if ((Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
            {
                Acceleration.X += -5;
            }
            if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
            {
                Acceleration.X += 5;
            }
            //Temporary Jump
            if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed && VelocityY.Y == 0))
            {
                Acceleration.Y -= 100;
            }
            //Makes sure player stops when buttons arent pressed and can change direction easily
            if (VelocityX.X > 0 && (Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
            {
                VelocityX.X = -30;
            }
            else if (VelocityX.X < 0 && (Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
            {
                VelocityX.X = 30;
            }
            else if (Acceleration.X == 0)
            {
                VelocityX.X = 0;
            }

            
            
        }
    }
}
