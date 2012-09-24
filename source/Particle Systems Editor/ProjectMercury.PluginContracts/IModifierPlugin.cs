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
    using ProjectMercury.Modifiers;

    /// <summary>
    /// Defines the interface for a plugin which provides access to a Modifier.
    /// </summary>
    public interface IModifierPlugin : IPlugin
    {
        /// <summary>
        /// Gets the category of the modifier.
        /// </summary>
        String Category { get; }

        /// <summary>
        /// Creates an instance of the modifier type exposed by this plugin, and returns a reference to it.
        /// </summary>
        /// <returns>A reference to a new modifier instance.</returns>
        AbstractModifier ConstructInstance();
    }
}