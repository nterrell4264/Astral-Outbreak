using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class CoreBoss : Enemy, Boss
    {
        public static bool CoreLives { get; set; }
        private bool awake;
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
                    CoreLives = false;
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
            }
        }
        static CoreBoss()
        {
            CoreLives = true;
        }
        public CoreBoss(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            //Creates a weapon for the JackRabbitBoss,
            MyWeapon = new Weapon(.4f, 1, 900, 700);
            MyWeapon.BulletSize = 5;
            MyWeapon.Source = this;
            Gravity = false;
            MyWeapon.MultiShot = true;
            MyWeapon.ShotDelay = 0;
            MyWeapon.BulletSpeed = 600;
            MyWeapon.Gravity = true;
            MyWeapon.Range = 448;
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //Check if awake
            if (awake)
            {
                
            }
            else
            {
                if (CheckLineOfSight(RoomManager.Active.MapData) && RoomManager.Active.AllowBossActivation())
                    Awaken();
            }
        }

        //The boss awaken method
        public void Awaken()
        {
            awake = true;
            RoomManager.Active.BossActive = true;
            RoomManager.Active.CurrentBoss = this;
        }


    }
}
