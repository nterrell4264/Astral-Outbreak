using Microsoft.Xna.Framework.Graphics;
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
        //private Dictionary<string,Texture2D> masterList;
        public Dictionary<string,Texture2D> masterList { get; private set; } //Use until this can draw on its own, then use above

        //Constructor
        public SpriteManager()
        {
            masterList = new Dictionary<string, Texture2D>();
        }

        //Methods
        public void Draw(SpriteBatch sb) //MAIN DRAW METHOD
        {

        }

        public void AddTexture(Texture2D texture)
        {
            masterList.Add(texture.Name, texture);
        }
    }
}
