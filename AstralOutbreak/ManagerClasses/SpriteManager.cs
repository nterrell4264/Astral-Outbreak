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

        public SpriteManager(Game1 game)
        {
            main = game;
            masterList = new Dictionary<string, Texture2D>();
            fontList = new Dictionary<string, SpriteFont>();
        }

        //Class
        private Dictionary<string, Texture2D> masterList;
        private Dictionary<string, SpriteFont> fontList;
        private Game1 main; //Used to accommodate for window size changes.

        //Methods
        public void Update()
        {
            //Hard-coded UI updates because I don't know 
        }

        public void Draw(SpriteBatch sb)//Will call individual Draw Methods for each entity based on what called it
        {
            if (Game1.CurrentState == GameState.Playing)
            {
                for(int i = 0; i < 11; i++)
                    for (int j = 0; j < 11; j++)
                    {
                        sb.Draw(masterList["MoreSprites"], destinationRectangle: new Rectangle(i * 127 - (int)RoomManager.Active.CameraX % 127, j * 127 - (int)RoomManager.Active.CameraY % 127, 127, 127), sourceRectangle: new Rectangle(0,0,127,127), color: new Color(.4f, .5f, .6f, .9f), layerDepth: 1);
                    }
                sb.End();
                sb.Begin(SpriteSortMode.BackToFront);

                //Roommanager.active.physicsobjects is the list of objects
                for (int i = 0; i < RoomManager.Active.PhysicsObjects.Count; i++)
                {
                    if (RoomManager.Active.IsOnScreen(RoomManager.Active.PhysicsObjects[i]))
                    {
                        if (RoomManager.Active.PhysicsObjects[i] is Wall)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Wall, i);

                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is Turret)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Turret, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is Pod)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Pod, i);

                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is Projectile)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Projectile, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is Slug)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Slug, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is JackRabbit)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as JackRabbit, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is DashRabbit)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as DashRabbit, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is MultiRabbit)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as MultiRabbit, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is SwarmMob)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as SwarmMob, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is BatShield)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as BatShield, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is Bat)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Bat, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is Player)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Player, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is Item)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as Item, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is CoreBoss)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as CoreBoss, i);
                        }
                        else if (RoomManager.Active.PhysicsObjects[i] is EscapePod)
                        {
                            Draw(sb, RoomManager.Active.PhysicsObjects[i] as EscapePod, i);
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
                
                //sb.DrawString(fontList["font"], "" + RoomManager.Active.PlayerOne.Velocity.X, new Vector(20, 20), Color.White);
            }
            else if(Game1.CurrentState == GameState.MainMenu)
            {
                sb.Draw(masterList["BestTitleScreenEver"], destinationRectangle: new Rectangle(0, 0, Game1.Graphics.PreferredBackBufferWidth, Game1.Graphics.PreferredBackBufferHeight));
            }
            else if (Game1.CurrentState == GameState.Victory)
            {
                sb.Draw(masterList["BestYouWinScreenEver"], destinationRectangle: new Rectangle(0, 0, Game1.Graphics.PreferredBackBufferWidth, Game1.Graphics.PreferredBackBufferHeight));
            }
            sb.End();
            sb.Begin();
            //Menus
            foreach (MenuContent menuPart in MenuManager.items)
            {
                if (menuPart.IsVisible)
                {
                    if (menuPart is MenuString)
                    {
                        sb.DrawString(spriteFont: fontList[(menuPart as MenuString).SpriteFont], text: (menuPart as MenuString).text, position: new Vector2(menuPart.Location.X, menuPart.Location.Y), color: Color.Black, rotation: 0, origin: new Vector2(0, 0), scale: 1, effects: SpriteEffects.None, layerDepth: menuPart.depth);
                    }
                    else
                    {
                        Texture2D texture = masterList["Menus/" + menuPart.textureName];
                        sb.Draw(texture: texture, destinationRectangle: new Rectangle(menuPart.Location, new Point(texture.Width, texture.Height)), color: Color.White, layerDepth: menuPart.depth);
                    }
                }
            }
            //Boss health
            if (Game1.CurrentState == GameState.Playing && RoomManager.Active.CurrentBoss != null)
            {
                double bossPercent = RoomManager.Active.CurrentBoss.Health / RoomManager.Active.CurrentBoss.MaxHealth;
                Color barColor = Color.Green;
                if (bossPercent <= .75) barColor = Color.YellowGreen;
                if (bossPercent <= .5) barColor = Color.Yellow;
                if (bossPercent <= .25) barColor = Color.Orange;
                if (bossPercent <= .1) barColor = Color.Red;
                sb.Draw(masterList["rect"], new Rectangle(main.GraphicsDevice.Viewport.Width / 2 - 200,
                 3, (int)(398 * bossPercent), 19), barColor);
            }
            //Dialogue box
            if ((Game1.CurrentState == GameState.Playing || DialogueManager.DisplayOnMenu) && DialogueManager.Active)
            {
                sb.Draw(masterList["TextBox"], DialogueManager.Box, new Color(.5f, .5f, .6f, .25f));
                sb.Draw(masterList["Avatars"], DialogueManager.AvatarDest, DialogueManager.AvatarPos, DialogueManager.AvatarCol);
                sb.DrawString(fontList["textfont"], DialogueManager.CurrentDialogue, DialogueManager.TextDest, Color.Black);
            }
            sb.End();
            sb.Begin();
            sb.Draw(masterList["MoreSprites"],destinationRectangle: new Rectangle(Game1.Inputs.MouseX, Game1.Inputs.MouseY, 14, 14), 
                sourceRectangle: new Rectangle(240, 56, 14, 14), origin: new Vector2(7, 7));
        }





        // spriteBatch.Draw(spriteManager.masterList["rect"],
        // new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X - (int)RoomManager.Active.CameraX, 
        //               (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY,
        //             (int)RoomManager.Active.PhysicsObjects[i].Width, (int)RoomManager.Active.PhysicsObjects[i].Height),
        //           Color.Blue);
        //Sub methods of Draw made for each type of entity

        //DRAW FOR PLAYER
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
            switch (player.CurrentPlayerState)
            {
                default:
                    break;
                case PlayerState.Idle:
                    pos = new Rectangle(3, 181, 32, 56);
                    if (player.FaceRight == true)
                    {
                        destArm = new Rectangle(destArm.X - 3, destArm.Y - 2, destArm.Width, destArm.Height);
                    }
                    else
                    {
                        destArm = new Rectangle(destArm.X + 3, destArm.Y - 2, destArm.Width, destArm.Height);
                    }
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
                    if (player.UpwardDash)
                    {
                        if (player.FaceRight)
                        {
                            rot = -(float)Math.PI / 2;
                            dest.X += 56;
                            dest.Y += 56;
                        }
                        else
                        {
                            rot = (float)Math.PI / 2;
                        }
                    }
                    break;
                case PlayerState.Falling: pos = new Rectangle(4, 78, 32, 55);
                    destArm = new Rectangle(destArm.X, destArm.Y - 5, destArm.Width, destArm.Height);
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
                        case 1: pos = new Rectangle(71, 7, 28, 54);
                            break;
                        case 2: pos = new Rectangle(141, 6, 28, 55);
                            break;
                        case 3: pos = new Rectangle(200, 6, 28, 55);
                            break;
                        case 4: pos = new Rectangle(261, 7, 28, 53);
                            break;
                        case 5: pos = new Rectangle(333, 6, 28, 55);
                            break;
                    }
                    break;
            }
            //if(player.CurrentPlayerState != PlayerState.Rolling)
            //    dest = new Rectangle((int)(player.Position.X - pos.Width / 2 + player.Width - RoomManager.Active.CameraX),
            //    (int)(player.Position.Y - RoomManager.Active.CameraY + (int)player.Height / 2 + player.Height - pos.Height), pos.Width, pos.Height);
            sb.Draw(masterList["PlayerSprites"],
            destinationRectangle: dest,
            sourceRectangle: pos, rotation: rot, origin: new Vector2(player.Width / 2, player.Height / 2),
            color: Color.White, effects: flip, layerDepth: .6f);
            float gunRot = player.Aim.GetAngle();
            Vector2 armOrg = new Vector2(5, 10);
            if (!player.FaceRight)
            {
                gunRot -= (float)Math.PI;
                armOrg = new Vector2(28, 10);
            }
            if (player.CurrentPlayerState != PlayerState.Dashing) {
            sb.Draw(masterList["PlayerSprites"],
                destinationRectangle: destArm,
                sourceRectangle: new Rectangle(5, 148, 33, 16), rotation: gunRot + rot, origin: armOrg,
                color: Color.White, effects: flip, layerDepth: .5f);
            }
            if (player.InvulnTime > 0 && player.CurrentPlayerState!= PlayerState.Rolling)
            {
                sb.Draw(masterList["PlayerSprites"],
                destinationRectangle: dest,
                sourceRectangle: pos, rotation: rot, origin: new Vector2(player.Width / 2, player.Height / 2),
                color: new Color(1,0,0, (player.InvulnTime * 4) % 1), effects: flip, layerDepth: .59f);
                if (player.CurrentPlayerState != PlayerState.Dashing)
                    sb.Draw(masterList["PlayerSprites"],
                    destinationRectangle: destArm,
                    sourceRectangle: new Rectangle(5, 148, 33, 16), rotation: gunRot + rot, origin: armOrg,
                    color: new Color(1, 0, 0, (player.InvulnTime * 4) % 1), effects: flip, layerDepth: .49f);
            }
        }




        //DRAW FOR SLUGS
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




        //DRAW FOR JACKRABBIT
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





        //DRAW FOR DASH BOSS
        public void Draw(SpriteBatch sb, DashRabbit enemy, int i)
        {
            //Rectangle pos = new Rectangle();
            //Rectangle dest = new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Position.X - (int)RoomManager.Active.CameraX - 3,
            //      (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY - 5,
            //      35, 61);
            //SpriteEffects flip = SpriteEffects.None;
            //if (!enemy.FaceRight)
            //    flip = SpriteEffects.FlipHorizontally;
            //switch (enemy.CurrentJackRabbitState)
            //{
            //    default:
            //        break;
            //    case JackRabbitState.Falling:
            //        pos = new Rectangle(5, 160, 31, 61);
            //        break;
            //    case JackRabbitState.Idle:
            //        pos = new Rectangle(4, 227, 35, 59);
            //        break;
            //    case JackRabbitState.Moving:

            //        int t = (int)(enemy.CurrentActionTime * 8) % 6;
            //        switch (t)
            //        {
            //            default:
            //                pos = new Rectangle(5, 99, 46, 55);
            //                dest.X -= 6;
            //                dest.Width += 12;
            //                break;
            //            case 1:
            //                pos = new Rectangle(68, 99, 47, 54);
            //                dest.X -= 6;
            //                dest.Width += 12;
            //                break;
            //            case 2:
            //                pos = new Rectangle(138, 105, 33, 60);
            //                break;
            //            case 3:
            //                pos = new Rectangle(206, 104, 30, 61);
            //                break;
            //            case 4:
            //                pos = new Rectangle(258, 99, 44, 54);
            //                dest.X -= 6;
            //                dest.Width += 12;
            //                break;
            //            case 5:
            //                pos = new Rectangle(323, 101, 46, 54);
            //                dest.X -= 6;
            //                dest.Width += 12;
            //                break;
            //        }
            //        break;
            //    case JackRabbitState.Shooting:

            //        int s = (int)(enemy.CurrentActionTime * 8) % 6;
            //        switch (s)
            //        {
            //            default:
            //                pos = new Rectangle(6, 28, 32, 63);
            //                break;
            //            case 1:
            //                pos = new Rectangle(68, 28, 37, 63);
            //                break;
            //            case 2:
            //                pos = new Rectangle(131, 28, 41, 63);
            //                dest.X -= 6;
            //                dest.Width += 12;
            //                break;
            //            case 3:
            //                pos = new Rectangle(205, 28, 34, 63);
            //                break;
            //            case 4:
            //                pos = new Rectangle(270, 28, 41, 63);
            //                dest.X -= 6;
            //                dest.Width += 12;
            //                break;
            //            case 5:
            //                pos = new Rectangle(329, 28, 30, 63);
            //                break;
            //        }
            //        break;

            //}
            Rectangle dest = new Rectangle((int)RoomManager.Active.PhysicsObjects[i].Center.X - (int)RoomManager.Active.CameraX - 48,
                 (int)RoomManager.Active.PhysicsObjects[i].Position.Y - (int)RoomManager.Active.CameraY,
                 80, 51);
            Rectangle pos = new Rectangle();

            SpriteEffects flip = SpriteEffects.None;
            if (enemy.FaceRight)
                flip = SpriteEffects.FlipHorizontally;
            switch (enemy.CurrentJackRabbitState)
            {
                default:
                    break;
                case JackRabbitState.Falling:
                    pos = new Rectangle(96, 52, 65, 34);
                    break;
                case JackRabbitState.Moving:
                    int o = (int)(enemy.CurrentActionTime * 8) % 4;
                    switch (o)
                    {
                        default:
                            pos = new Rectangle(96, 52, 65, 34);
                            break;
                        case 1:
                            pos = new Rectangle(186, 53, 64, 34);
                            break;
                        case 2:
                            pos = new Rectangle(268, 53, 64, 34);
                            break;
                        case 3:
                            pos = new Rectangle(358, 54, 64, 34);
                            break;
                    }
                    break;
                case JackRabbitState.Idle:
                    int y = (int)(enemy.CurrentActionTime * 8) % 4;
                    switch (y)
                    {
                        default:
                            pos = new Rectangle(96, 52, 65, 34);
                            break;
                        case 1:
                            pos = new Rectangle(186, 53, 64, 34);
                            break;
                        case 2:
                            pos = new Rectangle(268, 53, 64, 34);
                            break;
                        case 3:
                            pos = new Rectangle(358, 54, 64, 34);
                            break;
                    }
                    break;
            }
            Color col = Color.White;// new Color(enemy.Health / enemy.MaxHealth / 2, enemy.Health / enemy.MaxHealth / 2, 1, 1);
            sb.Draw(masterList["SlugSprites"], destinationRectangle: dest,
            sourceRectangle: pos, color: col, effects: flip, layerDepth: .4f);
            if (enemy.CurrentJackRabbitState == JackRabbitState.Falling)
            {
                dest = new Rectangle(dest.X - (int)enemy.Velocity.X / 120, dest.Y - (int)enemy.Velocity.Y / 120, dest.Width, dest.Height);
                sb.Draw(masterList["SlugSprites"], destinationRectangle: dest,
                sourceRectangle: pos, color: new Color(col, .5f), effects: flip, layerDepth: .41f);
                dest = new Rectangle(dest.X - (int)enemy.Velocity.X / 120, dest.Y - (int)enemy.Velocity.Y / 120, dest.Width, dest.Height);
                sb.Draw(masterList["SlugSprites"], destinationRectangle: dest,
                sourceRectangle: pos, color: new Color(col, .25f), effects: flip, layerDepth: .42f);
            }
        }



        //Draw for Multi Boss
        public void Draw(SpriteBatch sb, MultiRabbit enemy, int i)
        {
            Rectangle pos = new Rectangle();
            Rectangle pos2 = new Rectangle();
            Rectangle pos3 = new Rectangle();
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
                            pos2 = new Rectangle(329, 28, 30, 63);
                            pos3 = new Rectangle(270, 28, 41, 63);
                            break;
                        case 1:
                            pos = new Rectangle(68, 28, 37, 63);
                            pos2 = new Rectangle(6, 28, 32, 63);
                            pos3 = new Rectangle(329, 28, 30, 63);
                            break;
                        case 2:
                            pos = new Rectangle(131, 28, 41, 63);
                            pos2 = new Rectangle(68, 28, 37, 63);
                            pos3 = new Rectangle(6, 28, 32, 63);
                            dest.X -= 6;
                            dest.Width += 12;
                            break;
                        case 3:
                            pos = new Rectangle(205, 28, 34, 63);
                            pos2 = new Rectangle(131, 28, 41, 63);
                            pos3 = new Rectangle(68, 28, 37, 63);
                            break;
                        case 4:
                            pos = new Rectangle(270, 28, 41, 63);
                            pos2 = new Rectangle(205, 28, 34, 63);
                            pos3 = new Rectangle(131, 28, 41, 63);
                            dest.X -= 6;
                            dest.Width += 12;
                            break;
                        case 5:
                            pos = new Rectangle(329, 28, 30, 63);
                            break;
                    }
                    break;

            }
            Color col = new Color(enemy.Health / enemy.MaxHealth / 2, enemy.Health / enemy.MaxHealth / 2, 1, 1);
            sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest,
            sourceRectangle: pos, color: col, effects: flip, layerDepth: .4f);
            if (enemy.CurrentJackRabbitState == JackRabbitState.Falling)
            {
                dest = new Rectangle(dest.X - (int)enemy.Velocity.X / 120, dest.Y - (int)enemy.Velocity.Y / 120, dest.Width, dest.Height);
                sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest,
                sourceRectangle: pos, color: new Color(col, .5f), effects: flip, layerDepth: .41f);
                dest = new Rectangle(dest.X - (int)enemy.Velocity.X / 120, dest.Y - (int)enemy.Velocity.Y / 120, dest.Width, dest.Height);
                sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest,
                sourceRectangle: pos, color: new Color(col, .25f), effects: flip, layerDepth: .42f);
            }
            else if (enemy.Spinning)
            {
                sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest,
                sourceRectangle: pos2, color: new Color(col, .5f), effects: flip, layerDepth: .41f);
                sb.Draw(masterList["JackrabbitSprites"], destinationRectangle: dest,
                sourceRectangle: pos3, color: new Color(col, .25f), effects: flip, layerDepth: .42f);
            }
        }


        //Draw for Walls
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
                    col = new Color(.8f, .8f, .8f);
                    sheet = masterList["MoreSprites"];
                    pos = new Rectangle(194, 156, 28, 8);

                    break;
                case WallType.BossDoor:
                    sheet = masterList["MoreSprites"];
                    if (RoomManager.Active.BossActive)
                    {
                        pos = new Rectangle(192, 47, 28, 28);
                        col = new Color(.5f, .5f, .6f, 1f);
                    }
                    else
                    {
                        pos = new Rectangle(150, 47, 28, 28);
                        col = new Color(.5f, .5f, .6f, 1f);
                    }
                    break;
                case WallType.SecretTunnel:
                    sheet = masterList["TileSheet"];
                    pos = new Rectangle(0, 0, 28, 28);
                    break;
                
            }
            if (wall.MyType == WallType.Fire)
            {
                col = Color.Orange;
                Rectangle dest2 = new Rectangle(dest.X, dest.Y - 8, 28, 12);
                Rectangle pos2 = pos;
                switch ((int)(RoomManager.Active.TotalPlayTime * 30) % 5)
                {
                    default:
                    case 0:
                        pos2 = new Rectangle(143, 19, 28, 12);
                        break;
                    case 1:
                        pos2 = new Rectangle(172, 19, 28, 12);
                        break;
                    case 2:
                        pos2 = new Rectangle(201, 19, 28, 12);
                        break;
                    case 3:
                        pos2 = new Rectangle(230, 19, 28, 12);
                        break;
                    case 4:
                        pos2 = new Rectangle(259, 19, 28, 12);
                        break;
                }
                sb.Draw(masterList["MoreSprites"], destinationRectangle: dest2,
            sourceRectangle: pos2, color: Color.White, effects: flip, layerDepth: 0f);
                dest2 = new Rectangle(dest.X, dest.Y + 4, 28, 12);
                sb.Draw(masterList["MoreSprites"], destinationRectangle: dest2,
            sourceRectangle: pos2, color: Color.White, effects: flip, layerDepth: 0f);
                dest2 = new Rectangle(dest.X, dest.Y + 16, 28, 12);
                sb.Draw(masterList["MoreSprites"], destinationRectangle: dest2,
            sourceRectangle: pos2, color: Color.White, effects: flip, layerDepth: 0f);
            }
            sb.Draw(sheet, destinationRectangle: dest,
            sourceRectangle: pos, color: col, effects: flip, layerDepth: 1);
        }


        //Draw for Bats
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

        //Turret draw
        public void Draw(SpriteBatch sb, Turret enemy, int i)
        {
            Vector2 org = new Vector2(14, 14);
            Rectangle dest;
            Rectangle pos = new Rectangle();
            if (!enemy.Damaged && enemy.Awake)
            {
                pos = new Rectangle(300, 12, 46, 28);
            }
            else
            {
                pos = new Rectangle(350, 12, 46, 28);
            }
            dest = new Rectangle((int)enemy.Center.X - (int)RoomManager.Active.CameraX,
            (int)enemy.Center.Y - (int)RoomManager.Active.CameraY,
            48, 28);
            sb.Draw(texture: masterList["MoreSprites"], destinationRectangle: dest, sourceRectangle: pos, color: new Color(1, 1, 1, 1f), origin: org, rotation: enemy.Aim.GetAngle(),effects: SpriteEffects.None, layerDepth: .4f);
        }

        //Pod draw
        public void Draw(SpriteBatch sb, Pod enemy, int i)
        {
            Vector2 org = new Vector2(14, 14);
            Rectangle dest;
            Rectangle pos = new Rectangle();
            pos = new Rectangle(131, 153, 34, 28);
            if(enemy.Direction != Facing.Left && enemy.Direction != Facing.Up)
                dest = new Rectangle((int)enemy.Position.X + 14 - (int)RoomManager.Active.CameraX,
                (int)enemy.Position.Y + 14 - (int)RoomManager.Active.CameraY,
                34, 28);
            else if(enemy.Direction == Facing.Left)
            {
                dest = new Rectangle((int)enemy.Position.X - (int)RoomManager.Active.CameraX,
                (int)enemy.Position.Y + 14 - (int)RoomManager.Active.CameraY,
                34, 28);
            }
            else
            {
                dest = new Rectangle((int)enemy.Position.X + 14 - (int)RoomManager.Active.CameraX,
                (int)enemy.Position.Y - (int)RoomManager.Active.CameraY,
                34, 28);
            }
            sb.Draw(texture: masterList["MoreSprites"], destinationRectangle: dest, sourceRectangle: pos, color: new Color(.8f, .8f, .8f, 1f), origin: org, rotation: enemy.Aim.GetAngle(), effects: SpriteEffects.None, layerDepth: .4f);
        }

        //Core Boss draw
        public void Draw(SpriteBatch sb, CoreBoss enemy, int i)
        {
            Vector2 org = new Vector2(69, 60);
            Rectangle dest;
            Rectangle pos = new Rectangle();
            pos = new Rectangle(437, 14, 118, 121);
            dest = new Rectangle((int)enemy.Center.X - (int)RoomManager.Active.CameraX,
            (int)enemy.Center.Y - (int)RoomManager.Active.CameraY,
            118, 121);
            sb.Draw(texture: masterList["MoreSprites"], destinationRectangle: dest, sourceRectangle: pos, color: new Color(1, 1, 1, 1f), origin: org, effects: SpriteEffects.None, layerDepth: .4f);
        }

        //Escape Pod draw
        public void Draw(SpriteBatch sb, EscapePod enemy, int i)
        {
            Rectangle dest;
            Rectangle pos = new Rectangle();
            pos = new Rectangle(4, 151, 86, 137);
            dest = new Rectangle((int)enemy.Position.X - (int)RoomManager.Active.CameraX,
            (int)enemy.Position.Y - (int)RoomManager.Active.CameraY,
            86, 137);
            sb.Draw(texture: masterList["MoreSprites"], destinationRectangle: dest, sourceRectangle: pos, color: new Color(.6f, .7f, .8f, 1f), effects: SpriteEffects.None, layerDepth: .4f);
        }

        //Non boss bats
        public void Draw(SpriteBatch sb, Bat enemy, int i)
        {
            Rectangle dest;

            Rectangle pos = new Rectangle();
            int s = (int)(enemy.CurrentActionTime * 8) % 4;
            switch (s)
            {
                default:
                    pos = new Rectangle(71, 14, 54, 15);
                    dest = new Rectangle((int)enemy.Center.X - (int)RoomManager.Active.CameraX - 27,
                  (int)enemy.Center.Y - (int)RoomManager.Active.CameraY - 7,
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
            sb.Draw(texture: masterList["MiscSprites"], destinationRectangle: dest, sourceRectangle: pos, color: new Color(1, 1, 1, .9f), effects: SpriteEffects.None, layerDepth: .4f);
        }




        //draw for Bat Shield
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
            sb.Draw(texture: masterList["MiscSprites"], destinationRectangle: dest, sourceRectangle: pos, color: new Color(1, 1, .5f, .9f), effects: SpriteEffects.None, layerDepth: .61f);
        }


        //Draw for Items
        public void Draw(SpriteBatch sb, Item item, int i)
        {
            Rectangle dest = new Rectangle((int)item.Position.X - (int)RoomManager.Active.CameraX,
                  (int)item.Position.Y - (int)RoomManager.Active.CameraY,
                  (int)item.Width, (int)item.Height);
            Rectangle pos = new Rectangle();
            switch(item.MyType)
            {
                default:
                    break;
                case ItemType.HealthPickup:
                    pos = new Rectangle(25,6,10,10);
                    break;

                case ItemType.AbilityUnlock:
                    switch(item.Value)
                    {
                        //Default takes the place of case 0
                        default: pos = new Rectangle(88,76,14,14);
                            break;
                        case 1: pos = new Rectangle(116, 76, 14, 14);
                            break;
                        case 2: pos = new Rectangle(130, 76, 14, 14);
                            break;
                        case 3: pos = new Rectangle(102, 76, 14, 14);
                            break;
                    }
                    break;
            }
            sb.Draw(texture: masterList["MiscSprites"], destinationRectangle: dest, sourceRectangle: pos, color: Color.White, layerDepth: .21f);
        }
        //Draw for Bullets
        public void Draw(SpriteBatch sb, Projectile bullet, int i)
        {
            if (!bullet.IsDead)
            {
                Color col = Color.Gray;
                Rectangle dest = new Rectangle((int)bullet.Position.X - (int)RoomManager.Active.CameraX,
                      (int)bullet.Position.Y - (int)RoomManager.Active.CameraY,
                      (int)bullet.Width, (int)bullet.Height);
                Rectangle pos = new Rectangle();
                pos = new Rectangle(25, 6, 10, 10);
                sb.Draw(texture: masterList["MiscSprites"], destinationRectangle: dest, sourceRectangle: pos, color: col, layerDepth: .71f);
            }
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
