using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{

    //Represents an object on the map
    public enum MapItem { None, Wall, Slug, Demon}

    /// <summary>
    /// A grid of objects that are in the game.
    /// </summary>
    public class Map
    {
        /// <summary>
        /// All of the data relevant to the map.
        /// </summary>
        public MapItem[,] MapData { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        //Starting position of the player
        public int PlayerStartX { get; set; }
        public int PlayerStartY { get; set; }


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
                if (Resizable)
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
            Loaded = new bool[0, 0];
            Width = 0;
            Height = 0;
            Resizable = false;
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
                }
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
                    return new Wall(new Vector(x*Scale, y*Scale), Scale, Scale);
                    break;
                case MapItem.Slug:
                    break;
                case MapItem.Demon:
                    break;
                default:
                    break;
            }
            return null;
        }

        /// <summary>
        /// Returns all of the items that need to be loaded in the buffer area the screen
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
                for(int j = nY; j < height + nY && j < Height; j++)
                {
                    //for each bit check if it should be loaded (This has to do with not loading enemies on the screen, only in the buffer area)
                    if ((this[i,j] == MapItem.Wall || this[i,j] == MapItem.None) || ((i*Scale < newX) || (i*Scale > newX + width) || (j * Scale < newY) || (j * Scale > newY + height)))
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
            }
            
            return list;
        }
       

    }
}
