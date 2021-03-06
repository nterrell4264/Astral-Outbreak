﻿using System;
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
                if(!Damaged)
                    base.Health = value;
                if (IsDead)
                {
                    if (RoomManager.Active.BossActive)
                    {
                        Damaged = true;
                        IsDead = false;
                        CurrentActionTime = 3;
                        Shooting = false;
                    }
                }
            }
        }

        public Turret(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            //Creates a weapon for the Turret,
            MyWeapon = new Weapon(.2f, 2, 1000, 1000);
            MyWeapon.BulletSize = 5;
            MyWeapon.Source = this;
            Gravity = false;
            Aim = new Vector(0, -1);
        }


        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //Check if awake
            if (Awake && !Damaged)
            {
                if (CurrentActionTime < 3)
                    Shoot(RoomManager.Active.PlayerOne.Center - Center);
                else if (CurrentActionTime > 5)
                    CurrentActionTime = 0;


                //if (CoreBoss.CoreLives && RoomManager.Active.AllowBossActivation() && !RoomManager.Active.BossActive)
                //{
                //    RoomManager.Active.BossActive = true;
                //}
            }
            else if(!Damaged)
            {
                CurrentActionTime = 3;
            }
            Awake = CheckLineOfSight(RoomManager.Active.MapData);
            if (Damaged && CurrentActionTime > 10)
            {
                Damaged = false;
                Health = MaxHealth;
            }
        }


        public override bool CheckLineOfSight(Map map)
        {
            return (map.CheckLineOfSight(Center.X, Center.Y, RoomManager.Active.PlayerOne.Center.X, RoomManager.Active.PlayerOne.Center.Y));
        }

    }
}
