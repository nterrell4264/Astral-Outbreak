using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public class KeySet
    {
        public ActionButton Button { get; set; }
        public Keys Key { get; set; }
        public ButtonStatus Status { get; set; }

        public KeySet(ActionButton button, Keys key)
        {
            Button = button;
            Key = key;
            Status = ButtonStatus.Unpressed;
        }
    }
}
