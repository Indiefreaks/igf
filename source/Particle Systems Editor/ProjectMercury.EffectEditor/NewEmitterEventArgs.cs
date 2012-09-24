/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using ProjectMercury.PluginContracts;
    using ProjectMercury.Emitters;

    internal class NewEmitterEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewEmitterEventArgs"/> class.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="budget">The budget.</param>
        /// <param name="term">The term.</param>
        public NewEmitterEventArgs(IEmitterPlugin plugin, int budget, float term)
            : base()
        {
            this.Plugin = plugin;
            this.Budget = budget;
            this.Term = term;
        }

        /// <summary>
        /// Gets or sets the plugin which was selected to create the emitter.
        /// </summary>
        /// <value>The plugin.</value>
        public IEmitterPlugin Plugin { get; private set; }

        /// <summary>
        /// Gets or sets the required budget.
        /// </summary>
        /// <value>The budget.</value>
        public int Budget { get; private set; }

        /// <summary>
        /// Gets or sets the required term.
        /// </summary>
        /// <value>The term.</value>
        public float Term { get; private set; }

        /// <summary>
        /// Gets or sets the emitter which was created.
        /// </summary>
        /// <value>The created emitter.</value>
        public AbstractEmitter AddedEmitter { get; set; }
    }
}