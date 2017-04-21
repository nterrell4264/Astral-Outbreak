using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public enum JackRabbitState { Moving, Idle, Falling, Shooting }

    public class JackRabbit : Enemy
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

            prevY = Velocity.Y;
            Gravity = true;
        }

        public override void Step(float deltaTime)
        {
            switch (currentState)
            {
                case JackRabbitState.Moving:

                    //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
                    if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
                    {
                        Velocity.X = 0;
                        currentState = JackRabbitState.Shooting;
                        FaceRight = false;
                        Shoot(new Vector(-1, 0));
                        break;
                    }
                    //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
                    else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
                    {
                        Velocity.X = 0;
                        currentState = JackRabbitState.Shooting;
                        FaceRight = true;
                        Shoot(new Vector(1, 0));
                        break;
                    }

                    //If the the previous y position is larger than the current, sets the JackRabbit to falling, move this to the top of the if statements to give this state priority
                    if (prevY > Velocity.Y)
                    {
                        currentState = JackRabbitState.Falling;
                    }
                    break;

                case JackRabbitState.Idle:

                    //Checks if the player is to the left of the JackRabbit, and will move towards the player
                    if (RoomManager.Active.PlayerOne.Position.X < Position.X)
                    {
                        currentState = JackRabbitState.Moving;
                        FaceRight = false;
                        Velocity.X = -50;
                        break;
                    }
                    //Checks if the player is to the right of the JackRabbit, and will move towards the player
                    else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
                    {
                        currentState = JackRabbitState.Moving;
                        FaceRight = true;
                        Velocity.X = 50;
                        break;
                    }
                    //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
                    else if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
                    {
                        Velocity.X = 0;
                        currentState = JackRabbitState.Shooting;
                        FaceRight = false;
                        Shoot(new Vector(-1, 0));
                        break;
                    }
                    //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
                    else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
                    {
                        Velocity.X = 0;
                        currentState = JackRabbitState.Shooting;
                        FaceRight = true;
                        Shoot(new Vector(1, 0));
                        break;
                    }
                    //If the the previous y position is larger than the current, sets the JackRabbit to falling, move this to the top of the if statements to give this state priority
                    if (prevY > Velocity.Y)
                    {
                        currentState = JackRabbitState.Falling;
                    }
                    break;

                case JackRabbitState.Falling:

                    if(prevY == Velocity.Y)
                    {
                        //Checks if the player is to the left of the JackRabbit, and will move towards the player
                        if (RoomManager.Active.PlayerOne.Position.X < Position.X)
                        {
                            currentState = JackRabbitState.Moving;
                            FaceRight = false;
                            Velocity.X = -50;
                            break;
                        }
                        //Checks if the player is to the right of the JackRabbit, and will move towards the player
                        else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
                        {
                            currentState = JackRabbitState.Moving;
                            FaceRight = true;
                            Velocity.X = 50;
                            break;
                        }
                        //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
                        else if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
                        {
                            Velocity.X = 0;
                            currentState = JackRabbitState.Shooting;
                            FaceRight = false;
                            Shoot(new Vector(-1, 0));
                            break;
                        }
                        //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
                        else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
                        {
                            Velocity.X = 0;
                            currentState = JackRabbitState.Shooting;
                            FaceRight = true;
                            Shoot(new Vector(1, 0));
                            break;
                        }

                        if(Velocity.X == 0)
                        {
                            currentState = JackRabbitState.Idle;
                            break;
                        }
                    }

                    //Checks if the player is to the left of the JackRabbit, and will move towards the player
                    if (RoomManager.Active.PlayerOne.Position.X < Position.X)
                    {
                        FaceRight = false;
                        Velocity.X = -50;
                        break;
                    }
                    //Checks if the player is to the right of the JackRabbit, and will move towards the player
                    else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
                    {
                        FaceRight = true;
                        Velocity.X = 50;
                        break;
                    }
                    //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
                    else if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
                    {
                        Velocity.X = 0;
                        FaceRight = false;
                        Shoot(new Vector(-1, 0));
                        break;
                    }
                    //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
                    else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
                    {
                        Velocity.X = 0;
                        FaceRight = true;
                        Shoot(new Vector(1, 0));
                        break;
                    }
                    break;

                case JackRabbitState.Shooting:

                    //Checks if the player is to the left of the JackRabbit, and will move towards the player
                    if (RoomManager.Active.PlayerOne.Position.X < Position.X)
                    {
                        currentState = JackRabbitState.Moving;
                        FaceRight = false;
                        Velocity.X = -50;
                        break;
                    }
                    //Checks if the player is to the right of the JackRabbit, and will move towards the player
                    else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
                    {
                        currentState = JackRabbitState.Moving;
                        FaceRight = true;
                        Velocity.X = 50;
                        break;
                    }

                    //If the the previous y position is larger than the current, sets the JackRabbit to falling, move this to the top of the if statements to give this state priority
                    if (prevY > Velocity.Y)
                    {
                        currentState = JackRabbitState.Falling;
                    }
                    break;

                default:
                    currentState = JackRabbitState.Idle;
                    break;
            }


            ////If the player is within the range of JackRabbit's weapon and to the left, it shoots left
            //if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
            //{
            //    Velocity.X = 0;
            //    currentState = JackRabbitState.ShootingLeft;
            //    FaceRight = false;
            //    Shoot(new Vector(-1, 0));
            //}
            ////If the player is within the range of JackRabbit's weapon and to the right, it shoots right
            //else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30))
            //{
            //    Velocity.X = 0;
            //    currentState = JackRabbitState.ShootingRight;
            //    FaceRight = true;
            //    Shoot(new Vector(1, 0));
            //}
            ////Checks if the player is to the left of the JackRabbit, and will move towards the player
            //else if (RoomManager.Active.PlayerOne.Position.X < Position.X)
            //{
            //    currentState = JackRabbitState.MovingLeft;
            //    FaceRight = false;
            //    Velocity.X = -50;
            //}
            ////Checks if the player is to the right of the JackRabbit, and will move towards the player
            //else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
            //{
            //    currentState = JackRabbitState.MovingRight;
            //    FaceRight = true;
            //    Velocity.X = 50;
            //}
            ////If the the previous y position is larger than the current, sets the JackRabbit to falling, move this to the top of the if statements to give this state priority
            //else if (prevY > Position.Y)
            //{
            //    currentState = JackRabbitState.Falling;
            //}
            base.Step(deltaTime);
            prevY = Velocity.Y;
        }
    }
}
