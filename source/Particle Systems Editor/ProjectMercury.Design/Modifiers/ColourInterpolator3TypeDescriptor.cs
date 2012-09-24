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
    /// Defines the custom type descriptor for the colour interpolator3 class.
    /// </summary>
    public sealed class ColourInterpolator3TypeDescriptor : AbstractTypeDescriptor<ColourInterpolator3>
    {
        private readonly Type ModifierType = typeof(ColourInterpolator3);

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
                PropertyDescriptorFactory.Create(ModifierType.GetProperty("InitialColour"),
                    new CategoryAttribute("Colour Interpolator 3"),
                    new DisplayNameAttribute("Initial Colour"),
                    new DescriptionAttribute("Gets or sets the initial colour of particles when they are released.")),

                PropertyDescriptorFactory.Create(ModifierType.GetProperty("MedianColour"),
                    new CategoryAttribute("Colour Interpolator 3"),
                    new DisplayNameAttribute("Median Colour"),
                    new DescriptionAttribute("Gets or sets the colour of particles when they reach their defined median age.")),

                PropertyDescriptorFactory.Create(ModifierType.GetProperty("Median"),
                    new CategoryAttribute("Colour Interpolator 3"),
                    new DisplayNameAttribute("Median Age"),
                    new DescriptionAttribute("Gets or sets the point in a particles life where it becomes MedianColour.")),

                PropertyDescriptorFactory.Create(ModifierType.GetProperty("FinalColour"),
                    new CategoryAttribute("Colour Interpolator 3"),
                    new DisplayNameAttribute("Final Colour"),
                    new DescriptionAttribute("Gets or sets the final colour of particles when they are retired."))
            });
        }
    }
}