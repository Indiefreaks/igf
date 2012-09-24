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
    /// and deserializing ScaleInterpolator3 objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class ScaleInterpolator3Serializer : ContentTypeSerializer<ScaleInterpolator3>
    {
        /// <summary>
        /// Serializes an object to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void Serialize(IntermediateWriter output, ScaleInterpolator3 value, ContentSerializerAttribute format)
        {
            output.WriteObject("InitialScale", value.InitialScale);
            output.WriteObject("Median",       value.Median);
            output.WriteObject("MedianScale",  value.MedianScale);
            output.WriteObject("FinalScale",   value.FinalScale);
        }

        /// <summary>
        /// Deserializes a ScaleInterpolator3 object from intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        /// <returns>A deserialized ScaleInterpolator3 instance.</returns>
        protected override ScaleInterpolator3 Deserialize(IntermediateReader input, ContentSerializerAttribute format, ScaleInterpolator3 existingInstance)
        {
            ScaleInterpolator3 value = existingInstance ?? new ScaleInterpolator3();

            value.InitialScale = input.ReadObject<Single>("InitialScale");
            value.Median       = input.ReadObject<Single>("Median");
            value.MedianScale  = input.ReadObject<Single>("MedianScale");
            value.FinalScale   = input.ReadObject<Single>("FinalScale");

            return value;
        }
    }
}