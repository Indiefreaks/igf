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
    /// Defines a factory class for getting modifier type descriptors.
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
            if (objectType == typeof(BoxForceModifier))
                return new BoxForceModifierTypeDescriptor();

            if (objectType == typeof(ColourInterpolator2))
                return new ColourInterpolator2TypeDescriptor();

            if (objectType == typeof(ColourInterpolator3))
                return new ColourInterpolator3TypeDescriptor();

            if (objectType == typeof(DampingModifier))
                return new DampingModifierTypeDescriptor();

            if (objectType == typeof(ForceInterpolator2))
                return new ForceInterpolator2TypeDescriptor();

            if (objectType == typeof(HueShiftModifier))
                return new HueShiftModifierTypeDescriptor();

            if (objectType == typeof(LinearGravityModifier))
                return new LinearGravityModifierTypeDescriptor();

            if (objectType == typeof(OpacityFastFadeModifier))
                return new OpacityFastFadeModifierTypeDescriptor();

            if (objectType == typeof(OpacityInterpolator2))
                return new OpacityInterpolator2TypeDescriptor();

            if (objectType == typeof(OpacityInterpolator3))
                return new OpacityInterpolator3TypeDescriptor();

            if (objectType == typeof(RotationModifier))
                return new RotationModifierTypeDescriptor();

            if (objectType == typeof(ScaleInterpolator2))
                return new ScaleInterpolator2TypeDescriptor();

            if (objectType == typeof(ScaleInterpolator3))
                return new ScaleInterpolator3TypeDescriptor();

            if (objectType == typeof(SphereForceModifier))
                return new SphereForceModifierTypeDescriptor();

            if (objectType == typeof(VelocityClampModifier))
                return new VelocityClampModifierTypeDescriptor();

            return base.GetTypeDescriptor(objectType, instance);
        }
    }
}