/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Renderers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Xna.Framework.Graphics;
    using ProjectMercury.Emitters;

    /// <summary>
    /// Defines a factory class to create blend states.
    /// </summary>
    static internal class BlendStateFactory
    {
        /// <summary>
        /// Initialises the <see cref="BlendStateFactory"/> class.
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        static BlendStateFactory()
        {
#if WINDOWS_PHONE
            BlendStateFactory.Alpha = BlendState.NonPremultiplied;
#else
            BlendStateFactory.Alpha = new BlendState
            {
                AlphaBlendFunction    = BlendFunction.Add,
                AlphaSourceBlend      = Blend.SourceAlpha,
                AlphaDestinationBlend = Blend.InverseSourceAlpha,
                ColorBlendFunction    = BlendFunction.Add,
                ColorSourceBlend      = Blend.SourceAlpha,
                ColorDestinationBlend = Blend.InverseSourceAlpha,
            };
#endif
#if WINDOWS_PHONE
            BlendStateFactory.Add = BlendState.Additive;
#else
            BlendStateFactory.Add = new BlendState
            {
                AlphaBlendFunction    = BlendFunction.Add,
                AlphaSourceBlend      = Blend.SourceAlpha,
                AlphaDestinationBlend = Blend.InverseSourceAlpha,
                ColorBlendFunction    = BlendFunction.Add,
                ColorSourceBlend      = Blend.SourceAlpha,
                ColorDestinationBlend = Blend.One
            };
#endif
#if !WINDOWS_PHONE
            BlendStateFactory.Screen = new BlendState
            {
                AlphaBlendFunction    = BlendFunction.Add,
                AlphaSourceBlend      = Blend.SourceAlpha,
                AlphaDestinationBlend = Blend.InverseSourceAlpha,
                ColorBlendFunction    = BlendFunction.Add,
                ColorSourceBlend      = Blend.InverseDestinationColor,
                ColorDestinationBlend = Blend.One
            };

            BlendStateFactory.Subtract = new BlendState
            {
                AlphaBlendFunction    = BlendFunction.Add,
                AlphaSourceBlend      = Blend.SourceAlpha,
                AlphaDestinationBlend = Blend.InverseSourceAlpha,
                ColorBlendFunction    = BlendFunction.ReverseSubtract,
                ColorSourceBlend      = Blend.SourceAlpha,
                ColorDestinationBlend = Blend.One
            };

            BlendStateFactory.Compare = new BlendState
            {
                AlphaBlendFunction    = BlendFunction.Add,
                AlphaSourceBlend      = Blend.SourceAlpha,
                AlphaDestinationBlend = Blend.InverseSourceAlpha,
                ColorBlendFunction    = BlendFunction.Max,
                ColorSourceBlend      = Blend.One,
                ColorDestinationBlend = Blend.One,
            };

            BlendStateFactory.Contrast = new BlendState
            {
                AlphaBlendFunction    = BlendFunction.Add,
                AlphaSourceBlend      = Blend.SourceAlpha,
                AlphaDestinationBlend = Blend.InverseSourceAlpha,
                ColorBlendFunction    = BlendFunction.Add,
                ColorSourceBlend      = Blend.DestinationColor,
                ColorDestinationBlend = Blend.One,
            };
#endif
        }

        /// <summary>
        /// Gets or sets a blend state which represents alpha blending.
        /// </summary>
        private static BlendState Alpha;

        /// <summary>
        /// Gets or sets a blend state which represents additive blending.
        /// </summary>
        private static BlendState Add;
#if !WINDOWS_PHONE
        /// <summary>
        /// Gets or sets a blend state which represents screen blending.
        /// </summary>
        private static BlendState Screen;

        /// <summary>
        /// Gets or sets a blend state which represents subtractive blending.
        /// </summary>
        private static BlendState Subtract;

        /// <summary>
        /// Gets or sets a blend state which represents compare blending.
        /// </summary>
        private static BlendState Compare;

        /// <summary>
        /// Gets or sets a blend state which represents contrast blending.
        /// </summary>
        private static BlendState Contrast;
#endif
        /// <summary>
        /// Gets a blend state which corresponds to the specified emitter blend mode.
        /// </summary>
        /// <param name="blendMode">The blend mode of the emitter.</param>
        /// <returns>A blend state instance.</returns>
        static public BlendState GetBlendState(EmitterBlendMode blendMode)
        {
            switch (blendMode)
            {
                case EmitterBlendMode.Add:
                    return BlendStateFactory.Add;

                case EmitterBlendMode.Alpha:
                    return BlendStateFactory.Alpha;
#if !WINDOWS_PHONE
                case EmitterBlendMode.Compare:
                    return BlendStateFactory.Compare;

                case EmitterBlendMode.Contrast:
                    return BlendStateFactory.Contrast;

                case EmitterBlendMode.Screen:
                    return BlendStateFactory.Screen;

                case EmitterBlendMode.Subtract:
                    return BlendStateFactory.Subtract;
#endif
                default:
                    throw new InvalidEnumArgumentException("blendMode", (Int32)blendMode, typeof(EmitterBlendMode));
            }
        }
    }
}