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
    public sealed class ColourInterpolator3 : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the initial colour of particles when they are released.
        /// </summary>
        public Vector3 InitialColour { get; set; }

        private Single _median;

        /// <summary>
        /// Gets or sets the point in a particles life where it becomes MedianColour.
        /// </summary>
        public Single Median
        {
            get { return this._median; }
            set
            {
                Check.ArgumentFinite("Median", value);
                Check.ArgumentWithinRange("Median", value, 0f, 1f);

                this._median = value;
            }
        }

        /// <summary>
        /// Gets or sets the colour of particles when they reach their defined median age.
        /// </summary>
        public Vector3 MedianColour { get; set; }

        /// <summary>
        /// Gets or sets the final colour of particles when they are retired.
        /// </summary>
        public Vector3 FinalColour { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of ColourInterpolator3 which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new ColourInterpolator3
            {
                InitialColour = this.InitialColour,
                Median        = this.Median,
                MedianColour  = this.MedianColour,
                FinalColour   = this.FinalColour
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
            Single w;
            Vector3 x, y;

            var particle = iterator.First;

            do
            {
#if UNSAFE
                Single age = particle->Age;
#else
                Single age = particle.Age;
#endif
                if (age < this.Median)
                {
                    x = this.InitialColour;
                    y = this.MedianColour;
                    w = age / this.Median;
                }
                else
                {
                    x = this.MedianColour;
                    y = this.FinalColour;
                    w = (age - this.Median) / (1f - this.Median);
                }
#if UNSAFE
                particle->Colour.X = x.X + ((y.X - x.X) * w);
                particle->Colour.Y = x.Y + ((y.Y - x.Y) * w);
                particle->Colour.Z = x.Z + ((y.Z - x.Z) * w);
#else
                particle.Colour.X = x.X + ((y.X - x.X) * w);
                particle.Colour.Y = x.Y + ((y.Y - x.Y) * w);
                particle.Colour.Z = x.Z + ((y.Z - x.Z) * w);
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