using System;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Extensions
{
    public static class RandomExtensions
    {
        public static float GetRandomFloat(Random dice, float min, float max)
        {
            return min + (float) dice.NextDouble()*(max - min);
        }

        public static float GetRandomFloat(Random dice)
        {
            return GetRandomFloat(dice, -1.0f, 1.0f);
        }

        #region Random Sphere Math Helper Methods
        /// <summary>
        /// Generates a random Vector3 point in a Polar Coordinate System.
        /// </summary>
        /// <param name="dice">The Random instance to be used when generating data.</param>
        /// <returns>Returns a randomly generated Polar Coordinate System based Vector3 instance.</returns>
        static private Vector3 GetRandomPolarCoordinates(Random dice)
        {
            double x, y, z, w, t;

            z = 2.0 * dice.NextDouble() - 1.0;
            t = 2.0 * MathHelper.Pi * dice.NextDouble();
            w = Math.Sqrt(1 - z * z);
            x = w * Math.Cos(t);
            y = w * Math.Sin(t);

            return new Vector3((float)x, (float)y, (float)z);
        }

        /// <summary>
        /// Generates a random Vector3 on the surface of a sphere.
        /// </summary>
        /// <param name="dice">The Random instance to be used when generating data.</param>
        /// <param name="radius">The radius of the Sphere.</param>
        /// <returns>Returns a randomly generated Vector3 positionned on the surface of a Sphere.</returns>
        static public Vector3 GetRandomPointOnSphere(Random dice, float radius)
        {
            Vector3 randomPolarCoordinates = RandomExtensions.GetRandomPolarCoordinates(dice);

            return Vector3.Multiply(randomPolarCoordinates, radius);
        }

        /// <summary>
        /// Generates a random Vector3 positionned inside a sphere.
        /// </summary>
        /// <param name="dice">The Random instance to be used when generating data.</param>
        /// <param name="maxDistanceFromOrigin">The maximum distance from origin the point can be generated. Usually, the radius of a sphere.</param>
        /// <returns>Returns a randomly generated Vector3 positionned inside a sphere of a given maximum radius.</returns>
        static public Vector3 GetRandomPointInSphere(Random dice, int maxDistanceFromOrigin)
        {
            Vector3 randomPolarCoordinates = RandomExtensions.GetRandomPolarCoordinates(dice);

            return randomPolarCoordinates * dice.Next(0, maxDistanceFromOrigin);
        }


        /// <summary>
        /// Generates a random Vector3 positionned inside a cube.
        /// </summary>
        /// <param name="dice">The Random instance to be used when generating data.</param>
        /// <param name="maxDistanceFromCenter">The maximum distance from center the point can be generated</param>
        /// <returns>Returns a randomly generated Vector3 positionned inside a cube.</returns>
        static public Vector3 GetRandomPointInCube(Random dice, int maxDistanceFromCenter)
        {
            return new Vector3(
            (float)(((dice.NextDouble() * 2) - 1) * maxDistanceFromCenter),
            (float)(((dice.NextDouble() * 2) - 1) * maxDistanceFromCenter),
            (float)(((dice.NextDouble() * 2) - 1) * maxDistanceFromCenter));
        }
        #endregion
    }
}