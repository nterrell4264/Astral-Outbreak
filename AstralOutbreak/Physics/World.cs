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
                    //This object is moving, which means we need to check for collisions!
                    for (int j = 0; i < PhysicsObjects.Count; j++)
                    {
                        //For each other object check for collisions
                        if (i != j && obj.CheckCollision(PhysicsObjects[j], obj.Velocity * time))
                        {
                            //Doing the full move triggers a collision!
                            if(PhysicalLogic == null || PhysicalLogic(obj, PhysicsObjects[j]))
                                //We got the go ahead from the delegate
                                if(obj.CheckCollision(PhysicsObjects[j], obj.VelocityX * time))
                                {
                                    //We now know that movement on the X axis causes a collision
                                    if(obj.Position.X < PhysicsObjects[j].Position.X)
                                    {
                                        //Im to the left, reduce my movement
                                        obj.VelocityX.X = PhysicsObjects[j].Position.X - obj.Position.X - obj.Width;
                                    }
                                    else
                                    {
                                        //Im to the right, reduce my movement
                                        obj.VelocityX.X = PhysicsObjects[j].Position.X - obj.Position.X + PhysicsObjects[j].Width;
                                    }
                                }
                                if (obj.CheckCollision(PhysicsObjects[j], obj.VelocityY * time))
                                {
                                    //We now know that movement on the Y axis causes a collision
                                    if (obj.Position.Y < PhysicsObjects[j].Position.Y)
                                    {
                                        //Im above, reduce my movement
                                        obj.VelocityY.Y = PhysicsObjects[j].Position.Y - obj.Position.Y - obj.Height;
                                    }
                                    else
                                    {
                                        //Im below, reduce my movement
                                        obj.VelocityY.Y = PhysicsObjects[j].Position.Y - obj.Position.Y + PhysicsObjects[j].Height;
                                    }
                                }
                            //If anyone cares, let them know about the collision
                            if (Collide != null)
                                Collide(obj, PhysicsObjects[j]);
                        }
                        //Change my position
                        obj.Position += obj.Velocity;
                    }
            }
        }//End of method


        //Event that handles non-physical collision results
        public event CollisionResolution Collide;

    }
}
