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
    /// Defines a modifier which interpolates the opacity of particles over the course of their lifetime.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class OpacityInterpolator2 : AbstractModifier
    {
        /// <summary>
        /// The initial opacity of particles as they are released.
        /// </summary>
        public Single InitialOpacity { get; set; }

        /// <summary>
        /// The final opacity of particles as they are retired.
        /// </summary>
        public Single FinalOpacity { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractModifier which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new OpacityInterpolator2
            {
                InitialOpacity = this.InitialOpacity,
                FinalOpacity = this.FinalOpacity
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
                particle->Colour.W = (this.InitialOpacity + ((this.FinalOpacity - this.InitialOpacity) * particle->Age));
#else
                particle.Colour.W = (this.InitialOpacity + ((this.FinalOpacity - this.InitialOpacity) * particle.Age));
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