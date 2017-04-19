using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;



namespace AstralOutbreak
{
    public enum PlayerState { Idle, Falling, Rolling, Dashing, Running, Damaged}

    
    /// <summary>
    /// Unit controlled directly by the player
    /// </summary>
    public class Player : Entity
    {
        private PlayerState currentplayerstate;
        private float speedLimit = 300;
        private float invulnTime = 0;

        public PlayerState CurrentPlayerState
        {
            get { return currentplayerstate; }
            set
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
                    CurrentPlayerState = PlayerState.Damaged;
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
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            Acceleration.X = 0;
            Acceleration.Y = 0;
            if (invulnTime > 0)
                invulnTime -= deltaTime;

            switch (CurrentPlayerState)
            {
                case PlayerState.Dashing:

                    break;

                case PlayerState.Falling:

                    break;

                case PlayerState.Idle:
                    if ((Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed)
                        && !(Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
                    {
                        CurrentPlayerState = PlayerState.Running;
                        FaceRight = false;
                        break;
                    }
                    if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed)
                        && !(Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
                    {
                        CurrentPlayerState = PlayerState.Running;
                        FaceRight = true;
                        break;
                    }
                    if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed))
                    {
                        Velocity.Y -= 200;
                        CurrentPlayerState = PlayerState.Falling;
                        break;
                    }
                    if ((Game1.Inputs.RollButtonState == ButtonStatus.Held || Game1.Inputs.RollButtonState == ButtonStatus.Pressed))
                    {
                        CurrentPlayerState = PlayerState.Rolling;
                        break;
                    }
                    if (Velocity.Y != 0)
                            CurrentPlayerState = PlayerState.Falling;
                    break;

                case PlayerState.Rolling:

                    break;

                case PlayerState.Running:

                    break;

                default:
                    CurrentPlayerState = PlayerState.Idle;
                    break;
            }

            //if(CurrentPlayerState == PlayerState.Damaged)
            //{
            //    if (CurrentActionTime >= 0.1f)
            //    {
            //        if (Velocity.Y == 0) currentplayerstate = PlayerState.Idle;
            //        else currentplayerstate = PlayerState.Falling;
            //    }
            //    else
            //    {
            //        SpeedLimit = 400;
            //        if (FaceRight) Velocity.X = 400;
            //        else Velocity.X = -400;
            //        SpeedLimit = speedLimit;
            //        return;
            //    }
            //}
            //if ((Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
            //{
            //    if (Velocity.X > -speedLimit / 2)
            //        Velocity.X = -speedLimit / 2;
            //    Acceleration.X += -5;
            //    FaceRight = false;
            //    CurrentPlayerState = PlayerState.Running;
            //}
            //if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
            //{
            //    if (Velocity.X < speedLimit / 2)
            //        Velocity.X = speedLimit / 2;
            //    Acceleration.X += 5;
            //    FaceRight = true;
            //    CurrentPlayerState = PlayerState.Running;
            //}
            ////Temporary Jump
            //if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed && CurrentPlayerState != PlayerState.Falling) && Velocity.Y == 0)
            //{
            //    Velocity.Y -= 200;
            //    CurrentPlayerState = PlayerState.Falling;
            //}
            //if ((Game1.Inputs.RollButtonState == ButtonStatus.Held || Game1.Inputs.RollButtonState == ButtonStatus.Pressed))
            //{
            //    invulnTime = 0.3f;
            //    CurrentActionTime = 0.3f;
            //    SpeedLimit = 2 * speedLimit;
            //    if (FaceRight) Velocity.X = 2 * speedLimit;
            //    else Velocity.X = -2 * speedLimit;
            //    SpeedLimit = speedLimit;
            //    CurrentPlayerState = PlayerState.Rolling;
            //}
            //
            ////Makes sure player stops when buttons arent pressed and can change direction easily
            //if (Velocity.X > 0 && (Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
            //{
            //    Velocity.X = 0;
            //    FaceRight = false;
            //    CurrentPlayerState = PlayerState.Running;
            //}
            //else if (Velocity.X < 0 && (Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
            //{
            //    Velocity.X = 0;
            //    FaceRight = true;
            //    CurrentPlayerState = PlayerState.Running;
            //}
            //else if (Acceleration.X == 0)
            //{
            //    Velocity.X = 0;
            //    CurrentPlayerState = PlayerState.Idle;
            //}
            //if (Game1.Inputs.M1Clicked)
            //    Shoot(new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX - Center.X, Game1.Inputs.MouseY + RoomManager.Active.CameraY - Center.Y));

            
            
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
