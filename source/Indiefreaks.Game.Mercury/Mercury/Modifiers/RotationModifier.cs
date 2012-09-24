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
    /// Defines a modifier which rotates particles at a defined rate on each axis.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class RotationModifier : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the rotation rate, in radians, for each rotation axis.
        /// </summary>
        public Vector3 RotationRate { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractModifier which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new RotationModifier
            {
                RotationRate = this.RotationRate
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
            Single deltaPitch = this.RotationRate.X * deltaSeconds;
            Single deltaYaw   = this.RotationRate.Y * deltaSeconds;
            Single deltaRoll  = this.RotationRate.Z * deltaSeconds;

            var particle = iterator.First;

            do
            {
#if UNSAFE
                particle->Rotation.X += deltaPitch;
                particle->Rotation.Y += deltaYaw;
                particle->Rotation.Z += deltaRoll;
#else
                particle.Rotation.X += deltaPitch;
                particle.Rotation.Y += deltaYaw;
                particle.Rotation.Z += deltaRoll;
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