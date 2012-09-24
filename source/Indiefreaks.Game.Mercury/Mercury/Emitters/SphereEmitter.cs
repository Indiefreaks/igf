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
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an emitter which release particles from within a sphere.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public class SphereEmitter : AbstractEmitter
    {
        private Single _radius;

        /// <summary>
        /// Gets or sets the radius of the sphere.
        /// </summary>
        public Single Radius
        {
            get { return this._radius; }
            set
            {
                Check.ArgumentFinite("Radius", value);
                Check.ArgumentNotLessThan("Radius", value, 0f);

                this._radius = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether particles should be released only on the edge of the sphere.
        /// </summary>
        public Boolean Shell { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether particles should radiate out from the centre of the sphere.
        /// </summary>
        public Boolean Radiate { get; set; }

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="exisitingInstance">An existing emitter instance.</param>
        protected override AbstractEmitter DeepCopy(AbstractEmitter exisitingInstance)
        {
            SphereEmitter value = (exisitingInstance as SphereEmitter) ?? new SphereEmitter();

            value.Radiate = this.Radiate;
            value.Radius  = this.Radius;
            value.Shell   = this.Shell;

            base.DeepCopy(value);

            return value;
        }

        /// <summary>
        /// Generates offset and force vectors for a newly released particle.
        /// </summary>
        /// <param name="offset">Defines an offset vector from the trigger position.</param>
        /// <param name="force">A unit vector defining the initial force applied to the particle.</param>
        protected override void GenerateOffsetAndForce(out Vector3 offset, out Vector3 force)
        {
            Vector3 angle = RandomUtil.NextUnitVector3();

            Single radiusMultiplier = this.Shell ? this.Radius : this.Radius * RandomUtil.NextSingle();

            offset = new Vector3
            {
                X = angle.X * radiusMultiplier,
                Y = angle.Y * radiusMultiplier,
                Z = angle.Z * radiusMultiplier
            };

            force = this.Radiate ? angle : RandomUtil.NextUnitVector3();
        }
    }
}