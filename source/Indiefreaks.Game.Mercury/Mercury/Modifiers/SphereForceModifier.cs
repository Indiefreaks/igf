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
    /// Defines a modifier which applies a force vector to particles when they enter a spherical area.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class SphereForceModifier : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the position of the force area.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the radius of the sphere.
        /// </summary>
        public Single Radius { get; set; }

        /// <summary>
        /// Gets or sets the force vector.
        /// </summary>
        public Vector3 ForceVector { get; set; }

        /// <summary>
        /// Gets or sets the strength of the force.
        /// </summary>
        public Single Strength { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractModifier which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new SphereForceModifier
            {
                Position = this.Position,
                Radius = this.Radius,
                ForceVector = this.ForceVector,
                Strength = this.Strength
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
            Single squareRadius = (this.Radius * this.Radius);

            Single deltaStrength = this.Strength * deltaSeconds;

            Single deltaForceX = this.ForceVector.X * deltaStrength;
            Single deltaForceY = this.ForceVector.Y * deltaStrength;
            Single deltaForceZ = this.ForceVector.Z * deltaStrength;

            Vector3 forcePosition = this.Position;

            var particle = iterator.First;

            do
            {
#if UNSAFE
                Vector3 position = particle->Position;
#else
                Vector3 position = particle.Position;
#endif
                Single distanceX = forcePosition.X - position.X;
                Single distanceY = forcePosition.Y - position.Y;
                Single distanceZ = forcePosition.Z - position.Z;

                Single squareDistance = ((distanceX * distanceX) + (distanceY * distanceY) + (distanceZ * distanceZ));

                if (squareDistance < squareRadius)
                {
#if UNSAFE
                    particle->Velocity.X += deltaForceX;
                    particle->Velocity.Y += deltaForceY;
                    particle->Velocity.Z += deltaForceZ;
#else
                    particle.Velocity.X += deltaForceX;
                    particle.Velocity.Y += deltaForceY;
                    particle.Velocity.Z += deltaForceZ;
#endif
                }
            }
#if UNSAFE
            while (iterator.MoveNext(&particle));
#else
            while (iterator.MoveNext(ref particle));
#endif
        }
    }
}