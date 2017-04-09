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
        SpriteBatch sb;
        //Singleton pattern
        //private static SpriteManager instance;

        //public static SpriteManager Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new SpriteManager();
        //        }
        //        return instance;
        //    }
        //}

        public SpriteManager(SpriteBatch import)
        {
            masterList = new Dictionary<string, Texture2D>();
            SpriteBatch sb = (import);
        }

        //Class
        //private Dictionary<string,Texture2D> masterList;
        public Dictionary<string,Texture2D> masterList { get; private set; } //Use until this can draw on its own, then use above

        //Methods
        public void Update()//Will call individual Draw Methods for each entity based on what called it
        {
            //Roommanager.active.physicsobjects is the list of objects
            for (int i = 0; i < RoomManager.Active.PhysicsObjects.Count; i++)
            {
                if (RoomManager.Active.PhysicsObjects[i] is Player)
                {
                    Draw(sb, RoomManager.Active.PhysicsObjects[i] as Player);
                }
                else if (RoomManager.Active.PhysicsObjects[i] is Slug)
                {

                }
                else if (RoomManager.Active.PhysicsObjects[i] is JackRabbit)
                {

                }
            }
        }
        public void Draw(SpriteBatch sb) //MAIN DRAW METHOD
        {
            
        }
        public void Draw(SpriteBatch sb, Player player)
        {

        }
        public void Draw(SpriteBatch sb, Slug enemy)
        {

        }
        public void Draw(SpriteBatch sb, JackRabbit enemy)
        {

        }

        public void AddTexture(Texture2D texture)
        {
            masterList.Add(texture.Name, texture);
        }
    }
}
