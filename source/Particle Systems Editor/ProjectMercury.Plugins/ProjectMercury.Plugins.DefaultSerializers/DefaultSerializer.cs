/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Plugins.DefaultSerializers
{
    using System;
    using System.ComponentModel.Composition;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury.PluginContracts;

    /// <summary>
    /// Provides a default particle effect serializer plugin.
    /// </summary>
    [Export(typeof(ISerializerPlugin))]
    public sealed class DefaultSerializer : ISerializerPlugin
    {
        /// <summary>
        /// Gets the file filter to use in the file dialog when the plugin is selected.
        /// </summary>
        /// <value></value>
        /// <remarks>ie: "Particle Effect Files|*.pfx"</remarks>
        public String FileFilter
        {
            get { return "Intermediate Xml (*.xml)|*.xml"; }
        }

        /// <summary>
        /// Gets a value indicating wether or not the plugin is able to serialize particle effects.
        /// </summary>
        /// <value></value>
        public Boolean CanSerialize
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating wether or not the plugin is able to deserialize particle effects.
        /// </summary>
        /// <value></value>
        public Boolean CanDeserialize
        {
            get { return true; }
        }

        /// <summary>
        /// Serializes the specified particle effect to the specified file path.
        /// </summary>
        /// <param name="particleEffect">The particle effect to be serialized.</param>
        /// <param name="filePath">The path to the desired output file.</param>
        public void Serialize(ParticleEffect particleEffect, string filePath)
        {
            XDocument xmlDocument = new XDocument();

            using (XmlWriter writer = xmlDocument.CreateWriter())
            {
                IntermediateSerializer.Serialize<ParticleEffect>(writer, particleEffect, ".\\");
            }

            xmlDocument.Save(filePath);
        }

        /// <summary>
        /// Deserializes a particle effect from the specified file path.
        /// </summary>
        /// <param name="filePath">The path to the desired input file.</param>
        /// <returns>A new instance of a particle effect.</returns>
        public ParticleEffect Deserialize(string filePath)
        {
            XDocument xmlDocument = XDocument.Load(filePath);

            using (XmlReader reader = xmlDocument.CreateReader())
            {
                return IntermediateSerializer.Deserialize<ParticleEffect>(reader, ".\\");
            }
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value></value>
        public String Name
        {
            get { return "Default Serializer"; }
        }

        /// <summary>
        /// Gets a brief description of the plugin.
        /// </summary>
        /// <value></value>
        public String Description
        {
            get { return "Serializes particle effects using the default content pipeline."; }
        }

        /// <summary>
        /// Gets a display icon for the plugin.
        /// </summary>
        /// <value></value>
        public Uri DisplayIcon
        {
            get { return new Uri(""); }
        }

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        /// <value></value>
        public String Author
        {
            get { return "ProjectMercury Team Members"; }
        }

        /// <summary>
        /// Gets the version number of the plugin.
        /// </summary>
        /// <value></value>
        public Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        /// <summary>
        /// Gets the minimum version of the engine with which the plugin is compatible.
        /// </summary>
        /// <value></value>
        public Version RequiredEngineVersion
        {
            get { return new Version(4, 0, 0, 0); }
        }
    }
}