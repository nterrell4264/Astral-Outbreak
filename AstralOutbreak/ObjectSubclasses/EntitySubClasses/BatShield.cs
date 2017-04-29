using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    //A part of a Bat Shield around the Player these are manipulated inside the player class
    public class BatShield: Entity
    {
        //Time for shield movement, keeps shields in sync
        public static float ShieldTimer { get; set; }
        //Time for the shield to respawn (Counts down)
        public float RespawnTimer { get; set; }
        //Contact Damage for Enemies
        public float Damage { get; set; }
        //Shield doesn't get unloaded
        public override bool Unload
        {
            get
            {
                return false;
            }
        }
        public override float Health
        {
            get
            {
                return base.Health;
            }

            set
            {
                base.Health = value;
                //If we died, reset the respawn timer
                if (IsDead)
                    RespawnTimer = 5;
            }
        }

        //Sets the static variable
        static BatShield()
        {
            ShieldTimer = 0;
        }

        //Na na na na na na na na na na, Bat Shield.
        public BatShield(Vector2 pos, float width, float height, float health, float damage, bool mobile = false) : base(pos, width, height, health, mobile)
        {
            Gravity = false;
            Damage = damage;
            RespawnTimer = 1f;
        }
    }
}
