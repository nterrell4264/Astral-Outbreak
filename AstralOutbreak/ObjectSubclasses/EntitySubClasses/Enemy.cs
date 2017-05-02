using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class Enemy : Entity
    {
        public float Damage { get; private set; }
        public bool Corrective { get; set; }

        public override float Health
        {
            get
            {
                return base.Health;
            }

            set
            {
                base.Health = value;
                if(base.Health <= 0)
                {
                    if(Game1.Rand.Next(100) < 25 && !(this is SwarmMob))
                    {
                        List<PhysicsObject> pickups = new List<PhysicsObject>();
                        pickups.Add(new Item(Position, 8, 8, ItemType.HealthPickup, 2));
                        RoomManager.Active.AddEntities(pickups);
                    }
                }
            }
        }

        public Enemy(Vector2 pos, float width, float height, float health, float damage, bool mobile = true) : base(pos, width, height, health, mobile)
        {
            this.Damage = damage;
        }


        public void Strike(GameObject other)
        {
            if (other is Entity && !(other is Projectile))
            {
                (other as Entity).Health -= Damage;
                if(other is BatShield)
                {
                    this.Health -= (other as BatShield).Damage;
                }
            }

        }

        public virtual bool CheckLineOfSight(Map map)
        {
            return (map.CheckLineOfSight(Center.X, Position.Y, RoomManager.Active.PlayerOne.Center.X, RoomManager.Active.PlayerOne.Position.Y));
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            Corrective = false;
        }
    }
}
