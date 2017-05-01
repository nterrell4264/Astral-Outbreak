using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace AstralOutbreak
{
    public class RoomManager
    {
        //The room currently in play
        public static Room Active { get; set; }
        

        //Stores the one instance of this class
        private static RoomManager data;
        public static RoomManager Data
        {
            get
            {
                if (data == null)
                    data = new RoomManager();
                return data;
            }
        }

        //Access the rooms map
        public static Map MapData
        {
            get { return Active.MapData; }
            set { Active.MapData = value; }
        }



        //Private constructor
        private RoomManager()
        {
            Active = null;
        }

        //Yet to be implemented
        public static bool SaveGame(String fileName)
        {
            try
            {

                using (StreamWriter output = new StreamWriter(File.OpenWrite(fileName)))
                {
                    output.WriteLine(JsonConvert.SerializeObject(MapData.Save()));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool LoadGame(String fileName)
        {
            if (File.Exists(fileName))
                try
                {
                    using (StreamReader input = new StreamReader(File.OpenRead(fileName)))
                    {
                        Map loaded = (JsonConvert.DeserializeObject<Map>(input.ReadToEnd()));
                        if (loaded != null)
                        {
                            Active.LoadRoom(loaded);
                            return true;
                        }
                        else
                            return false;
                    }
                }
                catch
                {
                    return false;
                }
            else
                return false;
        }

    }
}
