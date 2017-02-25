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
        //Note: This class doesn't do anything important at this time due to the use of my physics instead of Farseer Phyisics.
        //      The reason it still exists is so that if we decide we want all game objects to do something that isn't physics
        //      related, we can without putting it in the physics classes.

        /// <summary>
        /// A simple constructor that handles all of the physics object parameters.
        /// </summary>
        /// <param name="pos">Position (Top left corner)</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="mobile">Can it move?</param>
        public GameObject(Vector2 pos, float width, float height, bool mobile = false) : base(new Vector(pos.X, pos.Y), width, height, mobile)
        {
        }

        

    }
}
