/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Emitters
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of Emitters
    /// </summary>
    public sealed class EmitterCollection : List<AbstractEmitter>, ISupportDeepCopy<EmitterCollection>
    {
        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of EmitterCollection which is a copy of this instance.</returns>
        public EmitterCollection DeepCopy()
        {
            EmitterCollection copy = new EmitterCollection();

            foreach (AbstractEmitter emitter in this)
                copy.Add(emitter.DeepCopy());

            return copy;
        }

        public void Terminate()
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Terminate();
            }
        }

        /// <summary>
        /// Gets a reference to the emitter with the specified name.
        /// </summary>
        /// <param name="name">The name of the emitter.</param>
        /// <returns>A reference to the emitter with the specified name.</returns>
        public AbstractEmitter this[String name]
        {
            get
            {
                for (Int32 i = 0; i < this.Count; i++)
                    if (String.Equals(this[i].Name, name))
                        return this[i];

                throw new KeyNotFoundException();
            }
        }
    }
}