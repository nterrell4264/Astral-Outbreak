using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public enum JackRabbitState { Idle, Moving, Falling, Shooting }

    public class JackRabbit : Enemy
    {
        private JackRabbitState currentState;

        //prevY is a float used to store the previous Y value of the position, letting us know if the object is falling
        private float prevY;
        private float prevX;
        private bool awake;
        private float sleepTimer;

        public JackRabbitState CurrentJackRabbitState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                CurrentActionTime = 0;
                if (currentState == JackRabbitState.Idle || currentState == JackRabbitState.Shooting)
                    MaxVelocity = new Vector(0, 600);
                else
                    MaxVelocity = new Vector(225, 600);
                if (currentState != JackRabbitState.Shooting)
                    Shooting = false;
            }
        }

        public JackRabbit(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            //Creates a weapon for the JackRabbit, set at default values currently, we'll have to agree on better values later
            MyWeapon = new Weapon(.4f, 1, 550, 300);
            MyWeapon.BulletSize = 5;
            MyWeapon.Source = this;

            sleepTimer = .5f;
            prevY = Velocity.Y;
            prevX = Position.X;
            Gravity = true;
        }

        public override void Step(float deltaTime)
        {
            if (awake)
            {
                sleepTimer = .5f;

                switch (CurrentJackRabbitState)
                {
                    case JackRabbitState.Moving:

                        if (Position.X >= RoomManager.Active.PlayerOne.Position.X - 20 && Position.X <= RoomManager.Active.PlayerOne.Position.X + 20 && Position.Y != RoomManager.Active.PlayerOne.Position.Y)
                        {
                            CurrentJackRabbitState = JackRabbitState.Idle;
                            Velocity.X = 0;
                            break;
                        }

                        if (FaceRight)
                        {

                            //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
                            if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && 
                                (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                            {
                                Velocity.X = 0;
                                CurrentJackRabbitState = JackRabbitState.Shooting;
                                FaceRight = true;
                                Shoot(new Vector(1, 0));
                                break;
                            }

                            //If the the previous y position is larger than the current, sets the JackRabbit to falling, move this to the top of the if statements to give this state priority
                            if (prevY != Velocity.Y)
                            {
                                CurrentJackRabbitState = JackRabbitState.Falling;
                                break;
                            }

                            if (!RoomManager.Active.CheckCollision(this, new Vector(1, -5 * Height / 2)) && RoomManager.Active.CheckCollision(this, new Vector(1, 0)))
                            {
                                Velocity.Y -= 310;
                                CurrentJackRabbitState = JackRabbitState.Falling;
                                break;
                            }

                            //Checks if the player is to the left of the JackRabbit, and will move towards the player
                            if (RoomManager.Active.PlayerOne.Position.X < Position.X)
                            {
                                FaceRight = false;
                                Velocity.X = -225;
                                //if (Velocity.X < 0 && prevX == Position.X)
                                //{
                                //    currentState = JackRabbitState.Idle;
                                //    break;
                                //}
                                break;
                            }
                            Velocity.X = 225;
                            //if (Velocity.X > 0 && prevX == Position.X)
                            //{
                            //    currentState = JackRabbitState.Idle;
                            //    break;
                            //}
                        }
                        else
                        {
                            //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
                            if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && 
                                (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                            {
                                Velocity.X = 0;
                                CurrentJackRabbitState = JackRabbitState.Shooting;
                                FaceRight = false;
                                Shoot(new Vector(-1, 0));
                                break;
                            }
                            if (!RoomManager.Active.CheckCollision(this, new Vector(-1, -5 * Height / 2)) && RoomManager.Active.CheckCollision(this, new Vector(-1, 0)))
                            {
                                Velocity.Y -= 310;
                                CurrentJackRabbitState = JackRabbitState.Falling;
                                break;
                            }

                            //Checks if the player is to the right of the JackRabbit, and will move towards the player
                            if (RoomManager.Active.PlayerOne.Position.X > Position.X)
                            {
                                CurrentJackRabbitState = JackRabbitState.Moving;
                                FaceRight = true;
                                Velocity.X = 225;
                                //if (Velocity.X > 0 && prevX == Position.X)
                                //{
                                //    currentState = JackRabbitState.Idle;
                                //    break;
                                //}
                                break;
                            }
                            Velocity.X = -225;
                            //if (Velocity.X < 0 && prevX == Position.X)
                            //{
                            //    currentState = JackRabbitState.Idle;
                            //    break;
                            //}
                        }

                        

                        break;

                    case JackRabbitState.Idle:

                        //Checks if the player is to the left of the JackRabbit, and will move towards the player
                        if (RoomManager.Active.PlayerOne.Position.X + 20 < Position.X)
                        {
                            CurrentJackRabbitState = JackRabbitState.Moving;
                            FaceRight = false;
                            break;
                        }
                        //Checks if the player is to the right of the JackRabbit, and will move towards the player
                        else if (RoomManager.Active.PlayerOne.Position.X - 20 > Position.X)
                        {
                            CurrentJackRabbitState = JackRabbitState.Moving;
                            FaceRight = true;
                            break;
                        }
                        //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
                        else if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && 
                            (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                        {
                            Velocity.X = 0;
                            CurrentJackRabbitState = JackRabbitState.Shooting;
                            FaceRight = false;
                            Shoot(new Vector(-1, 0));
                            break;
                        }
                        //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
                        else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && 
                            (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                        {
                            Velocity.X = 0;
                            CurrentJackRabbitState = JackRabbitState.Shooting;
                            FaceRight = true;
                            Shoot(new Vector(1, 0));
                            break;
                        }
                        //If the the previous y position is larger than the current, sets the JackRabbit to falling, move this to the top of the if statements to give this state priority
                        if (prevY > Velocity.Y)
                        {
                            CurrentJackRabbitState = JackRabbitState.Falling;
                        }
                        break;

                    case JackRabbitState.Falling:

                        if (prevY == Velocity.Y)
                        {
                            //Checks if the player is to the left of the JackRabbit, and will move towards the player
                            if (RoomManager.Active.PlayerOne.Position.X < Position.X)
                            {
                                CurrentJackRabbitState = JackRabbitState.Moving;
                                FaceRight = false;
                                break;
                            }
                            //Checks if the player is to the right of the JackRabbit, and will move towards the player
                            else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
                            {
                                CurrentJackRabbitState = JackRabbitState.Moving;
                                FaceRight = true;
                                break;
                            }
                            //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
                            else if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && 
                                (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                            {
                                Velocity.X = 0;
                                CurrentJackRabbitState = JackRabbitState.Shooting;
                                FaceRight = false;
                                Shoot(new Vector(-1, 0));
                                break;
                            }
                            //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
                            else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && 
                                (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                            {
                                Velocity.X = 0;
                                CurrentJackRabbitState = JackRabbitState.Shooting;
                                FaceRight = true;
                                Shoot(new Vector(1, 0));
                                break;
                            }

                            if (Velocity.X == 0)
                            {
                                CurrentJackRabbitState = JackRabbitState.Idle;
                                break;
                            }
                        }

                        //Checks if the player is to the left of the JackRabbit, and will move towards the player
                        if (RoomManager.Active.PlayerOne.Position.X < Position.X)
                        {
                            FaceRight = false;
                            Velocity.X = -225;
                            break;
                        }
                        //Checks if the player is to the right of the JackRabbit, and will move towards the player
                        else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
                        {
                            FaceRight = true;
                            Velocity.X = 225;
                            break;
                        }
                        //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
                        else if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && 
                            (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                        {
                            Velocity.X = 0;
                            FaceRight = false;
                            Shoot(new Vector(-1, 0));
                            break;
                        }
                        //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
                        else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && 
                            (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                        {
                            Velocity.X = 0;
                            FaceRight = true;
                            Shoot(new Vector(1, 0));
                            break;
                        }
                        break;

                    case JackRabbitState.Shooting:
                        if (CurrentActionTime > .8f)
                        {
                            //If the player is within the range of JackRabbit's weapon and to the left, it shoots left
                            if (RoomManager.Active.PlayerOne.Position.X > Position.X - MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X < Position.X && 
                                (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                            {
                                Velocity.X = 0;
                                FaceRight = false;
                                Shoot(new Vector(-1, 0));
                                break;
                            }
                            //If the player is within the range of JackRabbit's weapon and to the right, it shoots right
                            else if (RoomManager.Active.PlayerOne.Position.X < Position.X + MyWeapon.Range && RoomManager.Active.PlayerOne.Position.X > Position.X && 
                                (RoomManager.Active.PlayerOne.Position.Y < Position.Y + 30 && RoomManager.Active.PlayerOne.Position.Y > Position.Y - 30) && CheckLineOfSight(RoomManager.MapData))
                            {
                                Velocity.X = 0;
                                FaceRight = true;
                                Shoot(new Vector(1, 0));
                                break;
                            }
                            //Checks if the player is to the left of the JackRabbit, and will move towards the player
                            if (RoomManager.Active.PlayerOne.Position.X < Position.X)
                            {
                                CurrentJackRabbitState = JackRabbitState.Moving;
                                FaceRight = false;
                                break;
                            }
                            //Checks if the player is to the right of the JackRabbit, and will move towards the player
                            else if (RoomManager.Active.PlayerOne.Position.X > Position.X)
                            {
                                CurrentJackRabbitState = JackRabbitState.Moving;
                                FaceRight = true;
                                break;
                            }

                            //If the the previous y position is larger than the current, sets the JackRabbit to falling, move this to the top of the if statements to give this state priority
                            if (prevY > Velocity.Y)
                            {
                                CurrentJackRabbitState = JackRabbitState.Falling;
                                break;
                            }

                        }
                        break;

                    default:
                        CurrentJackRabbitState = JackRabbitState.Idle;

                        break;
                }
            }
            else
            {
                sleepTimer -= deltaTime;
                if (sleepTimer <= 0)
                {
                    currentState = JackRabbitState.Idle;
                    Shooting = false;
                    Velocity.X /= 1.1f;
                }
            }
            awake = CheckLineOfSight(RoomManager.MapData);

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
            prevX = Position.X;
        }
    }
}
