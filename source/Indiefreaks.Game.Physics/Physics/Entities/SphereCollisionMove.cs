using System;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Collision;

namespace Indiefreaks.Xna.Physics.Entities
{
    /// <summary>
    /// Sphere based BEPUEntityCollisionMove class implementation
    /// </summary>
    public class SphereCollisionMove : BEPUEntityCollisionMove<Sphere, float>
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="collisionObject">The ParentObject this instance will be associated with</param>
        public SphereCollisionMove(ICollisionObject collisionObject) : base(collisionObject)
        {
            SpaceObject = new Sphere(ParentObject.World.Translation, ParentObject.WorldBoundingSphere.Radius);
            CollisionObjectScale = ParentObject.WorldBoundingSphere.Radius;
        }

        protected override void OnCollisionObjectScaleChanged()
        {
            Entity.Radius = CollisionObjectScale;
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