/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

#if XBOX || WINDOWS_PHONE
namespace System
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Indicates that a class can be serialized. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class, Inherited = false)]
    [ComVisible(true)]
    public sealed class SerializableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the System.SerializableAttribute class.
        /// </summary>
        public SerializableAttribute()
        {
            throw new NotImplementedException();
        }
    }
}
#endif