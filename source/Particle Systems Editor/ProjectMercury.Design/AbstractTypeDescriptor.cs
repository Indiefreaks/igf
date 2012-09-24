/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Design
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Defines the abstract base class for a type descriptor.
    /// </summary>
    public abstract class AbstractTypeDescriptor<T> : CustomTypeDescriptor where T : class
    {
        private readonly Type Type = typeof(T);

        /// <summary>
        /// Returns the fully qualified name of the class represented by this type descriptor.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing the fully qualified class name of the type this type descriptor is describing. The default is null.
        /// </returns>
        public override string GetClassName()
        {
            return Type.FullName;
        }

        /// <summary>
        /// Returns the name of the class represented by this type descriptor.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing the name of the component instance this type descriptor is describing. The default is null.
        /// </returns>
        public override string GetComponentName()
        {
            return Type.Name;
        }
    }
}