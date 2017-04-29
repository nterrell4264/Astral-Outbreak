using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    //An enemy that swarms the player as a swarm boss.
    public class SwarmMob: Enemy
    {
        //Is the mob active. Static because swarms activate as one.
        public static bool Awake { get; set; }

        public static Vector Target { get; set; }
        public override float Health
        {
            get
            {
                return base.Health;
            }

            set
            {
                if (Health > value && !Awake)
                    Awaken();
                base.Health = value;
                if (IsDead)
                {
                    MySwarm.Kill(this);
                }
            }
        }

        //Bosses don't unload
        public override bool Unload
        {
            get
            {
                return false;
            }

            set
            {
                //base.Unload = value;
                //if (Unload)
                //    MySwarm.Kill(this);
            }
        }

        //Swarm
        private static SwarmAI mySwarm;
        public static SwarmAI MySwarm
        {
            get
            {
                if (mySwarm == null)
                    mySwarm = new SwarmAI();
                return mySwarm;
            }
        }
        static SwarmMob()
        {
            SwarmMob.Target = new Vector(0, 0);
        }

        public SwarmMob(Vector2 pos, float width, float height, float health, float damage, bool mobile = true) : base(pos, width, height, health, 1, mobile)
        {
            Gravity = false;
            MaxVelocity = new Vector(400, 400);
            MySwarm.Mobs.Add(this);
        }

        public SwarmMob(SwarmMob mob) : base(new Vector(mob.Position), mob.Width, mob.Height, mob.Health, mob.Damage, mob.Mobile)
        {
            CurrentActionTime += (float) Game1.Rand.NextDouble();
            Gravity = false;
            MaxVelocity = new Vector(350 + Game1.Rand.Next(200), 350 + Game1.Rand.Next(200));
            MySwarm.Mobs.Add(this);
        }

        public override void Step(float deltaTime)
        {
            base.Step(deltaTime);
            //If it is awake
            if (Awake)
            {
                MySwarm.Step(deltaTime, this);
                if(Math.Abs(Position.X - Target.X) > Math.Abs(Position.Y - Target.Y))
                {
                    Acceleration.X = Target.X - Center.X;
                }
                else
                {
                    Acceleration.Y = Target.Y - Center.Y;
                }
            }
            else
            {
                if (CheckLineOfSight(RoomManager.Active.MapData) && RoomManager.Active.AllowBossActivation())
                    Awaken();
            }

        }
        public void Awaken()
        {
            Awake = true;
            MySwarm.Activate();
            Target = (MySwarm.GetCenter() + RoomManager.Active.PlayerOne.Center) / 2;
            List<PhysicsObject> swarmMobs = new List<PhysicsObject>();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    swarmMobs.Add(new SwarmMob(this));
                    swarmMobs[5 * i + j].Position += new Vector(0, 0);
                }
            }
            RoomManager.Active.AddEntities(swarmMobs);
        }
    }

}
