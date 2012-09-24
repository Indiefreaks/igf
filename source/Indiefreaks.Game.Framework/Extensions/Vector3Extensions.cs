using System;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Extensions
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        public static Vector3 Truncate(Vector3 value, float max)
        {
            float maxSquared = max * max;
            float vectorSquared = value.LengthSquared();

            if (vectorSquared <= maxSquared)
                return value;
            else
                return value * (max / (float)Math.Sqrt(vectorSquared));
        }

        public static Vector3 Extend(Vector3 value, float min)
        {
            float minSquared = min*min;
            float vectorSquared = value.LengthSquared();

            if (vectorSquared >= minSquared || vectorSquared == 0)
                return value;
            else
                return value*(min/(float) Math.Sqrt(vectorSquared));
        }

        public static Vector3 Perpendicular(Vector3 vector, Vector3 axis)
        {
            return (vector - Parallel(vector, axis));
        }

        public static Vector3 Parallel(Vector3 vector, Vector3 axis)
        {
            float projection = Vector3.Dot(vector, axis);
            return axis * projection;
        }
    }
}