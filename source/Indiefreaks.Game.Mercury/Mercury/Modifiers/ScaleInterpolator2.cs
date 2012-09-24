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

    /// <summary>
    /// Defines a modifier which adjusts the scale of a particle over its lifetime.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class ScaleInterpolator2 : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the initial scale of particles as they are released.
        /// </summary>
        public Single InitialScale { get; set; }

        /// <summary>
        /// Gets or sets the final scale of particles as they are retired.
        /// </summary>
        public Single FinalScale { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of ScaleInterpolator2 which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new ScaleInterpolator2
            {
                InitialScale = this.InitialScale,
                FinalScale   = this.FinalScale
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
            var particle = iterator.First;

            do
            {
#if UNSAFE
                particle->Scale = this.InitialScale + ((this.FinalScale - this.InitialScale) * particle->Age);
#else
                particle.Scale = this.InitialScale + ((this.FinalScale - this.InitialScale) * particle.Age);
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