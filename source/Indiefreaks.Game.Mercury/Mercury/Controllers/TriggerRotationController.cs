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
    /// Defines a controller which rotates the particles and their forces.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class TriggerRotationController : AbstractController
    {
        /// <summary>
        /// Gets or sets the trigger rotation.
        /// </summary>
        public Vector3 TriggerRotation { get; set; }

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="existingInstance">An existing controller instance.</param>
        protected override AbstractController DeepCopy(AbstractController existingInstance)
        {
            var value = (existingInstance as TriggerRotationController) ?? new TriggerRotationController();

            value.TriggerRotation = this.TriggerRotation;

            return value;
        }

        /// <summary>
        /// Adds additional logic to an emitter trigger event.
        /// </summary>
        /// <param name="context">The trigger context.</param>
        public override void Process(ref TriggerContext context)
        {
            context.Rotation.X += this.TriggerRotation.X;
            context.Rotation.Y += this.TriggerRotation.Y;
            context.Rotation.Z += this.TriggerRotation.Z;
        }
    }
}