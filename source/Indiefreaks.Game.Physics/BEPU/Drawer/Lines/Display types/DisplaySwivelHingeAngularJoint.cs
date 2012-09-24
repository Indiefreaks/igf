﻿using BEPUphysics.Constraints.TwoEntity.Joints;
using Microsoft.Xna.Framework;

namespace BEPU.Drawer.Lines
{
    /// <summary>
    /// Graphical representation of a SwivelHinge.
    /// </summary>
    public class DisplaySwivelHingeAngularJoint : SolverDisplayObject<SwivelHingeAngularJoint>
    {
        private readonly Line axisA;
        private readonly Line axisB;


        public DisplaySwivelHingeAngularJoint(SwivelHingeAngularJoint constraint, LineDrawer drawer)
            : base(drawer, constraint)
        {
            axisA = new Line(Color.DarkRed, Color.DarkRed, drawer);
            axisB = new Line(Color.DarkRed, Color.DarkRed, drawer);

            myLines.Add(axisA);
            myLines.Add(axisB);
        }


        /// <summary>
        /// Moves the constraint lines to the proper location relative to the entities involved.
        /// </summary>
        public override void Update()
        {
            //Move lines around
            axisA.PositionA = LineObject.ConnectionA.Position;
            axisA.PositionB = LineObject.ConnectionA.Position + LineObject.WorldHingeAxis;

            axisB.PositionA = LineObject.ConnectionB.Position;
            axisB.PositionB = LineObject.ConnectionB.Position + LineObject.WorldTwistAxis;
        }
    }
}