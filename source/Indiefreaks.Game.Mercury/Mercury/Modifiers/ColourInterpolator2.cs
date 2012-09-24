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
    /// Defines a modifier which interpolates the colour of a particle over the course of its lifetime.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class ColourInterpolator2 : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the initial colour of particles when they are released.
        /// </summary>
        public Vector3 InitialColour { get; set; }

        /// <summary>
        /// Gets or sets the final colour of particles when they are retired.
        /// </summary>
        public Vector3 FinalColour { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractModifier which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new ColourInterpolator2
            {
                InitialColour = this.InitialColour,
                FinalColour = this.FinalColour
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
            Vector3 initialColour = this.InitialColour;
            Vector3 delta   = this.FinalColour - initialColour;
            
            var particle = iterator.First;

            do
            {
#if UNSAFE
                particle->Colour.X = (initialColour.X + (delta.X * particle->Age));
                particle->Colour.Y = (initialColour.Y + (delta.Y * particle->Age));
                particle->Colour.Z = (initialColour.Z + (delta.Z * particle->Age));
#else
                particle.Colour.X = (initialColour.X + (delta.X * particle.Age));
                particle.Colour.Y = (initialColour.Y + (delta.Y * particle.Age));
                particle.Colour.Z = (initialColour.Z + (delta.Z * particle.Age));
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