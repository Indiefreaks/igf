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
    using System.Globalization;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury;
    using ProjectMercury.Emitters;
    using ProjectMercury.Modifiers;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing ColourInterpolator3 objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class ColourInterpolator3Serializer : ContentTypeSerializer<ColourInterpolator3>
    {
        /// <summary>
        /// Serializes an object to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void Serialize(IntermediateWriter output, ColourInterpolator3 value, ContentSerializerAttribute format)
        {
            output.WriteObject("InitialColour", value.InitialColour);
            output.WriteObject("Median",        value.Median);
            output.WriteObject("MedianColour",  value.MedianColour);
            output.WriteObject("FinalColour",   value.FinalColour);
        }

        /// <summary>
        /// Deserializes a ColourInterpolator3 object from intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        /// <returns>A deserialized ColourInterpolator3 instance.</returns>
        protected override ColourInterpolator3 Deserialize(IntermediateReader input, ContentSerializerAttribute format, ColourInterpolator3 existingInstance)
        {
            ColourInterpolator3 value = existingInstance ?? new ColourInterpolator3();

            value.InitialColour = input.ReadObject<Vector3>("InitialColour");
            value.Median        = input.ReadObject<Single> ("Median");
            value.MedianColour  = input.ReadObject<Vector3>("MedianColour");
            value.FinalColour   = input.ReadObject<Vector3>("FinalColour");

            return value;
        }
    }
}