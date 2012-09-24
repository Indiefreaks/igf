/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Controllers
{
    using System;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a controller which adds an random offset to the trigger
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class TriggerRandomOffsetController : AbstractController
    {
        /// <summary>
        /// Gets or sets the radii of the ellipsoid that the trigger will be offset inside
        /// </summary>
        public Vector3 Size { get; set; }

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="existingInstance">An existing controller instance.</param>
        protected override AbstractController DeepCopy(AbstractController existingInstance)
        {
            var value = (existingInstance as TriggerRandomOffsetController) ?? new TriggerRandomOffsetController();

            value.Size = this.Size;

            return value;
        }

        /// <summary>
        /// Adds additional logic to an emitter trigger event.
        /// </summary>
        /// <param name="context">The trigger context.</param>
        public override void Process(ref TriggerContext context)
        {
            Vector3 offset = RandomUtil.NextUnitVector3() * this.Size;
            
            context.Position.X += offset.X;
            context.Position.Y += offset.Y;
            context.Position.Z += offset.Z;
        }
    }
}