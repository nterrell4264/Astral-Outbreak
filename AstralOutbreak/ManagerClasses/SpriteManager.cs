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
                    if (RoomManager.Active.IsOnScreen(RoomManager.Active.PhysicsObjects[i]))
                    {
                        if (RoomManager.Active.PhysicsObjects[i] is Wall)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Wall, i);

                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is Slug)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Slug, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is JackRabbit)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as JackRabbit, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is JackRabbitBoss)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as JackRabbitBoss, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is SwarmMob)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as SwarmMob, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is BatShield)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as BatShield, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is Player)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Player, i);
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
                }
                sb.DrawString(fontList["font"], "" + RoomManager.Active.PlayerOne.Velocity.X, new Vector(20, 20), Color.White);
            }
            foreach (MenuContent menuPart in MenuManager.items)
            {
                if (menuPart is MenuString)
                {
                    sb.DrawString(spriteFont: fontList[(menuPart as MenuString).SpriteFont], text: (menuPart as MenuString).text, position: new Vector2(menuPart.Location.X, menuPart.Location.Y), color: Color.Black,rotation: 0, origin: new Vector2(0,0), scale: 1, effects: SpriteEffects.None, layerDepth: .1f);
                }
                else
                {
                    Texture2D texture = masterList["Menus/" + menuPart.textureName];
                    sb.Draw(texture: texture, destinationRectangle:new Rectangle(menuPart.Location, new Point(texture.Width, texture.Height)), color: Color.White, layerDepth: .2f);
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
            Rectangle dest = new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X - (int)RoomManager.Active.CameraX + (int)player.Width / 2 - 2,
                  (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY + (int)player.Height / 2,
                  (int)RoomManager.Active.PhysicsObjects[i].Width + 4, (int)RoomManager.Active.PhysicsObjects[i].Height);
            Rectangle pos = new Rectangle();
            Rectangle destArm = new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Center.X - (int)RoomManager.Active.CameraX,
                  (int)RoomManager.Active.PhysicsObjects[i].Center.Y - (int)RoomManager.Active.CameraY,
                  33, 16);
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
                sourceRectangle: pos, rotation: rot, origin: new Vector2(player.Width / 2, player.Height / 2),
                color: Color.White, effects: flip, layerDepth: .6f);
            float gunRot = RoomManager.Active.PlayerOne.Aim.GetAngle();
            Vector2 armOrg = new Vector2(5, 10);
            if (!RoomManager.Active.PlayerOne.FaceRight)
            {
                gunRot -= (float)Math.PI;
                armOrg = new Vector2(28, 10);
            }

            sb.Draw(masterList["PlayerSprites"],
                destinationRectangle: destArm,
                sourceRectangle: new Rectangle(5,148,33,16), rotation: gunRot + rot, origin: armOrg,
                color: Color.White, effects: flip, layerDepth: .5f);

            if (player.InvulnTime > 0 && player.CurrentPlayerState!= PlayerState.Rolling)
            {
                sb.Draw(masterList["PlayerSprites"],
                destinationRectangle: dest,
                sourceRectangle: pos, rotation: rot, origin: new Vector2(player.Width / 2, player.Height / 2),
                color: new Color(1,0,0, (player.InvulnTime * 4) % 1), effects: flip, layerDepth: .6f);
                sb.Draw(masterList["PlayerSprites"],
                destinationRectangle: destArm,
                sourceRectangle: new Rectangle(5, 148, 33, 16), rotation: gunRot + rot, origin: armOrg,
                color: new Color(1, 0, 0, (player.InvulnTime * 4) % 1), effects: flip, layerDepth: .5f);
            }
        }





        public void Draw(SpriteBatch sb, Slug enemy, int i)
        {
            Rectangle dest = new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Center.X - (int)RoomManager.Active.CameraX - 32,
                 (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY,
                 64, 34);
            Rectangle pos = new Rectangle();

            SpriteEffects flip = SpriteEffects.None;
            if (enemy.FaceRight)
                flip = SpriteEffects.FlipHorizontally;
            switch (enemy.CurrentSlugState)
            {
                default:
                    break;
                case SlugState.Falling:
                    pos = new Rectangle(97, 5, 65, 34);
                    break;
                case SlugState.Moving:
                    int o = (int)(enemy.CurrentActionTime * 8) % 4;
                    switch (o)
                    {
                        default:
                            pos = new Rectangle(97, 5, 65, 34);
                            break;
                        case 1:
                            pos = new Rectangle(187, 6, 64, 34);
                            break;
                        case 2:
                            pos = new Rectangle(269, 6, 64, 34);
                            break;
                        case 3:
                            pos = new Rectangle(359, 7, 64, 34);
                            break;
                    }
                    break;
                case SlugState.Idle:
                    int y = (int)(enemy.CurrentActionTime * 8) % 4;
                    switch (y)
                    {
                        default:
                            pos = new Rectangle(97, 5, 65, 34);
                            break;
                        case 1:
                            pos = new Rectangle(187, 6, 64, 34);
                            break;
                        case 2:
                            pos = new Rectangle(269, 6, 64, 34);
                            break;
                        case 3:
                            pos = new Rectangle(359, 7, 64, 34);
                            break;
                    }
                    break;
            }
            sb.Draw(masterList["SlugSprites"], destinationRectangle: dest,
                   sourceRectangle: pos, color: Color.White, effects: flip, layerDepth: .4f);
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
            sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest,
            sourceRectangle: pos, color: Color.White, effects: flip, layerDepth: .4f);
        }

        public void Draw(SpriteBatch sb, JackRabbitBoss enemy, int i)
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
                    pos = new Rectangle(5, 160, 31, 61);
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
            sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest,
            sourceRectangle: pos, color: Color.Crimson, effects: flip, layerDepth: .4f);
            if (enemy.CurrentJackRabbitState == JackRabbitState.Falling)
            {
                dest = new Rectangle(dest.X - (int)enemy.Velocity.X / 120, dest.Y - (int)enemy.Velocity.Y / 120, dest.Width, dest.Height);
                sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest,
                sourceRectangle: pos, color: new Color(Color.Crimson, .5f), effects: flip, layerDepth: .39f);
                dest = new Rectangle(dest.X - (int)enemy.Velocity.X / 120, dest.Y - (int)enemy.Velocity.Y / 120, dest.Width, dest.Height);
                sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest,
                sourceRectangle: pos, color: new Color(Color.Crimson, .25f), effects: flip, layerDepth: .38f);
            }
        }

        public void Draw(SpriteBatch sb, Wall wall, int i)
        {
            Rectangle pos = new Rectangle();
            Rectangle dest = new Rectangle((int)wall.Position.X - (int)RoomManager.Active.CameraX,
                  (int)wall.Position.Y - (int)RoomManager.Active.CameraY,
                  (int)wall.Width, (int)wall.Height);
            SpriteEffects flip = SpriteEffects.None;
            Texture2D sheet = masterList["rect"];
            Color col = Color.White;
            switch (wall.MyType)
            {
                default:
                case WallType.Regular:
                    sheet = masterList["TileSheet"];
                    switch (wall.Adj)
                    {
                        case WallAdj.None:
                            pos = new Rectangle(0, 0, 28, 28);
                            break;
                        case WallAdj.Top:
                            pos = new Rectangle(28, 0, 28, 28);
                            break;
                        case WallAdj.Left:
                            pos = new Rectangle(56, 0, 28, 28);
                            break;
                        case WallAdj.Right:
                            pos = new Rectangle(84, 0, 28, 28);
                            break;
                        case WallAdj.Bottom:
                            pos = new Rectangle(0, 28, 28, 28);
                            break;
                        case WallAdj.Top | WallAdj.Left:
                            pos = new Rectangle(28, 28, 28, 28);
                            break;
                        case WallAdj.Top | WallAdj.Right:
                            pos = new Rectangle(56, 28, 28, 28);
                            break;
                        case WallAdj.Top | WallAdj.Bottom:
                            pos = new Rectangle(84, 28, 28, 28);
                            break;
                        case WallAdj.Left | WallAdj.Right:
                            pos = new Rectangle(0, 56, 28, 28);
                            break;
                        case WallAdj.Left | WallAdj.Bottom:
                            pos = new Rectangle(28, 56, 28, 28);
                            break;
                        case WallAdj.Bottom | WallAdj.Right:
                            pos = new Rectangle(56, 56, 28, 28);
                            break;
                        case WallAdj.Right | WallAdj.Top | WallAdj.Left:
                            pos = new Rectangle(84, 56, 28, 28);
                            break;
                        case WallAdj.Left | WallAdj.Top | WallAdj.Bottom:
                            pos = new Rectangle(0, 84, 28, 28);
                            break;
                        case WallAdj.Right | WallAdj.Top | WallAdj.Bottom:
                            pos = new Rectangle(28, 84, 28, 28);
                            break;
                        case WallAdj.Left | WallAdj.Right | WallAdj.Bottom:
                            pos = new Rectangle(56, 84, 28, 28);
                            break;
                        case WallAdj.Right | WallAdj.Left | WallAdj.Top | WallAdj.Bottom:
                            pos = new Rectangle(84, 84, 28, 28);
                            break;
                        default:
                            break;
                    }
                    break;
                case WallType.Platform:
                    col = Color.Black;
                    break;
                case WallType.BossDoor:
                    col = Color.DimGray;
                    break;
                case WallType.SecretTunnel:
                    sheet = masterList["TileSheet"];
                    pos = new Rectangle(84, 84, 28, 28);
                    break;
                
            }
            if (wall.MyType == WallType.Fire)
                col = Color.Red;
            sb.Draw(sheet, destinationRectangle: dest,
            sourceRectangle: pos, color: col, effects: flip, layerDepth: 1);
        }

        public void Draw(SpriteBatch sb, SwarmMob enemy, int i)
        {
            Rectangle dest;

            Rectangle pos = new Rectangle();
            int s = (int)(enemy.CurrentActionTime*8) % 4;
            switch (s)
            {
                default:
                    pos = new Rectangle(71, 14, 54, 15);
                    dest = new Rectangle((int)enemy.Center.X - (int)RoomManager.Active.CameraX - 27,
                  (int)enemy.Center.Y - (int)RoomManager.Active.CameraY -7,
                  54, 15);
                    break;
                case 1:
                    pos = new Rectangle(132, 15, 56, 17);
                    dest = new Rectangle((int)enemy.Center.X - (int)RoomManager.Active.CameraX - 28,
                  (int)enemy.Center.Y - (int)RoomManager.Active.CameraY - 8,
                  56, 17);
                    break;
                case 2:
                    pos = new Rectangle(198, 14, 36, 23);
                    dest = new Rectangle((int)enemy.Center.X - (int)RoomManager.Active.CameraX - 18,
                  (int)enemy.Center.Y - (int)RoomManager.Active.CameraY - 11,
                  36, 23);
                    break;
                case 3:
                    pos = new Rectangle(132, 15, 56, 17);
                    dest = new Rectangle((int)enemy.Center.X - (int)RoomManager.Active.CameraX - 28,
                  (int)enemy.Center.Y - (int)RoomManager.Active.CameraY - 8,
                  56, 17);
                    break;
            }
            sb.Draw(texture: masterList["MiscSprites"], destinationRectangle: dest, sourceRectangle: pos, color: new Color(1,1,1,.9f), effects: SpriteEffects.None, layerDepth: .4f);
        }
        public void Draw(SpriteBatch sb, BatShield bat, int i)
        {
            Rectangle dest;

            Rectangle pos = new Rectangle();
            int s = (int)(bat.CurrentActionTime * 8) % 4;
            switch (s)
            {
                default:
                    pos = new Rectangle(71, 14, 54, 15);
                    dest = new Rectangle((int)bat.Center.X - (int)RoomManager.Active.CameraX - 27,
                  (int)bat.Center.Y - (int)RoomManager.Active.CameraY - 7,
                  54, 15);
                    break;
                case 1:
                    pos = new Rectangle(132, 15, 56, 17);
                    dest = new Rectangle((int)bat.Center.X - (int)RoomManager.Active.CameraX - 28,
                  (int)bat.Center.Y - (int)RoomManager.Active.CameraY - 8,
                  56, 17);
                    break;
                case 2:
                    pos = new Rectangle(198, 14, 36, 23);
                    dest = new Rectangle((int)bat.Center.X - (int)RoomManager.Active.CameraX - 18,
                  (int)bat.Center.Y - (int)RoomManager.Active.CameraY - 11,
                  36, 23);
                    break;
                case 3:
                    pos = new Rectangle(132, 15, 56, 17);
                    dest = new Rectangle((int)bat.Center.X - (int)RoomManager.Active.CameraX - 28,
                  (int)bat.Center.Y - (int)RoomManager.Active.CameraY - 8,
                  56, 17);
                    break;
            }
            sb.Draw(texture: masterList["MiscSprites"], destinationRectangle: dest, sourceRectangle: pos, color: new Color(1, 1, 1, .9f), effects: SpriteEffects.None, layerDepth: .61f);
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
