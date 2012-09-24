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
    /// Defines a modifier which changes the colour of particles by adjusting the hue.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class HueShiftModifier : AbstractModifier
    {
        /// <summary>
        /// Transforms RGB colours in YIQ space.
        /// </summary>
        static private Matrix YiqTransform = new Matrix(0.299f, 0.587f, 0.114f, 0.000f,
                                                                 0.596f, -.274f, -.321f, 0.000f,
                                                                 0.211f, -.523f, 0.311f, 0.000f,
                                                                 0.000f, 0.000f, 0.000f, 1.000f);
        /// <summary>
        /// Transforms YIQ colours in RGB space.
        /// </summary>
        static private Matrix RgbTransform = Matrix.Invert(HueShiftModifier.YiqTransform);

        /// <summary>
        /// Gets or sets the hue shift in degrees per second.
        /// </summary>
        public Single HueShift { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of HueShiftModifier which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new HueShiftModifier
            {
                HueShift = this.HueShift
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
            Single h = ((this.HueShift * deltaSeconds) * Calculator.Pi) / 180f;

            Single u = Calculator.Cos(h);
            Single w = Calculator.Sin(h);

            Matrix hueTransform = new Matrix(1f, 0f, 0f, 0f,
                                             0f, u,  -w, 0f,
                                             0f, w,   u, 0f,
                                             0f, 0f, 0f, 1f);

            var particle = iterator.First;

#if UNSAFE
            Vector4 colour = particle->Colour;
#else
            Vector4 colour = particle.Colour;
#endif
            do
            {
                // Convert the current colour of the particle to YIQ colour space...
                Vector4.Transform(ref colour, ref HueShiftModifier.YiqTransform, out colour);

                // Transform the colour in YIQ space...
                Vector4.Transform(ref colour, ref hueTransform, out colour);

                // Convert the colour back to RGB space...
                Vector4.Transform(ref colour, ref HueShiftModifier.RgbTransform, out colour);

#if UNSAFE
                particle->Colour.X = colour.X;
                particle->Colour.Y = colour.Y;
                particle->Colour.Z = colour.Z;
#else
                particle.Colour.X = colour.X;
                particle.Colour.Y = colour.Y;
                particle.Colour.Z = colour.Z;
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