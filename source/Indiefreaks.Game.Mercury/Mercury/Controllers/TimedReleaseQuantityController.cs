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
    /// Defines a controller which adjusts the release quantity of a trigger to represent particles
    /// released per second.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class TimedReleaseQuantityController : AbstractController
    {
        /// <summary>
        /// Hold the most recent frame time in whole and fractional seconds.
        /// </summary>
        private Single _totalTime;


        /// <summary>
        /// The time of the last trigger, one by proxy
        /// </summary>
        private Dictionary<int, float> _lastTime = new Dictionary<int, float>();

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="existingInstance">An existing controller instance.</param>
        protected override AbstractController DeepCopy(AbstractController existingInstance)
        {
            var value = (existingInstance as TimedReleaseQuantityController) ?? new TimedReleaseQuantityController();

            return value;
        }

        /// <summary>
        /// Enables the controller to run update logic on each update of the emitter.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        public override void Update(Single deltaSeconds)
        {
            _totalTime += deltaSeconds;
        }

        /// <summary>
        /// Adds additional logic to an emitter trigger event.
        /// </summary>
        /// <param name="context">The trigger context.</param>
        public override void Process(ref TriggerContext context)
        {
            float lastTime;
            int releaseQuantity = 0;
            if(_lastTime.TryGetValue(context.ProxyIndex,out lastTime))
            {
                float elaspsed = _totalTime - lastTime;
                float fracQuantity = context.ReleaseQuantity * elaspsed;

                if ((int)fracQuantity <= context.ReleaseQuantity)
                {
                    releaseQuantity = (int)fracQuantity;
                    lastTime = _totalTime - (fracQuantity - releaseQuantity) / context.ReleaseQuantity;
                }
                else
                    lastTime = _totalTime;

                _lastTime[context.ProxyIndex] = lastTime;
            }
            else
                _lastTime[context.ProxyIndex] = lastTime; 

            context.ReleaseQuantity = releaseQuantity;
        }
    }
}