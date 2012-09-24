/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Emitters
{
    /// <summary>
    /// Defines the possible blending modes for a particle emitter.
    /// </summary>
    public enum EmitterBlendMode
    {
        /// <summary>
        /// Alpha blending.
        /// </summary>
        Alpha,

        /// <summary>
        /// Additive blending.
        /// </summary>
        Add,
#if !WINDOWS_PHONE
        /// <summary>
        /// Screen blending.
        /// </summary>
        Screen,

        /// <summary>
        /// Subtractive blending.
        /// </summary>
        Subtract,

        /// <summary>
        /// Compare blending.
        /// </summary>
        Compare,

        /// <summary>
        /// Contrast blending.
        /// </summary>
        Contrast,
#endif
        /// <summary>
        /// No blending.
        /// </summary>
        None
    }
}