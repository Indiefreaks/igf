using BEPUphysics.Collidables;
using BEPUphysics.DataStructures;
using BEPUphysics.MathExtensions;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Collision;

namespace Indiefreaks.Xna.Physics.Entities
{
    /// <summary>
    /// StaticMesh based BEPUCollisionMove class implementation
    /// </summary>
    public class StaticMeshCollisionMove : BEPUCollisionMove
    {
        private readonly Quaternion _collisionObjectRotation;
        private readonly Vector3 _collisionObjectScale;
        private readonly Vector3 _collisionObjectTranslation;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="collisionObject">The ParentObject this instance will be associated with</param>
        /// <param name="model">The Model used to compute the BEPUPgysics StaticMesh</param>
        public StaticMeshCollisionMove(ICollisionObject collisionObject, Model model) : base(collisionObject)
        {
            Vector3[] vertices;
            int[] indices;

            TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);

            ParentObject.World.Decompose(out _collisionObjectScale, out _collisionObjectRotation, out _collisionObjectTranslation);

            SpaceObject = new StaticMesh(vertices, indices, new AffineTransform(_collisionObjectScale, _collisionObjectRotation, _collisionObjectTranslation));
        }

        /// <summary>
        /// Returns the BEPUPhysics StaticMesh instance
        /// </summary>
        public StaticMesh StaticMesh
        {
            get { return SpaceObject as StaticMesh; }
        }

        #region Overrides of BEPUCollisionMove

        public override void Initialize()
        {
            StaticMesh.Events.InitialCollisionDetected += OnStaticMeshCollisionDetected;
            StaticMesh.Tag = ParentObject;

            base.Initialize();
        }

        /// <summary>
        /// Distance the object will move this frame. Valid after calling Begin().
        /// </summary>
        public override float Distance
        {
            get { return 0f; }
        }

        /// <summary>
        /// Direction the object will move this frame. Valid after calling Begin().
        /// </summary>
        public override Vector3 Normal
        {
            get { return Vector3.Zero; }
        }

        /// <summary>
        /// World bounding area the object will move to this frame.
        /// </summary>
        public override BoundingSphere WorldBoundingSphere
        {
            get { return BoundingSphere.CreateFromBoundingBox(StaticMesh.BoundingBox); }
        }

        /// <summary>
        /// World bounding area the object will move to this frame.
        /// </summary>
        public override BoundingBox WorldBoundingBox
        {
            get { return StaticMesh.BoundingBox; }
        }
        
        /// <summary>
        /// Raises the appropriate ParentObject collision events depending on its CollisionType value
        /// </summary>
        /// <param name="sender">The current ISpaceObject instance</param>
        /// <param name="other">The ISpaceObject instance which collided</param>
        /// <param name="pair"/>
        private void OnStaticMeshCollisionDetected(StaticMesh sender, Collidable other, CollidablePairHandler pair)
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
        }

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Update().
        /// </summary>
        /// <param name="objectposition">Object-space location the force is applied to the object.
        ///             This allows off-center forces, which cause rotation.</param><param name="objectforce">Amount of object-space force to apply to the object.</param>
        public override void ApplyObjectForce(Vector3 objectposition, Vector3 objectforce)
        {
        }

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Begin().
        /// </summary>
        /// <param name="worldforce">Amount of world-space force to apply to the object.</param><param name="constantforce">Determines if the force is from a constant
        ///             source such as gravity, wind, or similar (eg: applied by the caller
        ///             every frame instead of a single time).</param>
        public override void ApplyWorldForce(Vector3 worldforce, bool constantforce)
        {
        }

        /// <summary>
        /// Applies force to the object. The total force is used to move the
        ///             object during the next call to Update().
        /// </summary>
        /// <param name="worldposition">World-space location the force is applied to the object.
        ///             This allows off-center forces, which cause rotation.</param><param name="worldforce">Amount of world-space force to apply to the object.</param>
        public override void ApplyWorldForce(Vector3 worldposition, Vector3 worldforce)
        {
        }

        /// <summary>
        /// Removes all accumulated forces acting on the object. This will halt the object
        ///             movement, however future forces (such as gravity) can immediately begin acting
        ///             on the object again.
        /// </summary>
        public override void RemoveForces()
        {
        }

        /// <summary>
        /// Applies eventual ParentObject collision information changes to the current ISpaceObject instance.
        /// </summary>
        public override void Begin()
        {
        }

        /// <summary>
        /// Applies the current ISpaceObject World matrix changes to the ParentObject.
        /// </summary>
        public override void End()
        {
        }

        #endregion
    }
}