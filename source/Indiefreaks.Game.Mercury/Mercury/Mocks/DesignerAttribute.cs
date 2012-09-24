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
    using System.Diagnostics;

    /// <summary>
    /// Specifies the class used to implement design-time services for a component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class DesignerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.DesignerAttribute class using
        /// the name of the type that provides design-time services.
        /// </summary>
        /// <param name="designerTypeName">The concatenation of the fully qualified name of the
        /// type that provides design-time services for the component this attribute is bound to,
        /// and the name of the assembly this type resides in.</param>
        public DesignerAttribute(String designerTypeName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.DesignerAttribute class using
        /// the type that provides design-time services.
        /// </summary>
        /// <param name="designerType">A System.Type that represents the class that provides design
        /// time services for the component this attribute is bound to.</param>
        public DesignerAttribute(Type designerType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.DesignerAttribute class using
        /// the designer type and the base class for the designer.
        /// </summary>
        /// <param name="designerTypeName">The concatenation of the fully qualified name of the
        /// type that provides design-time services for the component this attribute is bound to,
        /// and the name of the assembly this type resides in.</param>
        /// <param name="designerBaseTypeName">The fully qualified name of the base class to
        /// associate with the designer class.</param>
        public DesignerAttribute(String designerTypeName, String designerBaseTypeName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.DesignerAttribute class,
        /// using the name of the designer class and the base class for the designer.
        /// </summary>
        /// <param name="designerTypeName">The concatenation of the fully qualified name of the
        /// type that provides design-time services for the component this attribute is bound to,
        /// and the name of the assembly this type resides in.</param>
        /// <param name="designerBaseType">A System.Type that represents the base class to
        /// associate with the designerTypeName.</param>
        public DesignerAttribute(String designerTypeName, Type designerBaseType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.DesignerAttribute class using
        /// the types of the designer and designer base class.
        /// </summary>
        /// <param name="designerType">A System.Type that represents the class that provides design
        /// time services for the component this attribute is bound to.</param>
        /// <param name="designerBaseType">A System.Type that represents the base class to
        /// associate with the designerType.</param>
        public DesignerAttribute(Type designerType, Type designerBaseType)
        {
            throw new NotImplementedException();
        }
    }
}
#endif