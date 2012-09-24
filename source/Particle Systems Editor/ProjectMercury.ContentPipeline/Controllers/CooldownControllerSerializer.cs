﻿/*
 * Copyright © 2011 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
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
    using ProjectMercury.Controllers;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing CooldownController objects.
    /// </summary>
    [ContentTypeSerializer]
    public sealed class CooldownControllerSerializer : ContentTypeSerializer<CooldownController>
    {
        /// <summary>
        /// Serializes an object to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override void Serialize(IntermediateWriter output, CooldownController value, ContentSerializerAttribute format)
        {
            output.WriteObject("CooldownPeriod", value.CooldownPeriod);
        }

        /// <summary>
        /// Deserializes a CooldownController object from intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        /// <returns>A deserialized CooldownController instance.</returns>
        protected override CooldownController Deserialize(IntermediateReader input, ContentSerializerAttribute format, CooldownController existingInstance)
        {
            CooldownController value = existingInstance ?? new CooldownController();

            value.CooldownPeriod = input.ReadObject<Single>("CooldownPeriod");

            return value;
        }
    }
}