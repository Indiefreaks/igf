/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using ProjectMercury.Emitters;

    internal class TextureReferenceChangedEventArgs : EmitterEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureReferenceChangedEventArgs"/> class.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        /// <param name="textureReference">The texture reference.</param>
        public TextureReferenceChangedEventArgs(AbstractEmitter emitter, TextureReference textureReference)
            : base(emitter)
        {
            this.TextureReference = textureReference;
        }

        /// <summary>
        /// Gets or sets the texture reference.
        /// </summary>
        /// <value>The texture reference.</value>
        public TextureReference TextureReference { get; private set; }
    };
}