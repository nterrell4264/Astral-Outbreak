using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public enum BatState { UpLeft, UpRight, Up, DownLeft, DownRight}

    //An Bat that chases the player
    public class Bat : Enemy
    {
        //Is the mob active. Static because swarms activate as one.
        public bool Awake { get; set; }

        private BatState myBatState;

        public BatState CurrentBatState
        {
            get { return myBatState; }
            set
            {
                CurrentActionTime = 0;
                myBatState = value;
                switch (CurrentBatState)
                {
                    case BatState.UpLeft:
                        Velocity.X = -300;
                        Velocity.Y = -300;
                        break;
                    case BatState.UpRight:
                        Velocity.X = 300;
                        Velocity.Y = -300;
                        break;
                    case BatState.DownLeft:
                        Velocity.X = -300;
                        Velocity.Y = 300;
                        break;
                    case BatState.DownRight:
                        Velocity.X = 300;
                        Velocity.Y = 300;
                        break;
                    case BatState.Up:
                        Velocity.X = 0;
                        Velocity.Y = -300;
                        break;
                    default:
                        break;
                }
            }
        }


        public static Vector Target { get { return RoomManager.Active.PlayerOne.Center; } }
        public override float Health
        {
            get
            {
                return base.Health;
            }

            set
            {
                if (Health > value && !Awake)
                    Awake = true;
                base.Health = value;
            }
        }


        public Bat(Vector2 pos, float width, float height, float health, float damage, bool mobile = true) : base(pos, width, height, health, 1, mobile)
        {
            Gravity = false;
            MaxVelocity = new Vector(300, 300);
            PlatformDown = true;
        }

        public Bat(SwarmMob mob) : base(new Vector(mob.Position), mob.Width, mob.Height, mob.Health, mob.Damage, mob.Mobile)
        {
            CurrentActionTime += (float)Game1.Rand.NextDouble();
            Gravity = false;
            MaxVelocity = new Vector(400, 400);
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //If it is awake
            if (Awake)
            {
                //switch (CurrentBatState)
                //{
                //    case BatState.UpLeft:
                //        if(Velocity.X != -300)
                //        {
                //            if(Center.Y > RoomManager.Active.PlayerOne.Center.Y)
                //            {
                //                CurrentBatState = BatState.UpRight;
                //                break;
                //            }
                //            else
                //            {
                //                CurrentBatState = BatState.DownRight;
                //            }
                //        }
                //        if (Velocity.Y != -300)
                //        {
                //            if (Center.X > RoomManager.Active.PlayerOne.Center.X)
                //            {
                //                CurrentBatState = BatState.DownLeft;
                //                break;
                //            }
                //            else
                //            {
                //                CurrentBatState = BatState.DownRight;
                //            }
                //        }
                //        if (CurrentActionTime > .5f)
                //        {
                //            CurrentBatState = BatState.DownLeft;
                //        }
                //        break;
                //    case BatState.UpRight:
                //        if (Velocity.X != 300)
                //        {
                //            if (Center.Y > RoomManager.Active.PlayerOne.Center.Y)
                //            {
                //                CurrentBatState = BatState.UpLeft;
                //                break;
                //            }
                //            else
                //            {
                //                CurrentBatState = BatState.DownLeft;
                //            }
                //        }
                //        if (Velocity.Y != -300)
                //        {
                //            if (Center.X > RoomManager.Active.PlayerOne.Center.X)
                //            {
                //                CurrentBatState = BatState.DownLeft;
                //                break;
                //            }
                //            else
                //            {
                //                CurrentBatState = BatState.DownRight;
                //            }
                //        }
                //        if (CurrentActionTime > .5f)
                //        {
                //            CurrentBatState = BatState.DownRight;
                //        }
                //        break;
                //    case BatState.DownLeft:
                //        if (Velocity.X != -300)
                //        {
                //            if (Center.Y > RoomManager.Active.PlayerOne.Center.Y)
                //            {
                //                CurrentBatState = BatState.UpRight;
                //                break;
                //            }
                //            else
                //            {
                //                CurrentBatState = BatState.DownRight;
                //            }
                //        }
                //        if (Velocity.Y != 300)
                //        {
                //            if (Center.X > RoomManager.Active.PlayerOne.Center.X)
                //            {
                //                CurrentBatState = BatState.UpLeft;
                //                break;
                //            }
                //            else
                //            {
                //                CurrentBatState = BatState.UpRight;
                //            }
                //        }
                //        if (CurrentActionTime > .5f)
                //        {
                //            CurrentBatState = BatState.UpLeft;
                //        }
                //        break;
                //    case BatState.DownRight:
                //        if (Velocity.X != 300)
                //        {
                //            if (Center.Y > RoomManager.Active.PlayerOne.Center.Y)
                //            {
                //                CurrentBatState = BatState.UpLeft;
                //                break;
                //            }
                //            else
                //            {
                //                CurrentBatState = BatState.DownLeft;
                //            }
                //        }
                //        if (Velocity.Y != 300)
                //        {
                //            if (Center.X > RoomManager.Active.PlayerOne.Center.X)
                //            {
                //                CurrentBatState = BatState.UpLeft;
                //                break;
                //            }
                //            else
                //            {
                //                CurrentBatState = BatState.UpRight;
                //            }
                //        }
                //        if (CurrentActionTime > .5f)
                //        {
                //            CurrentBatState = BatState.UpRight;
                //        }
                //        break;
                //    default:
                //        if(Velocity.Y != -300)
                //        {
                //            if (Center.Y > RoomManager.Active.PlayerOne.Center.Y)
                //            {
                //                CurrentBatState = BatState.UpRight;
                //                break;
                //            }
                //            else
                //            {
                //                CurrentBatState = BatState.DownRight;
                //            }
                //            if (RoomManager.Active.PlayerOne.Position.Y -  48 > Position.Y)
                //            {
                //                CurrentBatState = BatState.Up;
                //            }
                //        }
                //        break;

                //}
                if (Math.Abs(Position.X - Target.X) > Math.Abs(Position.Y - Target.Y))
                {
                    Acceleration.X = Target.X - Center.X;
                }
                else
                {
                    Acceleration.Y = Target.Y - Center.Y;
                }
                //if(RoomManager.Active.PlayerOne.Position.Y < Position.Y - 48)
                //{
                //    CurrentBatState = BatState.Up;
                //}
            }
            else
            {
                if (CheckLineOfSight(RoomManager.Active.MapData))
                    Awake = true;
            }

        }
    }

}

