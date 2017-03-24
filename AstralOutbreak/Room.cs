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

        //Map information
        public Map MapData { get; set; }

        //Changing the list lock
        private Object listLock = new Object();

        //Bounds of the camera
        public float CameraX { get; set; }
        public float CameraY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        //Buffer width around the screen
        private const float BUFFER = 32;

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
                        obj.Unload = true;
                    }
                }
                lock (listLock)
                {
                    if(PhysicsObjects[i] is GameObject && (PhysicsObjects[i] as GameObject).Unload)
                    {
                        PhysicsObjects.RemoveAt(i);
                        i--;
                    }
                }
                
            }
            //Update Physics
            Update(deltaTime);
        }

        /// <summary>
        /// Tracks an object with the camera and loads new stuffs
        /// </summary>
        /// <param name="target"></param>
        public void CameraTrack(GameObject target)
        {
            float newX = target.Position.X - Width / 2;
            float newY = target.Position.Y - Height / 2;
            if (newX < 0)
                newX = 0;
            if (newX + Width > MapData.Width * MapData.Scale)
                newX = MapData.Width * MapData.Scale - Width;
            if (newY < 0)
                newY = 0;
            if (newY + Height > MapData.Height * MapData.Scale)
                newY = MapData.Height * MapData.Scale - Height;

            List<GameObject> newData = MapData.Load(newX, newY, Width, Height, BUFFER);
            lock (listLock)
            {
                for(int i = 0; i < newData.Count; i++)
                {
                    PhysicsObjects.Add(newData[i]);
                }
            }
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
