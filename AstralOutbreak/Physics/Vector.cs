using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AstralOutbreak
{
    /// <summary>
    /// A vector that uses floats and operates as a class instead of a struct.
    /// </summary>
    public class Vector
    {
        //Vector Stats
        public float X { get; set; }
        public float Y { get; set; }

        /// <summary>
        /// A basic constructor that makes a simple vector from two floats.
        /// </summary>
        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns magnitude squared.
        /// </summary>
        public float MagnitudeSquared()
        {
            return X * X + Y * Y;
        }
        /// <summary>
        /// Returns the magnitude
        /// </summary>
        public float Magnitude()
        {
            return (float)Math.Sqrt(MagnitudeSquared());
        }

        /// <summary>
        ///Returns the angle formed by the Vector and the x axis 
        /// </summary>
        public float GetAngle()
        {
            if (X == 0)
            {
                if (Y > 0)
                    return (float)(Math.PI / 2);
                if (Y < 0)
                    return (float)(Math.PI * 3 / 2);
                return 0;
            }
            if (X > 0)
                return (float)Math.Atan(Y / X);
            return (float)(Math.Atan(Y / X) + Math.PI);

        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }
        /// <summary>
        /// Subtracts vector b from vector a.
        /// </summary>
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }
        /// <summary>
        /// Simple scalar multiplication of a vector.
        /// </summary>
        public static Vector operator *(Vector a, float b)
        {
            return new Vector(a.X * b, a.Y * b);
        }
        public static Vector operator *(float b, Vector a)
        {
            return new Vector(a.X * b, a.Y * b);
        }
        /// <summary>
        /// Simple scalar division of a vector.
        /// </summary>
        public static Vector operator /(Vector a, float b)
        {
            return new Vector(a.X / b, a.Y / b);
        }
        public static Vector operator /(float b, Vector a)
        {
            return new Vector(a.X / b, a.Y / b);
        }

        //Implicit conversion to xna
        public static implicit operator Vector2(Vector v)
        {
            return new Vector2(v.X, v.Y);
        }



    }
}
