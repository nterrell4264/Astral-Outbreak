using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{

    //Represents an object on the map
    public enum MapItem { None, Wall, Slug, Demon, Item, Boss}

    /// <summary>
    /// A grid of objects that are in the game.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// All of the data relevant to the map.
        /// </summary>
        public MapItem[,] MapData { get; set; }
        public int[,] TileValue { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        //Starting position of the player
        public int PlayerStartX { get; set; }
        public int PlayerStartY { get; set; }

        //Player Starting gear
        public Upgrades PlayerUpgrades { get; set; }
        public float MaxHealth { get; set; }
        public float Health { get; set; }
        public float TotalPlayTime { get; set; }


        /// <summary>
        /// This array lets the map know what assets are currently loaded so that it doesn't give the same entity twice
        /// </summary>
        public bool[,] Loaded { get; set; }

        /// <summary>
        /// Denotes the width/height of each gridspace
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// A boolean that denotes if the resize can occur.
        /// </summary>
        public bool Resizable { get; set; }

        /// <summary>
        /// Gets the mapitem at a given coordinate
        /// </summary>
        /// <param name="index1">x coordinate</param>
        /// <param name="index2">y coordinate</param>
        /// <returns></returns>
        public MapItem this[int index1, int index2]
        {
            get
            {
                if (index1 < Width && index2 < Height)
                    return MapData[index1, index2];
                return MapItem.None;

            }
            set
            {
                if (index1 < Width && index2 < Height)
                    MapData[index1, index2] = value;
                else if (Resizable)
                {
                    Resize(index1 + 1, index2 + 1);
                    MapData[index1, index2] = value;
                }
            }
        }

        /// <summary>
        /// Makes an empty map without any space
        /// </summary>
        public Map()
        {
            PlayerStartX = 0;
            PlayerStartY = 0;
            MapData = new MapItem[0,0];
            TileValue = new int[0, 0];
            Loaded = new bool[0, 0];
            Width = 0;
            Height = 0;
            Resizable = false;
            TotalPlayTime = 0;
        }

        /// <summary>
        /// Makes an empty world with given dimensions
        /// </summary>
        /// <param name="width">Width of map</param>
        /// <param name="height">Height of map</param>
        public Map(int width, int height)
        {
            MapData = new MapItem[width, height];
            Loaded = new bool[width, height];
            TileValue = new int[width, height];
            Width = width;
            Height = height;
            Resizable = false;
            PlayerStartX = 0;
            PlayerStartY = 0;

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    MapData[i, j] = MapItem.None;
                    Loaded[i, j] = false;
                    TileValue[i, j] = 0;
                }
            TotalPlayTime = 0;

        }

        /// <summary>
        /// Makes an empty world with given dimensions
        /// </summary>
        /// <param name="width">Width of map</param>
        /// <param name="height">Height of map</param>
        public Map(Map map)
        {
            
            Width = map.Width;
            Height = map.Height;
            Resizable = false;
            Scale = map.Scale;
            MapData = new MapItem[Width, Height];
            TileValue = new int[Width, Height];
            Loaded = new bool[Width, Height];
            PlayerUpgrades = RoomManager.Active.PlayerOne.MyUpgrades;
            PlayerStartX = (int)(RoomManager.Active.PlayerOne.Position.X / Scale);
            PlayerStartY = (int)(RoomManager.Active.PlayerOne.Position.Y / Scale);
            List<PhysicsObject> lis = RoomManager.Active.PhysicsObjects;
            for (int i = 0; i < lis.Count; i++)
            {
                if (lis[i] is GameObject)
                {
                    GameObject obj = lis[i] as GameObject;
                    if ((obj.OriginX >= 0 && obj.OriginY >= 0))
                        Loaded[obj.OriginX, obj.OriginY] = true;
                }
            }
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    if (Loaded[i, j] || !map.Loaded[i,j])
                    {
                        MapData[i, j] = map.MapData[i, j];
                        TileValue[i, j] = map.TileValue[i, j];
                        Loaded[i, j] = false;
                    }
                }
            MaxHealth = RoomManager.Active.PlayerOne.MaxHealth;
            Health = RoomManager.Active.PlayerOne.Health;
            TotalPlayTime = RoomManager.Active.TotalPlayTime;

        }

        /// <summary>
        /// Resizes the room. Can lose data. Adds empty space.
        /// Don't call this outside of the external editor.
        /// </summary>
        public void Resize(int width, int height)
        {
            //Make a variable to store the data
            MapItem[,] temp = new MapItem[width, height];
            //For columbs that are on both the new and the old
            for (int i = 0; i < width && i < Width; i++)
            {
                //for rows that are on both the new and the old
                for(int j = 0; j < height && j < Height; j++)
                {
                    //Copy over the data
                    temp[i, j] = MapData[i, j];
                }
                //for rows that are new
                for(int j = Height; j < height; j++)
                {
                    //Fill them with nothing
                    temp[i, j] = MapItem.None;
                }
            }
            //For new columbs
            for (int i = Width; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //Fill with nothing
                    temp[i, j] = MapItem.None;
                }
            }

            //Assign the new value over the old
            MapData = temp;
            Width = width;
            Height = height;

        }


        public GameObject Get(int x, int y)
        {
            if (Loaded[x, y])
                return null;
            Loaded[x, y] = true;
            switch (MapData[x,y])
            {
                case MapItem.None:
                    return null;
                    break;
                case MapItem.Wall:
                    Wall v = null;
                    switch ((WallType)TileValue[x,y])
                    {
                        default:
                        case WallType.Regular:
                        case WallType.BossDoor:
                            v = new Wall(new Vector(x * Scale, y * Scale), Scale, Scale);
                            break;
                        case WallType.Platform:
                            v = new Wall(new Vector(x * Scale, y * Scale), Scale, 2);
                            break;
                    }
                    if ( x < 1 || MapData[x - 1, y] == MapItem.Wall)
                        v.Adj = v.Adj | WallAdj.Left;
                    if (x >= Width - 1 || MapData[x + 1, y] == MapItem.Wall)
                        v.Adj = v.Adj | WallAdj.Right;
                    if (y < 1 || MapData[x, y - 1] == MapItem.Wall)
                        v.Adj = v.Adj | WallAdj.Top;
                    if (y >= Height - 1 || MapData[x, y + 1] == MapItem.Wall)
                        v.Adj = v.Adj | WallAdj.Bottom;
                    v.MyType = (WallType) TileValue[x,y];
                    return v;
                    break;
                case MapItem.Slug:
                    if (TileValue[x, y] == 0)
                    {
                        Pod p = new Pod(new Vector(x * Scale, y * Scale), 28, 28, 20);
                        if (y >= Height - 1 || MapData[x, y + 1] == MapItem.Wall)
                            p.Direction = Facing.Up;
                        else if (y < 1 || MapData[x, y - 1] == MapItem.Wall)
                            p.Direction = Facing.Down;
                        else if (x < 1 || MapData[x - 1, y] == MapItem.Wall)
                            p.Direction = Facing.Right;
                        else if (x >= Width - 1 || MapData[x + 1, y] == MapItem.Wall)
                            p.Direction = Facing.Left;
                        return p;
                    }
                    return new Slug(new Vector(x * Scale, y * Scale), 56, 28, 20);
                    break;
                case MapItem.Demon:
                    if(TileValue[x,y] == 0)
                        return new Bat(new Vector(x * Scale, y * Scale), 12, 12, 1, 1);
                    return new JackRabbit(new Vector(x * Scale, y * Scale), 28, 56, 10);
                    break;
                case MapItem.Item:
                    return new Item(new Vector(x * Scale + 7, y * Scale + 7), 14, 14, ItemType.AbilityUnlock, TileValue[x,y]);
                    break;
                case MapItem.Boss:
                    switch (TileValue[x,y])
                    {
                        default:
                        case 0:
                            return new Turret(new Vector(x * Scale, y * Scale), 28, 28, 10);
                        case 1:
                            return new DashRabbit(new Vector(x * Scale, y * Scale), 84, 42, 250, damage: 1);
                        case 2:
                            return new SwarmMob(new Vector(x * Scale, y * Scale), 12, 12, 1, 1);
                        case 3:
                            return new MultiRabbit(new Vector(x * Scale, y * Scale), 28, 56, 250, damage: 1);
                        case 4:
                            return new CoreBoss(new Vector(x * Scale - 59 + 28, y * Scale + 56 - 121), 118, 121, 2000);
                        case 6:
                            return new EscapePod(new Vector(x * Scale - 14, y * Scale - 14), 56, 56);

                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        /// <summary>
        /// Returns all of the items that need to be loaded in the buffer area the screen. Will only load enemies in the buffer area.
        /// </summary>
        /// <param name="newX">New x for the screen</param>
        /// <param name="newY">New y for the screen</param>
        /// <param name="width">Width of the screen</param>
        /// <param name="height">Height of the screen</param>
        /// <param name="buffer">Space around the edge of the screen that is to be loaded</param>
        /// <returns>A list that contains some null values and some gameobjects</returns>
        public List<GameObject> Load(float newX, float newY, float width, float height, float buffer)
        {
            //Convert to ints in the right scale.
            int nX = (int) ((newX - buffer) / Scale);
            if (nX < 0)
                nX = 0;
            int nY = (int) ((newY - buffer) / Scale);
            if (nY < 0)
                nY = 0;
            int w  = (int) ((width + 2 * buffer) / Scale);
            int h  = (int) ((height + 2 * buffer) / Scale);

            //Create a list
            List<GameObject> list = new List<GameObject>(w + h);
            //Fill the list
            for (int i = nX; i < w + nX && i < Width; i++)
            {
                for(int j = nY; j < h + nY && j < Height; j++)
                {
                    //Get the object
                    var obj = Get(i, j);
                    if (obj != null)
                    {
                        //for each bit check if it should be loaded (This has to do with not loading enemies on the screen, only in the buffer area)
                        if (this[i,j] == MapItem.Wall || (((obj.Position.X + obj.Width < newX) 
                            || (obj.Position.X > newX + width) || (obj.Position.Y + obj.Height < newY) || (obj.Position.Y > newY + height)) && j < nY + h - 10))
                        {
                            //Add it to the list
                            obj.OriginX = i;
                            obj.OriginY = j;
                            list.Add(obj);
                        }
                        else
                        {
                            Loaded[i, j] = false;
                        }
                    }
                }
            }
            
            return list;
        }

        /// <summary>
        /// Returns all of the items that need to be loaded in the buffer area the screen. Enemies can load anywhere on screen.
        /// </summary>
        /// <param name="newX">New x for the screen</param>
        /// <param name="newY">New y for the screen</param>
        /// <param name="width">Width of the screen</param>
        /// <param name="height">Height of the screen</param>
        /// <param name="buffer">Space around the edge of the screen that is to be loaded</param>
        /// <returns>A list that contains some null values and some gameobjects</returns>
        public List<GameObject> LoadHard(float newX, float newY, float width, float height, float buffer)
        {
            //Convert to ints in the right scale.
            int nX = (int)((newX - buffer) / Scale);
            if (nX < 0)
                nX = 0;
            int nY = (int)((newY - buffer) / Scale);
            if (nY < 0)
                nY = 0;
            int w = (int)((width + 2 * buffer) / Scale);
            int h = (int)((height + 2 * buffer) / Scale);

            //Create a list
            List<GameObject> list = new List<GameObject>(w + h);
            //Fill the list
            for (int i = nX; i < w + nX && i < Width; i++)
            {
                for (int j = nY; j < height + nY && j < Height; j++)
                {
                    //Get the object
                    var obj = Get(i, j);
                    if (obj != null)
                    {
                        //Add it to the list
                        obj.OriginX = i;
                        obj.OriginY = j;
                        list.Add(obj);
                    }
                }
            }

            return list;
        }

        public bool CheckLineOfSight(float x1, float y1, float x2, float y2)
        {
            x1 = (int)((x1) / Scale);
            y1 = (int)((y1) / Scale);
            x2 = (int)((x2) / Scale);
            y2 = (int)((y2) / Scale);
            if (x1 < 0 || y1 < 0 || x2 < 0 || y2 < 0 || x1 >= MapData.GetLength(0) || y1 >= MapData.GetLength(1) || x2 >= MapData.GetLength(0) || y2 >= MapData.GetLength(1))
                return false;

            if(x1 == x2)
            {
                if (y1 > y2)
                    for (int yActive = (int)y2; yActive < y1; yActive++)
                    {
                        if (MapData[(int)x1, yActive] == MapItem.Wall && TileValue[(int)x1, yActive] != 2)
                            return false;
                    }
                else
                    for (int yActive = (int)y1; yActive < y2; yActive++)
                    {
                        if (MapData[(int)x1, yActive] == MapItem.Wall && TileValue[(int)x1, yActive] != 2)
                            return false;
                    }
                return true;
            }
            float slope = (y2 - y1) / (x2 - x1);
            float yIntercept = y1 - (x1 * slope);
            if (slope != 0)
            {
                if (x1 < x2)
                    for (int xActive = (int)x1; xActive < x2; xActive++)
                    {
                        if (slope > 0)
                        {
                            for (int i = 0; i < slope; i++)
                                if (MapData[xActive, (int)(yIntercept + (xActive * slope) + i)] == MapItem.Wall && TileValue[xActive, (int)(yIntercept + (xActive * slope) + i)] != 2)
                                    return false;
                        }
                        else
                        {
                            for (int i = 0; i > slope; i--)
                                if (MapData[xActive, (int)(yIntercept + (xActive * slope) + i)] == MapItem.Wall && TileValue[xActive, (int)(yIntercept + (xActive * slope) + i)] != 2)
                                    return false;
                        }
                    }
                else if (x2 < x1)
                    for (int xActive = (int)x2; xActive < x1; xActive++)
                    {
                        if (slope > 0)
                        {
                            for (int i = 0; i < slope; i++)
                                if (MapData[xActive, (int)(yIntercept + (xActive * slope) + i)] == MapItem.Wall && TileValue[xActive, (int)(yIntercept + (xActive * slope) + i)] != 2)
                                    return false;
                        }
                        else
                        {
                            for (int i = 0; i > slope; i--)
                                if (MapData[xActive, (int)(yIntercept + (xActive * slope) + i)] == MapItem.Wall && TileValue[xActive, (int)(yIntercept + (xActive * slope) + i)] != 2)
                                    return false;
                        }
                    }
            }
            else
            {
                if (x1 < x2)
                    for (int xActive = (int)x1; xActive < x2; xActive++)
                    {
                        if (MapData[xActive, (int)y1] == MapItem.Wall && TileValue[xActive, (int)y1] != 2)
                            return false;
                    }
                else
                    for (int xActive = (int)x2; xActive < x1; xActive++)
                    {
                        if (MapData[xActive, (int)y1] == MapItem.Wall && TileValue[xActive, (int)y1] != 2)
                            return false;
                    }
            }
            return true;
        }

        public Map Save()
        {
            Map sav = new Map(this);
            
            return sav;
        }

        public void Reload()
        {
            for (int i = 0; i < Loaded.GetLength(0); i++)
                for (int j = 0; j < Loaded.GetLength(1); j++)
                    Loaded[i, j] = false;
        }

        public int CountEnemies()
        {
            int count = 0;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    if (!Loaded[i, j] && (this[i,j] == MapItem.Demon || this[i,j] == MapItem.Slug))
                    {
                        count++;
                    }
                }
            return count;
        }

        public int CountItems()
        {
            int count = 0;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    if (!Loaded[i, j] && (this[i, j] == MapItem.Item))
                    {
                        count++;
                    }
                }
            return count;
        }
    }
}
