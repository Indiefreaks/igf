﻿using System;
using BEPUphysics.BroadPhaseSystems;
using BEPUphysics.Collidables;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionTests;
using BEPUphysics.CollisionTests.CollisionAlgorithms.GJK;
using BEPUphysics.CollisionTests.Manifolds;
using BEPUphysics.Constraints.Collision;
using BEPUphysics.PositionUpdating;
using BEPUphysics.Settings;
using Microsoft.Xna.Framework;
using BEPUphysics.ResourceManagement;

namespace BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a terrain-convex collision pair.
    ///</summary>
    public abstract class TerrainPairHandler : StandardPairHandler
    {
        Terrain terrain;
        ConvexCollidable convex;

        NonConvexContactManifoldConstraint contactConstraint = new NonConvexContactManifoldConstraint();


        protected override Collidable CollidableA
        {
            get { return convex; }
        }
        protected override Collidable CollidableB
        {
            get { return terrain; }
        }
        protected override Entities.Entity EntityA
        {
            get { return convex.entity; }
        }
        protected override Entities.Entity EntityB
        {
            get { return null; }
        }
        /// <summary>
        /// Gets the contact constraint used by the pair handler.
        /// </summary>
        public override ContactManifoldConstraint ContactConstraint
        {
            get { return contactConstraint; }
        }
        /// <summary>
        /// Gets the contact manifold used by the pair handler.
        /// </summary>
        public override ContactManifold ContactManifold
        {
            get { return TerrainManifold; }
        }

        protected abstract TerrainContactManifold TerrainManifold
        {
            get;
        }


        ///<summary>
        /// Initializes the pair handler.
        ///</summary>
        ///<param name="entryA">First entry in the pair.</param>
        ///<param name="entryB">Second entry in the pair.</param>
        public override void Initialize(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {

            terrain = entryA as Terrain;
            convex = entryB as ConvexCollidable;

            if (terrain == null || convex == null)
            {
                terrain = entryB as Terrain;
                convex = entryA as ConvexCollidable;

                if (terrain == null || convex == null)
                    throw new Exception("Inappropriate types used to initialize pair.");
            }

            //Contact normal goes from A to B.
            broadPhaseOverlap.entryA = convex;
            broadPhaseOverlap.entryB = terrain;

            UpdateMaterialProperties(convex.entity != null ? convex.entity.material : null, terrain.material);

            base.Initialize(entryA, entryB);




        }


        ///<summary>
        /// Cleans up the pair handler.
        ///</summary>
        public override void CleanUp()
        {
            base.CleanUp();

            terrain = null;
            convex = null;

        }


        ///<summary>
        /// Updates the time of impact for the pair.
        ///</summary>
        ///<param name="requester">Collidable requesting the update.</param>
        ///<param name="dt">Timestep duration.</param>
        public override void UpdateTimeOfImpact(Collidable requester, float dt)
        {
            //TODO: This conditional early outing stuff could be pulled up into a common system, along with most of the pair handler.
            if (convex.IsActive && convex.entity.PositionUpdateMode == PositionUpdateMode.Continuous)
            {
                //TODO: This system could be made more robust by using a similar region-based rejection of edges.
                //CCD events are awfully rare under normal circumstances, so this isn't usually an issue.

                //Only perform the test if the minimum radii are small enough relative to the size of the velocity.
                Vector3 velocity;
                Vector3.Multiply(ref convex.entity.linearVelocity, dt, out velocity);
                float velocitySquared = velocity.LengthSquared();

                var minimumRadius = convex.Shape.minimumRadius * MotionSettings.CoreShapeScaling;
                timeOfImpact = 1;
                if (minimumRadius * minimumRadius < velocitySquared)
                {
                    var triangle = Resources.GetTriangle();
                    triangle.collisionMargin = 0;
                    Vector3 terrainUp = new Vector3(terrain.worldTransform.LinearTransform.M21, terrain.worldTransform.LinearTransform.M22, terrain.worldTransform.LinearTransform.M23);
                    //Spherecast against all triangles to find the earliest time.
                    for (int i = 0; i < TerrainManifold.overlappedTriangles.count; i++)
                    {
                        terrain.Shape.GetTriangle(ref TerrainManifold.overlappedTriangles.Elements[i], ref terrain.worldTransform, out triangle.vA, out triangle.vB, out triangle.vC);
                        //Put the triangle into 'localish' space of the convex.
                        Vector3.Subtract(ref triangle.vA, ref convex.worldTransform.Position, out triangle.vA);
                        Vector3.Subtract(ref triangle.vB, ref convex.worldTransform.Position, out triangle.vB);
                        Vector3.Subtract(ref triangle.vC, ref convex.worldTransform.Position, out triangle.vC);

                        RayHit rayHit;
                        if (GJKToolbox.CCDSphereCast(new Ray(Toolbox.ZeroVector, velocity), minimumRadius, triangle, ref Toolbox.RigidIdentity, timeOfImpact, out rayHit) &&
                            rayHit.T > Toolbox.BigEpsilon)
                        {

                            Vector3 AB, AC;
                            Vector3.Subtract(ref triangle.vB, ref triangle.vA, out AB);
                            Vector3.Subtract(ref triangle.vC, ref triangle.vA, out AC);
                            Vector3 normal;
                            Vector3.Cross(ref AC, ref AB, out normal);
                            float dot;
                            Vector3.Dot(ref normal, ref terrainUp, out dot);
                            if (dot < 0)
                                Vector3.Dot(ref normal, ref rayHit.Normal, out dot);
                            else
                            {
                                Vector3.Dot(ref normal, ref rayHit.Normal, out dot);
                                dot = -dot;
                            }
                            //Only perform sweep if the object is in danger of hitting the object.
                            //Triangles can be one sided, so check the impact normal against the triangle normal.
                            if (dot < 0)
                            {
                                timeOfImpact = rayHit.T;
                            }
                        }
                    }
                    Resources.GiveBack(triangle);
                }



            }

        }


        protected internal override void GetContactInformation(int index, out ContactInformation info)
        {
            info.Contact = TerrainManifold.contacts.Elements[index];
            //Find the contact's normal and friction forces.
            info.FrictionForce = 0;
            info.NormalForce = 0;
            for (int i = 0; i < contactConstraint.frictionConstraints.count; i++)
            {
                if (contactConstraint.frictionConstraints.Elements[i].PenetrationConstraint.contact == info.Contact)
                {
                    info.FrictionForce = contactConstraint.frictionConstraints.Elements[i].accumulatedImpulse;
                    info.NormalForce = contactConstraint.frictionConstraints.Elements[i].PenetrationConstraint.accumulatedImpulse;
                    break;
                }
            }

            //Compute relative velocity
            if (convex.entity != null)
            {
                Vector3 velocity;
                Vector3.Subtract(ref info.Contact.Position, ref convex.entity.position, out velocity);
                Vector3.Cross(ref convex.entity.angularVelocity, ref velocity, out velocity);
                Vector3.Add(ref velocity, ref convex.entity.linearVelocity, out info.RelativeVelocity);
            }
            else
                info.RelativeVelocity = new Vector3();


            info.Pair = this;
        }

    }

}
