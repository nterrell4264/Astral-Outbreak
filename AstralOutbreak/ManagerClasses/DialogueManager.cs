using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public enum Triggers { Start, Death,
        //Boss Triggers
        SlugBossStart, SlugBossEnd, MultiBossStart, MultiBossEnd, BatBossStart, BatBossEnd, ComputerBossStart, ComputerBossEnd,
        //Item Pickups
        DashPickup, MultiShotPickup, BatShieldPickup}

    //Manages dialogue pop ups
    public class DialogueManager
    {
        private int index;

        public static DialogueManager Dialogue { get; private set; }
        public static bool Active { get; set; }

        private List<DialogueInfo> currentDialogue;


        public static Rectangle Box
        {
            get
            {
                return new Rectangle(Game1.Graphics.PreferredBackBufferWidth / 8, Game1.Graphics.PreferredBackBufferHeight * 7 / 8 - 16, Game1.Graphics.PreferredBackBufferWidth * 3 / 4, Game1.Graphics.PreferredBackBufferHeight / 16);
            }
        }

        public static Rectangle AvatarDest
        {
            get
            {
                return new Rectangle(Box.X + Box.Height/ 10, Box.Y + Box.Height / 10, Box.Height * 8 / 10, Box.Height * 8 / 10);
            }
        }

        public static Vector2 TextDest
        {
            get
            {
                return new Vector2(Box.X + Box.Height, Box.Y + Box.Height / 10);
            }
        }

        public static Rectangle AvatarPos
        {
            get
            {
                switch (Dialogue.currentDialogue[Dialogue.index].MySpeaker)
                {
                    default:
                    case Speaker.Player:
                        return new Rectangle(0, 0, 28, 28);
                        break;
                    case Speaker.Jack:
                        return new Rectangle(28, 0, 28, 28);
                        break;
                    case Speaker.Bat:
                        return new Rectangle(56, 0, 28, 28);
                        break;
                    case Speaker.Slug:
                        return new Rectangle(84, 0, 28, 28);
                        break;
                    case Speaker.Computer:
                        return new Rectangle(112, 0, 28, 28);
                        break;
                }
            }
        }

        public static Color AvatarCol
        {
            get
            {
                return Dialogue.currentDialogue[Dialogue.index].MyTint;
            }
        }
        public static String CurrentDialogue
        {
            get
            {
                return Dialogue.currentDialogue[Dialogue.index].MyText;
            }
        }

        static DialogueManager()
        {
            Dialogue = new DialogueManager();
            Dialogue.currentDialogue.Add(new DialogueInfo("Test", Speaker.Player, Color.White));
            Active = false;

        }
        private DialogueManager()
        {
            currentDialogue = new List<DialogueInfo>();
        }
        
        public static void Update()
        {
            if (Active)
            {
                if (Dialogue.index < Dialogue.currentDialogue.Count - 1)
                    Dialogue.index++;
                else
                {
                    Active = false;
                }
            }
        }
        public static void Update(Triggers t)
        {
            Dialogue.index = 0;
            List<DialogueInfo> newDialogue = new List<DialogueInfo>();
            switch (t)
            {
                case Triggers.Start:
                    newDialogue.Add(new DialogueInfo("Ugh, where am I? I think I got knocked out durring the attack.", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("The ship still seems to be intact, but I wouldn't be suprised if it was compromised.", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("I should try to find the ship's main computer, and find out where the escape pods are.", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("I can probably get out of this room by rolling with <LEFT SHIFT>.", Speaker.Player, Color.White));
                    break;
                case Triggers.Death:
                    newDialogue.Add(new DialogueInfo("I have... failed.", Speaker.Player, Color.Red));
                    break;
                case Triggers.SlugBossStart:
                    newDialogue.Add(new DialogueInfo("Glug, Glug!", Speaker.Slug, Color.White));
                    break;
                case Triggers.SlugBossEnd:
                    newDialogue.Add(new DialogueInfo("Glug... glug.", Speaker.Slug, Color.White));
                    break;
                case Triggers.MultiBossStart:
                    newDialogue.Add(new DialogueInfo("Test", Speaker.Jack, new Color(.5f, .5f, 1, 1)));
                    newDialogue.Add(new DialogueInfo("Test", Speaker.Player, Color.White));
                    break;
                case Triggers.MultiBossEnd:
                    newDialogue.Add(new DialogueInfo("Test", Speaker.Player, Color.White));
                    break;
                case Triggers.BatBossStart:
                    newDialogue.Add(new DialogueInfo("Test", Speaker.Player, Color.White));
                    break;
                case Triggers.BatBossEnd:
                    newDialogue.Add(new DialogueInfo("Test", Speaker.Player, Color.White));
                    break;
                case Triggers.ComputerBossStart:
                    newDialogue.Add(new DialogueInfo("Test", Speaker.Player, Color.White));
                    break;
                case Triggers.ComputerBossEnd:
                    newDialogue.Add(new DialogueInfo("Test", Speaker.Player, Color.White));
                    break;
                case Triggers.DashPickup:
                    newDialogue.Add(new DialogueInfo("Wow, with this I should be able to dash by hitting spacebar!", Speaker.Player, Color.White));
                    break;
                case Triggers.MultiShotPickup:
                    newDialogue.Add(new DialogueInfo("Three shots are better than one.", Speaker.Player, Color.White));
                    break;
                case Triggers.BatShieldPickup:
                    newDialogue.Add(new DialogueInfo("Ahhggg!!! More bats!", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("Mommy? Mommy!", Speaker.Bat, new Color(1, 1, .5f, .9f)));
                    newDialogue.Add(new DialogueInfo("AHHHHH!!!!", Speaker.Player, Color.White));
                    break;
                default:
                    break;
            }
            Dialogue.currentDialogue = newDialogue;
            Active = true;
        }




    }
}
