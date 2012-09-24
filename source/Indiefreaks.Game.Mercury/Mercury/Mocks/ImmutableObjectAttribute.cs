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
    using System;

    /// <summary>
    /// Specifies that an object has no subproperties capable of being edited. This class cannot be
    /// inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ImmutableObjectAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.ImmutableObjectAttribute class.
        /// </summary>
        /// <param name="immutable">true if the object is immutable; otherwise, false. </param>
        public ImmutableObjectAttribute(Boolean immutable)
            : base()
        {
            throw new NotImplementedException();
        }
    }
}
#endif