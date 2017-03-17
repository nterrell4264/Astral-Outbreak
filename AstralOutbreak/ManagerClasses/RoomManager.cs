using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
