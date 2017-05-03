using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class DashRabbit : Enemy
    {
        private JackRabbitState currentState;

        //prevY is a float used to store the previous Y value of the position, letting us know if the object is falling
        private float prevY;
        private float prevX;
        private bool awake;
        private int smashCount;
        public JackRabbitState CurrentJackRabbitState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                CurrentActionTime = 0;
                if (currentState == JackRabbitState.Idle || currentState == JackRabbitState.Shooting)
                    MaxVelocity = new Vector(0, 600);
                else if (currentState == JackRabbitState.Falling)
                    MaxVelocity = new Vector(1000, 900);
                else
                    MaxVelocity = new Vector(225, 600);
                if (currentState != JackRabbitState.Shooting)
                    Shooting = false;
            }
        }
        public override float Health
        {
            get
            {
                return base.Health;
            }

            set
            {
                if (awake)
                    base.Health = value;
                else
                    base.Health = MaxHealth;
                if (IsDead)
                {
                    RoomManager.Active.BossActive = false;
                }
            }
        }

        //Bosses don't unload
        public override bool Unload
        {
            get
            {
                return base.Unload && !RoomManager.Active.BossActive;
            }

            set
            {
                if (!RoomManager.Active.BossActive)
                {
                    base.Unload = value;
                    return;
                }
                if (value)
                {
                    Collides = false;
                    if (this.Position.Y > RoomManager.Active.PlayerOne.Position.Y - 1)
                    {
                        this.Position.Y = RoomManager.Active.PlayerOne.Position.Y - 1;
                        float diff = Position.X - RoomManager.Active.PlayerOne.Position.X;
                        if (diff < 0)
                        {
                            FaceRight = true;
                            Velocity = new Vector(900, -900);
                            currentState = JackRabbitState.Falling;
                        }
                        else
                        {
                            FaceRight = false;
                            Velocity = new Vector(-900, -900);
                            currentState = JackRabbitState.Falling;
                        }
                    }
                }
                else
                {
                    Collides = true;
                }
            }
        }

        public DashRabbit(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            //Creates a weapon for the JackRabbitBoss,
            MyWeapon = new Weapon(.4f, 3, 900, 700);
            MyWeapon.BulletSize = 5;
            MyWeapon.Source = this;
            prevY = Velocity.Y;
            prevX = Position.X;
            Gravity = true;
            smashCount = 6;
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //Check if awake
            if (awake)
            {
                if (smashCount > 0)
                {
                    switch (currentState)
                    {
                        //When Idle, wait a bit, then charge the player
                        case JackRabbitState.Idle:
                            if (CurrentActionTime > 1f)
                            {
                                smashCount--;
                                float v = 500 + 500 * (1 - (Health / MaxHealth));
                                float diff = Position.X - RoomManager.Active.PlayerOne.Position.X;
                                if (diff < 0)
                                {
                                    FaceRight = true;
                                    Velocity = new Vector(v, -200);
                                    currentState = JackRabbitState.Falling;
                                }
                                else
                                {
                                    FaceRight = false;
                                    Velocity = new Vector(-v, -200);
                                    currentState = JackRabbitState.Falling;
                                }
                            }
                            break;
                        //When moving, apply friction
                        case JackRabbitState.Moving:
                            Velocity.X /= 1.1f;
                            if (Velocity.X < 1 && Velocity.X > -1)
                            {
                                currentState = JackRabbitState.Idle;
                            }
                            break;
                        //When Falling/Jumping check if we have landed
                        case JackRabbitState.Falling:
                            if (Velocity.Y == prevY)
                            {
                                currentState = JackRabbitState.Moving;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if(smashCount == 0)
                {
                    Gravity = false;
                    Velocity = (RoomManager.Active.PlayerOne.Center - Center - new Vector(0, 100)) * 4;
                    CurrentJackRabbitState = JackRabbitState.Falling;
                    smashCount--;
                }
                else
                {
                    if(CurrentActionTime < .25f)
                    {
                        
                    }
                    else if(CurrentActionTime < .5f)
                    {
                        Velocity = new Vector(0, 0);
                    }
                    else if(CurrentActionTime < 1)
                    {
                        Velocity.Y = 1000;
                    }
                    else if (Velocity.Y != prevY)
                    {
                        smashCount = 2 + (int)(4 * Health / MaxHealth);
                        Gravity = true;
                        CurrentJackRabbitState = JackRabbitState.Idle;
                        Velocity.Y = 0;
                    }

                }
            }
            else
            {
                if (CheckLineOfSight(RoomManager.Active.MapData) && RoomManager.Active.AllowBossActivation())
                    Awaken();
            }






            prevY = Velocity.Y;
            prevX = Position.X;
        }

        //The boss awaken method
        public void Awaken()
        {
            awake = true;
            RoomManager.Active.BossActive = true;
        }


    }
}
