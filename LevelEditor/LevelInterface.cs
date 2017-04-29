using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstralOutbreak;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Newtonsoft.Json;

namespace LevelEditor
{
    
    public enum CursorMode
    {
        Erase, Player, Wall, Slug, Demon, Item, Boss
    }

    public class LevelInterface
    {
        //Map to be editing
        public Map MapData { get; set; }

        //CursorMode
        public CursorMode CursorItem { get; set; }
        public int CursorValue { get; set; }

        //Map Position
        public int MapX { get; set; }
        public int MapY { get; set; }

        //Zoom
        public int Scale { get; set; }

        //Gridlines
        public bool Gridlines { get; set; }

        //Square width/height of placement
        public int CursorSize { get; set; }

        //Textures
        public static Texture2D PlayerStartTexture { get; set; }
        public static Texture2D SlugTexture { get; set; }
        public static Texture2D DemonTexture { get; set; }
        public static Texture2D WallTexture { get; set; }
        public static Texture2D RoundTexture { get; set; }
        public static Texture2D GridTexture { get; set; }
        public static SpriteFont Font { get; set; }


        /// <summary>
        /// Creates an interface from the map to edit the map.
        /// </summary>
        /// <param name="mapdata"></param>
        public LevelInterface(Map mapdata)
        {
            MapData = mapdata;
            CursorItem = CursorMode.Erase;
            MapX = 0;
            MapY = 0;
            Gridlines = true;
            Scale = 8;
            mapdata.Scale = 28;
            CursorValue = 1;
        }
        /// <summary>
        /// Constructor that reads in a map from file
        /// </summary>
        /// <param name="fileName"></param>
        public LevelInterface(String fileName)
        {
            if (File.Exists(fileName))
                try
                {
                    using (StreamReader input = new StreamReader(File.OpenRead(fileName)))
                    {
                        MapData = (JsonConvert.DeserializeObject<Map>(input.ReadToEnd()));
                    }
                }
                catch
                {
                    MapData = new Map(1024, 1024);
                }
            else
                MapData = new Map(1024, 1024);
            MapData.Scale = 28;
            CursorItem = CursorMode.Erase;
            CursorSize = 1;
            MapX = 0;
            MapY = 0;
            Gridlines = true;
            Scale = 8;
            MapData.Scale = 28;
            CursorValue = 1;
            if (MapData.TileValue.GetLength(0) != MapData.Width || MapData.TileValue.GetLength(1) != MapData.Height)
                MapData.TileValue = new int[MapData.Width, MapData.Height];
        }

