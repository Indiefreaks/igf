/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;

    internal class NewTextureReferenceEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewTextureReferenceEventArgs"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public NewTextureReferenceEventArgs(string filePath)
            : base()
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets or sets the added texture reference.
        /// </summary>
        /// <value>The added texture reference.</value>
        public TextureReference AddedTextureReference { get; set; }
    };
}