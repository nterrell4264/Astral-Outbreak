﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class Enemy : Entity
    {
        private float damage;

        public Enemy(Vector2 pos, float width, float height, float health, float damage, bool mobile = true) : base(pos, width, height, health, mobile)
        {
            this.damage = damage;
        }

        public void Strike(GameObject other)
        {
            if (other is Entity && !(other is Projectile))
            {
                (other as Entity).Health -= damage;
            }

        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);

        }
    }
}
