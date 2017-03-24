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
                Acceleration.X += -5;
            }
            if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
            {
                Acceleration.X += 5;
            }
            //Temporary Jump
            if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed && Velocity.Y == 0))
            {
                Acceleration.Y -= 100;
            }
            //Makes sure player stops when buttons arent pressed and can change direction easily
            if (Velocity.X > 0 && (Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
            {
                Velocity.X = -30;
            }
            else if (Velocity.X < 0 && (Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
            {
                Velocity.X = 30;
            }
            else if (Acceleration.X == 0)
            {
                Velocity.X = 0;
            }
            
        }
    }
}
