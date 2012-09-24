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

    internal class CloneModifierEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloneModifierEventArgs"/> class.
        /// </summary>
        /// <param name="prototype">The prototype.</param>
        public CloneModifierEventArgs(AbstractModifier prototype)
            : base()
        {
            this.Prototype = prototype;
        }

        /// <summary>
        /// Gets or sets the prototype modifier.
        /// </summary>
        /// <value>The prototype.</value>
        public AbstractModifier Prototype { get; private set; }

        /// <summary>
        /// Gets or sets the modifier which was cloned from the prototype.
        /// </summary>
        /// <value>The cloned modifier.</value>
        public AbstractModifier AddedModifier { get; set; }
    }
}