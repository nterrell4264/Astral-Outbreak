using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class JackRabbitBoss : Enemy
    {
        private JackRabbitState currentState;

        //prevY is a float used to store the previous Y value of the position, letting us know if the object is falling
        private float prevY;
        private float prevX;
        private bool awake;
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
                    MaxVelocity = new Vector(900, 900);
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
                return false;
            }

            set
            {
                //base.Unload = value;
                //if (Unload)
                //    MySwarm.Kill(this);
            }
        }

        public JackRabbitBoss(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            //Creates a weapon for the JackRabbitBoss,
            MyWeapon = new Weapon(.4f, 3, 900, 700);
            MyWeapon.BulletSize = 5;
            MyWeapon.Source = this;
            if(awake)


            prevY = Velocity.Y;
            prevX = Position.X;
            Gravity = true;
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //Check if awake
            if (awake)
            {
                switch (currentState)
                {
                    //When Idle, wait a bit, then charge the player
                    case JackRabbitState.Idle:
                        if(CurrentActionTime > 1f)
                        {
                            float diff = Position.X - RoomManager.Active.PlayerOne.Position.X;
                            if(diff < 0)
                            {
                                FaceRight = true;
                                Velocity = new Vector(900, -200);
                                currentState = JackRabbitState.Falling;
                            }
                            else
                            {
                                FaceRight = false;
                                Velocity = new Vector(-900, -200);
                                currentState = JackRabbitState.Falling;
                            }
                        }
                        break;
                    //When moving, apply friction
                    case JackRabbitState.Moving:
                        Velocity.X /= 1.1f;
                        if(Velocity.X < 1 && Velocity.X > -1)
                        {
                            currentState = JackRabbitState.Idle;
                        }
                        break;
                    //When Falling/Jumping check if we have landed
                    case JackRabbitState.Falling:
                        if(Velocity.Y == prevY)
                        {
                            currentState = JackRabbitState.Moving;
                        }
                        break;
                    default:
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
