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
    /// Defines a modifier which adjusts the scale of particles over their lifetime.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class ScaleInterpolator3 : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the initial scale of particles when they are released.
        /// </summary>
        public Single InitialScale { get; set; }

        private Single _median;

        /// <summary>
        /// Gets or sets the point in a particles life where it becomes MedianScale.
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
        /// Gets or sets the median scale.
        /// </summary>
        public Single MedianScale { get; set; }

        /// <summary>
        /// Gets or sets the final scale of particles when they are retired.
        /// </summary>
        public Single FinalScale { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of ScaleInterpolator3 which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new ScaleInterpolator3
            {
                InitialScale = this.InitialScale,
                Median       = this.Median,
                MedianScale  = this.MedianScale,
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
                Single age = particle->Age;
#else
                Single age = particle.Age;
#endif
                if (age < this.Median)
#if UNSAFE
                    particle->Scale = this.InitialScale + ((this.MedianScale - this.InitialScale) * (age / this.Median));
#else
                    particle.Scale = this.InitialScale + ((this.MedianScale - this.InitialScale) * (age / this.Median));
#endif
                else
#if UNSAFE
                    particle->Scale = this.MedianScale + ((this.FinalScale - this.MedianScale) * ((age - this.Median) / (1f - this.Median)));
#else
                    particle.Scale = this.MedianScale + ((this.FinalScale - this.MedianScale) * ((age - this.Median) / (1f - this.Median)));
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