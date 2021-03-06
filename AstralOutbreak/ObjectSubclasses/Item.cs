﻿using System;
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
        public bool Consumed { get; set; }

        public override bool Unload
        {
            get
            {
                return base.Unload;
            }
            set
            {
                if (Consumed)
                {
                    base.Unload = true;
                    if ((OriginX >= 0 && OriginY >= 0))
                        RoomManager.MapData.Loaded[OriginX, OriginY] = true;
                }
                else
                {
                    base.Unload = value;
                }
            }
        }

        public Item(Vector2 pos, float width, float height, ItemType type, int val, bool mobile = true) : base(pos, width, height, mobile)
        {
            Gravity = true;
            MyType = type;
            Value = val;
            Consumed = false;
        }
    }
}
