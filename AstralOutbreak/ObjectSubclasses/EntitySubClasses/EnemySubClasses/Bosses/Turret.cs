using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public class Turret: Enemy
    {
        public bool Awake { get; set; }
        //Am I regenerating or active?
        public bool Damaged { get; set; }
        //If I would die and a boss is actve, don't
        public override float Health
        {
            get
            {
                return base.Health;
            }

            set
            {
                base.Health = MaxHealth;
                if (IsDead)
                {
                    if (RoomManager.Active.BossActive)
                    {
                        Damaged = true;
                        IsDead = false;
                        CurrentActionTime = 0;
                        Shooting = false;
                    }
                }
            }
        }

        public Turret(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            //Creates a weapon for the Turret,
            MyWeapon = new Weapon(.2f, 1, 1000, 700);
            MyWeapon.BulletSize = 5;
            MyWeapon.Source = this;
            Gravity = false;
        }


        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //Check if awake
            if (Awake && !Damaged)
            {
                Shoot(RoomManager.Active.PlayerOne.Center - Center);
            }
            Awake = CheckLineOfSight(RoomManager.Active.MapData);
            if (Damaged && CurrentActionTime > 10)
            {
                Damaged = false;
                Health = MaxHealth;
            }
        }

    }
}
