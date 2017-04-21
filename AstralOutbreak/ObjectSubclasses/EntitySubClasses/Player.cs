using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;



namespace AstralOutbreak
{
    public enum PlayerState { Idle, Falling, Rolling, Dashing, Running}

    
    /// <summary>
    /// Unit controlled directly by the player
    /// </summary>
    public class Player : Entity
    {
        private PlayerState currentplayerstate;
        private float speedLimit = 300;
        private float invulnTime = 0;
        private float previousY;

        public PlayerState CurrentPlayerState
        {
            get { return currentplayerstate; }
            private set
            {
                currentplayerstate = value;
                CurrentActionTime = 0;
            }
        }

        public float SpeedLimit
        {
            get { return speedLimit; }
            set { speedLimit = value; }
        }

        public float InvulnTime
        {
            get { return invulnTime; }
            set { invulnTime = value; }
        }

        public override bool Unload
        {
            get
            {
                return false;
            }
        }

        public override float Health
        {
            get { return base.Health; }
            set
            {
                if (value < Health && invulnTime == 0)
                {
                    invulnTime = 0.2f;  
                }
                base.Health = value;
                IsDead = Health <= 0;
            }
        }

        public Player(Vector2 pos, float width, float height, float health, bool mobile = true) : base(pos, width, height, health, mobile)
        {
            Gravity = true;
            MaxVelocity.X = speedLimit;
            MyWeapon = new Weapon(.2f, 3, 500, 5000);
            MyWeapon.Source = this;
            MyWeapon.BulletSize = 5;
            previousY = Velocity.Y;
        }
        
        /// <summary>
        /// Called once per update
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //Reset accel
            Acceleration.X = 0;
            Acceleration.Y = 0;
            if (invulnTime > 0)
                invulnTime -= deltaTime;

