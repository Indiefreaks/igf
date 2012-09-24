/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using ProjectMercury.Emitters;

    internal class EmitterReinitialisedEventArgs : EmitterEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmitterReinitialisedEventArgs"/> class.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        /// <param name="budget">The new budget value.</param>
        /// <param name="term">The new term value.</param>
        public EmitterReinitialisedEventArgs(AbstractEmitter emitter, int budget, float term)
            : base(emitter)
        {
            this.Budget = budget;
            this.Term = term;
        }

        /// <summary>
        /// Gets or sets the new budget.
        /// </summary>
        /// <value>The budget.</value>
        public int Budget { get; private set; }

        /// <summary>
        /// Gets or sets the new term.
        /// </summary>
        /// <value>The term.</value>
        public float Term { get; private set; }
    }
}