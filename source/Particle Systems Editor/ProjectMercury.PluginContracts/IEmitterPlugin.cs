/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.PluginContracts
{
    using ProjectMercury.Emitters;

    /// <summary>
    /// Defines the interface for a plugin which provides access to an Emitter.
    /// </summary>
    public interface IEmitterPlugin : IPlugin
    {
        /// <summary>
        /// Constructs an instance of the emitter type exposed by this plugin, and returns a reference to it.
        /// </summary>
        /// <returns>A reference to a new emitter instance.</returns>
        AbstractEmitter ConstructInstance();
    }
}