using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AstralOutbreak
{
    public class Weapon
    {
        //Source of the bullet
        public GameObject Source { get; set; }

        //Does the bullet obey gravity? Default to No
        public Boolean Gravity { get; set; }

        //minimum Delay between shots in seconds
        public float ShotDelay { get; set; }

        //Damage per bullet
        public float Damage { get; set; }

        //Health of the bullet
        private float bulletHealth;

        //Bullet Speed
        private float bulletSpeed;
        //Will not change range
        //Cannot be 0
        public float BulletSpeed
        {
            get { return bulletSpeed; }
            set
            {
                //Keep the range the same
                if (value > 0)
                    bulletHealth = Range / value;
                else if (value < 0)
                    bulletHealth = Range / value;
                if(value != 0)
                    bulletSpeed = value;
            }
        }

        //size of the square bullet
        public float BulletSize { get; set; }

        //Range of the bullet = speed * health
        //Will not change speed or direction
        public float Range
        {
            get
            {
                return bulletHealth * bulletSpeed;
            }
            set
            {
                if(value >= 0)
                    bulletHealth = value / bulletSpeed;
                else
                    bulletHealth = value / bulletSpeed;
            }
        }

        public bool MultiShot { get; set; }

        //Default constructor
        public Weapon()
        {
            ShotDelay = 1;
            Damage = 1;
            bulletHealth = 10;
            bulletSpeed = 100;
            Range = 50;
            BulletSize = 8;
            MultiShot = false;
            Gravity = false;
        }

        //Fancy Constructor
        public Weapon(float delay, float damage, float speed, float range)
        {
            ShotDelay = delay;
            Damage = damage;
            if (speed != 0)
                bulletSpeed = speed;
            else
                bulletSpeed = 1;
            Range = range;
            BulletSize = 1;
            MultiShot = false;
        }

        /// <summary>
        /// Shoot a bullet from a position in a direction
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="direction">Direction of shot</param>
        /// <returns></returns>
        public void Shoot(Vector direction)
        {
            if (Source != null)
            {
                Vector shotVel = direction * BulletSpeed / direction.Magnitude();
                var p = new Projectile(new Vector(Source.Center.X - BulletSize / 2, Source.Center.Y - BulletSize / 2), BulletSize, BulletSize, bulletHealth, Damage, Source);
                p.Gravity = this.Gravity;
                RoomManager.Active.AddBullet(p, shotVel);
                if (MultiShot)
                {
                    p = new Projectile(new Vector(Source.Center.X - BulletSize / 2, Source.Center.Y - BulletSize / 2), BulletSize, BulletSize, bulletHealth, Damage, Source);
                    p.Gravity = this.Gravity;
                    RoomManager.Active.AddBullet(p, shotVel.Rotate(Math.PI / 45));
                    p = new Projectile(new Vector(Source.Center.X - BulletSize / 2, Source.Center.Y - BulletSize / 2), BulletSize, BulletSize, bulletHealth, Damage, Source);
                    p.Gravity = this.Gravity;
                    RoomManager.Active.AddBullet(p, shotVel.Rotate(-Math.PI / 45));
                }
            }
        }


    }
}
