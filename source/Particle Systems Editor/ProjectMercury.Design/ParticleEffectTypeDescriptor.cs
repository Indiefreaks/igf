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
    using ProjectMercury;

    /// <summary>
    /// Defines the custom type descriptor for a particle effect.
    /// </summary>
    public class ParticleEffectTypeDescriptor : AbstractTypeDescriptor<ParticleEffect>
    {
        /// <summary>
        /// Defines a type description provider for the ParticleEffect class.
        /// </summary>
        private class DescriptionProvider : TypeDescriptionProvider
        {
            /// <summary>
            /// Gets the default instance.
            /// </summary>
            static public TypeDescriptionProvider Default
            {
                get { return new DescriptionProvider(); }
            }
            
            /// <summary>
            /// Gets a custom type descriptor for the given type.
            /// </summary>
            /// <param name="objectType">The type of object for which to retrieve the type
            /// descriptor.</param>
            /// <param name="instance">An instance of the type. Can be null if no instance was
            /// passed to the System.ComponentModel.TypeDescriptor.</param>
            /// <returns>An System.ComponentModel.ICustomTypeDescriptor that can provide metadata
            /// for the type.</returns>
            public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, Object instance)
            {
                if (objectType == typeof(ParticleEffect))
                    return new ParticleEffectTypeDescriptor();

                return base.GetTypeDescriptor(objectType, instance);
            }
        }

        public static void Register()
        {
            TypeDescriptor.AddProvider(DescriptionProvider.Default, typeof(ParticleEffect));
        }

        private readonly Type ParticleEffectType = typeof(ParticleEffect);

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
                PropertyDescriptorFactory.Create(ParticleEffectType.GetProperty("Name"),
                    new CategoryAttribute("Particle Effect"),
                    new DescriptionAttribute("The name of the particle effect.")),

                PropertyDescriptorFactory.Create(ParticleEffectType.GetProperty("Author"),
                    new CategoryAttribute("Particle Effect"),
                    new DescriptionAttribute("The author of the particle effect.")),

                PropertyDescriptorFactory.Create(ParticleEffectType.GetProperty("Description"),
                    new CategoryAttribute("Particle Effect"),
                    new DescriptionAttribute("The description of the ParticleEffect.")),
            });
        }
    }
}