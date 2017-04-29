using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public enum SwarmState { ChargingLeft, ChargingRight, Aligning }

    //Handler class for the swarm boss
    public class SwarmAI
    {
        private const float VELOCITY = 300;

        //List of all swarm mobs
        public List<SwarmMob> Mobs{ get; set; }

        //Collider (INTERNAL USE ONLY)
        private PhysicsObject collider;

        private SwarmState myState;

        public SwarmState CurrentState
        {
            get { return myState; }
            set
            {
                myState = value;
                switch (CurrentState)
                {
                    case SwarmState.ChargingLeft:
                        ChargeDist = 1.5f * (SwarmMob.Target.X - RoomManager.Active.PlayerOne.Position.X);
                        break;
                    case SwarmState.ChargingRight:
                        ChargeDist = 1.5f * ( RoomManager.Active.PlayerOne.Position.X - SwarmMob.Target.X);
                        break;
                    case SwarmState.Aligning:
                        SwarmMob.Target = new Vector(SwarmMob.Target.X, RoomManager.Active.PlayerOne.Position.Y);
                        break;
                    default:
                        break;
                }
            }
        }

        public float ChargeDist;

        //Last Call
        private float lastCall;

        //Static constructor
        public SwarmAI()
        {
            Mobs = new List<SwarmMob>();
            lastCall = -1;
            collider = new PhysicsObject(SwarmMob.Target, 1, 1);
        }


        //Activates the Swarm
        public void Activate()
        {
            RoomManager.Active.BossActive = true;
            SwarmMob.Target = GetCenter();
        }

        //Deactivate the Swarm
        public void Die()
        {
            RoomManager.Active.BossActive = false;
        }

        //Removes a mob from the swarm (presumably due to death)
        public void Kill(SwarmMob mob)
        {
            Mobs.Remove(mob);
            if (Mobs.Count == 0)
                Die();
        }

        //Steps if and only if the timestamp is newer than the last call
        public void Step(float deltaTime, SwarmMob mob)
        {
            if (Mobs.Count != 0 && mob.Equals(Mobs[0]))
            {
                //Velocity of the focal point
                switch (CurrentState)
                {
                    case SwarmState.ChargingLeft:
                        if (ChargeDist <= 0 || RoomManager.Active.CheckCollision(collider, VELOCITY * deltaTime * new Vector(-1, 0)))
                        {
                            CurrentState = SwarmState.Aligning;
                            break;
                        }
                        SwarmMob.Target.X -= VELOCITY * deltaTime;
                        ChargeDist -= VELOCITY * deltaTime;
                        
                        break;
                    case SwarmState.ChargingRight:
                        if (ChargeDist <= 0 || RoomManager.Active.CheckCollision(collider, VELOCITY * deltaTime * new Vector(-1, 0)))
                        {
                            CurrentState = SwarmState.Aligning;
                            break;
                        }
                        SwarmMob.Target.X += VELOCITY * deltaTime;
                        ChargeDist -= VELOCITY * deltaTime;
                        
                        break;
                    case SwarmState.Aligning:
                        if (RoomManager.Active.PlayerOne.Position.X > SwarmMob.Target.X)
                            CurrentState = SwarmState.ChargingRight;
                        else
                            CurrentState = SwarmState.ChargingLeft;
                        break;
                    default:
                        break;
                }





                //SwarmMob.Target = new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX, Game1.Inputs.MouseY + RoomManager.Active.CameraY);
            }
        }

        //Get Center
        public Vector GetCenter()
        {
            Vector v = new Vector(0, 0);
            for(int i = 0; i < Mobs.Count; i++)
            {
                v += Mobs[i].Center;
            }
            if(Mobs.Count != 0)
                v = v / Mobs.Count;
            return v;
        }

        public void Clear()
        {
            Mobs = new List<SwarmMob>();
            lastCall = -1;
        }


    }
}
