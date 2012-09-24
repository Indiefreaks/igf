using System.Collections.Generic;
using BEPUphysics.CollisionShapes;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.MathExtensions;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Collision;

namespace Indiefreaks.Xna.Physics.Entities
{
    public class CompoundBodyCollisionMove : BEPUEntityCollisionMove<CompoundBody, Vector3>
    {
        public CompoundBodyCollisionMove(ICollisionObject collisionObject, IList<CompoundShapeEntry> childrenShapes) : base(collisionObject)
        {
            SpaceObject = new CompoundBody(childrenShapes, collisionObject.Mass);
        }

        #region Overrides of BEPUCollisionMove

        protected override void OnCollisionObjectScaleChanged()
        {
            for (int i = 0; i < Entity.CollisionInformation.Shape.Shapes.Count; i++)
            {
                CompoundShapeEntry childShape = Entity.CollisionInformation.Shape.Shapes[i];

                if (childShape.Shape is TransformableShape)
                    ((TransformableShape) childShape.Shape).Transform = Matrix3X3.CreateFromMatrix(Matrix.CreateScale(CollisionObjectScale)*CollisionObjectWorldTransform);
            }
        }

        public override void End()
        {
            ParentObject.World = Matrix.CreateScale(CollisionObjectScale)*Entity.WorldTransform;
        }

        #endregion
    }
}