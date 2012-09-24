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

    internal class EmitterEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmitterEventArgs"/> class.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        public EmitterEventArgs(AbstractEmitter emitter) : base()
        {
            this.Emitter = emitter;
        }

        /// <summary>
        /// Gets or sets the emitter.
        /// </summary>
        /// <value>The emitter.</value>
        public AbstractEmitter Emitter { get; private set; }
    }
}