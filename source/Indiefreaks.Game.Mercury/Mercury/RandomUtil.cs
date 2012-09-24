/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a utility class for choosing random numbers or performing random operations.
    /// </summary>
    static public class RandomUtil
    {
        /// <summary>
        /// Initialises the <see cref="RandomUtil"/> class.
        /// </summary>
        static RandomUtil()
        {
            RandomUtil._random = new Random();
        }

        /// <summary>
        /// Gets or sets the random number generator.
        /// </summary>
        private static Random _random;

        /// <summary>
        /// Returns a random floating point value between zero and one inclusive.
        /// </summary>
        /// <returns>A random floating point number between zero and one inclusive.</returns>
        static public Single NextSingle()
        {
            return (Single)RandomUtil._random.NextDouble();
        }

        /// <summary>
        /// Returns a random floating point value within the specified range.
        /// </summary>
        /// <param name="min">The inclusive minimum value of the random number returned.</param>
        /// <param name="max">The inclusive maximum value of the random number returned.</param>
        /// <returns>A random floating point value within the specified range.</returns>
        static public Single NextSingle(Single min, Single max)
        {
            return ((max - min) * RandomUtil.NextSingle()) + min;
        }

        /// <summary>
        /// Returns a random floating point value within the specified range.
        /// </summary>
        /// <param name="range">A range value defining the minimum and maximum values.</param>
        /// <returns>A random floating point value within the specified range.</returns>
        static public Single NextSingle(Range range)
        {
            return ((range.Maximum - range.Minimum) * RandomUtil.NextSingle()) + range.Minimum;
        }

        /// <summary>
        /// Returns a random colour vector within the specified colour range.
        /// </summary>
        /// <param name="range">A colour range value defining the minimum and maximum values.</param>
        /// <returns>A random colour vector within the specified colour range.</returns>
        static public Vector3 NextColour(ColourRange range)
        {
            return new Vector3
            {
                X = RandomUtil.NextSingle(range.Red),
                Y = RandomUtil.NextSingle(range.Green),
                Z = RandomUtil.NextSingle(range.Blue)
            };
        }

        /// <summary>
        /// Returns a random rotation vector within the specified rotation range.
        /// </summary>
        /// <param name="range">A rotation range value defining the minimum and maximum values.</param>
        /// <returns>A random rotation vector within the specified rotation range.</returns>
        static public Vector3 NextRotation(RotationRange range)
        {
            return new Vector3
            {
                X = RandomUtil.NextSingle(range.Pitch),
                Y = RandomUtil.NextSingle(range.Yaw),
                Z = RandomUtil.NextSingle(range.Roll)
            };
        }

        /// <summary>
        /// Returns a random three dimensional unit vector.
        /// </summary>
        /// <returns>A random three dimensional unit vector.</returns>
        static public Vector3 NextUnitVector3()
        {
            //Algorithm documented here http://www.cgafaq.info/wiki/Random_Points_On_Sphere
            Single radians = RandomUtil.NextSingle(-Calculator.Pi, Calculator.Pi);

            Single z = RandomUtil.NextSingle(-1f, 1f);

            Single t = Calculator.Sqrt(1f - (z * z));

            Vector2 planar = new Vector2
            {
                X = Calculator.Cos(radians) * t,
                Y = Calculator.Sin(radians) * t
            };

            return new Vector3
            {
                X = planar.X,
                Y = planar.Y,
                Z = z
            };
        }

        
        /// <summary>
        /// Returns a random boolean value.
        /// </summary>
        static public bool NextBool()
        {
            return _random.Next(2) == 1;
        }
    }
}