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
    /// Defines a property descriptor that works with properties.
    /// </summary>
    internal class PropertyPropertyDescriptor : MemberPropertyDescriptor
    {
        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>The property.</value>
        private PropertyInfo Property { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="attributes">The attributes.</param>
        public PropertyPropertyDescriptor(PropertyInfo property, params Attribute[] attributes)
            : base(property, attributes)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            this.Property = property;
        }

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property on a component.
        /// </summary>
        /// <param name="component">The component with the property for which to retrieve the value.</param>
        /// <returns>
        /// The value of a property for a given component.
        /// </returns>
        public override Object GetValue(Object component)
        {
            return this.Property.GetValue(component, null);
        }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override void SetValue(Object component, Object value)
        {
            this.Property.SetValue(component, value, null);
            
            this.OnValueChanged(component, EventArgs.Empty);
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/> that represents the type of the property.</returns>
        public override Type PropertyType
        {
            get { return this.Property.PropertyType; }
        }
    }


}