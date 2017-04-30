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

        //Last deltatime
        private float delta;


        //Bounds of the camera
        public float CameraX { get; set; }
        public float CameraY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        //Buffer width around the screen
        private const float BUFFER = 700;
        private const float CORRECTIVE = 300;


        //Keep track of the player
        public Player PlayerOne { get; set; }

        //Is a boss active?
        public bool BossActive { get; set; }

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
            List<PhysicsObject> loaded = new List<PhysicsObject>(PhysicsObjects.Count);
            this.delta = deltaTime;
            for(int i = 0; i < PhysicsObjects.Count; i++)
            {
                //For each entity
                if(PhysicsObjects[i] is Entity)
                {
                    var obj = (PhysicsObjects[i] as Entity);
                    //Call its step
                    obj.Step(deltaTime);
                    //And make sure it isn't dead
                }
                if(i < PhysicsObjects.Count)
                    if(!(i >= 0 && PhysicsObjects[i] is GameObject && (PhysicsObjects[i] as GameObject).Unload) && !(PhysicsObjects[i] is Entity && (PhysicsObjects[i] as Entity).IsDead))
                    {
                        loaded.Add(PhysicsObjects[i]);
                    }
            }
            lock (listLock)
            {
                PhysicsObjects = loaded;
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
            float newX = (Game1.Inputs.MouseX + RoomManager.Active.CameraX + target.Center.X * 4) / 5 - (Width / 2);
            if (newX < 0)
                newX = 0;
            if (newX > target.Position.X)
                newX = target.Position.X;
            if (newX + Width < target.Position.X + target.Width)
                newX = target.Position.X - Width + target.Width;
            float newY = ((Game1.Inputs.MouseY + RoomManager.Active.CameraY) * 3 + target.Center.Y * 7) / 10 - (Height / 2);
            if (target is Player && (target as Player).CurrentPlayerState == PlayerState.Rolling)
            {
                newY = ((Game1.Inputs.MouseY + RoomManager.Active.CameraY) * 3 + target.Position.Y * 7) / 10 - (Height / 2);
            }
            if (newY < 0)
                newY = 0;
            if (newY > target.Position.Y)
                newY = target.Position.Y;
            if (newY + Height < target.Position.Y + target.Height)
                newY = target.Position.Y - Height + target.Height;
            if (MapData != null)
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
            PlayerOne = new Player(new Vector2(mapdata.PlayerStartX * scale, mapdata.PlayerStartY * scale), 28, 55, 10);
            PhysicsObjects.Add(PlayerOne);
            CameraTrack(PlayerOne);
            List<GameObject> newData = MapData.LoadHard(CameraX, CameraY, Width, Height, BUFFER);
            lock (listLock)
            {
                for (int i = 0; i < newData.Count; i++)
                {
                    PhysicsObjects.Add(newData[i]);
                }
            }
            BossActive = false;
            SwarmMob.Awake = false;
            SwarmMob.MySwarm.Clear();
        }

        //Re-sets up the room from the current map
        public void ReloadRoom()
        {
            BossActive = false;
            SwarmMob.Awake = false;
            SwarmMob.MySwarm.Clear();
            MapData.Reload();
            PhysicsObjects = new List<PhysicsObject>();
            float scale = MapData.Scale;
            PlayerOne = new Player(new Vector2(MapData.PlayerStartX * scale, MapData.PlayerStartY * scale), 28, 55, 10);
            PhysicsObjects.Add(PlayerOne);
            CameraTrack(PlayerOne);
            List<GameObject> newData = MapData.LoadHard(CameraX, CameraY, Width, Height, BUFFER);
            lock (listLock)
            {
                for (int i = 0; i < newData.Count; i++)
                {
                    PhysicsObjects.Add(newData[i]);
                }
            }
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
                obj.Unload = obj.Position.X < CameraX - BUFFER || obj.Position.X > CameraX + BUFFER + Width || obj.Position.Y < CameraY - BUFFER || obj.Position.Y > CameraY + BUFFER + Height;
                i++;
            }
        }





        //Returns true if we want both objects to collide with each other in a physical sense
        public bool DetermineCollision(PhysicsObject obj1, PhysicsObject obj2)
        {
            
            //Swarms
            if (obj2 is SwarmMob || obj1 is SwarmMob)
                return false;
            if (obj2 is BatShield)
                return false;
            //Wall
            if (obj2 is Wall)
            {
                Wall w = obj2 as Wall;
                switch (w.MyType)
                {
                    case WallType.Fire:
                    case WallType.Regular:
                        break;
                    case WallType.Platform:
                        return !(obj1 is Projectile) && (obj1.Position.Y + obj1.Height <= obj2.Position.Y && (Game1.Inputs.DownButtonState == ButtonStatus.Unpressed || !(obj1 is Player)) 
                            && !(obj1 is SwarmMob));
                    case WallType.BossDoor:
                        return BossActive;
                    case WallType.SecretTunnel:
                        return false;
                    default:
                        break;
                }
            }
            if (obj2 is Item)
                return false;
            if (obj1 is Item && !(obj2 is Wall))
                return false;
            if (obj1 is Enemy && obj2 is Enemy)
                return false;
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
                if ((obj2 is Player && (obj2 as Player).InvulnTime > 0) || (obj2 is Wall && 
                    ((obj2 as Wall).MyType == WallType.BossDoor && !BossActive || (obj2 as Wall).MyType == WallType.Platform))){ }
                else
                    (obj1 as Projectile).Strike(obj2 as GameObject);
            }
            if (obj2 is Projectile && obj1 is GameObject)
            {
                if (obj1 is Player && (obj1 as Player).InvulnTime > 0) { }
                else
                    (obj2 as Projectile).Strike(obj1 as GameObject);
            }

            //Enemies hit player
            if(obj1 is Enemy && obj2 is Player)
            {
                if((obj2 as Player).InvulnTime <= 0)
                    (obj1 as Enemy).Strike(obj2 as GameObject);
            }
            if (obj2 is Enemy && obj1 is Player)
            {
                if ((obj1 as Player).InvulnTime <= 0)
                    (obj2 as Enemy).Strike(obj1 as GameObject);
            }
            //Enemies hit Shield
            if (obj1 is Enemy && obj2 is BatShield)
            {
                (obj1 as Enemy).Strike(obj2 as GameObject);
            }
            //Player burns from heat wall
            if (obj1 is Player && obj2 is Wall)
            {
                if ((obj1 as Player).InvulnTime <= 0 && (obj2 as Wall).MyType == WallType.Fire)
                    PlayerOne.Health--;
            }

            //Enemies Correct
            if (obj1 is Enemy && obj2 is Enemy)
            {
                if (!(obj1 as Enemy).Corrective)
                {
                    if (obj1.Position.X < obj2.Position.X)
                    {
                        if(CheckCollision(obj1, obj1.Velocity + new Vector(-CORRECTIVE * delta, 0)))
                            if(obj1.MaxVelocity.X == 0)
                                obj1.Velocity.X -= CORRECTIVE;
                            else
                                obj1.Velocity.X -= CORRECTIVE / 3;

                        (obj1 as Enemy).Corrective = true;
                    }
                    else if (obj1.Position.X > obj2.Position.X)
                    {
                        if (CheckCollision(obj1, obj1.Velocity + new Vector(CORRECTIVE * delta, 0)))

                            if (obj1.MaxVelocity.X == 0)
                                obj1.Velocity.X += CORRECTIVE;
                            else
                                obj1.Velocity.X += CORRECTIVE / 3;
                        (obj1 as Enemy).Corrective = true;

                    }
                    else if ((obj2 as Enemy).Corrective)
                    {
                        if (CheckCollision(obj1, obj1.Velocity + new Vector(CORRECTIVE * delta, 0)))
                            if (obj1.MaxVelocity.X == 0)
                                obj1.Velocity.X += CORRECTIVE;
                            else
                                obj1.Velocity.X += CORRECTIVE / 3;
                        (obj1 as Enemy).Corrective = true;
                    }
                    else
                    {
                        if (CheckCollision(obj1, obj1.Velocity + new Vector(-CORRECTIVE * delta, 0)))
                            if (obj1.MaxVelocity.X == 0)
                                obj1.Velocity.X -= CORRECTIVE;
                            else
                                obj1.Velocity.X -= CORRECTIVE / 3;
                        (obj1 as Enemy).Corrective = true;
                    }
                }
            }

            //Items
            if (obj1 is Player && obj2 is Item)
            {
                (obj1 as Player).Consume(obj2 as Item);
            }
            else if (obj2 is Player && obj1 is Item)
            {
                (obj2 as Player).Consume(obj1 as Item);
            }
        }

        public bool CheckCollisionEnemy(PhysicsObject obj, Vector displacement)
        {
            //This object is moving, which means we need to check for collisions!
            for (int i = 0; i < PhysicsObjects.Count; i++)
            {
                //For each other object check for collisions
                if (!obj.Equals(PhysicsObjects[i]) && obj.CheckCollision(PhysicsObjects[i], displacement))
                {
                    //Doing the transform triggers a collision!
                    if (PhysicsObjects[i] is Enemy)
                        return true;
                }
            }
            return false;
        }

        //Checks if the player is in a boss door
        public bool AllowBossActivation()
        {
            //Check if the player is in a door
            for (int i = 0; i < PhysicsObjects.Count; i++)
            {
                //For each other object check for collisions
                if (PhysicsObjects[i] is Wall && (PhysicsObjects[i] as Wall).MyType == WallType.BossDoor && PlayerOne.CheckCollision(PhysicsObjects[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsOnScreen(PhysicsObject obj)
        {
            if (obj.Position.X < CameraX - obj.Width)
                return false;
            if (obj.Position.X > CameraX + Width)
                return false;
            if (obj.Position.Y < CameraY - obj.Height)
                return false;
            if (obj.Position.Y > CameraY + Height)
                return false;

            return true;
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

        //Adds entities to the room
        public void AddEntities(List<PhysicsObject> objs)
        {
            lock (listLock)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    if(objs[i] != null)
                        PhysicsObjects.Add(objs[i]);
                }
            }
        }
        //Adds entities to the room
        public void AddEntity(PhysicsObject objs)
        {
            lock (listLock)
            {
                if (objs != null)
                    PhysicsObjects.Add(objs);
            }
        }

        //Yet to be implemented
        public Map SaveGame()
        {
            return MapData;
        }

    }
}
