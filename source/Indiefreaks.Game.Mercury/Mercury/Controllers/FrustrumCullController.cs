/*
 * Copyright © 2011 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
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
    /// Defines a controller that cancels an emitter trigger if the trigger position is outside of
    /// a view frustum.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class FrustumCullController : AbstractController
    {
        /// <summary>
        /// Gets or sets the view frustum.
        /// </summary>
        public BoundingFrustum ViewFrustum { get; set; }

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="existingInstance">An existing controller instance.</param>
        protected override AbstractController DeepCopy(AbstractController existingInstance)
        {
            var value = (existingInstance as FrustumCullController) ?? new FrustumCullController();

            value.ViewFrustum = this.ViewFrustum;

            return value;
        }

        /// <summary>
        /// Adds additional logic to an emitter trigger event.
        /// </summary>
        /// <param name="context">The trigger context.</param>
        public override void Process(ref TriggerContext context)
        {
            if (this.ViewFrustum.Contains(context.Position) != ContainmentType.Contains)
                context.Cancelled = true;
        }
    }
}