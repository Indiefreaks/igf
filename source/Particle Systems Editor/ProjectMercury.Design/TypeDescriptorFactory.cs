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
    using ProjectMercury.Controllers;

    using AbstractController = ProjectMercury.Controllers.AbstractController;
    using AbstractEmitter    = ProjectMercury.Emitters.AbstractEmitter;
    using AbstractModifier   = ProjectMercury.Modifiers.AbstractModifier;
    using AbstractRenderer   = ProjectMercury.Renderers.AbstractRenderer;

    /// <summary>
    /// Defines a factory class for getting type descriptors.
    /// </summary>
    public sealed class TypeDescriptorFactory : TypeDescriptionProvider
    {
        /// <summary>
        /// Gets a custom type descriptor for the given type and object.
        /// </summary>
        /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param>
        /// <param name="instance">An instance of the type. Can be null if no instance was passed to the <see cref="T:System.ComponentModel.TypeDescriptor"/>.</param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor"/> that can provide metadata for the type.
        /// </returns>
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, Object instance)
        {
            if (objectType == typeof(ParticleEffect))
                return new ParticleEffectTypeDescriptor();

            else
                return this.GetDescriptorSubFactory(objectType)
                           .GetTypeDescriptor(objectType, instance);
        }

        /// <summary>
        /// Gets a type descriptor factory for the specified type.
        /// </summary>
        /// <param name="objectType">The type of object for which to retrieve the type descriptor factory.</param>
        /// <returns>A <see cref="System.ComponentModel.TypeDescriptionProvider"/> that can provide a type descriptor
        /// for the type.</returns>
        private TypeDescriptionProvider GetDescriptorSubFactory(Type objectType)
        {
            if (objectType.IsSubclassOf(typeof(AbstractController)))
                return new Controllers.TypeDescriptorFactory();

            if (objectType.IsSubclassOf(typeof(AbstractEmitter)))
                return new Emitters.TypeDescriptorFactory();

            if (objectType.IsSubclassOf(typeof(AbstractModifier)))
                return new Modifiers.TypeDescriptorFactory();

            if (objectType.IsSubclassOf(typeof(AbstractRenderer)))
                return new Renderers.TypeDescriptorFactory();

            throw new ArgumentException();
        }
    }
}