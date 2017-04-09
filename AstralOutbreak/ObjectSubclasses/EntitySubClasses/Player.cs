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
        private float speedLimit = 500;
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
                if (value < Health) CurrentPlayerState = PlayerState.Damaged;
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
            if(CurrentPlayerState == PlayerState.Damaged)
            {
                if (CurrentActionTime >= 0.1f)
                {
                    if (Velocity.Y == 0) currentplayerstate = PlayerState.Idle;
                    else currentplayerstate = PlayerState.Falling;
                }
                else
                {
                    if (FaceRight) Velocity.X = -100;
                    else Velocity.X = 100;
                    return;
                }
            }
            if ((Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
            {
                if (Velocity.X > -speedLimit / 10)
                    Velocity.X = -speedLimit / 10;
                Acceleration.X += -5;
                FaceRight = false;
                CurrentPlayerState = PlayerState.Running;
            }
            if ((Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
            {
                if (Velocity.X < speedLimit / 10)
                    Velocity.X = speedLimit / 10;
                Acceleration.X += 5;
                FaceRight = true;
                CurrentPlayerState = PlayerState.Running;
            }
            //Temporary Jump
            if ((Game1.Inputs.JumpButtonState == ButtonStatus.Pressed && CurrentPlayerState != PlayerState.Falling) && Velocity.Y == 0)
            {
                Velocity.Y -= 200;
                CurrentPlayerState = PlayerState.Falling;
            }
            //Makes sure player stops when buttons arent pressed and can change direction easily
            if (Velocity.X > 0 && (Game1.Inputs.LeftButtonState == ButtonStatus.Held || Game1.Inputs.LeftButtonState == ButtonStatus.Pressed))
            {
                Velocity.X = 0;
                FaceRight = false;
                CurrentPlayerState = PlayerState.Running;
            }
            else if (Velocity.X < 0 && (Game1.Inputs.RightButtonState == ButtonStatus.Held || Game1.Inputs.RightButtonState == ButtonStatus.Pressed))
            {
                Velocity.X = 0;
                FaceRight = true;
                CurrentPlayerState = PlayerState.Running;
            }
            else if (Acceleration.X == 0)
            {
                Velocity.X = 0;
                CurrentPlayerState = PlayerState.Idle;
            }
            if (Game1.Inputs.M1Clicked)
                Shoot(new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX - Center.X, Game1.Inputs.MouseY + RoomManager.Active.CameraY - Center.Y));

            
            
        }
    }
}
