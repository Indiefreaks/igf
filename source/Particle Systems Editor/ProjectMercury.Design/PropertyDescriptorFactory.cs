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
    using System.Reflection;

    /// <summary>
    /// Defines a factory class for building property descriptor objects.
    /// </summary>
    static internal class PropertyDescriptorFactory
    {
        /// <summary>
        /// Creates a property descriptor object for the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="attributes">The attributes which should be applied.</param>
        /// <returns>An object which derives from System.ComponentModel.PropertyDescriptor.</returns>
        static public PropertyDescriptor Create(MemberInfo member, params Attribute[] attributes)
        {
            if (member.MemberType == MemberTypes.Field)
                return new FieldPropertyDescriptor(member as FieldInfo, attributes);

            if (member.MemberType == MemberTypes.Property)
                return new PropertyPropertyDescriptor(member as PropertyInfo, attributes);

            throw new ArgumentException("member must be either a field or a property.");
        }
    }
}