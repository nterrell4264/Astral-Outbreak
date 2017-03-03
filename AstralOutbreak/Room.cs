using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    /// <summary>
    /// Handles the nonphysics elements of a room.
    /// </summary>
    public class Room: World
    {
        //Id for this room
        public int RoomNumber { get; set; }

        //Rooms IDs that this room links to
        public List<int> RoomLinks { get; set; }

        //Bounds of the room
        public float Width { get; set; }
        public float Height { get; set; }
        
        /// <summary>
        /// A simple room with gravity
        /// </summary>
        /// <param name="gravity">Force of gravity</param>
        public Room(float width, float height, Vector2 gravity = default(Vector2)): base(gravity, new Vector2(100, 100))
        {
            Width = width;
            Height = height;
            PhysicalLogic = DetermineCollision;
            Collide += HandleCollision;
        }

        /// <summary>
        /// Updates the room and the physics.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Step(float deltaTime)
        {
            for(int i = 0; i < PhysicsObjects.Count; i++)
            {
                //For each entity
                if(PhysicsObjects[i] is Entity)
                {
                    var obj = (PhysicsObjects[i] as Entity);
                    //Call its step
                    obj.Step(deltaTime);
                    //And make sure it isn't dead
                    if (obj.IsDead)
                    {
                        PhysicsObjects.RemoveAt(i);
                        i--;
                    }
                }
            }
            //Update Physics
            Update(deltaTime);
        }

        //Returns true if we want both objects to collide with each other in a physical sense
        public bool DetermineCollision(PhysicsObject obj1, PhysicsObject obj2)
        {
            return true;
        }

        //Handles nonphysical results of physical collisions
        public void HandleCollision(PhysicsObject obj1, PhysicsObject obj2)
        {
            //Bullets damage entities
            if(obj1 is Projectile && obj2 is GameObject)
            {
                (obj1 as Projectile).Strike(obj2 as GameObject);
            }
            if (obj2 is Projectile && obj1 is GameObject)
            {
                (obj2 as Projectile).Strike(obj1 as GameObject);
            }

        }





    }
}
