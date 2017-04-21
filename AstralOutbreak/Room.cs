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
        private const float BUFFER = 1400;

        //Keep track of the player
        public Player PlayerOne { get; set; }

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
            PlayerOne = null;
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
                        lock (listLock)
                        {
                            PhysicsObjects.RemoveAt(i);
                            i--;
                        }
                    }
                }
                lock (listLock)
                {
                    if(i >= 0 && PhysicsObjects[i] is GameObject && (PhysicsObjects[i] as GameObject).Unload)
                    {
                        PhysicsObjects.RemoveAt(i);
                        i--;
                    }
                }
                
            }
            //Update Physics
            Update(deltaTime);
            CameraTrack(PlayerOne);
            CheckUnload();
            LoadFromMap();
        }

        /// <summary>
        /// Tracks an object with the camera and loads new stuffs
        /// </summary>
        /// <param name="target"></param>
        public void CameraTrack(GameObject target)
        {
            if (target == null)
                return;
            float newX = target.Position.X - (Width / 2);
            if (newX < 0)
                newX = 0;
            float newY = target.Position.Y - (Height / 2);
            if (newY < 0)
                newY = 0;
            if(MapData != null)
            {
                if (newX + Width > MapData.Width * MapData.Scale)
                    newX = MapData.Width * MapData.Scale - Width;
                if (newY + Height > MapData.Height * MapData.Scale)
                    newY = MapData.Height * MapData.Scale - Height;
            }
            
            CameraX = newX;
            CameraY = newY;
            
        }

        /// <summary>
        /// Loads data from the map.
        /// </summary>
        public void LoadFromMap()
        {
            if (MapData != null)
            {
                List<GameObject> newData = MapData.Load(CameraX, CameraY, Width, Height, BUFFER / 2);
                lock (listLock)
                {
                    for (int i = 0; i < newData.Count; i++)
                    {
                        PhysicsObjects.Add(newData[i]);
                    }
                }
            }
        }
        
        //Sets up a room from a map
        public void LoadRoom(Map mapdata)
        {
            MapData = mapdata;
            float scale = mapdata.Scale;
            PlayerOne = new Player(new Vector2(mapdata.PlayerStartX * scale, mapdata.PlayerStartY * scale), 32, 55, 10);
            PhysicsObjects.Add(PlayerOne);
            List<GameObject> newData = MapData.LoadHard(CameraX, CameraY, Width, Height, BUFFER);
            lock (listLock)
            {
                for (int i = 0; i < newData.Count; i++)
                {
                    PhysicsObjects.Add(newData[i]);
                }
            }
            CameraTrack(PlayerOne);
        }

        
        public void CheckUnload()
        {
            int i = 0;
            while (true)
            {
                GameObject obj = null;
                lock (listLock)
                {
                    if (i >= PhysicsObjects.Count || !(PhysicsObjects[i] is GameObject))
                        return;

                    obj = PhysicsObjects[i] as GameObject;
                }
                if (obj.Position.X < CameraX - BUFFER || obj.Position.X > CameraX + BUFFER + Width 
                    || obj.Position.Y < CameraY - BUFFER || obj.Position.Y > CameraY + BUFFER + Height)
                    obj.Unload = true;
                i++;
            }
        }





        //Returns true if we want both objects to collide with each other in a physical sense
        public bool DetermineCollision(PhysicsObject obj1, PhysicsObject obj2)
        {
            if (obj1 is Player && obj2 is Enemy)
                return false;
            else if (obj1 is Enemy && obj2 is Player)
                return false;
            else
                return !(obj2 is Projectile) && !(obj1 is Projectile);
        }

        //Handles nonphysical results of physical collisions
        public void HandleCollision(PhysicsObject obj1, PhysicsObject obj2)
        {
            //Bullets damage entities
            if(obj1 is Projectile && obj2 is GameObject)
            {
                if (obj2 is Player && (obj2 as Player).InvulnTime > 0) { }
                else
                    (obj1 as Projectile).Strike(obj2 as GameObject);
            }
            if (obj2 is Projectile && obj1 is GameObject)
            {
                if (obj1 is Player && (obj1 as Player).InvulnTime > 0) { }
                else
                    (obj2 as Projectile).Strike(obj1 as GameObject);
            }

            if(obj1 is Enemy && obj2 is Player)
            {
                if((obj2 as Player).InvulnTime == 0)
                    (obj1 as Enemy).Strike(obj2 as GameObject);
            }
            if (obj2 is Enemy && obj1 is Player)
            {
                if ((obj1 as Player).InvulnTime == 0)
                    (obj2 as Enemy).Strike(obj1 as GameObject);
            }
        }



        //Adds a bullet
        public void AddBullet(GameObject obj, Vector vel)
        {
            lock (listLock)
            {
                PhysicsObjects.Add(obj);
            }
            obj.Velocity = vel;
        }



    }
}
