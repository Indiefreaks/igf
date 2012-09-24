/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.PluginContracts
{
    using System;

    /// <summary>
    /// Defines the interface for a plugin.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets a brief description of the plugin.
        /// </summary>
        String Description { get; }

        /// <summary>
        /// Gets a display icon for the plugin.
        /// </summary>
        Uri DisplayIcon { get; }

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        String Author { get; }

        /// <summary>
        /// Gets the version number of the plugin.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gets the minimum version of the engine provider with which the plugin is compatible.
        /// </summary>
        Version RequiredEngineVersion { get; }
    }
}