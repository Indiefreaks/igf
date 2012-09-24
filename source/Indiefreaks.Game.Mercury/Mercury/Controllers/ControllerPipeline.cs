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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Defines a controller pipeline.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2229:ImplementSerializationConstructors")]
    [Serializable]
    public sealed class ControllerPipeline : List<AbstractController>, ISupportDeepCopy<ControllerPipeline>
    {
        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of ControllerPipeline which is a copy of this instance.</returns>
        public ControllerPipeline DeepCopy()
        {
            var copy = new ControllerPipeline();

            foreach (var controller in this)
                copy.Add(controller.DeepCopy());

            return copy;
        }

        public void Update(Single deltaSeconds)
        {
            foreach (var controller in this)
                controller.Update(deltaSeconds);
        }

        /// <summary>
        /// Adds additional logic to an emitter trigger event.
        /// </summary>
        /// <param name="context">A reference to the trigger context.</param>
        public void Process(ref TriggerContext context)
        {
            foreach (var controller in this)
            {
                controller.Process(ref context);

                if (context.Cancelled)
                    break;
            }
        }
    }
}