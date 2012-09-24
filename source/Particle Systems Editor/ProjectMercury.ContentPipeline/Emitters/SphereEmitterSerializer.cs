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
    using System.Globalization;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury.Emitters;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing SphereEmitter objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class SphereEmitterSerializer : AbstractEmitterSerializer<SphereEmitter>
    {
        /// <summary>
        /// Serializes only the fields of the derived emitter class to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void SerializeDerivedFields(IntermediateWriter output, SphereEmitter value, ContentSerializerAttribute format)
        {
            output.WriteObject("Radius",  value.Radius);
            output.WriteObject("Shell",   value.Shell);
            output.WriteObject("Radiate", value.Radiate);
        }

        /// <summary>
        /// Serializes only the fields of the derived emitter class to intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        protected override void DeserializeDerivedFields(IntermediateReader input, ContentSerializerAttribute format, SphereEmitter existingInstance)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (format == null)
                throw new ArgumentNullException("format");

            if (existingInstance == null)
                throw new ArgumentNullException("existingInstance");

            existingInstance.Radius  = input.ReadObject<Single> ("Radius");
            existingInstance.Shell   = input.ReadObject<Boolean>("Shell");
            existingInstance.Radiate = input.ReadObject<Boolean>("Radiate");
        }
    }
}