using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public class Pod : Enemy
    {
        private bool rest;

        //Is the pod at rest, or shooting?
        public bool Resting
        {
            get { return rest; }
            set
            {
                rest = value;
                CurrentActionTime = 0;
            }
        }

        public Pod(Vector2 pos, float width, float height, float health, float damage = 1f, bool mobile = true) : base(pos, width, height, health, damage, mobile)
        {
            //Creates a weapon for the Pod
            MyWeapon = new Weapon(.2f, 1, 1000, 700);
            MyWeapon.BulletSize = 5;
            MyWeapon.Source = this;
            Gravity = false;
        }


        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //Check if awake
            if (!Resting)
            {
                Shoot(new Vector(0, -1));
                if (CurrentActionTime > MyWeapon.ShotDelay * 3)
                    Resting = true;
            }
            else
            {
                if(CurrentActionTime > 1f)
                {
                    Resting = false; 
                }
            }
        }

    }
}