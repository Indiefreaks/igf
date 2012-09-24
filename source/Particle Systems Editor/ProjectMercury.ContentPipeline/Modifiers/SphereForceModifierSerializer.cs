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
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury.Modifiers;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing SphereForceModifier objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class SphereForceModifierSerializer : ContentTypeSerializer<SphereForceModifier>
    {
        /// <summary>
        /// Serializes an object to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void Serialize(IntermediateWriter output, SphereForceModifier value, ContentSerializerAttribute format)
        {
            output.WriteObject("Position",    value.Position);
            output.WriteObject("Radius",      value.Radius);
            output.WriteObject("ForceVector", value.ForceVector);
            output.WriteObject("Strength",    value.Strength);
        }

        /// <summary>
        /// Deserializes a SphereForceModifier object from intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        /// <returns>A deserialized SphereForceModifier instance.</returns>
        protected override SphereForceModifier Deserialize(IntermediateReader input, ContentSerializerAttribute format, SphereForceModifier existingInstance)
        {
            SphereForceModifier value = existingInstance ?? new SphereForceModifier();

            value.Position    = input.ReadObject<Vector3>("Position");
            value.Radius      = input.ReadObject<Single> ("Radius");
            value.ForceVector = input.ReadObject<Vector3>("ForceVector");
            value.Strength    = input.ReadObject<Single> ("Strength");

            return value;
        }
    }
}