        public void Save(String fileName)
        {
            try
            {
                using (StreamWriter output = new StreamWriter(File.OpenWrite(fileName)))
                {
                    output.WriteLine(JsonConvert.SerializeObject(MapData));
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        /// <summary>
        /// Handles click events
        /// </summary>
        /// <param name="x">Mouse pos</param>
        /// <param name="y">Mouse pos</param>
        /// <param name="width">Width of viewport</param>
        /// <param name="height">Height of viewport</param>
        public void Click(int x, int y, int width, int height)
        {
            //Scale x to map coord
            x /= Scale;
            //Account for the border
            x--;
            //Scale y to map coord
            y /= Scale;
            //Account for the border
            y--;
            //MapData.Resizable = true;
            //Ensure that the mouse is on the screen
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                if(CursorItem == CursorMode.Player)
                {
                    MapData.PlayerStartX = MapX + x;
                    MapData.PlayerStartY = MapY + y;
                    return;
                }
                for (int i = 0; i < CursorSize; i++)
                    for (int j = 0; j < CursorSize; j++)
                    {
                        switch (CursorItem)
                        {
                            case CursorMode.Erase:
                                MapData[MapX + x + i, MapY + y + j] = MapItem.None;
                                if (MapData.TileValue.GetLength(0) > MapX + x + i && MapX + x + i >= 0 && MapData.TileValue.GetLength(1) > MapY + y + j && MapY + y + j >= 0)
                                    MapData.TileValue[MapX + x + i, MapY + y + j] = 0;
                                break;
                            case CursorMode.Wall:
                                MapData[MapX + x + i, MapY + y + j] = MapItem.Wall;
                                break;
                            case CursorMode.Slug:
                                MapData[MapX + x + i, MapY + y + j] = MapItem.Slug;
                                break;
                            case CursorMode.Demon:
                                MapData[MapX + x + i, MapY + y + j] = MapItem.Demon;
                                break;
                            case CursorMode.Item:
                                MapData[MapX + x + i, MapY + y + j] = MapItem.Item;
                                break;
                            case CursorMode.Boss:
                                MapData[MapX + x + i, MapY + y + j] = MapItem.Boss;
                                break;
                            default:
                                break;
                        }
                        if(CursorItem != CursorMode.Erase && MapData.TileValue.GetLength(0) > MapX + x + i && MapX + x + i  >= 0 && MapData.TileValue.GetLength(1) > MapY + y + j && MapY + y + j >= 0)
                            MapData.TileValue[MapX + x + i, MapY + y + j] = CursorValue;

                    }
            }
        }


        /// <summary>
        /// Draws the map
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Draw(SpriteBatch sb, int mouseX, int mouseY, int width, int height)
        {
            //Use some variables to simplify code structure
            Texture2D text = WallTexture;
            Color col = Color.MediumSeaGreen;
            for (int i = MapX; i < MapData.Width && i < MapX + width; i++)
            {
                for(int j = MapY; j < MapData.Height && j < MapY + height; j++)
                {
                    Rectangle square = new Rectangle(Scale + (Scale * (i - MapX)), Scale + (Scale * (j - MapY)), Scale, Scale);

                    //Switch to  get tile details
                    switch (MapData[i,j])
                    {
                        case MapItem.None:
                            if (j > 0 && MapData[i, j - 1] == MapItem.Demon)
                                col = Color.TransparentBlack;
                            else
                                col = Color.LightSlateGray;
                            text = WallTexture;
                            break;
                        case MapItem.Wall:
                            text = WallTexture;
                            switch (MapData.TileValue[i, j])
                            {
                                case 0:
                                case 1:
                                    col = Color.Black;
                                    break;
                                case 2:
                                    col = Color.Sienna;
                                    break;
                                case 3:
                                    col = Color.SandyBrown;
                                    break;
                                case 4:
                                    col = Color.HotPink;
                                    break;
                                case 5:
                                    col = Color.Plum;
                                    break;
                            }
                            break;
                        case MapItem.Slug:
                            square = new Rectangle(Scale + (Scale * (i - MapX)), Scale + (Scale * (j - MapY)), Scale * 2, Scale);
                            text = SlugTexture;
                            col = Color.Blue;
                            break;
                        case MapItem.Demon:
                            square = new Rectangle(Scale + (Scale * (i - MapX)), Scale + (Scale * (j - MapY)), Scale, Scale * 2);
                            text = DemonTexture;
                            col = Color.Red;
                            break;
                        case MapItem.Item:
                            text = RoundTexture;
                            col = Color.Goldenrod;
                            if(MapData.TileValue[i,j] == 0)
                                col = Color.MintCream;
                            break;
                        case MapItem.Boss:
                            col = Color.BlanchedAlmond;
                            break; 
                        default:
                            break;

                    }
                    //Line that draws the tiles
                    sb.Draw(text, square, col);

                }
            }
            //Player Start
            if(MapX <= MapData.PlayerStartX && MapX + width > MapData.PlayerStartX && MapY <= MapData.PlayerStartY && MapY + height > MapData.PlayerStartY)
                sb.Draw(PlayerStartTexture, new Rectangle(Scale + (Scale * (MapData.PlayerStartX - MapX)), Scale + (Scale * (MapData.PlayerStartY - MapY)), Scale, Scale * 2), new Color(Color.Green, .125f));

            //Gridlines
            if(Gridlines)
                for(int i = 0; i < width; i++)
                {
                    for(int j = 0; j < height; j++)
                    {
                        sb.Draw(GridTexture, new Rectangle(Scale + (Scale * (i)), Scale + (Scale * (j)), Scale, Scale), Color.Brown);
                    }
                }


            //Mouse cursor
            switch (CursorItem)
            {
                case CursorMode.Erase:
                    text = WallTexture;
                    col = Color.LightSlateGray;
                    break;
                case CursorMode.Wall:
                    text = WallTexture;
                    col = Color.Black;
                    break;
                case CursorMode.Slug:
                    text = SlugTexture;
                    col = Color.Blue;
                    break;
                case CursorMode.Demon:
                    text = DemonTexture;
                    col = Color.Red;
                    break;
                case CursorMode.Player:
                    text = PlayerStartTexture;
                    col = Color.Green;
                    break;
                case CursorMode.Item:
                    text = WallTexture;
                    col = Color.Goldenrod;
                    break;
                default:
                    break;
            }

            //sb.Draw(RoundTexture, new Rectangle(mouseX - 2, mouseY - 2, 5, 5), new Color(Color.Brown, 1));
            //sb.Draw(text, new Rectangle(mouseX - 1, mouseY - 1, 3, 3), col);
            sb.Draw(RoundTexture, new Rectangle(mouseX - mouseX % (Scale), mouseY - mouseY % (Scale), CursorSize * Scale, CursorSize * Scale), new Color(col, .25f));

            sb.DrawString(Font, ""+ CursorValue, new Vector2(0, 0), Color.Red);


        }


    }
}
