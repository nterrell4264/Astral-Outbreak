using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public enum ItemType { HealthPickup, WeaponUpgrade, AbilityUnlock}
    public class Item : GameObject
    {

        public ItemType MyType { get; set; }
        public int Value { get; set; }

        public Item(Vector2 pos, float width, float height, ItemType type, int val, bool mobile = false) : base(pos, width, height, mobile)
        {
            MyType = type;
            Value = val;
        }
    }
}
