using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public enum Facing { Up, Left, Right, Down}

    public class Pod : Enemy
    {
        private bool rest;
        public Facing Direction { get; set; }
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
                switch (Direction)
                {
                    case Facing.Up:
                        Shoot(new Vector(0, -1));
                        break;
                    case Facing.Left:
                        Shoot(new Vector(-1, 0));
                        break;
                    case Facing.Right:
                        Shoot(new Vector(1, 0));
                        break;
                    case Facing.Down:
                        Shoot(new Vector(0, 1));
                        break;
                    default:
                        break;
                }
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