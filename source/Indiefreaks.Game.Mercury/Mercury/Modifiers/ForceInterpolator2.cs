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
    /// Defines a modifier which interpolates between two force vectors and applies the resultant force to particles.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class ForceInterpolator2 : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the initial force vector applied to particles as they are released.
        /// </summary>
        public Vector3 InitialForce { get; set; }

        /// <summary>
        /// Gets or sets the final force vector applied to particles as they are retired.
        /// </summary>
        public Vector3 FinalForce { get; set; }

        // todo: Should there be a seperate "Strength" value for each force, or a global strength value
        //       that affects them both?

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractModifier which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new ForceInterpolator2
            {
                InitialForce = this.InitialForce,
                FinalForce = this.FinalForce
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
            Single initialForceDeltaX = this.InitialForce.X * deltaSeconds;
            Single initialForceDeltaY = this.InitialForce.Y * deltaSeconds;
            Single initialForceDeltaZ = this.InitialForce.Z * deltaSeconds;

            Single finalForceDeltaX = this.FinalForce.X * deltaSeconds;
            Single finalForceDeltaY = this.FinalForce.Y * deltaSeconds;
            Single finalForceDeltaZ = this.FinalForce.Z * deltaSeconds;

            var particle = iterator.First;

            do
            {
#if UNSAFE
                particle->Velocity.X += initialForceDeltaX + ((finalForceDeltaX - initialForceDeltaX) * particle->Age);
                particle->Velocity.Y += initialForceDeltaY + ((finalForceDeltaY - initialForceDeltaY) * particle->Age);
                particle->Velocity.Z += initialForceDeltaZ + ((finalForceDeltaZ - initialForceDeltaZ) * particle->Age);
#else
                particle.Velocity.X += initialForceDeltaX + ((finalForceDeltaX - initialForceDeltaX) * particle.Age);
                particle.Velocity.Y += initialForceDeltaY + ((finalForceDeltaY - initialForceDeltaY) * particle.Age);
                particle.Velocity.Z += initialForceDeltaZ + ((finalForceDeltaZ - initialForceDeltaZ) * particle.Age);
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