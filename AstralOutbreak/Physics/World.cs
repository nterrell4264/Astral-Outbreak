using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralOutbreak
{
    public delegate bool CollisionLogic(PhysicsObject obj1, PhysicsObject obj2);
    public delegate void CollisionResolution(PhysicsObject obj1, PhysicsObject obj2);


    /// <summary>
    /// Environment in which physics objects interact.
    /// </summary>
    public class World
    {
        //Physics objects that move and interact
        public List<PhysicsObject> PhysicsObjects { get; set; }
        //Gravity as an acceleration
        public Vector Gravity { get; set; }

        /// <summary>
        /// Returns true if the objects collide and stop.
        /// </summary>
        public CollisionLogic PhysicalLogic { get; set; }


        /// <summary>
        /// A simple empty world.
        /// </summary>
        public World()
        {
            PhysicsObjects = new List<PhysicsObject>();
            Gravity = new Vector(0,0);
        }
        /// <summary>
        /// A simple empty world with gravity and a terminal velocity.
        /// </summary>
        public World(Vector2 grav, Vector2 terminal)
        {
            PhysicsObjects = new List<PhysicsObject>();
            Gravity = new Vector(grav.X, grav.Y);
        }


        public void Update(float time)
        {
            Vector displacement;
            //Loop through all the objects
            for (int i = 0; i < PhysicsObjects.Count; i++)
            {
                //Prepare the object
                PhysicsObject obj = PhysicsObjects[i];
                obj.Velocity += obj.Acceleration;
                //Enforce Gravity
                if (obj.Gravity)
                {
                    obj.Velocity += Gravity;
                }

                //Enforce Terminal velocity
                if (obj.MaxVelocity.X >= 0)
                {
                    if (obj.Velocity.X > obj.MaxVelocity.X)
                    {
                        obj.Velocity.X = obj.MaxVelocity.X;
                    }
                    else if (obj.Velocity.X < -obj.MaxVelocity.X)
                    {
                        obj.Velocity.X = -obj.MaxVelocity.X;
                    }
                }
                if (obj.MaxVelocity.Y >= 0)
                {
                    if (obj.Velocity.Y > obj.MaxVelocity.Y)
                    {
                        obj.Velocity.Y = obj.MaxVelocity.Y;
                    }
                    else if (obj.Velocity.Y < -obj.MaxVelocity.Y)
                    {
                        obj.Velocity.Y = -obj.MaxVelocity.Y;
                    }
                }
                displacement = obj.Velocity * time;
                //Check for collisions
                if ((obj.Velocity.X != 0 || obj.Velocity.Y != 0) && obj.Collides)
                {
                    //This object is moving, which means we need to check for collisions!
                    for (int j = 0; j < PhysicsObjects.Count; j++)
                    {
                        //For each other object check for collisions
                        if (i != j && obj.CheckCollision(PhysicsObjects[j], displacement))
                        {
                            //Doing the full move triggers a collision!
                            if (PhysicalLogic == null || PhysicalLogic(obj, PhysicsObjects[j]))
                            {
                                //We got the go ahead from the delegate
                                if (obj.CheckCollision(PhysicsObjects[j], obj.VelocityX * time))
                                {
                                    //We now know that movement on the X axis causes a collision
                                    if (obj.Position.X < PhysicsObjects[j].Position.X)
                                    {
                                        //Im to the left, reduce my movement
                                        obj.Velocity.X = (PhysicsObjects[j].Position.X - obj.Position.X - obj.Width) / time;
                                    }
                                    else
                                    {
                                        //Im to the right, reduce my movement
                                        obj.Velocity.X = (PhysicsObjects[j].Position.X - obj.Position.X + PhysicsObjects[j].Width) / time;
                                    }
                                }

                                else if (obj.CheckCollision(PhysicsObjects[j], obj.VelocityY * time))
                                {
                                    //We now know that movement on the Y axis causes a collision
                                    if (obj.Position.Y < PhysicsObjects[j].Position.Y)
                                    {
                                        //Im above, reduce my movement
                                        obj.Velocity.Y = (PhysicsObjects[j].Position.Y - obj.Position.Y - obj.Height) / time;
                                    }
                                    else
                                    {
                                        //Im below, reduce my movement
                                        obj.Velocity.Y = (PhysicsObjects[j].Position.Y - obj.Position.Y + PhysicsObjects[j].Height) / time;
                                    }
                                }
                            }
                            //If anyone cares, let them know about the collision
                            if (Collide != null)
                                Collide(obj, PhysicsObjects[j]);
                        }
                    }
                }
                displacement = obj.Velocity * time;
                //Change my position
                obj.Position += displacement;
            }
        }//End of method


        public bool CheckCollision(PhysicsObject obj, Vector displacement)
        {
            //This object is moving, which means we need to check for collisions!
            for (int i = 0; i < PhysicsObjects.Count; i++)
            {
                //For each other object check for collisions
                if (!obj.Equals(PhysicsObjects[i]) && obj.CheckCollision(PhysicsObjects[i], displacement))
                {
                    //Doing the transform triggers a collision!
                    if (PhysicalLogic == null || PhysicalLogic(obj, PhysicsObjects[i]))
                        return true;
                }
            }
            return false;
        }

        //Event that handles non-physical collision results
        public event CollisionResolution Collide;

    }
}
