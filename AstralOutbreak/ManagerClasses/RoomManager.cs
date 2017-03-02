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
        public Room current { get; set; }
        

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


        //Private constructor
        private RoomManager()
        {
            current = null;
        }
    }
}
