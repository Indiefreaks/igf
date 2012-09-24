using System;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Collision;

namespace Indiefreaks.Xna.Physics.Entities
{
    /// <summary>
    /// Box based BEPUEntityCollisionMove implementation
    /// </summary>
    public class BoxCollisionMove : BEPUEntityCollisionMove<Box, Vector3>
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="collisionObject">The ParentObject this instance will be associated with</param>
        public BoxCollisionMove(ICollisionObject collisionObject)
            : base(collisionObject)
        {
            ParentObject.World.GetScaleComponent(out CollisionObjectScale);

            SpaceObject = new Box(
                ParentObject.World.Translation,
                (ParentObject.ObjectBoundingBox.Max.X - ParentObject.ObjectBoundingBox.Min.X) * CollisionObjectScale.X,
                (ParentObject.ObjectBoundingBox.Max.Y - ParentObject.ObjectBoundingBox.Min.Y) * CollisionObjectScale.Y,
                (ParentObject.ObjectBoundingBox.Max.Z - ParentObject.ObjectBoundingBox.Min.Z) * CollisionObjectScale.Z,
                ParentObject.Mass);
        }

        protected override void OnCollisionObjectScaleChanged()
        {
            Entity.Width = (ParentObject.ObjectBoundingBox.Max.X - ParentObject.ObjectBoundingBox.Min.X) * CollisionObjectScale.X;
            Entity.Height = (ParentObject.ObjectBoundingBox.Max.Y - ParentObject.ObjectBoundingBox.Min.Y) * CollisionObjectScale.Y;
            Entity.Length = (ParentObject.ObjectBoundingBox.Max.Z - ParentObject.ObjectBoundingBox.Min.Z) * CollisionObjectScale.Z;
        }

        /// <summary>
        /// Applies the current ISpaceObject World matrix changes to the ParentObject.
        /// </summary>
        public override void End()
        {
            ParentObject.World = Matrix.CreateScale(CollisionObjectScale) * Entity.WorldTransform;
        }
    }
}