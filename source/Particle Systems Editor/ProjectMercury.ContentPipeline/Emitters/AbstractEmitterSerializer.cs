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
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
    using ProjectMercury.Controllers;
    using ProjectMercury.Emitters;
    using ProjectMercury.Modifiers;

    /// <summary>
    /// Provides an implementation of ContentTypeSerializer methods and properties for serializing
    /// and deserializing <typeparamref name="T"/> objects.
    /// </summary>
    public abstract class AbstractEmitterSerializer<T> : ContentTypeSerializer<T> where T : AbstractEmitter, new()
    {
        /// <summary>
        /// Serializes an object to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected override sealed void Serialize(IntermediateWriter output, T value, ContentSerializerAttribute format)
        {
            output.WriteObject("Name",                     value.Name);
            output.WriteObject("Budget",                   value.Budget);
            output.WriteObject("Term",                     value.Term);
            output.WriteObject("ReleaseQuantity",          value.ReleaseQuantity);
            output.WriteObject("Enabled",                  value.Enabled);
            output.WriteObject("ReleaseSpeed",             value.ReleaseSpeed);
            output.WriteObject("ReleaseColour",            value.ReleaseColour);
            output.WriteObject("ReleaseOpacity",           value.ReleaseOpacity);
            output.WriteObject("ReleaseScale",             value.ReleaseScale);
            output.WriteObject("ReleaseRotation",          value.ReleaseRotation);
            output.WriteObject("ParticleTextureAssetPath", value.ParticleTextureAssetPath);
            output.WriteObject("BlendMode",                value.BlendMode);

            this.SerializeDerivedFields(output, value, format);

            output.WriteObject("Modifiers", value.Modifiers, "Modifier");
            output.WriteObject("Controllers", value.Controllers, "Controller");
        }

        /// <summary>
        /// Serializes only the fields of the derived emitter class to intermediate XML format.
        /// </summary>
        /// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
        /// <param name="value">The strongly typed object to be serialized.</param>
        /// <param name="format">Specifies the content format for this object.</param>
        protected abstract void SerializeDerivedFields(IntermediateWriter output, T value, ContentSerializerAttribute format);

        /// <summary>
        /// Deserializes a <typeparamref name="T"/> object from intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        /// <returns>A deserialized <typeparamref name="T"/> instance.</returns>
        protected override sealed T Deserialize(IntermediateReader input, ContentSerializerAttribute format, T existingInstance)
        {
            T value = existingInstance ?? new T();

            value.Name                     = input.ReadObject<String>          ("Name");
            value.Budget                   = input.ReadObject<Int32>           ("Budget");
            value.Term                     = input.ReadObject<Single>          ("Term");
            value.ReleaseQuantity          = input.ReadObject<Int32>           ("ReleaseQuantity");
            value.Enabled                  = input.ReadObject<Boolean>         ("Enabled");
            value.ReleaseSpeed             = input.ReadObject<Range>           ("ReleaseSpeed");
            value.ReleaseColour            = input.ReadObject<ColourRange>     ("ReleaseColour");
            value.ReleaseOpacity           = input.ReadObject<Range>           ("ReleaseOpacity");
            value.ReleaseScale             = input.ReadObject<Range>           ("ReleaseScale");
            value.ReleaseRotation          = input.ReadObject<RotationRange>   ("ReleaseRotation");
            value.ParticleTextureAssetPath = input.ReadObject<String>          ("ParticleTextureAssetPath");
            value.BlendMode                = input.ReadObject<EmitterBlendMode>("BlendMode");

            this.DeserializeDerivedFields(input, format, value);

            value.Modifiers = input.ReadObject<ModifierCollection>("Modifiers", "Modifier");
            value.Controllers = input.ReadObject<ControllerPipeline>("Controllers", "Controller");

            return value;
        }

        /// <summary>
        /// Serializes only the fields of the derived emitter class to intermediate XML format.
        /// </summary>
        /// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
        /// <param name="format">Specifies the intermediate source XML format.</param>
        /// <param name="existingInstance">The strongly typed object containing the received data, or null if the
        /// deserializer should construct a new instance.</param>
        protected abstract void DeserializeDerivedFields(IntermediateReader input, ContentSerializerAttribute format, T existingInstance);
    }
}