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
    using ProjectMercury.Emitters;

    /// <summary>
    /// Defines an emitter which releases particles from within a cylinder.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public class CylinderEmitter : CircleEmitter
    {
        /// <summary>
        /// Gets or sets the height of the cylinder.
        /// </summary>
        public Single Height { get; set; }
 
        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="exisitingInstance">An existing emitter instance.</param>
        protected override AbstractEmitter DeepCopy(AbstractEmitter exisitingInstance)
        {
            CylinderEmitter value = (exisitingInstance as CylinderEmitter) ?? new CylinderEmitter();

            value.Height = this.Height;

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
            // A cylinder is a circle with a height!
            base.GenerateOffsetAndForce(out offset, out force);

            offset.Z = RandomUtil.NextSingle(-this.Height*0.5f, this.Height*0.5f);
        }
    }
}