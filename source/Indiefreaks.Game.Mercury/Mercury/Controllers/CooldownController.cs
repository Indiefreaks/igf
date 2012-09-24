/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

using System.Collections.Generic;

namespace ProjectMercury.Controllers
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Defines a controller which adds a minimum trigger period to an emitter.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class CooldownController : AbstractController
    {
        /// <summary>
        /// Total elapsed seconds.
        /// </summary>
        private Single TotalSeconds;

        /// <summary>
        /// The time of the last allowed trigger, one trigger by proxy
        /// </summary>
        private Dictionary<int, float> LastTriggers = new Dictionary<int, float>();

        /// <summary>
        /// The cooldown period after the emitter has been triggered.
        /// </summary>
        public Single CooldownPeriod { get; set; }

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="existingInstance">An existing controller instance.</param>
        protected override AbstractController DeepCopy(AbstractController existingInstance)
        {
            var value = (existingInstance as CooldownController) ?? new CooldownController();

            value.CooldownPeriod = this.CooldownPeriod;

            return value;
        }

        /// <summary>
        /// Enables the controller to run update logic on each update of the emitter.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        public override void Update(Single deltaSeconds)
        {
            this.TotalSeconds += deltaSeconds;
        }

        /// <summary>
        /// Adds additional logic to an emitter trigger event.
        /// </summary>
        /// <param name="context">The trigger context.</param>
        public override void Process(ref TriggerContext context)
        {
            float lastTrigger;
            if(LastTriggers.TryGetValue(context.ProxyIndex,out lastTrigger))
            {
                if((TotalSeconds - lastTrigger) < CooldownPeriod)
                {
                    context.Cancelled = true;
                }
                else
                {
                    LastTriggers[context.ProxyIndex] = TotalSeconds;
                }
            }
            else
            {
                LastTriggers[context.ProxyIndex] = TotalSeconds;
            }
        }
    }
}