            //Single state drive for the player
            switch (CurrentPlayerState)
            {
                //Dash
                case PlayerState.Dashing:
                    if (CurrentActionTime > .25f)
                    {
                        CurrentPlayerState = PlayerState.Falling;
                        MaxVelocity.X = 300f;
                        Velocity.X /= 2;
                        Gravity = true;
                    }
                    break;
                //In the air
                case PlayerState.Falling:
                    if(previousY == Velocity.Y)
                    {
                        if(Velocity.X == 0)
                        {
                            CurrentPlayerState = PlayerState.Idle;
                        }
                        else
                        {
                            CurrentPlayerState = PlayerState.Running;
                        }
                        break;
                    }
                    if (!(Game1.Inputs.RightButtonState == ButtonStatus.Unpressed) && !(Game1.Inputs.LeftButtonState == ButtonStatus.Unpressed))
                    {
                        Velocity.X = 0;
                    }
                    if (Velocity.X > 0)
                    {
                        if (Velocity.X < speedLimit / 2)
                            Velocity.X = speedLimit / 2;
                        Acceleration.X += 10;

                        if ((Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
                        {
                            Velocity.X = -.1f;
                            Acceleration.X = -10;
                            break;
                        }
                        else if (Game1.Inputs.RightButtonState == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            Acceleration.X = 0;
                        }
                        if (Game1.Inputs.DashButtonState == ButtonStatus.Pressed)
                        {
                            MaxVelocity.X = 600f;
                            Velocity.X = MaxVelocity.X;
                            CurrentPlayerState = PlayerState.Dashing;
                            Velocity.Y = 0;
                            Gravity = false;
                            break;
                        }
                    }
                    else if(Velocity.X < 0)
                    {
                        if (Velocity.X > -speedLimit / 2)
                            Velocity.X = -speedLimit / 2;
                        Acceleration.X += -10;

                        if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
                        {
                            Velocity.X = .1f;
                            Acceleration.X = 10;
                            break;
                        }
                        else if (Game1.Inputs.LeftButtonState == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            Acceleration.X = 0;
                        }
                        if (Game1.Inputs.DashButtonState == ButtonStatus.Pressed)
                        {
                            MaxVelocity.X = 600f;
                            Velocity.X = -MaxVelocity.X;
                            CurrentPlayerState = PlayerState.Dashing;
                            Velocity.Y = 0;
                            Gravity = false;
                            break;
                        }
                    }
                    else
                    {
                        if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
                        {
                            if(!(Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
                                Velocity.X = .1f;
                            break;
                        }
                        else if ((Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
                        {
                            Velocity.X = -.1f;
                            break;
                        }
                        if (Game1.Inputs.DashButtonState == ButtonStatus.Pressed)
                        {
                            MaxVelocity.X = 600f;
                            if (FaceRight)
                                Velocity.X = MaxVelocity.X;
                            else
                                Velocity.X = -MaxVelocity.X;
                            CurrentPlayerState = PlayerState.Dashing;
                            Velocity.Y = 0;
                            Gravity = false;
                            break;
                        }
                    }
                    break;
                //Not moving
                case PlayerState.Idle:
                    Velocity.X = 0;
                    if ((Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed)
                        && !(Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
                    {
                        Velocity.X = -.1f;
                        CurrentPlayerState = PlayerState.Running;
                        if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed))
                        {
                            Velocity.Y -= 310;
                            CurrentPlayerState = PlayerState.Falling;
                            break;
                        }
                        break;
                    }
                    if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed)
                        && !(Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
                    {
                        Velocity.X = .1f;
                        CurrentPlayerState = PlayerState.Running;
                        if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed))
                        {
                            Velocity.Y -= 310;
                            CurrentPlayerState = PlayerState.Falling;
                            break;
                        }
                        break;
                    }
                    if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed))
                    {
                        Velocity.Y -= 310;
                        CurrentPlayerState = PlayerState.Falling;
                        break;
                    }
                    if ((Game1.Inputs.RollButtonState == ButtonStatus.Held || Game1.Inputs.RollButtonState == ButtonStatus.Pressed))
                    {
                        CurrentPlayerState = PlayerState.Rolling;
                        break;
                    }
                    if (Velocity.Y != previousY)
                            CurrentPlayerState = PlayerState.Falling;
                    if (Game1.Inputs.DashButtonState == ButtonStatus.Pressed)
                    {
                        MaxVelocity.X = 600f;
                        if (FaceRight)
                            Velocity.X = MaxVelocity.X;
                        else
                            Velocity.X = -MaxVelocity.X;
                        CurrentPlayerState = PlayerState.Dashing;
                        Gravity = false;
                        break;
                    }
                    break;
                //Roll sequence
                case PlayerState.Rolling:

                    break;
                //Moving on the ground
                case PlayerState.Running:
                    if(!(Game1.Inputs.RightButtonState == ButtonStatus.Unpressed) && !(Game1.Inputs.LeftButtonState == ButtonStatus.Unpressed))
                    {
                        Velocity.X = 0;
                        CurrentPlayerState = PlayerState.Idle;
                        break;
                    }
                    if (Velocity.X > 0)
                    {
                        if (Game1.Inputs.RightButtonState == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            CurrentPlayerState = PlayerState.Idle;
                            break;
                        }
                        if (Velocity.X < speedLimit / 2)
                            Velocity.X = speedLimit / 2;
                        Acceleration.X += 10;

                        if (Velocity.X > 0 && (Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
                        {
                            Velocity.X = -.1f;
                            CurrentPlayerState = PlayerState.Running;
                        }
                        if(Game1.Inputs.RightButtonState == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            CurrentPlayerState = PlayerState.Idle;
                        }
                        if(Game1.Inputs.DashButtonState == ButtonStatus.Pressed)
                        {
                            MaxVelocity.X = 600f;
                            Velocity.X = MaxVelocity.X;
                            CurrentPlayerState = PlayerState.Dashing;
                            Gravity = false;
                            break;
                        }
                    }
                    else if(Velocity.X < 0)
                    {
                        if (Game1.Inputs.LeftButtonState == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            CurrentPlayerState = PlayerState.Idle;
                            break;
                        }
                        if (Velocity.X > -speedLimit / 2)
                            Velocity.X = -speedLimit / 2;
                        Acceleration.X += -10;

                        if (Velocity.X < 0 && (Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
                        {
                            Velocity.X = .1f;
                            CurrentPlayerState = PlayerState.Running;
                        }
                        if (Game1.Inputs.DashButtonState == ButtonStatus.Pressed)
                        {
                            MaxVelocity.X = 600f;
                            Velocity.X = -MaxVelocity.X;
                            CurrentPlayerState = PlayerState.Dashing;
                            Gravity = false;
                            break;
                        }

                    }
                    else
                    {
                        if ((Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed)
                        && (Game1.Inputs.RightButtonState == ButtonStatus.Unpressed))
                        {
                            Velocity.X = -.1f;
                        }
                        else if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed)
                            && (Game1.Inputs.LeftButtonState == ButtonStatus.Unpressed))
                        {
                            Velocity.X = .1f;
                        }
                        else
                            CurrentPlayerState = PlayerState.Idle;
                    }

                    if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed))
                    {
                        Velocity.Y -= 310;
                        CurrentPlayerState = PlayerState.Falling;
                        break;
                    }
                    if ((Game1.Inputs.RollButtonState == ButtonStatus.Held || Game1.Inputs.RollButtonState == ButtonStatus.Pressed))
                    {
                        CurrentPlayerState = PlayerState.Rolling;
                        break;
                    }
                    if (Velocity.Y != previousY)
                    {
                        CurrentPlayerState = PlayerState.Falling;
                        break;
                    }
                    break;

                default:
                    CurrentPlayerState = PlayerState.Idle;
                    break;
            }
            Vector aim = new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX - Center.X, Game1.Inputs.MouseY + RoomManager.Active.CameraY - Center.Y);
            if (Game1.Inputs.M1Clicked && CurrentPlayerState != PlayerState.Dashing)
            {
                Shoot(aim);
                if (aim.X > 0)
                    FaceRight = true;
                else if (aim.X < 0)
                    FaceRight = false;
            }
            else
            {
                if (Velocity.X > 0)
                    FaceRight = true;
                else if (Velocity.X < 0)
                    FaceRight = false;
            }
            previousY = Velocity.Y;

           
            
            
        }



        public void Consume(Item other)
        {
            switch (other.MyType)
            {
                case ItemType.HealthPickup:
                    Health += other.Value;
                    break;
                case ItemType.WeaponUpgrade:
                    MyWeapon.Damage += other.Value;
                    break;
                case ItemType.AbilityUnlock:
                    break;
                default:
                    break;
            }
            other.Unload = true;
        }




    }//End Class
}
