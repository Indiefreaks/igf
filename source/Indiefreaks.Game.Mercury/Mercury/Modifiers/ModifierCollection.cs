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
    using System.Collections.Generic;

    /// <summary>
    /// Defines a collection of modifiers.
    /// </summary>
    public sealed class ModifierCollection : List<AbstractModifier>, ISupportDeepCopy<ModifierCollection>
    {
        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of ModifierCollection which is a copy of this instance.</returns>
        public ModifierCollection DeepCopy()
        {
            ModifierCollection copy = new ModifierCollection();

            foreach (AbstractModifier modifier in this)
                copy.Add(modifier.DeepCopy());

            return copy;
        }

        /// <summary>
        /// Invokes the Process method of all modifiers in the collection.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        /// <param name="iterator">A pointer to a particle array iterator.</param>
        internal void RunProcessors(Single deltaSeconds, ref ParticleIterator iterator)
        {
            for (Int32 i = 0; i < this.Count; i++)
            {
                this[i].Process(deltaSeconds, ref iterator);

                iterator.Reset();
            }
        }
    }
}