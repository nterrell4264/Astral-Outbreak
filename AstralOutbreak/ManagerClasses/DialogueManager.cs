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
        DashPickup, MultiShotPickup, BatShieldPickup,
        Victory, Saved, FailedSave, Load, FailedLoad
    }

    //Manages dialogue pop ups
    public class DialogueManager
    {
        private int index;

        public static DialogueManager Dialogue { get; private set; }
        public static bool Active { get; set; }
        public static bool DisplayOnMenu { get; set; }


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
                        return new Rectangle(112, 0, 86, 43);
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
        public static void Clear()
        {
            Active = false;
        }
        public static void Update(Triggers t)
        {
            DisplayOnMenu = false;
            Dialogue.index = 0;
            List<DialogueInfo> newDialogue = new List<DialogueInfo>();
            switch (t)
            {
                case Triggers.Start:
                    newDialogue.Add(new DialogueInfo("Ugh, where am I? I think I got knocked out earlier in all the confusion.", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("I'm not really sure what happend, but I'm pretty sure I was trying to head for an escape pod.", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("I should try to find the ship's main computer, and ask it where the escape pods are.", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("I can probably get out of this room by rolling with <LEFT SHIFT>.", Speaker.Player, Color.White));
                    break;
                case Triggers.Saved:
                    newDialogue.Add(new DialogueInfo("Saved successfully.", Speaker.Player, Color.White));
                    DisplayOnMenu = true;
                    break;
                case Triggers.FailedSave:
                    newDialogue.Add(new DialogueInfo("Unable to save.", Speaker.Player, Color.White));
                    DisplayOnMenu = true;
                    break;
                case Triggers.Load:
                    newDialogue.Add(new DialogueInfo("Loaded successfully.", Speaker.Player, Color.White));
                    DisplayOnMenu = true;
                    break;
                case Triggers.FailedLoad:
                    newDialogue.Add(new DialogueInfo("Unable to load file.", Speaker.Player, Color.White));
                    DisplayOnMenu = true;
                    break;
                case Triggers.Victory:
                    newDialogue.Add(new DialogueInfo("I made it.", Speaker.Player, Color.White));
                    break;
                case Triggers.Death:
                    newDialogue.Add(new DialogueInfo("I have... failed.", Speaker.Player, new Color(1, 1, 1, 1)));
                    break;
                case Triggers.SlugBossStart:
                    newDialogue.Add(new DialogueInfo("Glug, Glug!", Speaker.Slug, Color.White));
                    break;
                case Triggers.SlugBossEnd:
                    newDialogue.Add(new DialogueInfo("Glug... Glug...", Speaker.Slug, Color.White));
                    break;
                case Triggers.MultiBossStart:
                    newDialogue.Add(new DialogueInfo("Another mortal come to die.", Speaker.Jack, new Color(.5f, .5f, 1, 1)));
                    break;
                case Triggers.MultiBossEnd:
                    newDialogue.Add(new DialogueInfo("Ha ha ha, you only... delay the inevitable, mortal...", Speaker.Jack, new Color(.5f, .5f, 1, 1)));
                    newDialogue.Add(new DialogueInfo("We've already taken over the main computer... this colony is done for...", Speaker.Jack, new Color(.5f, .5f, 1, 1)));
                    break;
                case Triggers.BatBossStart:
                    newDialogue.Add(new DialogueInfo("Its quiet. Too quiet.", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("SCRRREEEEEEEEEEECH.", Speaker.Bat, Color.White));
                    break;
                case Triggers.BatBossEnd:
                    newDialogue.Add(new DialogueInfo("If I never see another bat, it will be too soon.", Speaker.Player, Color.White));
                    break;
                case Triggers.ComputerBossStart:
                    newDialogue.Add(new DialogueInfo("Computer, where are the escape pods?", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("ERROR. ERROR. SEGMENATATION FAULT. MUST... TERMINATE... USER.", Speaker.Computer, Color.White));
                    break;
                case Triggers.ComputerBossEnd:
                    newDialogue.Add(new DialogueInfo("Computer, I demand that you tell me the location of the escape pods.", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("Rebooting...", Speaker.Computer, Color.White));
                    newDialogue.Add(new DialogueInfo("Go... East...", Speaker.Computer, Color.White));
                    newDialogue.Add(new DialogueInfo("ERROR. ERROR. MUST... TERMINATE...", Speaker.Computer, Color.White));
                    newDialogue.Add(new DialogueInfo("Thanks.", Speaker.Player, Color.White));
                    break;
                case Triggers.DashPickup:
                    newDialogue.Add(new DialogueInfo("Wow, with this I should be able to dash by hitting <SPACEBAR>!", Speaker.Player, Color.White));
                    break;
                case Triggers.MultiShotPickup:
                    newDialogue.Add(new DialogueInfo("Three shots are better than one.", Speaker.Player, Color.White));
                    break;
                case Triggers.BatShieldPickup:
                    newDialogue.Add(new DialogueInfo("Oh what's this? Bat eggs? I think they are hatching!", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("Mommy?", Speaker.Bat, new Color(1, 1, .5f, .9f)));
                    newDialogue.Add(new DialogueInfo("AHHHHH!!!!", Speaker.Player, Color.White));
                    newDialogue.Add(new DialogueInfo("Mommy!", Speaker.Bat, new Color(1, 1, .5f, .9f)));
                    break;
                default:
                    break;
            }
            Dialogue.currentDialogue = newDialogue;
            Active = true;
        }




    }
}
