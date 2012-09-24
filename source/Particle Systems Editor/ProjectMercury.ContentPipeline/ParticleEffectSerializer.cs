/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.ContentPipeline
{
    using System;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury.Emitters;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing ParticleEffect objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class ParticleEffectSerializer : ContentTypeSerializer<ParticleEffect>
    {
        /// <summary>
        /// Serializes an object to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void Serialize(IntermediateWriter output, ParticleEffect value, ContentSerializerAttribute format)
        {
            output.WriteObject("Name",        value.Name);
            output.WriteObject("Author",      value.Author);
            output.WriteObject("Description", value.Description);
            output.WriteObject("Emitters",    value.Emitters, "Item");
        }

        /// <summary>
        /// Deserializes a ParticleEffect object from intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        /// <returns>A deserialized ParticleEffect instance.</returns>
        protected override ParticleEffect Deserialize(IntermediateReader input, ContentSerializerAttribute format, ParticleEffect existingInstance)
        {
            ParticleEffect value = existingInstance ?? new ParticleEffect();

            value.Name        = input.ReadObject<String>           ("Name");
            value.Author      = input.ReadObject<String>           ("Author");
            value.Description = input.ReadObject<String>           ("Description");
            value.Emitters    = input.ReadObject<EmitterCollection>("Emitters", "Item");

            return value;
        }
    }
}