using System;
using BEPUphysics;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Collision;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Physics
{
    /// <summary>
    /// Base SunBurn ICollisionMove class.
    /// </summary>
    public abstract class BEPUCollisionMove : ICollisionMove
    {
        private bool _collisionHandled = true;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="collisionObject">The parent object this instance will be associataed with</param>
        protected BEPUCollisionMove(ICollisionObject collisionObject)
        {
            ParentObject = collisionObject;
        }
        
        public void Enable()
        {
            if(SpaceObject.Space == null)
                SceneInterface.ActiveSceneInterface.GetManager<BEPUPhysicsManager>(true).Submit(this);;

        }

        public void Disable()
        {
            if(SpaceObject.Space != null)
                SceneInterface.ActiveSceneInterface.GetManager<BEPUPhysicsManager>(true).Remove(this);
        }

        /// <summary>
        /// Returns the BEPUPhysics ISpaceObject instance.
        /// </summary>
        public ISpaceObject SpaceObject { get; protected set; }

        /// <summary>
        /// Returns the current ICollisionObject associated with this instance.
        /// </summary>
        public ICollisionObject ParentObject { get; protected set; }

        /// <summary>
        /// Initializes the current instance
        /// </summary>
        public virtual void Initialize()
        {
            IsInitialized = true;
        }

        /// <summary>
        /// Raises the appropriate ParentObject collision events depending on its CollisionType value
        /// </summary>
        /// <param name="sender">The current ISpaceObject instance</param>
        /// <param name="other">The ISpaceObject instance which collided</param>
        /// <param name="pair"/>
        protected void OnCollisionDetected(Collidable sender, Collidable other, CollidablePairHandler pair)
        {
            if (sender.Tag == null || other.Tag == null)
                return;

            ICollisionObject senderCollisionObject = (ICollisionObject) sender.Tag;
            ICollisionObject otherCollisionObject = (ICollisionObject) other.Tag;

            switch (ParentObject.CollisionType)
            {
                case CollisionType.Trigger:
                    {
                        ParentObject.OnCollisionTrigger(senderCollisionObject, otherCollisionObject);
                        break;
                    }
                case CollisionType.Collide:
                    {
                        var collisionPoint = new CollisionPoint
                                                 {
                                                     ContactObject = otherCollisionObject,
                                                     ContactPoint = pair.Contacts[0].Contact.Position,
                                                     ContactTime = pair.TimeOfImpact,
                                                     Material = null,
                                                     SurfaceNormal = pair.Contacts[0].Contact.Normal
                                                 };

                        ParentObject.OnCollisionReact(senderCollisionObject, otherCollisionObject, collisionPoint, ref _collisionHandled);
                        break;
                    }
                default:
                case CollisionType.None:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Applies eventual ParentObject collision information changes to the current ISpaceObject instance.
        /// </summary>
        public abstract void Begin();

        /// <summary>
        /// Applies the current ISpaceObject World matrix changes to the ParentObject.
        /// </summary>
        public abstract void End();

        #region Implementation of ICollisionMove

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Begin().
        /// </summary>
        /// <param name="objectforce">Amount of object-space force to apply to the object.</param>
        public abstract void ApplyObjectForce(Vector3 objectforce);

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Update().
        /// </summary>
        /// <param name="objectposition">Object-space location the force is applied to the object.
        ///             This allows off-center forces, which cause rotation.</param><param name="objectforce">Amount of object-space force to apply to the object.</param>
        public abstract void ApplyObjectForce(Vector3 objectposition, Vector3 objectforce);

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Begin().
        /// </summary>
        /// <param name="worldforce">Amount of world-space force to apply to the object.</param><param name="constantforce">Determines if the force is from a constant
        ///             source such as gravity, wind, or similar (eg: applied by the caller
        ///             every frame instead of a single time).</param>
        public abstract void ApplyWorldForce(Vector3 worldforce, bool constantforce);

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Update().
        /// </summary>
        /// <param name="worldposition">World-space location the force is applied to the object.
        ///             This allows off-center forces, which cause rotation.</param><param name="worldforce">Amount of world-space force to apply to the object.</param>
        public abstract void ApplyWorldForce(Vector3 worldposition, Vector3 worldforce);

        /// <summary>
        /// Removes all accumulated forces acting on the object. This will halt the object
        ///             movement, however future forces (such as gravity) can immediately begin acting
        ///             on the object again.
        /// </summary>
        public abstract void RemoveForces();

        /// <summary>
        /// Called when the parent object is submitted to a manager.
        /// </summary>
        /// <param name="manager"/>
        public virtual void OnSubmittedToManager(IManagerService manager)
        {
            if (manager.ManagerType == typeof(IObjectManager))
                if (ParentObject.CollisionMove == this)
                {
                    if (!IsInitialized)
                        Initialize();

                    Enable();
                }
        }

        /// <summary>
        /// Called when the parent object is removed from a manager.
        /// </summary>
        /// <param name="manager"/>
        public virtual void OnRemovedFromManager(IManagerService manager)
        {
            if (manager.ManagerType == typeof(IObjectManager))
                if (ParentObject.CollisionMove == this)
                {
                    if (IsInitialized)
                        Disable();
                }
        }

        /// <summary>
        /// Calculates and applies the reaction force between the
        ///             object and the collision surface contained in the CollisionPoint.
        /// </summary>
        /// <param name="worldcollisionpoint">Contains information about the closest collision point to the collider.</param>
        public virtual void React(CollisionPoint worldcollisionpoint)
        {
        }

        /// <summary>
        /// Distance the object will move this frame. Valid after calling Begin().
        /// </summary>
        public abstract float Distance { get; }

        /// <summary>
        /// Direction the object will move this frame. Valid after calling Begin().
        /// </summary>
        public abstract Vector3 Normal { get; }

        /// <summary>
        /// World bounding area the object will move to this frame.
        /// </summary>
        public abstract BoundingSphere WorldBoundingSphere { get; }

        /// <summary>
        /// World bounding area the object will move to this frame.
        /// </summary>
        public abstract BoundingBox WorldBoundingBox { get; }

        public bool IsInitialized { get; private set; }

        #endregion
    }
}