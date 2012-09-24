/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Encapsulates iteration over a particle ring buffer.
    /// </summary>
#if UNSAFE
    public unsafe struct ParticleIterator
#else
    public struct ParticleIterator
#endif
    {
        /// <summary>
        /// Holds a reference to the ring buffer.
        /// </summary>
#if UNSAFE
        private readonly Particle* Buffer;
#else
        private readonly Particle[] Buffer;
#endif
        /// <summary>
        /// Holds the size of the ring buffer.
        /// </summary>
        private readonly Int32 Size;

        /// <summary>
        /// Holds the index at which the iteration started.
        /// </summary>
        private readonly Int32 StartIndex;

        /// <summary>
        /// Holds the total number of iterations to make.
        /// </summary>
        private readonly Int32 Count;
#if UNSAFE
        /// <summary>
        /// Gets the first particle in the iteration.
        /// </summary>
        public readonly Particle* First;
#else
        public Particle First;
#endif
        /// <summary>
        /// Holds the current iteration over the ring buffer.
        /// </summary>
        private Int32 CurrentIteration;

        /// <summary>
        /// Initialises a new instance of the <see cref="ParticleIterator"/> structure.
        /// </summary>
        /// <param name="buffer">A reference to a ring buffer of particles.</param>
        /// <param name="size">The size of the ring buffer.</param>
        /// <param name="startIndex">The index of the first item in the ring buffer.</param>
        /// <param name="count">The total number of active items.</param>
#if UNSAFE
        internal ParticleIterator(Particle* buffer, Int32 size, Int32 startIndex, Int32 count)
#else
        internal ParticleIterator(Particle[] buffer, Int32 size, Int32 startIndex, Int32 count)
#endif
        {
            this.Buffer            = buffer;
            this.Size              = size;
            this.StartIndex        = startIndex;
            this.Count             = count;
            this.CurrentIteration  = 0;
#if UNSAFE
            this.First             = this.Buffer + (startIndex);
#else
            this.First             = this.Buffer[startIndex];
#endif
        }

        /// <summary>
        /// Moves the specified particle to the next particle in the iteration.
        /// </summary>
        /// <param name="particle">The particle pointer or reference to increment.</param>
        /// <returns>True if the iteration has finished, else false.</returns>
#if UNSAFE
        public Boolean MoveNext(Particle** particle)
#else
        public Boolean MoveNext(ref Particle particle)
#endif
        {
#if !UNSAFE
            this.Buffer[(this.StartIndex + this.CurrentIteration) % this.Size] = particle;
            if (this.CurrentIteration == 0)
            {
                First = particle;
            }
#endif
            this.CurrentIteration++;

            if (this.CurrentIteration > (this.Count - 1))
                return false;
#if UNSAFE
            (*particle) = this.Buffer + ((this.StartIndex + this.CurrentIteration) % this.Size);
#else
            particle = this.Buffer[(this.StartIndex + this.CurrentIteration) % this.Size];
#endif
            return true;
        }

        /// <summary>
        /// Resets the particle iterator back to the start of the ring buffer.
        /// </summary>
        public void Reset()
        {
            this.CurrentIteration = 0;
        }
    }
}