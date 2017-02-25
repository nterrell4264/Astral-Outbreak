using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    /// <summary>
    /// A simple basic wall
    /// </summary>
    public class Wall : GameObject
    {
        /// <summary>
        /// Makes an immobile Wall
        /// </summary>
        /// <param name="pos">Position (Top left corner)</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public Wall(Vector2 pos, float width, float height, bool mobile = false) : base(pos, width, height, mobile)
        {
        }
    }
}
