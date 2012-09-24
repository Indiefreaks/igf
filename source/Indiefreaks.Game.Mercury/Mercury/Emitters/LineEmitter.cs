/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Emitters
{
    using System;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines an Emitter which releases Particles at a random point along a line.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public class LineEmitter : PlaneEmitter
    {
        private Single HalfLength;

        /// <summary>
        /// Gets or sets the length of the line.
        /// </summary>
        public Single Length
        {
            get { return this.HalfLength + this.HalfLength; }
            set
            {
                Check.ArgumentFinite("Length", value);
                Check.ArgumentNotLessThan("Length", value, 0f);

                this.HalfLength = value * 0.5f;
            }
        }


        /// <summary>
        /// If true, will emit particles perpendicular to the angle of the line.
        /// </summary>
        public Boolean Rectilinear;

        /// <summary>
        /// If true, will emit particles both ways. Only work when Rectilinear is enabled.
        /// </summary>
        public Boolean EmitBothWays;

        /// <summary>
        /// Returns an unitialised deep copy of the Emitter.
        /// </summary>
        /// <returns>A deep copy of the Emitter.</returns>
        protected override AbstractEmitter DeepCopy(AbstractEmitter exisitingInstance)
        {
            LineEmitter value = (exisitingInstance as LineEmitter) ?? new LineEmitter();

            value.Length = this.Length;
            value.Rectilinear = this.Rectilinear;
            value.EmitBothWays = this.EmitBothWays;

            base.DeepCopy(value);

            return value;
        }

        /// <summary>
        /// Generates an offset vector and force vector for a Particle when it is released.
        /// </summary>
        /// <param name="offset">The offset of the Particle from the trigger location.</param>
        /// <param name="force">A unit vector defining the initial force of the Particle.</param>
        protected override void GenerateOffsetAndForce(out Vector3 offset, out Vector3 force)
        {
            float lineOffset = RandomUtil.NextSingle(-this.HalfLength, this.HalfLength);

            offset = new Vector3
            {
                X = lineOffset,
                Y = 0,
                Z = 0
            };

            if (this.Rectilinear)
            {
                force = Vector3.UnitY;

                if (this.EmitBothWays)
                {
                    if (RandomUtil.NextBool())
                    {
                        force.Y *= -1f;
                    }
                }
            }
            else
            {
                force = RandomUtil.NextUnitVector3();
                
                if (base.ConstrainToPlane)
                {
                    force.Z = 0;
                }
            }
        }
    }
}