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
    /// Defines a particle emitter which releases particles from within a box.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class BoxEmitter : AbstractEmitter
    {
        /// <summary>
        /// Gets or sets the width of the box.
        /// </summary>
        public Single Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the box.
        /// </summary>
        public Single Height { get; set; }

        /// <summary>
        /// Gets or sets the depth of the box.
        /// </summary>
        public Single Depth { get; set; }

        /// <summary>
        /// Gets or sets the rotation vector of the box.
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="exisitingInstance">An existing emitter instance.</param>
        protected override AbstractEmitter DeepCopy(AbstractEmitter exisitingInstance)
        {
            BoxEmitter value = (exisitingInstance as BoxEmitter) ?? new BoxEmitter();

            value.Width    = this.Width;
            value.Height   = this.Height;
            value.Depth    = this.Depth;
            value.Rotation = this.Rotation;

            base.DeepCopy(value);

            return value;
        }

        /// <summary>
        /// Generates offset and force vectors for a newly released particle.
        /// </summary>
        /// <param name="offset">Defines an offset vector from the trigger position.</param>
        /// <param name="force">A unit vector defining the inital force applied to the particle.</param>
        protected override void GenerateOffsetAndForce(out Vector3 offset, out Vector3 force)
        {
            force = RandomUtil.NextUnitVector3();

            offset = new Vector3
            {
                X = RandomUtil.NextSingle(this.Width  * -0.5f, this.Width  * 0.5f),
                Y = RandomUtil.NextSingle(this.Height * -0.5f, this.Height * 0.5f),
                Z = RandomUtil.NextSingle(this.Depth  * -0.5f, this.Depth  * 0.5f)
            };

            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(this.Rotation.X, this.Rotation.Y, this.Rotation.Z);

            Vector3.Transform(ref offset, ref rotationMatrix, out offset);
        }
    }
}