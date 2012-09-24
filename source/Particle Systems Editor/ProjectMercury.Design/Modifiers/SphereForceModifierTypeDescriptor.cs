/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Design.Modifiers
{
    using System;
    using System.ComponentModel;
    using ProjectMercury.Modifiers;

    /// <summary>
    /// Defines the custom type descriptor for the sphere force modifier class.
    /// </summary>
    public sealed class SphereForceModifierTypeDescriptor : AbstractTypeDescriptor<SphereForceModifier>
    {
        private readonly Type ModifierType = typeof(SphereForceModifier);

        /// <summary>
        /// Returns a collection of property descriptors for the object represented by this type descriptor.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> containing the property descriptions for the object represented by this type descriptor. The default is <see cref="F:System.ComponentModel.PropertyDescriptorCollection.Empty"/>.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                PropertyDescriptorFactory.Create(ModifierType.GetProperty("Position"),
                    new CategoryAttribute("Sphere Force Modifier"),
                    new DisplayNameAttribute("Position"),
                    new DescriptionAttribute("Gets or sets the position of the centre of the force area.")),

                PropertyDescriptorFactory.Create(ModifierType.GetProperty("Radius"),
                    new CategoryAttribute("Sphere Force Modifier"),
                    new DisplayNameAttribute("Radius"),
                    new DescriptionAttribute("Gets or sets the radius of the sphere.")),

                PropertyDescriptorFactory.Create(ModifierType.GetProperty("ForceVector"),
                    new CategoryAttribute("Sphere Force Modifier"),
                    new DisplayNameAttribute("Force Vector"),
                    new DescriptionAttribute("Gets or sets the normalised force vector.")),

                PropertyDescriptorFactory.Create(ModifierType.GetProperty("Strength"),
                    new CategoryAttribute("Sphere Force Modifier"),
                    new DisplayNameAttribute("Strength"),
                    new DescriptionAttribute("Gets or sets the strength of the force."))
            });
        }
    }
}