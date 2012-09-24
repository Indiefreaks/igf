/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Emitters
{
    using System.ComponentModel;

    /// <summary>
    /// Defines an emitter which release particles from a single point.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class PointEmitter : AbstractEmitter
    {
        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="exisitingInstance">An existing emitter instance.</param>
        protected override AbstractEmitter DeepCopy(AbstractEmitter exisitingInstance)
        {
            PointEmitter value = (exisitingInstance as PointEmitter) ?? new PointEmitter();

            base.DeepCopy(value);

            return value;
        }
    }
}