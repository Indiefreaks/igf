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

    internal class CloneEmitterEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloneEmitterEventArgs"/> class.
        /// </summary>
        /// <param name="prototype">The prototype.</param>
        public CloneEmitterEventArgs(AbstractEmitter prototype)
            : base()
        {
            this.Prototype = prototype;
        }

        /// <summary>
        /// The prototype Emitter, the emitter which needs to be cloned.
        /// </summary>
        public AbstractEmitter Prototype { get; private set; }

        /// <summary>
        /// The clone emitter, set by the event handler.
        /// </summary>
        public AbstractEmitter AddedEmitter { get; set; }
    }
}