/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.ContentPipeline.Emitters
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury.Emitters;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing PointEmitter objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class PointEmitterSerializer : AbstractEmitterSerializer<PointEmitter>
    {
        /// <summary>
        /// Serializes only the fields of the derived emitter class to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void SerializeDerivedFields(IntermediateWriter output, PointEmitter value, ContentSerializerAttribute format)
        {
            // PointEmitter has no additional fields from AbstractEmitter...

            return;
        }

        /// <summary>
        /// Serializes only the fields of the derived emitter class to intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        protected override void DeserializeDerivedFields(IntermediateReader input, ContentSerializerAttribute format, PointEmitter existingInstance)
        {
            // PointEmitter has no additional fields from AbstractEmitter...

            return;
        }
    }
}