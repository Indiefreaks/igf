/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.ContentPipeline.Modifiers
{
    using System;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury.Modifiers;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing VelocityClampModifier objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class VelocityClampModifierSerializer : ContentTypeSerializer<VelocityClampModifier>
    {
        /// <summary>
        /// Serializes an object to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void Serialize(IntermediateWriter output, VelocityClampModifier value, ContentSerializerAttribute format)
        {
            output.WriteObject("MaximumVelocity", value.MaximumVelocity);
        }

        /// <summary>
        /// Deserializes a VelocityClampModifier object from intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        /// <returns>A deserialized VelocityClampModifier instance.</returns>
        protected override VelocityClampModifier Deserialize(IntermediateReader input, ContentSerializerAttribute format, VelocityClampModifier existingInstance)
        {
            VelocityClampModifier value = existingInstance ?? new VelocityClampModifier();

            value.MaximumVelocity = input.ReadObject<Single>("MaximumVelocity");

            return value;
        }
    }
}