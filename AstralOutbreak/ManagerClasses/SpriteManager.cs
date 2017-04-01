using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class SpriteManager
    {
        private static SpriteManager instance;

        public static SpriteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpriteManager();
                }
                return instance;
            }
        }

        private SpriteManager()
        {

        }
    }
}
