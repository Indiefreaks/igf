/*
* Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
* 
* This program is licensed under the Microsoft Permissive License (Ms-PL). You should
* have received a copy of the license along with the source code. If not, an online copy
* of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Emitters
{
    using System;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;
    using ProjectMercury.Emitters;

    /// <summary>
    /// Defines an emitter which releases particles from within a cylinder.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public abstract class PlaneEmitter : AbstractEmitter
    {
        /// <summary>
        /// Should random forces keep the particle in the XY plane or allow it to move in all 3
        /// Can be used in 2d or 3d. In 2d this will keep particles in the same plane as the emitter
        /// In 3d it keeps particles radiating in a planar fashion.
        /// </summary>
        public Boolean ConstrainToPlane { get; set; }

        /// <summary>
        /// Copies the properties of this instance into the specified existing instance.
        /// </summary>
        /// <param name="exisitingInstance">An existing emitter instance.</param>
        protected override AbstractEmitter DeepCopy(AbstractEmitter exisitingInstance)
        {
            PlaneEmitter value = (exisitingInstance as PlaneEmitter);
            
            value.ConstrainToPlane = this.ConstrainToPlane;

            base.DeepCopy(value);

            return value;
        }
    }
}