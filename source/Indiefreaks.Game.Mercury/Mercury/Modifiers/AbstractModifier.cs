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

    /// <summary>
    /// Defines the abstract base class for a modifier.
    /// </summary>
    public abstract class AbstractModifier : ISupportDeepCopy<AbstractModifier>
    {
        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractModifier which is a copy of this instance.</returns>
        public abstract AbstractModifier DeepCopy();

        /// <summary>
        /// Processes active particles.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        /// <param name="iterator">A particle iterator object.</param>
#if UNSAFE
        protected internal unsafe abstract void Process(Single deltaSeconds, ref ParticleIterator iterator);
#else
        protected internal abstract void Process(Single deltaSeconds, ref ParticleIterator iterator);
#endif
    }
}