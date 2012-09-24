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
    /// Defines a modifier which restricts the velocity of particles to a defined maximum.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class VelocityClampModifier : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the maximum velocity of particles.
        /// </summary>
        public Single MaximumVelocity { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractModifier which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new VelocityClampModifier
            {
                MaximumVelocity = this.MaximumVelocity
            };
        }

        /// <summary>
        /// Processes active particles.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        /// <param name="iterator">A particle iterator object.</param>
#if UNSAFE
        protected internal unsafe override void Process(Single deltaSeconds, ref ParticleIterator iterator)
#else
        protected internal override void Process(Single deltaSeconds, ref ParticleIterator iterator)
#endif
        {
            Single squareMaximumVelocity = (this.MaximumVelocity * this.MaximumVelocity);

            var particle = iterator.First;

            do
            {
#if UNSAFE
                Vector3 velocity = particle->Velocity;
#else
                Vector3 velocity = particle.Velocity;
#endif
                Single squareVelocity = ((velocity.X * velocity.X) +
                                         (velocity.Y * velocity.Y) +
                                         (velocity.Z * velocity.Z));

                if (squareVelocity > squareMaximumVelocity)
                {
                    Single actualVelocity = Calculator.Sqrt(squareVelocity);

#if UNSAFE
                    particle->Velocity.X = (particle->Velocity.X / actualVelocity) * this.MaximumVelocity;
                    particle->Velocity.Y = (particle->Velocity.Y / actualVelocity) * this.MaximumVelocity;
                    particle->Velocity.Z = (particle->Velocity.Z / actualVelocity) * this.MaximumVelocity;
#else
                    particle.Velocity.X = (particle.Velocity.X / actualVelocity) * this.MaximumVelocity;
                    particle.Velocity.Y = (particle.Velocity.Y / actualVelocity) * this.MaximumVelocity;
                    particle.Velocity.Z = (particle.Velocity.Z / actualVelocity) * this.MaximumVelocity;
#endif
                }
            }
#if UNSAFE
            while (iterator.MoveNext(&particle));
#else
            while (iterator.MoveNext(ref particle));
#endif
        }
    }
}