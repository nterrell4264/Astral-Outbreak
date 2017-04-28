using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class SpriteManager
    {
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

        public SpriteManager()
        {
            masterList = new Dictionary<string, Texture2D>();
            fontList = new Dictionary<string, SpriteFont>();
        }

        //Class
        private Dictionary<string,Texture2D> masterList;
        private Dictionary<string, SpriteFont> fontList;

        //Methods
        public void Update()
        {
            //Hard-coded UI updates because I don't know 
        }

        public void Draw(SpriteBatch sb)//Will call individual Draw Methods for each entity based on what called it
        {
            if (Game1.CurrentState == GameState.Playing)
            {
                //Roommanager.active.physicsobjects is the list of objects
                for (int i = 0; i < RoomManager.Active.PhysicsObjects.Count; i++)
                {
                    if (RoomManager.Active.PhysicsObjects[i] is Player)
                    {
                        Draw(sb, RoomManager.Active.PhysicsObjects[i] as Player, i);
                    }
                    else if (RoomManager.Active.PhysicsObjects[i] is Slug)
                    {
                        Draw(sb, RoomManager.Active.PhysicsObjects[i] as Slug, i);
                    }
                    else if (RoomManager.Active.PhysicsObjects[i] is JackRabbit)
                    {
                        Draw(sb, RoomManager.Active.PhysicsObjects[i] as JackRabbit, i);
                    }
                    else
                    {
                        sb.Draw(masterList["rect"],
                        new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X - (int)RoomManager.Active.CameraX,
                        (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY,
                        (int)RoomManager.Active.PhysicsObjects[i].Width, (int)RoomManager.Active.PhysicsObjects[i].Height),
                        Color.Black);
                    }
                }
                sb.DrawString(fontList["font"], "" + RoomManager.Active.PlayerOne.Velocity.X, new Vector(20, 20), Color.White);
            }
            foreach (MenuContent menuPart in MenuManager.items)
            {
                if (menuPart is MenuString)
                {
                    sb.DrawString(fontList[(menuPart as MenuString).SpriteFont], (menuPart as MenuString).Text, new Vector2(menuPart.Location.X, menuPart.Location.Y), Color.Black);
                }
                else
                {
                    Texture2D texture = masterList["Menus/" + menuPart.TextureName];
                    sb.Draw(texture, new Rectangle(menuPart.Location, new Point(texture.Width, texture.Height)), Color.White);
                }
            }
        }
        // spriteBatch.Draw(spriteManager.masterList["rect"],
       // new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X - (int)RoomManager.Active.CameraX, 
         //               (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY,
           //             (int)RoomManager.Active.PhysicsObjects[i].Width, (int)RoomManager.Active.PhysicsObjects[i].Height),
             //           Color.Blue);
        //Sub methods of Draw made for each type of entity
        public void Draw(SpriteBatch sb, Player player, int i)
        {
            float rot = 0;
            Rectangle dest = new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X - (int)RoomManager.Active.CameraX + (int)player.Width / 2,
                  (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY + (int)player.Height / 2,
                  (int)RoomManager.Active.PhysicsObjects[i].Width, (int)RoomManager.Active.PhysicsObjects[i].Height);
            Rectangle pos = new Rectangle();
            //Mark: Added horizontal flipping
            SpriteEffects flip = SpriteEffects.None;
            if (!player.FaceRight)
                flip = SpriteEffects.FlipHorizontally;
            switch(player.CurrentPlayerState)
            {
                default:
                    break;
                case PlayerState.Idle:
                    pos = new Rectangle(3, 181, 32, 56);
                    break;
                case PlayerState.Dashing:
                    dest.Width += 60;
                    if (player.FaceRight == true)
                    {
                        dest.X -= 60;
                    }
                    int t = (int)(player.CurrentActionTime * 8) % 3;
                    switch (t)
                    {
                        default:
                            pos = new Rectangle(4, 255, 92, 60);
                            break;
                        case 1:
                            pos = new Rectangle(105, 258, 82, 60);
                            break;
                        case 2:
                            pos = new Rectangle(199, 257, 88, 55);
                            break;
                    }
                    break;
                case PlayerState.Falling: pos = new Rectangle(4,78,32,55);
                    break;
                case PlayerState.Rolling:
                    pos = new Rectangle(3, 181, 32, 56);
                    if (player.FaceRight)
                        rot = player.CurrentActionTime * 30;
                    else
                        rot = player.CurrentActionTime * -30;
                    break;
                case PlayerState.Running: int o = (int)(player.CurrentActionTime * 8) % 6;
                    switch (o)
                    {
                        default: pos = new Rectangle(6, 6, 28, 55);
                            break;
                        case 1: pos = new Rectangle(71,7,28,54);
                            break;
                        case 2: pos = new Rectangle(141,6,28,55);
                            break;
                        case 3: pos = new Rectangle(200,6,28,55);
                            break;
                        case 4: pos = new Rectangle(261,7,28,53);
                            break;
                        case 5: pos = new Rectangle(333,6,28,55);
                            break;
                    }
                    break;
            }
                  sb.Draw(masterList["PlayerSprites"],
                  destinationRectangle: dest,
                  sourceRectangle: pos, rotation: rot, origin: new Vector2(player.Width / 2, player.Height/2),
                  color: Color.White, effects: flip);
        }
        public void Draw(SpriteBatch sb, Slug enemy, int i)
        {

            switch (enemy.CurrentState)
            {
                default:
                    break;
                case SlugState.Falling:
                    break;
                case SlugState.MovingLeft:
                    break;
                case SlugState.MovingRight:
                    break;

            }
        }
        public void Draw(SpriteBatch sb, JackRabbit enemy, int i)
        {
            Rectangle pos = new Rectangle();
            Rectangle dest = new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X - (int)RoomManager.Active.CameraX - 3,
                  (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY - 5,
                  35, 61);
            SpriteEffects flip = SpriteEffects.None;
            if (!enemy.FaceRight)
                flip = SpriteEffects.FlipHorizontally;
            switch (enemy.CurrentJackRabbitState)
            {
                default:
                    break;
                case JackRabbitState.Falling:
                    pos = new Rectangle(5,160,31,61);
                    break;
                case JackRabbitState.Idle:
                    pos = new Rectangle(4, 227, 35, 59);
                    break;
                case JackRabbitState.Moving:

                    int t = (int)(enemy.CurrentActionTime * 8) % 6;
                    switch (t)
                    {
                        default:
                            pos = new Rectangle(5, 99, 46, 55);
                            dest.X -= 6;
                            dest.Width += 12;
                            break;
                        case 1:
                            pos = new Rectangle(68, 99, 47, 54);
                            dest.X -= 6;
                            dest.Width += 12;
                            break;
                        case 2:
                            pos = new Rectangle(138, 105, 33, 60);
                            break;
                        case 3:
                            pos = new Rectangle(206, 104, 30, 61);
                            break;
                        case 4:
                            pos = new Rectangle(258, 99, 44, 54);
                            dest.X -= 6;
                            dest.Width += 12;
                            break;
                        case 5:
                            pos = new Rectangle(323, 101, 46, 54);
                            dest.X -= 6;
                            dest.Width += 12;
                            break;
                    }
                    break;
                case JackRabbitState.Shooting:

                    int s = (int)(enemy.CurrentActionTime * 8) % 6;
                    switch (s)
                    {
                        default:
                            pos = new Rectangle(6, 28, 32, 63);
                            break;
                        case 1:
                            pos = new Rectangle(68, 28, 37, 63);
                            break;
                        case 2:
                            pos = new Rectangle(131, 28, 41, 63);
                            dest.X -= 6;
                            dest.Width += 12;
                            break;
                        case 3:
                            pos = new Rectangle(205, 28, 34, 63);
                            break;
                        case 4:
                            pos = new Rectangle(270, 28, 41, 63);
                            dest.X -= 6;
                            dest.Width += 12;
                            break;
                        case 5:
                            pos = new Rectangle(329, 28, 30, 63);
                            break;
                    }
                    break;

            }
            sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest
                  ,
                  sourceRectangle: pos, color: Color.White, effects: flip);
        }

        public void AddTexture(Texture2D texture)
        {
            masterList.Add(texture.Name, texture);
        }
        public void AddFont(string fontName, SpriteFont font)
        {
            fontList.Add(fontName, font);
        }
    }
}
