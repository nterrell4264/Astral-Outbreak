using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class MultiRabbit : Enemy
    {
        private JackRabbitState currentState;

        //prevY is a float used to store the previous Y value of the position, letting us know if the object is falling
        private float prevY;
        private float prevX;
        private bool awake;
        private int shotsTillMove;
        private int spinAttackTimer;

        public bool Spinning { get { return spinAttackTimer <= 0 && CurrentJackRabbitState == JackRabbitState.Shooting; } }

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

        public MultiRabbit(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            //Creates a weapon for the JackRabbitBoss,
            MyWeapon = new Weapon(.4f, 1, 900, 700);
            MyWeapon.BulletSize = 5;
            MyWeapon.MultiShot = true;
            MyWeapon.Source = this;
            MyWeapon.Gravity = false;
            prevY = Velocity.Y;
            prevX = Position.X;
            Gravity = true;
            currentState = JackRabbitState.Shooting;
            spinAttackTimer = 5;
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //Check if awake
            if (awake)
            {
                switch (currentState)
                {
                    //When moving, apply friction
                    case JackRabbitState.Moving:
                        Velocity.X /= 1.2f;
                        if(Velocity.X < 1 && Velocity.X > -1)
                        {
                            CurrentJackRabbitState = JackRabbitState.Shooting;
                            FaceRight = RoomManager.Active.PlayerOne.Position.X > Position.X;
                            Shoot(RoomManager.Active.PlayerOne.Center - Center);
                            shotsTillMove = 3;
                        }
                        break;
                    //When Falling/Jumping check if we have landed
                    case JackRabbitState.Falling:
                        Velocity.X = Velocity.X / 1.01f;
                        if (Velocity.Y == prevY)
                        {
                            if (Velocity.X < 1 && Velocity.X > -1)
                            {
                                CurrentJackRabbitState = JackRabbitState.Shooting;
                                FaceRight = RoomManager.Active.PlayerOne.Position.X > Position.X;
                                Shoot(RoomManager.Active.PlayerOne.Center - Center);
                                shotsTillMove = 3;
                            }
                            else
                                CurrentJackRabbitState = JackRabbitState.Moving;
                        }
                        break;
                    default:
                    case JackRabbitState.Shooting:
                        if (spinAttackTimer <= 0)
                        {
                            if(CurrentActionTime < 2)
                            {
                                MyWeapon.MultiShot = false;
                                FaceRight = !FaceRight;
                                MyWeapon.ShotDelay = 0;
                                MyWeapon.BulletSpeed = 600;
                                Shoot(new Vector((float)Game1.Rand.NextDouble() - .5f, -(float)Game1.Rand.NextDouble()));
                                MyWeapon.Gravity = true;
                                MyWeapon.Range = 500;
                            }
                            else
                            {
                                MyWeapon.Range = 700;
                                MyWeapon.Gravity = false;
                                MyWeapon.BulletSpeed = 400;
                                MyWeapon.MultiShot = true;
                                spinAttackTimer = 1 + (int)(4 * Health / MaxHealth);
                                MyWeapon.ShotDelay = .4f;
                                float v = 300 + Game1.Rand.Next(600);
                                float diff = Position.X - RoomManager.Active.PlayerOne.Position.X;
                                if (diff < 0)
                                {
                                    FaceRight = true;
                                    Velocity = new Vector(v, -400);
                                    CurrentJackRabbitState = JackRabbitState.Falling;
                                }
                                else
                                {
                                    FaceRight = false;
                                    Velocity = new Vector(-v, -400);
                                    CurrentJackRabbitState = JackRabbitState.Falling;
                                }
                            }
                        }
                        else if (CurrentActionTime > .8f)
                        {
                            {
                                if (shotsTillMove > 0)
                                {
                                    shotsTillMove--;
                                    Shoot(RoomManager.Active.PlayerOne.Center - Center);
                                    CurrentJackRabbitState = JackRabbitState.Shooting;
                                    FaceRight = RoomManager.Active.PlayerOne.Position.X > Position.X;
                                }
                                else
                                {
                                    spinAttackTimer--;
                                    float v = 300 + Game1.Rand.Next(600);
                                    float diff = Position.X - RoomManager.Active.PlayerOne.Position.X;
                                    if (diff < 0)
                                    {
                                        FaceRight = true;
                                        Velocity = new Vector(v, -400);
                                        CurrentJackRabbitState = JackRabbitState.Falling;
                                    }
                                    else
                                    {
                                        FaceRight = false;
                                        Velocity = new Vector(-v, -400);
                                        CurrentJackRabbitState = JackRabbitState.Falling;
                                    }
                                }
                            }
                        }
                        break;
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
