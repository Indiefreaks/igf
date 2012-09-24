/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

#if XBOX || WINDOWS_PHONE
namespace System.ComponentModel
{
    /// <summary>
    /// Specifies the custom type description provider for a class. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class TypeDescriptionProviderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDescriptionProviderAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public TypeDescriptionProviderAttribute(String type)
        {
            throw new NotImplementedException();
        }
    }
}
#endif