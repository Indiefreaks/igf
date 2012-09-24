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
    /// Defines a modifier which applies a force vector to particles when they enter an axis aligned box area.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class BoxForceModifier : AbstractModifier
    {
        /// <summary>
        /// Gets or sets the position of the centre of the force area.
        /// </summary>
        public Vector3 Position { get; set; }

        private Single HalfWidth;

        /// <summary>
        /// Gets or sets the width of the force area.
        /// </summary>
        public Single Width
        {
            get { return this.HalfWidth * 2f; }
            set
            {
                Check.ArgumentFinite("Width", value);
                Check.ArgumentNotLessThan("Width", value, 0f);

                this.HalfWidth = value * 0.5f;
            }
        }

        private Single HalfHeight;

        /// <summary>
        /// Gets or sets the height of the force area.
        /// </summary>
        public Single Height
        {
            get { return this.HalfHeight * 2f; }
            set
            {
                Check.ArgumentFinite("Height", value);
                Check.ArgumentNotLessThan("Height", value, 0f);

                this.HalfHeight = value * 0.5f;
            }
        }

        private Single HalfDepth;

        /// <summary>
        /// Gets or sets the depth of the force area.
        /// </summary>
        public Single Depth
        {
            get { return this.HalfDepth * 2f; }
            set
            {
                Check.ArgumentFinite("Depth", value);
                Check.ArgumentNotLessThan("Depth", value, 0f);

                this.HalfDepth = value * 0.5f;
            }
        }

        /// <summary>
        /// Gets or sets the force vector.
        /// </summary>
        public Vector3 Force { get; set; }

        /// <summary>
        /// Gets or sets the strength of the force.
        /// </summary>
        public Single Strength { get; set; }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of BoxForceModifier which is a copy of this instance.</returns>
        public override AbstractModifier DeepCopy()
        {
            return new BoxForceModifier
            {
                Depth    = this.Depth,
                Force    = this.Force,
                Height   = this.Height,
                Position = this.Position,
                Strength = this.Strength,
                Width    = this.Width
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
            Single deltaStrength = this.Strength * deltaSeconds;

            Single deltaForceX = this.Force.X * deltaStrength;
            Single deltaForceY = this.Force.Y * deltaStrength;
            Single deltaForceZ = this.Force.Z * deltaStrength;

            var particle = iterator.First;

            do
            {
#if UNSAFE
                Vector3 position = particle->Position;
#else
                Vector3 position = particle.Position;
#endif

                if (position.X > (this.Position.X - this.HalfWidth))
                    if (position.X < (this.Position.X + this.HalfWidth))
                        if (position.Y > (this.Position.Y - this.HalfHeight))
                            if (position.Y < (this.Position.Y + this.HalfHeight))
                                if (position.Z > (this.Position.Z - this.HalfDepth))
                                    if (position.Z < (this.Position.Z + this.HalfDepth))
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