using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    public abstract class Entity : GameObject
    {
        //A float that keeps track of how long the entityhas been in its current action
        public float CurrentActionTime { get; set; }


        /// <summary>
        /// Makes a mobile entity with the given constraints.
        /// </summary>
        /// <param name="pos">Position (Top left corner)</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public Entity(Vector2 pos, float width, float height, bool mobile = true) : base(pos, width, height, mobile)
        {
            CurrentActionTime = 0;
        }

        /// <summary>
        /// Gives all Game Objects a way to update themselves each step.
        /// </summary>
        /// <param name="deltaTime"></param>
        public abstract void Step(float deltaTime);
    }
}
