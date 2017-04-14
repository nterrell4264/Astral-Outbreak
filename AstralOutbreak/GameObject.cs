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

        // IMPORTANT: OriginX and OriginY are map coordinates NOT Room coordinates
        // These are used to prevent enemies from spawning repeatidly if they move from their original position.
        public int OriginX { get; set; }
        public int OriginY { get; set; }

        /// <summary>
        /// Making this true tells the engine to unload the object.
        /// </summary>
        private bool unload;

        public virtual bool Unload
        {
            get { return unload; }
            set
            {
                unload = value;
                if(unload && (OriginX >= 0 || OriginY >= 0))
                    RoomManager.MapData.Loaded[OriginX, OriginY] = false;
            }
        }


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
            OriginX = -1;
            OriginY = -1;
        }

        

    }
}
