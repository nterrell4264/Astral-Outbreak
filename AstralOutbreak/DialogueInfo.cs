using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public enum Speaker { Player, Jack, Bat, Slug, Computer}

    public struct DialogueInfo
    {
        public String MyText { get; }
        public Speaker MySpeaker { get; }
        public Color MyTint { get; }

        public DialogueInfo(String text, Speaker speaker, Color tint)
        {
            MyText = text;
            MySpeaker = speaker;
            MyTint = tint;
        }

    }
}
