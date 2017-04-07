using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public enum JackRabbitState { MovingLeft, MovingRight, Jumping, Falling, ShootingRight, ShootingLeft }

    class JackRabbit : Enemy
    {
        private JackRabbitState currentState;

        //prevY is a float used to store the previous Y value of the position, letting us know if the object is falling
        private float prevY;

        public JackRabbitState CurrentState { get { return currentState; } }

        public JackRabbit(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            //Creates a weapon for the JackRabbit, set at default values currently, we'll have to agree on better values later
            MyWeapon = new Weapon();
            MyWeapon.Source = this;

            prevY = pos.Y;
        }

        public override void Step(float deltaTime)
        {
            //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
            if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X)
            {
                currentState = JackRabbitState.ShootingLeft;
                FaceRight = false;
                Shoot(new Vector(-1, 0));
            }
            //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
            else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X)
            {
                currentState = JackRabbitState.ShootingRight;
                FaceRight = true;
                Shoot(new Vector(1, 0));
            }
            //Checks if the player is to the left of the JackRabbit, and will move towards the player
            else if (RoomManager.Active.PlayerOne.Position.X < Position.X)
            {
                currentState = JackRabbitState.MovingLeft;
                FaceRight = false;
                Position.X -= 1;
            }
            //Checks if the player is to the right of the JackRabbit, and will move towards the player
            else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
            {
                currentState = JackRabbitState.MovingRight;
                FaceRight = true;
                Position.X += 1;
            }
            //If the the previous y position is larger than the current, sets the JackRabbit to falling, move this to the top of the if statements to give this state priority
            else if (prevY > Position.Y)
            {
                currentState = JackRabbitState.Falling;
            }
            base.Step(deltaTime);
            prevY = Position.Y;
        }
    }
}
