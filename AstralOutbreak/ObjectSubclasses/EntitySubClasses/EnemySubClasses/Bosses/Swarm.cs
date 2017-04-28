using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    //Handler class for the swarm boss
    public class Swarm
    {
        //List of all swarm mobs
        public List<SwarmMob> Mobs{ get; set; }

        //Last Call
        private float lastCall;

        //Static constructor
        public Swarm()
        {
            Mobs = new List<SwarmMob>();
            lastCall = -1;
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
        public void Step(float deltaTime, float timeStamp)
        {
            if (timeStamp > lastCall)
            {
                lastCall = timeStamp;
                //Velocity of the focal point
                Vector v = new Vector(1, 0);










                SwarmMob.Target = new Vector(Game1.Inputs.MouseX + RoomManager.Active.CameraX, Game1.Inputs.MouseY + RoomManager.Active.CameraY);
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
