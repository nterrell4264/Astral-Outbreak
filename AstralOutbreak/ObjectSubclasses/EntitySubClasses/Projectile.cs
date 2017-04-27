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

        //Reference to its source, so that it does not hit its source
        public GameObject Source { get; set; }

        public Projectile(Vector2 pos, float width, float height, float health, float damage, GameObject source = null, bool mobile = true) : base(pos, width, height, health, mobile)
        {
            Damage = damage;
            Source = source;
            Gravity = false;
        }

        //Each step, projectiles damage themselves
        public override void Step(float deltaTime)
        {
            Health -= deltaTime;
            base.Step(deltaTime);
        }

        //When Projectiles hit things they die and inflict damage.
        public void Strike(GameObject other)
        {
            if(other != Source && !(other is Projectile) && !IsDead)
            {
                Health = 0;
                if(other is Entity)
                    (other as Entity).Health -= Damage;
            }
                
        }
    }
}
