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
    using ProjectMercury.Modifiers;

    internal class NewModifierEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewModifierEventArgs"/> class.
        /// </summary>
        /// <param name="parentEmitter">The parent emitter.</param>
        /// <param name="plugin">The plugin.</param>
        public NewModifierEventArgs(AbstractEmitter parentEmitter, IModifierPlugin plugin)
            : base()
        {
            this.ParentEmitter = parentEmitter;
            this.Plugin = plugin;
        }

        /// <summary>
        /// Gets or sets the parent emitter.
        /// </summary>
        /// <value>The parent emitter.</value>
        public AbstractEmitter ParentEmitter { get; private set; }

        /// <summary>
        /// Gets or sets the plugin which was selected to create the modifier.
        /// </summary>
        /// <value>The plugin.</value>
        public IModifierPlugin Plugin { get; private set; }

        /// <summary>
        /// Gets or sets the modifier which was created.
        /// </summary>
        /// <value>The created modifier.</value>
        public AbstractModifier AddedModifier { get; set; }
    };
}