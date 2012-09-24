/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.ContentPipeline.Emitters
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury.Emitters;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing BoxEmitter objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class BoxEmitterSerializer : AbstractEmitterSerializer<BoxEmitter>
    {
        /// <summary>
        /// Serializes the derived fields.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="value">The value.</param>
        /// <param name="format">The format.</param>
        protected override void SerializeDerivedFields(IntermediateWriter output, BoxEmitter value, ContentSerializerAttribute format)
        {
            output.WriteObject("Width", value.Width);
            output.WriteObject("Height", value.Height);
            output.WriteObject("Depth", value.Depth);
            output.WriteObject("Rotation", value.Rotation);
        }

        /// <summary>
        /// Deserializes the derived fields.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="format">The format.</param>
        /// <param name="existingInstance">The existing instance.</param>
        protected override void DeserializeDerivedFields(IntermediateReader input, ContentSerializerAttribute format, BoxEmitter existingInstance)
        {
            existingInstance.Width = input.ReadObject<Single>("Width");
            existingInstance.Height = input.ReadObject<Single>("Height");
            existingInstance.Depth = input.ReadObject<Single>("Depth");
            existingInstance.Rotation = input.ReadObject<Vector3>("Rotation");
        }
    }
}