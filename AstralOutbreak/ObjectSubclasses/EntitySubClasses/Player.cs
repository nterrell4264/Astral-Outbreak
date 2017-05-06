using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;



namespace AstralOutbreak
{
    public enum PlayerState { Idle, Falling, Rolling, Dashing, Running}

    [Flags] public enum Upgrades
    {
        None = 0,
        Dash = 1,
        BatShield = 2,
        MultiShot = 4,


    }


    /// <summary>
    /// Unit controlled directly by the player
    /// </summary>
    public class Player : Entity
    {
        private PlayerState currentplayerstate;
        private float speedLimit = 300;
        private float invulnTime = 0;
        private float previousY;
        private float jumpTime;
        private bool canAirDash;
        private const float JUMPTIME = .25f;
        private const float DASHSPEED = 900;
        private const float ROLLSPEED = 450;



        //Batshields
        public BatShield[] Shield { get; set; }

        public PlayerState CurrentPlayerState
        {
            get { return currentplayerstate; }
            private set
            {
                if (value == PlayerState.Falling && currentplayerstate != PlayerState.Dashing && Game1.Inputs[ActionButton.JumpButton].Status == ButtonStatus.Pressed)
                    jumpTime = JUMPTIME;
                else jumpTime = 0;
                currentplayerstate = value;
                CurrentActionTime = 0;
                if (currentplayerstate == PlayerState.Dashing || currentplayerstate == PlayerState.Rolling)
                    Shooting = false;
            }
        }

        public override Vector BulletSource
        {
            get
            {
                if(FaceRight)
                    return base.BulletSource + Aim * 20 / Aim.Magnitude() + Aim.Rotate(-Math.PI / 2) * 6.5f / Aim.Magnitude();
                else
                    return base.BulletSource + Aim * 20 / Aim.Magnitude() + Aim.Rotate(Math.PI / 2) * 6.5f / Aim.Magnitude();
            }
        }

        public bool UpwardDash { get; set; }

