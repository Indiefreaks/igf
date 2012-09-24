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
    /// Provides extension methods for the IntermediateReader class.
    /// </summary>
    internal static class IntermediateReaderExtensions
    {
        /// <summary>
        /// Reads a single object from the input XML stream.
        /// </summary>
        /// <typeparam name="T">The type of object to read.</typeparam>
        /// <param name="instance">Extension instance.</param>
        /// <param name="elementName">The name of the XML node.</param>
        /// <returns>The deserialized object.</returns>
        static public T ReadObject<T>(this IntermediateReader instance, string elementName)
        {
            return instance.ReadObject<T>(new ContentSerializerAttribute { ElementName = elementName });
        }

        /// <summary>
        /// Reads a single object from the input XML stream.
        /// </summary>
        /// <typeparam name="T">The type of object to read.</typeparam>
        /// <param name="instance">Extension instance.</param>
        /// <param name="elementName">The name of the XML node.</param>
        /// <param name="collectionItemName">The name of child elements if <typeparamref name="T"/> is a collection type.</param>
        /// <returns>The deserialized object.</returns>
        static public T ReadObject<T>(this IntermediateReader instance, string elementName, string collectionItemName)
        {
            return instance.ReadObject<T>(new ContentSerializerAttribute { ElementName = elementName, CollectionItemName = collectionItemName });
        }
    }
}
