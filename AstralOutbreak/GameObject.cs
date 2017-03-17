using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    /// <summary>
    /// The most basic element of the game.
    /// </summary>
    public abstract class GameObject : PhysicsObject
    {
        /// <summary>
        /// Making this true tells the engine to unload the object.
        /// </summary>
        public bool Unload { get; set; }

        /// <summary>
        /// A simple constructor that handles all of the physics object parameters.
        /// </summary>
        /// <param name="pos">Position (Top left corner)</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="mobile">Can it move?</param>
        public GameObject(Vector2 pos, float width, float height, bool mobile = false) : base(new Vector(pos.X, pos.Y), width, height, mobile)
        {
            Unload = false;
        }

        

    }
}
