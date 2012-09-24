/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.ContentPipeline
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

    /// <summary>
    /// Provides extension methods for the IntermediateWriter class.
    /// </summary>
    internal static class IntermediateWriterExtensions
    {
        /// <summary>
        /// Writes a single object to the output XML stream.
        /// </summary>
        /// <typeparam name="T">The type of object to write.</typeparam>
        /// <param name="instance">Extension instance.</param>
        /// <param name="elementName">The name of the XML element.</param>
        /// <param name="value">The value to serialize.</param>
        static public void WriteObject<T>(this IntermediateWriter instance, string elementName, T value)
        {
            instance.WriteObject<T>(value, new ContentSerializerAttribute { ElementName = elementName });
        }

        /// <summary>
        /// Writes a single object to the output XML stream.
        /// </summary>
        /// <typeparam name="T">The type of object to write.</typeparam>
        /// <param name="instance">Extension instance.</param>
        /// <param name="elementName">The name of the XML element.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="collectionItemName">The name of child elements if <typeparamref name="T"/> is a collection type.</param>
        static public void WriteObject<T>(this IntermediateWriter instance, string elementName, T value, string collectionItemName)
        {
            instance.WriteObject<T>(value, new ContentSerializerAttribute { ElementName = elementName, CollectionItemName = collectionItemName });
        }
    }
}
