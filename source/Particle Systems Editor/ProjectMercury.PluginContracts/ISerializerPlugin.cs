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
    /// Defines the interface for a plugin which provides particle effect serialization.
    /// </summary>
    public interface ISerializerPlugin : IPlugin
    {
        /// <summary>
        /// Gets the file filter to use in the file dialog when the plugin is selected.
        /// </summary>
        /// <remarks>ie: "Particle Effect Files|*.pfx"</remarks>
        String FileFilter { get; }

        /// <summary>
        /// Gets a value indicating wether or not the plugin is able to serialize particle effects.
        /// </summary>
        Boolean CanSerialize { get; }

        /// <summary>
        /// Gets a value indicating wether or not the plugin is able to deserialize particle effects.
        /// </summary>
        Boolean CanDeserialize { get; }

        /// <summary>
        /// Serializes the specified particle effect to the specified file path.
        /// </summary>
        /// <param name="particleEffect">The particle effect to be serialized.</param>
        /// <param name="filePath">The path to the desired output file.</param>
        void Serialize(ParticleEffect particleEffect, string filePath);

        /// <summary>
        /// Deserializes a particle effect from the specified file path.
        /// </summary>
        /// <param name="filePath">The path to the desired input file.</param>
        /// <returns>A new instance of a particle effect.</returns>
        ParticleEffect Deserialize(string filePath);
    }
}