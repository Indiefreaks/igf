/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using ProjectMercury.Controllers;

    internal class CloneControllerEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloneControllerEventArgs"/> class.
        /// </summary>
        /// <param name="prototype">The prototype.</param>
        public CloneControllerEventArgs(AbstractController prototype)
            : base()
        {
            this.Prototype = prototype;
        }

        /// <summary>
        /// Gets or sets the prototype controller.
        /// </summary>
        /// <value>The prototype.</value>
        public AbstractController Prototype { get; private set; }

        /// <summary>
        /// Gets or sets the controller which was cloned from the prototype.
        /// </summary>
        /// <value>The cloned controller.</value>
        public AbstractController AddedController { get; set; }
    }
}