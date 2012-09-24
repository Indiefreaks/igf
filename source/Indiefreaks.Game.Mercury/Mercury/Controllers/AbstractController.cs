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

    /// <summary>
    /// Defines the abstract base class for a particle effect controller.
    /// </summary>
    public abstract class AbstractController : ISupportDeepCopy<AbstractController>
    {
        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of AbstractController which is a copy of this instance.</returns>
        public AbstractController DeepCopy()
        {
            return this.DeepCopy(null);
        }

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="existingInstance">An existing controller instance.</param>
        protected abstract AbstractController DeepCopy(AbstractController existingInstance);

        /// <summary>
        /// Enables the controller to run update logic on each update of the emitter.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        public virtual void Update(Single deltaSeconds)
        {
        }

        /// <summary>
        /// Adds additional logic to an emitter trigger event.
        /// </summary>
        /// <param name="context">A reference to the trigger context.</param>
        public abstract void Process(ref TriggerContext context);
    }
}