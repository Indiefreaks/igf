/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using ProjectMercury.Modifiers;

    internal class ModifierEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifierEventArgs"/> class.
        /// </summary>
        /// <param name="modifier">The modifier.</param>
        public ModifierEventArgs(AbstractModifier modifier)
            : base()
        {
            this.Modifier = modifier;
        }

        /// <summary>
        /// Gets or sets the modifier.
        /// </summary>
        /// <value>The modifier.</value>
        public AbstractModifier Modifier { get; private set; }
    }
}