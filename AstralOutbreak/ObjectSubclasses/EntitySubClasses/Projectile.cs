﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{

    public class Projectile : Entity
    {
        //Damage that the projectile inflicts on contact
        public float Damage { get; set; }

        //Reference to its source, so that it does not hit its source
        public GameObject Source { get; set; }

        public Projectile(Vector2 pos, float width, float height, float health, float damage, GameObject source = null, bool mobile = true) : base(pos, width, height, health, mobile)
        {
            if (source is CoreBoss || source is MultiRabbit)
                Collides = false;
            Damage = damage;
            Source = source;
            Gravity = false;
            if (source is Player)
                Velocity = source.Velocity + new Vector(0, 0);
            else
                Velocity = new Vector(0,0);
        }

        //Each step, projectiles damage themselves
        public override void Step(float deltaTime)
        {
            Health -= deltaTime;
            base.Step(deltaTime);
            if(!Collides)
            {
                for(int i = 0; i < 5; i++)
                {
                    if (this.CheckCollision(RoomManager.Active.PlayerOne.Shield[i]) && !(Source is Player))
                        Strike(RoomManager.Active.PlayerOne.Shield[i]);
                }
            }
        }

        //When Projectiles hit things they die and inflict damage.
        public void Strike(GameObject other)
        {
            if (!(other is Projectile))
            {
                if (Source is Player)
                {
                    if ((other is Enemy || other is Wall) && !(other is Projectile) && !IsDead)
                    {
                        Health = 0;
                        if (other is Entity)
                            (other as Entity).Health -= Damage;
                    }
                }
                else
                {
                    if (!(other is Enemy) && !IsDead)
                    {
                        Health = 0;
                        if (other is Entity)
                            (other as Entity).Health -= Damage;
                    }
                }
            }
        }
    }
}
