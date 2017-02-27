using System;
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

        public Projectile(Vector2 pos, float width, float height, float health, float damage, bool mobile = true) : base(pos, width, height, health, mobile)
        {
            Damage = damage;
            CollisionEvent += Strike;
        }

        //Each step, projectiles damage themselves
        public override void Step(float deltaTime)
        {
            Health -= deltaTime;
            base.Step(deltaTime);
        }

        //When Projectiles hit things they die and inflict damage.
        public void Strike(PhysicsObject other)
        {
            Health = 0;
            if (other is Entity)
                (other as Entity).Health -= Damage;
        }
    }
}
