using System;
using System.Collections.Generic;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.DataStructures;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.MathExtensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Collision;

namespace Indiefreaks.Xna.Physics.Entities
{
    /// <summary>
    /// ConvexHull based BEPUEntityCollisionMove class implementation
    /// </summary>
    public class ConvexHullCollisionMove : BEPUEntityCollisionMove<Entity<ConvexCollidable<TransformableShape>>,Vector3>
    {
        private static readonly Dictionary<WeakReference, ConvexHull> ModelConvexHulls = new Dictionary<WeakReference, ConvexHull>();
        private static readonly List<WeakReference> ModelReferencesToDelete = new List<WeakReference>();
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="collisionObject">The ParentObject this instance will be associated with</param>
        /// <param name="model">The Model used to compute the ConvexHull shape used for collision & physics</param>
        /// <remarks>If a previously created instance uses the same Model as the one provided here, it will reuse the computed ConvexHull instance to save CPU</remarks>
        public ConvexHullCollisionMove(ICollisionObject collisionObject, Model model) : base(collisionObject)
        {
            ModelReferencesToDelete.Clear();

            foreach (var modelConvexHull in ModelConvexHulls)
            {
                if (!modelConvexHull.Key.IsAlive)
                {
                    ModelReferencesToDelete.Add(modelConvexHull.Key);
                }
                else if (modelConvexHull.Key.Target == model)
                {
                    ConvexHull convexHull = modelConvexHull.Value;

                    TransformableShape shape = new TransformableShape(convexHull.CollisionInformation.Shape, Matrix3X3.CreateFromMatrix(ParentObject.World));

                    SpaceObject = new Entity<ConvexCollidable<TransformableShape>>(
                        new ConvexCollidable<TransformableShape>(shape),
                        ParentObject.Mass,
                        convexHull.LocalInertiaTensor,
                        convexHull.Volume);
                    Entity.Position = ParentObject.World.Translation;
                }
            }

            foreach (var modelReference in ModelReferencesToDelete)
            {
                ModelConvexHulls.Remove(modelReference);
            }

            if (SpaceObject == null)
            {
                Vector3[] vertices;
                int[] indices;

                TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);

                var modelReference = new WeakReference(model);
                var convexHull = new ConvexHull(vertices, ParentObject.Mass);

                ModelConvexHulls.Add(modelReference, convexHull);

                TransformableShape shape = new TransformableShape(convexHull.CollisionInformation.Shape, Matrix3X3.CreateFromMatrix(ParentObject.World));

                SpaceObject = new Entity<ConvexCollidable<TransformableShape>>(
                    new ConvexCollidable<TransformableShape>(shape),
                    ParentObject.Mass,
                    convexHull.LocalInertiaTensor,
                    convexHull.Volume);
                Entity.Position = ParentObject.World.Translation;
            }

            ParentObject.World.GetScaleComponent(out CollisionObjectScale);
        }

        protected override void OnCollisionObjectScaleChanged()
        {
            Entity.CollisionInformation.Shape.Transform = Matrix3X3.CreateFromMatrix(Matrix.CreateScale(CollisionObjectScale) * CollisionObjectWorldTransform);
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