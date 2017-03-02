using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{


    /// <summary>
    /// The most basic Object that handles the physics in the game.
    /// </summary>
    public class PhysicsObject
    {
        //Motion
        public bool Mobile { get; set; }
        public bool Gravity { get; set; }
        public Vector Position { get; set; }
        public Vector VelocityX { get; set; }
        public Vector VelocityY { get; set; }
        public Vector Velocity
        {
            get
            {
                return VelocityX + VelocityY;
            }
        }
        public Vector Acceleration { get; set; }

        //Rectangle parameters
        public float Width { get; set; }
        public float Height { get; set; }
        public Vector Center
        {
            get
            {
                return new Vector(Position.X + Width/2, Position.Y + Height/2);
            }
        }


        /// <summary>
        /// Creates a physics object with everything it needs.
        /// </summary>
        /// <param name="pos">Top left corner.</param>
        /// <param name="width">Width of the hitbox.</param>
        /// <param name="height">Height of the hitbox.</param>
        /// <param name="mobile">Can the object move.</param>
        public PhysicsObject(Vector pos, float width, float height, bool mobile = false, bool grav = false)
        {
            Gravity = grav;
            Mobile = mobile;
            Position = pos;
            VelocityX = new Vector(0, 0);
            VelocityY = new Vector(0, 0);
        }

        /// <summary>
        /// Checks for a collision between two objects.
        /// </summary>
        /// <param name="other">Object to check collisions against.</param>
        /// <returns>True if there is a collision.</returns>
        public bool CheckCollision(PhysicsObject other)
        {
            if (Position.X > other.Position.X + other.Width)
                return false;
            if (Position.X + Width < other.Position.X)
                return false;
            if (Position.Y > other.Position.Y + other.Height)
                return false;
            if (Position.Y + Height < other.Position.Y)
                return false;
            return true;
        }

        /// <summary>
        /// Checks for a collision between two objects after a movement.
        /// </summary>
        /// <param name="other">Object to check collisions against.</param>
        /// <param name="vel">Velocity of movement.</param>
        /// <returns>True if there is a collision.</returns>
        public bool CheckCollision(PhysicsObject other, Vector vel)
        {
            if (Position.X + vel.X > other.Position.X + other.Width)
                return false;
            if (Position.X + Width + vel.X < other.Position.X)
                return false;
            if (Position.Y + vel.Y > other.Position.Y + other.Height)
                return false;
            if (Position.Y + Height + vel.Y < other.Position.Y)
                return false;
            return true;
        }

    }
}
