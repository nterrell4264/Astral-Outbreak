using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    /// <summary>
    /// Environment in which physics objects interact.
    /// </summary>
    public class World
    {
        //Physics objects that move and interact
        public List<PhysicsObject> PhysicsObjects { get; set; }
        public Vector Gravity { get; set; }

        /// <summary>
        /// A simple empty world.
        /// </summary>
        public World()
        {
            PhysicsObjects = new List<PhysicsObject>();
            Gravity = new Vector(0,0);
        }
        /// <summary>
        /// A simple empty world with gravity.
        /// </summary>
        public World(Vector2 grav)
        {
            PhysicsObjects = new List<PhysicsObject>();
            Gravity = new Vector(grav.X, grav.Y);
        }


        public void Update(float time)
        {
            //Loop through all the objects
            for(int i = 0; i < PhysicsObjects.Count; i++)
            {
                //Prepare the object
                PhysicsObject obj = PhysicsObjects[i];
                obj.VelocityX.X += obj.Acceleration.X;
                obj.VelocityY.Y += obj.Acceleration.Y;
                //Enforce Gravity
                if (obj.Gravity)
                {
                    obj.VelocityX.X += Gravity.X;
                    obj.VelocityY.Y += Gravity.Y;
                }
                
                //Check for collisions
                if (obj.VelocityX.X != 0 || obj.VelocityY.Y != 0)
                    for (int j = 0; i < PhysicsObjects.Count; j++)
                    {
                        if (i != j && obj.CheckCollision(PhysicsObjects[j], obj.Velocity * time))
                        {
                            if(obj.CheckCollision(PhysicsObjects[j], obj.VelocityX * time))
                            {
                                if(obj.Position.X < PhysicsObjects[j].Position.X)
                                {
                                    obj.VelocityX.X = PhysicsObjects[j].Position.X - obj.Position.X - obj.Width;
                                }
                                else
                                {
                                    obj.VelocityX.X = PhysicsObjects[j].Position.X - obj.Position.X + PhysicsObjects[j].Width;
                                }
                            }
                            if (obj.CheckCollision(PhysicsObjects[j], obj.VelocityY * time))
                            {
                                if (obj.Position.Y < PhysicsObjects[j].Position.Y)
                                {
                                    obj.VelocityY.Y = PhysicsObjects[j].Position.Y - obj.Position.Y - obj.Height;
                                }
                                else
                                {
                                    obj.VelocityY.Y = PhysicsObjects[j].Position.Y - obj.Position.Y + PhysicsObjects[j].Height;
                                }
                            }
                            obj.Collide(PhysicsObjects[j]);
                        }
                        obj.Position += obj.Velocity;
                    }
                


            }
        }


    }
}
