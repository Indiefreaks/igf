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
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines the particle structure.
    /// </summary>
    public struct Particle
    {
        /// <summary>
        /// Represents the position of the particle in 3D space.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Represents the current velocity of the particle in 3D space.
        /// </summary>
        public Vector3 Velocity;

        /// <summary>
        /// Represents the rotation of the particle around all three axes.
        /// </summary>
        public Vector3 Rotation;

        /// <summary>
        /// Represents the colour of the particle.
        /// </summary>
        public Vector4 Colour;

        /// <summary>
        /// Represents the scale of the particle.
        /// </summary>
        public Single Scale;

        /// <summary>
        /// Contains the time at which the particle was released.
        /// </summary>
        public Single Inception;

        /// <summary>
        /// Contains the current age of the particle in the range [0,1].
        /// </summary>
        public Single Age;

        /// <summary>
        /// A reference to the effect proxy that gives us per instance data
        /// </summary>
        public int EffectProxyIndex;  //You cannot reference a managed type in this struct otherwise you can't take the address/use it in unsafe
    }
}