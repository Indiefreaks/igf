/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using System.IO;
    using Microsoft.Xna.Framework.Graphics;

    internal class TextureReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureReference"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public TextureReference(string filePath)
        {
            this.FilePath = filePath;

            using (FileStream inputStream = File.OpenRead(filePath))
            {
                this.Texture = Texture2D.FromStream(GraphicsDeviceService.Instance.GraphicsDevice, inputStream);
            }
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the name of the asset.
        /// </summary>
        /// <returns></returns>
        public string GetAssetName()
        {
            if (String.IsNullOrEmpty(this.FilePath))
                throw new InvalidOperationException();

            return Path.GetFileNameWithoutExtension(this.FilePath);
        }
    }
}