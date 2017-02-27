﻿using System;
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
        //Bool that denotes death
        public bool IsDead { get; set; }
        //Float that signifies the entities current health
        private float health;
        public float Health
        {
            get { return health; }
            set
            {
                health = value;
                IsDead = health <= 0;
            }
        }


        /// <summary>
        /// Makes a mobile entity with the given constraints.
        /// </summary>
        /// <param name="pos">Position (Top left corner)</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public Entity(Vector2 pos, float width, float height, float health, bool mobile = true) : base(pos, width, height, mobile)
        {
            CurrentActionTime = 0;
            IsDead = false;
            Health = health;
        }

        /// <summary>
        /// Gives all Game Objects a way to update themselves each step.
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void Step(float deltaTime)
        {
            CurrentActionTime += deltaTime;
        }
    }
}
