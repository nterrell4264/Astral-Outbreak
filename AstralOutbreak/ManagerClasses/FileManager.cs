using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace AstralOutbreak
{
    public class FileManager
    {
        //IMPORTANT:
        // Most the functions of this class have or will become superfluous.
        // I will remove the unnecessary code, but there are things I want to make sure work first.
        // -Mark


        public List<Room> roomsLoaded { get; set; }

        //Singleton
        private static FileManager manager;
        public static FileManager Manager
        {
            get
            {
                if (manager == null)
                    manager = new FileManager();
                return manager;
            }
        }

        private FileManager()
        {
            roomsLoaded = new List<Room>();
        }



        /// <summary>
        /// Loads all the rooms we want to load
        /// </summary>
        public void ReloadRooms()
        {
            UnloadRooms();
            //LoadRooms(RoomManager.Active.RoomLinks);
        }

        /// <summary>
        /// Unloads all the rooms and reloads the current room
        /// </summary>
        public void UnloadRooms()
        {
            roomsLoaded = new List<Room>();
            LoadRoom(RoomManager.Active.RoomNumber);
        }

        /// <summary>
        /// Loads all of the rooms in the list
        /// </summary>
        /// <param name="roomIDs">Ids of rooms you want to load</param>
        public void LoadRooms(List<int> roomIDs)
        {
            for(int i = 0; i < roomIDs.Count; i++)
            {
                LoadRoom(roomIDs[i]);
            }
        }

        /// <summary>
        /// Loads a room by its id number
        /// </summary>
        /// <param name="index"></param>
        public void LoadRoom(int index)
        {
            if(File.Exists("World\\Room<" + index + ">"))
                try
                {
                    using (StreamReader input = new StreamReader("World\\Room<" + index + ">"))
                    {


                        input.Close();
                    }
                }
                catch
                {

                }
                finally
                {

                }
                
        }

    }
}