        public Upgrades MyUpgrades { get; set; }

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
                if (value < Health && invulnTime <= 0)
                {
                    invulnTime = 1f;  
                }
                base.Health = value;
                IsDead = Health <= 0;
                if (IsDead)
                {
                    DialogueManager.Update(Triggers.Death);
                }
            }
        }

        public Player(Vector2 pos, float width, float height, float health, bool mobile = true) : base(pos, width, height, health, mobile)
        {
            Gravity = true;
            MaxVelocity.X = speedLimit;
            MyWeapon = new Weapon(.2f, 3, 900, 900);
            MyWeapon.Source = this;
            MyWeapon.BulletSize = 5;
            previousY = Velocity.Y;
            jumpTime = JUMPTIME;
            Shield = new BatShield[5];
            UpwardDash = false;
            for(int i = 0; i < Shield.Count(); i++)
            {
                Shield[i] = new BatShield(new Vector(0, 0), 16, 16, 1, 5);
            }

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
                        if(!UpwardDash)
                            MaxVelocity.X = speedLimit;
                        else
                            MaxVelocity.Y = -1;
                        Velocity.X /= 2;
                        Gravity = true;
                        UpwardDash = false;
                    }
                    break;
                //In the air
                case PlayerState.Falling:
                    if(previousY == Velocity.Y && jumpTime == 0 && Velocity.Y != MaxVelocity.Y)
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
                    
                    //If we are jumping
                    if (jumpTime > 0 && ((Game1.Inputs[ActionButton.JumpButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.JumpButton].Status == ButtonStatus.Pressed || jumpTime > JUMPTIME * 2 / 3)) && Velocity.Y < 0)
                    {
                        jumpTime -= deltaTime;
                        Velocity.Y = -84 / JUMPTIME;
                    }
                    else
                    {
                        jumpTime = 0;
                        if (Velocity.Y < 0)
                            Velocity.Y /= 1.1f;
                    }
                    if (!(Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Unpressed) && !(Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Unpressed))
                    {
                        Velocity.X = 0;

                    }
                    if (Velocity.X > 0)
                    {
                        if (Velocity.X < speedLimit / 2)
                            Velocity.X = speedLimit / 2;
                        Acceleration.X += 10;

                        if ((Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Pressed))
                        {
                            Velocity.X = -.1f;
                            Acceleration.X = -10;
                            break;
                        }
                        else if (Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            Acceleration.X = 0;
                        }
                        if (Game1.Inputs[ActionButton.DashButton].Status == ButtonStatus.Pressed && MyUpgrades.HasFlag(Upgrades.Dash) && canAirDash)
                        {
                            MaxVelocity.X = DASHSPEED;
                            Velocity.X = MaxVelocity.X;
                            CurrentPlayerState = PlayerState.Dashing;
                            Velocity.Y = 0;
                            Gravity = false;
                            canAirDash = false;
                            break;
                        }
                    }
                    else if(Velocity.X < 0)
                    {
                        if (Velocity.X > -speedLimit / 2)
                            Velocity.X = -speedLimit / 2;
                        Acceleration.X += -10;

                        if ((Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Pressed))
                        {
                            Velocity.X = .1f;
                            Acceleration.X = 10;
                            break;
                        }
                        else if (Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            Acceleration.X = 0;
                        }
                        if (Game1.Inputs[ActionButton.DashButton].Status == ButtonStatus.Pressed && MyUpgrades.HasFlag(Upgrades.Dash) && canAirDash)
                        {
                            MaxVelocity.X = DASHSPEED;
                            Velocity.X = -MaxVelocity.X;
                            CurrentPlayerState = PlayerState.Dashing;
                            Velocity.Y = 0;
                            Gravity = false;
                            canAirDash = false;
                            break;
                        }
                    }
                    else
                    {
                        if ((Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Pressed))
                        {
                            if(!(Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Pressed))
                                Velocity.X = .1f;
                            break;
                        }
                        else if ((Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Pressed))
                        {
                            Velocity.X = -.1f;
                            break;
                        }
                        if (Game1.Inputs[ActionButton.DashButton].Status == ButtonStatus.Pressed && MyUpgrades.HasFlag(Upgrades.Dash) && canAirDash)
                        {
                            //if (Game1.Inputs[ActionButton.DashButton].Status == ButtonStatus.Pressed && MyUpgrades.HasFlag(Upgrades.Dash) && canAirDash && Game1.Inputs[ActionButton.JumpButton].Status == ButtonStatus.Held)
                            //{
                                MaxVelocity.Y = DASHSPEED;
                                Velocity.Y = -MaxVelocity.Y;
                                CurrentPlayerState = PlayerState.Dashing;
                                Gravity = false;
                                canAirDash = false;
                                UpwardDash = true;
                                break;
                            //}
                            //MaxVelocity.X = DASHSPEED;
                            //if (FaceRight)
                            //    Velocity.X = MaxVelocity.X;
                            //else
                            //    Velocity.X = -MaxVelocity.X;
                            //CurrentPlayerState = PlayerState.Dashing;
                            //Velocity.Y = 0;
                            //Gravity = false;
                            //canAirDash = false;
                            //break;
                        }
                    }
                    break;
                //Not moving
                case PlayerState.Idle:
                    Velocity.X = 0;
                    if ((Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Pressed)
                        && !(Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Pressed))
                    {
                        Velocity.X = -.1f;
                        CurrentPlayerState = PlayerState.Running;
                        if ((Game1.Inputs[ActionButton.JumpButton].Status == ButtonStatus.Pressed))
                        {
                            Velocity.Y -= 310;
                            CurrentPlayerState = PlayerState.Falling;
                            break;
                        }
                        break;
                    }
                    if ((Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Pressed)
                        && !(Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Pressed))
                    {
                        Velocity.X = .1f;
                        CurrentPlayerState = PlayerState.Running;
                        if ((Game1.Inputs[ActionButton.JumpButton].Status == ButtonStatus.Pressed))
                        {
                            Velocity.Y -= 310;
                            CurrentPlayerState = PlayerState.Falling;
                            break;
                        }
                        break;
                    }
                    if ((Game1.Inputs[ActionButton.JumpButton].Status == ButtonStatus.Pressed))
                    {
                        Velocity.Y -= 310;
                        CurrentPlayerState = PlayerState.Falling;
                        break;
                    }
                    if ((Game1.Inputs[ActionButton.RollButton].Status == ButtonStatus.Pressed))
                    {
                        MaxVelocity.X = ROLLSPEED;
                        if (FaceRight)
                            Velocity.X = MaxVelocity.X;
                        else
                            Velocity.X = -MaxVelocity.X;
                        Position.Y += Height / 2;
                        Height = Height / 2;
                        CurrentPlayerState = PlayerState.Rolling;
                        break;
                    }
                    if (Velocity.Y != previousY)
                            CurrentPlayerState = PlayerState.Falling;
                    if (Game1.Inputs[ActionButton.DashButton].Status == ButtonStatus.Pressed && MyUpgrades.HasFlag(Upgrades.Dash))
                    {
                        MaxVelocity.X = DASHSPEED;
                        if (FaceRight)
                            Velocity.X = MaxVelocity.X;
                        else
                            Velocity.X = -MaxVelocity.X;
                        CurrentPlayerState = PlayerState.Dashing;
                        Gravity = false;
                        break;
                    }
                    if(!canAirDash) canAirDash = true;
                    break;
                //Roll sequence
                case PlayerState.Rolling:
                    if (CurrentActionTime > .25f && !RoomManager.Active.CheckCollision(this, new Vector(0, -Height)))
                    {
                        CurrentPlayerState = PlayerState.Falling;
                        MaxVelocity.X = speedLimit;
                        Velocity.X /= 2;
                        Position.Y -= Height;
                        Height *= 2;
                        invulnTime = 0;
                    }
                    else
                    {
                        if (Velocity.X != ROLLSPEED && FaceRight && RoomManager.Active.CheckCollision(this, new Vector(0, -Height)) && !RoomManager.Active.CheckCollision(this, new Vector(0, Height)))
                        {
                            Velocity.X = -ROLLSPEED;
                            FaceRight = false;
                        }
                        if (Velocity.X != -ROLLSPEED && !FaceRight && RoomManager.Active.CheckCollision(this, new Vector(0, -Height)) && RoomManager.Active.CheckCollision(this, new Vector(0, Height)))
                        {
                            Velocity.X = ROLLSPEED;
                            FaceRight = true;
                        }
                    }
                    invulnTime = .0001f;
                    break;
                //Moving on the ground
                case PlayerState.Running:
                    if(!(Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Unpressed) && !(Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Unpressed))
                    {
                        Velocity.X = 0;
                        CurrentPlayerState = PlayerState.Idle;
                        break;
                    }
                    if (Velocity.X > 0)
                    {
                        if (Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            CurrentPlayerState = PlayerState.Idle;
                            break;
                        }
                        if (Velocity.X < speedLimit / 2)
                            Velocity.X = speedLimit / 2;
                        Acceleration.X += 10;

                        if (Velocity.X > 0 && (Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Pressed))
                        {
                            Velocity.X = -.1f;
                            CurrentPlayerState = PlayerState.Running;
                        }
                        if(Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            CurrentPlayerState = PlayerState.Idle;
                        }
                        if(Game1.Inputs[ActionButton.DashButton].Status == ButtonStatus.Pressed && MyUpgrades.HasFlag(Upgrades.Dash))
                        {
                            MaxVelocity.X = DASHSPEED;
                            Velocity.X = MaxVelocity.X;
                            CurrentPlayerState = PlayerState.Dashing;
                            Gravity = false;
                            break;
                        }
                    }
                    else if(Velocity.X < 0)
                    {
                        if (Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Unpressed)
                        {
                            Velocity.X = 0;
                            CurrentPlayerState = PlayerState.Idle;
                            break;
                        }
                        if (Velocity.X > -speedLimit / 2)
                            Velocity.X = -speedLimit / 2;
                        Acceleration.X += -10;

                        if (Velocity.X < 0 && (Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Pressed))
                        {
                            Velocity.X = .1f;
                            CurrentPlayerState = PlayerState.Running;
                        }
                        if (Game1.Inputs[ActionButton.DashButton].Status == ButtonStatus.Pressed && MyUpgrades.HasFlag(Upgrades.Dash))
                        {
                            MaxVelocity.X = DASHSPEED;
                            Velocity.X = -MaxVelocity.X;
                            CurrentPlayerState = PlayerState.Dashing;
                            Gravity = false;
                            break;
                        }

                    }
                    else
                    {
                        if ((Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Pressed)
                        && (Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Unpressed))
                        {
                            Velocity.X = -.1f;
                        }
                        else if ((Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Held || Game1.Inputs[ActionButton.RightButton].Status == ButtonStatus.Pressed)
                            && (Game1.Inputs[ActionButton.LeftButton].Status == ButtonStatus.Unpressed))
                        {
                            Velocity.X = .1f;
                        }
                        else
                            CurrentPlayerState = PlayerState.Idle;
                    }

                    if ((Game1.Inputs[ActionButton.JumpButton].Status == ButtonStatus.Pressed))
                    {
                        Velocity.Y -= 310;
                        CurrentPlayerState = PlayerState.Falling;
                        break;
                    }
                    if ((Game1.Inputs[ActionButton.RollButton].Status == ButtonStatus.Pressed))
                    {
                        MaxVelocity.X = ROLLSPEED;
                        if (Velocity.X > 0)
                            Velocity.X = MaxVelocity.X;
                        else if(Velocity.X < 0)
                            Velocity.X = -MaxVelocity.X;
                        else if (FaceRight)
                            Velocity.X = MaxVelocity.X;
                        else
                            Velocity.X = -MaxVelocity.X;
                        Position.Y += Height / 2;
                        Height = Height / 2;
                        CurrentPlayerState = PlayerState.Rolling;
                        break;
                    }
                    if (Velocity.Y != previousY)
                    {
                        CurrentPlayerState = PlayerState.Falling;
                        break;
                    }
                    if (!canAirDash) canAirDash = true;
                    break;

                default:
                    CurrentPlayerState = PlayerState.Idle;
                    break;
            }
            Vector aim = new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX - Center.X, Game1.Inputs.MouseY + RoomManager.Active.CameraY - Center.Y);
            if (aim.Magnitude() > 56)
            {
                if (FaceRight)
                    aim = base.BulletSource + aim * 20 / aim.Magnitude() + aim.Rotate(-Math.PI / 2) * 6.5f / aim.Magnitude();
                else
                    aim = base.BulletSource + aim * 20 / aim.Magnitude() + aim.Rotate(Math.PI / 2) * 6.5f / aim.Magnitude();
                Aim = new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX - aim.X, Game1.Inputs.MouseY + RoomManager.Active.CameraY - aim.Y);
            }
            else
            {
                if(FaceRight)
                    Aim = new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX - Center.X, Game1.Inputs.MouseY + RoomManager.Active.CameraY - Center.Y).Rotate(Math.PI/45);
                else
                    Aim = new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX - Center.X, Game1.Inputs.MouseY + RoomManager.Active.CameraY - Center.Y).Rotate(-Math.PI / 45);
            }
            aim = new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX - Center.X, Game1.Inputs.MouseY + RoomManager.Active.CameraY - Center.Y);
            if (//(Game1.Inputs.M1State == ButtonStatus.Held || Game1.Inputs.M1State == ButtonStatus.Pressed) && 
                CurrentPlayerState != PlayerState.Dashing && CurrentPlayerState != PlayerState.Rolling)
            {
                if ((Game1.Inputs.M1State == ButtonStatus.Held || Game1.Inputs.M1State == ButtonStatus.Pressed))
                {
                    Shoot(Aim);
                }
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
            if (MyUpgrades.HasFlag(Upgrades.BatShield))
                UpdateBatShield(deltaTime);
            
            
        }


        /// <summary>
        /// Player consumes the item
        /// </summary>
        /// <param name="other"></param>
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
                    switch (other.Value)
                    {
                        case 0:
                            if (MaxHealth <= 95 && !other.Consumed)
                                MaxHealth += 5;
                            Health = MaxHealth;
                            break;
                        case 1:
                            MyUpgrades = MyUpgrades | Upgrades.Dash;
                            DialogueManager.Update(Triggers.DashPickup);
                            break;
                        case 2:
                            MyUpgrades = MyUpgrades | Upgrades.BatShield;
                            DialogueManager.Update(Triggers.BatShieldPickup);
                            break;
                        case 3:
                            MyUpgrades = MyUpgrades | Upgrades.MultiShot;
                            MyWeapon.MultiShot = true;
                            DialogueManager.Update(Triggers.MultiShotPickup);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            other.Consumed = true;
            other.Unload = true;
            Game1.menuManager.Reload();
        }

        //Updates the shield of bats
        public void UpdateBatShield(float deltaTime)
        {
            BatShield.ShieldTimer += deltaTime;
            for(int i = 0; i < Shield.Count(); i++)
            {
                if (Shield[i].RespawnTimer <= 0)
                {
                    Shield[i].Center = new Vector (Center.X + (42 * (float)Math.Cos(3 * BatShield.ShieldTimer * Math.PI / 2 + i * 2 * Math.PI / Shield.Count())),  
                                                    Center.Y + (42 * (float)Math.Sin(3 * BatShield.ShieldTimer * Math.PI / 2 + i * 2 * Math.PI / Shield.Count())));
                }
                else
                {
                    Shield[i].RespawnTimer -= deltaTime;
                    if(Shield[i].RespawnTimer <= 0)
                    {
                        Shield[i].Health = Shield[i].MaxHealth;
                        RoomManager.Active.AddEntity(Shield[i]);
                        Shield[i].Center = new Vector(Center.X + (42 * (float)Math.Cos(3 * BatShield.ShieldTimer * Math.PI / 2 + i * 2 * Math.PI / Shield.Count())),
                                                    Center.Y + (42 * (float)Math.Sin(3 * BatShield.ShieldTimer * Math.PI / 2 + i * 2 * Math.PI / Shield.Count())));
                    }
                }
            }
        }

        

    }//End Class
}
