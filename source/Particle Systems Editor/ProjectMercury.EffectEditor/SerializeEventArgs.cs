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

    internal class SerializeEventArgs : CoreOperationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializeEventArgs"/> class.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="filePath">The file path.</param>
        public SerializeEventArgs(ISerializerPlugin plugin, String filePath)
            : base()
        {
            this.Plugin = plugin;
            this.FilePath = filePath;
        }

        public ISerializerPlugin Plugin { get; private set; }

        public String FilePath { get; private set; }
    }
}