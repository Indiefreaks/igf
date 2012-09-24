using System;
using BEPUphysics.Collidables;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Entities;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Collision;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Physics
{
    /// <summary>
    /// Base BEPUCollisionMove implementation for BEPUPhysics Entity classes.
    /// </summary>
    /// <typeparam name="TEntity">The type of the Entity this class will use</typeparam>
    /// <typeparam name="TScaleValueType"></typeparam>
    public abstract class BEPUEntityCollisionMove<TEntity, TScaleValueType> : BEPUCollisionMove where TEntity : Entity where TScaleValueType : struct 
    {
        protected Matrix CollisionObjectWorldTransform;
        private CollisionType _collisionType = CollisionType.None;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="collisionObject">The ParentObject this instance will be associated with</param>
        protected BEPUEntityCollisionMove(ICollisionObject collisionObject) : base(collisionObject)
        {
            ParentObject.World.SRTMatrixToRTMatrix(out CollisionObjectWorldTransform);
        }

        /// <summary>
        /// Returns the current typed Entity instance
        /// </summary>
        public TEntity Entity
        {
            get { return SpaceObject as TEntity; }
        }

        /// <summary>
        /// Distance the object will move this frame. Valid after calling Begin().
        /// </summary>
        public override float Distance
        {
            get { return Entity.LinearVelocity.Length(); }
        }

        /// <summary>
        /// Direction the object will move this frame. Valid after calling Begin().
        /// </summary>
        public override Vector3 Normal
        {
            get
            {
                return Entity.LinearVelocity != Vector3.Zero ? Vector3.Normalize(Entity.LinearVelocity) : Entity.OrientationMatrix.Forward;
            }
        }

        protected TScaleValueType CollisionObjectScale;

        public TScaleValueType Scale
        {
            get { return CollisionObjectScale; }
            set
            {
                if (!CollisionObjectScale.Equals(value))
                {
                    CollisionObjectScale = value;
                    OnCollisionObjectScaleChanged();
                }
            }
        }

        /// <summary>
        /// World bounding area the object will move to this frame.
        /// </summary>
        public override BoundingBox WorldBoundingBox
        {
            get { return Entity.CollisionInformation.BoundingBox; }
        }

        /// <summary>
        /// World bounding area the object will move to this frame.
        /// </summary>
        public override BoundingSphere WorldBoundingSphere
        {
            get { return BoundingSphere.CreateFromBoundingBox(WorldBoundingBox); }
        }

        public override void Initialize()
        {
            Entity.IsAffectedByGravity = ParentObject.AffectedByGravity;
            Entity.CollisionInformation.Events.InitialCollisionDetected += OnEntityCollisionDetected;
            Entity.CollisionInformation.Tag = ParentObject;

            switch (_collisionType)
            {
                case CollisionType.Collide:
                    {
                        Entity.CollisionInformation.CollisionRules.Personal = CollisionRule.Defer;
                        break;
                    }
                case CollisionType.Trigger:
                case CollisionType.None:
                default:
                    {
                        Entity.CollisionInformation.CollisionRules.Personal = CollisionRule.NoSolver;
                        break;
                    }
            }

            base.Initialize();
        }

        /// <summary>
        /// Raises the appropriate ParentObject collision events depending on its CollisionType value
        /// </summary>
        /// <param name="sender">The current ISpaceObject instance</param>
        /// <param name="other">The ISpaceObject instance which collided</param>
        /// <param name="pair"/>
        private void OnEntityCollisionDetected(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
        {
            OnCollisionDetected(sender, other, pair);
        }

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Begin().
        /// </summary>
        /// <param name="objectforce">Amount of object-space force to apply to the object.</param>
        public override void ApplyObjectForce(Vector3 objectforce)
        {
            var worldForce = Vector3.TransformNormal(objectforce, ParentObject.World);
            ApplyWorldForce(worldForce, true);
        }

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Update().
        /// </summary>
        /// <param name="objectposition">Object-space location the force is applied to the object.
        ///             This allows off-center forces, which cause rotation.</param><param name="objectforce">Amount of object-space force to apply to the object.</param>
        public override void ApplyObjectForce(Vector3 objectposition, Vector3 objectforce)
        {
            Vector3 vector1, vector2;
            var world = ParentObject.World;
            Vector3.Transform(ref objectposition, ref world, out vector1);
            Vector3.TransformNormal(ref objectforce, ref world, out vector2);
            ApplyWorldForce(vector1, vector2);
        }

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Begin().
        /// </summary>
        /// <param name="worldforce">Amount of world-space force to apply to the object.</param>
        /// <param name="constantforce">Determines if the force is from a constant
        ///             source such as gravity, wind, or similar (eg: applied by the caller
        ///             every frame instead of a single time).</param>
        public override void ApplyWorldForce(Vector3 worldforce, bool constantforce)
        {
            Entity.ApplyImpulse(Entity.Position, worldforce);
        }

        public override void ApplyWorldForce(Vector3 worldposition, Vector3 worldforce)
        {
            Entity.ApplyImpulse(ref worldposition, ref worldforce);
        }

        /// <summary>
        /// Removes all accumulated forces acting on the object. This will halt the object
        ///             movement, however future forces (such as gravity) can immediately begin acting
        ///             on the object again.
        /// </summary>
        public override void RemoveForces()
        {
            Entity.LinearVelocity = Entity.LinearMomentum = Entity.AngularVelocity = Entity.AngularMomentum = Vector3.Zero;
        }

        /// <summary>
        /// Applies eventual ParentObject collision information changes to the current ISpaceObject instance.
        /// </summary>
        public override void Begin()
        {
            if (ParentObject.CollisionType != _collisionType)
            {
                _collisionType = ParentObject.CollisionType;

                switch (_collisionType)
                {
                    case CollisionType.Collide:
                        {
                            Entity.CollisionInformation.CollisionRules.Personal = CollisionRule.Normal;
                            break;
                        }
                    case CollisionType.Trigger:
                    case CollisionType.None:
                    default:
                        {
                            Entity.CollisionInformation.CollisionRules.Personal = CollisionRule.NoSolver;
                            break;
                        }
                }
            }

            if (ParentObject.Mass != Entity.Mass)
                Entity.Mass = ParentObject.Mass;

            if (ParentObject.AffectedByGravity != Entity.IsAffectedByGravity)
                Entity.IsAffectedByGravity = ParentObject.AffectedByGravity;

            if(ParentObject.MoveId != _moveId)
            {
                ParentObject.World.SRTMatrixToRTMatrix(out CollisionObjectWorldTransform);
                _moveId = ParentObject.MoveId;
            }
            
            Entity.WorldTransform = CollisionObjectWorldTransform;
        }

        private int _moveId;

        protected virtual void OnCollisionObjectScaleChanged()
        {
            
        }
    }
}