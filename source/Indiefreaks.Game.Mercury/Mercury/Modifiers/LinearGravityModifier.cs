/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Modifiers
{
    using System;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a modifier which applies a linear gravity force to particles.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class LinearGravityModifier : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the gravity direction as a unit vector.
        /// </summary>
        public Vector3 GravityVector { get; set; }

        /// <summary>
        /// Gets or sets the strength of gravity.
        /// </summary>
        public Single Strength { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractModifier which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new LinearGravityModifier
            {
                GravityVector = this.GravityVector,
                Strength = this.Strength
            };
        }

        /// <summary>
        /// Processes active particles.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        /// <param name="iterator">A particle iterator object.</param>
#if UNSAFE
        protected internal override unsafe void Process(Single deltaSeconds, ref ParticleIterator iterator)
#else
        protected internal override void Process(Single deltaSeconds, ref ParticleIterator iterator)
#endif
        {
            Single deltaStrength = this.Strength * deltaSeconds;

            Single deltaGravityX = this.GravityVector.X * deltaStrength;
            Single deltaGravityY = this.GravityVector.Y * deltaStrength;
            Single deltaGravityZ = this.GravityVector.Z * deltaStrength;

            var particle = iterator.First;

            do
            {
#if UNSAFE
                particle->Velocity.X += deltaGravityX;
                particle->Velocity.Y += deltaGravityY;
                particle->Velocity.Z += deltaGravityZ;
#else
                particle.Velocity.X += deltaGravityX;
                particle.Velocity.Y += deltaGravityY;
                particle.Velocity.Z += deltaGravityZ;
#endif
            }
#if UNSAFE
            while (iterator.MoveNext(&particle));
#else
            while (iterator.MoveNext(ref particle));
#endif
        }
    }
}