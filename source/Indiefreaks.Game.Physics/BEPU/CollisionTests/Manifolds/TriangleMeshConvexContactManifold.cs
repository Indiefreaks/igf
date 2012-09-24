﻿using System;
using System.Collections.Generic;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionTests.CollisionAlgorithms;
using Microsoft.Xna.Framework;
using BEPUphysics.DataStructures;
using BEPUphysics.Settings;
using BEPUphysics.ResourceManagement;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.MathExtensions;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace BEPUphysics.CollisionTests.Manifolds
{
    ///<summary>
    /// Manages persistent contact data between a triangle mesh and a convex.
    ///</summary>
    public abstract class TriangleMeshConvexContactManifold : ContactManifold
    {
        protected RawValueList<ContactSupplementData> supplementData = new RawValueList<ContactSupplementData>(4);
        Dictionary<TriangleIndices, TrianglePairTester> activePairTesters = new Dictionary<TriangleIndices, TrianglePairTester>(4);
        RawValueList<ContactData> candidatesToAdd;
        RawValueList<ContactData> reducedCandidates = new RawValueList<ContactData>();
        protected TriangleShape localTriangleShape = new TriangleShape();

        protected abstract TrianglePairTester GetTester();

        protected abstract void GiveBackTester(TrianglePairTester tester);

        HashSet<int> blockedVertexRegions = new HashSet<int>();
        HashSet<Edge> blockedEdgeRegions = new HashSet<Edge>();
        RawValueList<EdgeContact> edgeContacts = new RawValueList<EdgeContact>();
        RawValueList<VertexContact> vertexContacts = new RawValueList<VertexContact>();

        protected ConvexCollidable convex;

        ///<summary>
        /// Gets the convex collidable associated with this pair.
        ///</summary>
        public ConvexCollidable ConvexCollidable
        {
            get
            {
                return convex;
            }
        }

        ///<summary>
        /// Constructs a new contact manifold.
        ///</summary>
        protected TriangleMeshConvexContactManifold()
        {
            contacts = new RawList<Contact>(4);
            unusedContacts = new UnsafeResourcePool<Contact>(4);
            contactIndicesToRemove = new RawList<int>(4);
            candidatesToAdd = new RawValueList<ContactData>(1);
        }

        protected virtual RigidTransform MeshTransform
        {
            get
            {
                return RigidTransform.Identity;
            }
        }


        protected abstract bool UseImprovedBoundaryHandling { get; }
        protected internal abstract int FindOverlappingTriangles(float dt);
        protected abstract bool ConfigureTriangle(int i, out TriangleIndices indices);
        protected internal abstract void CleanUpOverlappingTriangles();

        ///<summary>
        /// Updates the manifold.
        ///</summary>
        ///<param name="dt">Timestep duration.</param>
        public override void Update(float dt)
        {

            //First, refresh all existing contacts.  This is an incremental manifold.
            var transform = MeshTransform;
            ContactRefresher.ContactRefresh(contacts, supplementData, ref convex.worldTransform, ref transform, contactIndicesToRemove);

            RemoveQueuedContacts();


            CleanUpOverlappingTriangles();
            //Get all the overlapped triangle indices.
            int triangleCount = FindOverlappingTriangles(dt);

            Matrix3X3 orientation;
            Matrix3X3.CreateFromQuaternion(ref convex.worldTransform.Orientation, out orientation);
            for (int i = 0; i < triangleCount; i++)
            {
                //Initialize the local triangle.
                TriangleIndices indices;
                if (ConfigureTriangle(i, out indices))
                {

                    //Find a pairtester for the triangle.
                    TrianglePairTester pairTester;
                    if (!activePairTesters.TryGetValue(indices, out pairTester))
                    {
                        pairTester = GetTester();
                        pairTester.Initialize(convex.Shape, localTriangleShape);
                        activePairTesters.Add(indices, pairTester);
                    }
                    pairTester.Updated = true;


                    //Put the triangle into the local space of the convex.
                    Vector3.Subtract(ref localTriangleShape.vA, ref convex.worldTransform.Position, out localTriangleShape.vA);
                    Vector3.Subtract(ref localTriangleShape.vB, ref convex.worldTransform.Position, out localTriangleShape.vB);
                    Vector3.Subtract(ref localTriangleShape.vC, ref convex.worldTransform.Position, out localTriangleShape.vC);
                    Matrix3X3.TransformTranspose(ref localTriangleShape.vA, ref orientation, out localTriangleShape.vA);
                    Matrix3X3.TransformTranspose(ref localTriangleShape.vB, ref orientation, out localTriangleShape.vB);
                    Matrix3X3.TransformTranspose(ref localTriangleShape.vC, ref orientation, out localTriangleShape.vC);

                    //Now, generate a contact between the two shapes.
                    ContactData contact;
                    TinyStructList<ContactData> contactList;
                    if (pairTester.GenerateContactCandidate(out contactList))
                    {
                        for (int j = 0; j < contactList.count; j++)
                        {
                            contactList.Get(j, out contact);


                            if (UseImprovedBoundaryHandling)
                            {
                                if (AnalyzeCandidate(ref indices, pairTester, ref contact))
                                {
                                    AddLocalContact(ref contact, ref orientation);
                                }
                            }
                            else
                            {
                                AddLocalContact(ref contact, ref orientation);
                            }

                        }
                    }

                    //Get the voronoi region from the contact candidate generation.  Possibly just recalculate, since most of the systems don't calculate it.
                    //Depending on which voronoi region it is in (Switch on enumeration), identify the indices composing that region.  For face contacts, don't bother- just add it if unique.
                    //For AB, AC, or BC, add an Edge to the blockedEdgeRegions set with the corresponding indices.
                    //For A, B, or C, add the index of the vertex to the blockedVertexRegions set.
                    //If the edge/vertex is already present in the set, then DO NOT add the contact.
                    //When adding a contact, add ALL other voronoi regions to the blocked sets. 
                }

            }


 

            if (UseImprovedBoundaryHandling)
            {
                for (int i = 0; i < edgeContacts.count; i++)
                {
                    //Only correct if it's allowed AND it's blocked.
                    //If it's not blocked, the contact being created is necessary!
                    //The normal generated by the triangle-convex tester is already known not to
                    //violate the triangle sidedness.
                    if (!blockedEdgeRegions.Contains(edgeContacts.Elements[i].Edge))
                    {
                        //If it's not blocked, use the contact as-is without correcting it.
                        AddLocalContact(ref edgeContacts.Elements[i].ContactData, ref orientation);

                    }
                    else if (edgeContacts.Elements[i].ShouldCorrect)
                    {
                        //If it is blocked, we can still make use of the contact.  But first, we need to change the contact normal to ensure that
                        //it will not interfere (and cause a bump or something).
                        float dot;
                        edgeContacts.Elements[i].CorrectedNormal.Normalize();
                        Vector3.Dot(ref edgeContacts.Elements[i].CorrectedNormal, ref edgeContacts.Elements[i].ContactData.Normal, out dot);
                        edgeContacts.Elements[i].ContactData.Normal = edgeContacts.Elements[i].CorrectedNormal;
                        edgeContacts.Elements[i].ContactData.PenetrationDepth *= MathHelper.Max(0, dot); //Never cause a negative penetration depth.
                        AddLocalContact(ref edgeContacts.Elements[i].ContactData, ref orientation);


                    }
                    //If it's blocked AND it doesn't allow correction, ignore its existence.


    
                }
                for (int i = 0; i < vertexContacts.count; i++)
                {
                    if (!blockedVertexRegions.Contains(vertexContacts.Elements[i].Vertex))
                    {
                        //If it's not blocked, use the contact as-is without correcting it.
                        AddLocalContact(ref vertexContacts.Elements[i].ContactData, ref orientation);
                    }
                    else if (vertexContacts.Elements[i].ShouldCorrect)
                    {
                        //If it is blocked, we can still make use of the contact.  But first, we need to change the contact normal to ensure that
                        //it will not interfere (and cause a bump or something).
                        float dot;
                        vertexContacts.Elements[i].CorrectedNormal.Normalize();
                        Vector3.Dot(ref vertexContacts.Elements[i].CorrectedNormal, ref vertexContacts.Elements[i].ContactData.Normal, out dot);
                        vertexContacts.Elements[i].ContactData.Normal = vertexContacts.Elements[i].CorrectedNormal;
                        vertexContacts.Elements[i].ContactData.PenetrationDepth *= MathHelper.Max(0, dot); //Never cause a negative penetration depth.
                        AddLocalContact(ref vertexContacts.Elements[i].ContactData, ref orientation);
                    }
                    //If it's blocked AND it doesn't allow correction, ignore its existence.

 
                }
                blockedEdgeRegions.Clear();
                blockedVertexRegions.Clear();
                vertexContacts.Clear();
                edgeContacts.Clear();

  
            }



            //Remove stale pair testers.
            //This will only remove 8 stale ones per frame, but it doesn't really matter.
            //VERY rarely will there be more than 8 in a single frame, and they will be immediately taken care of in the subsequent frame.
            var toRemove = new TinyList<TriangleIndices>();
            foreach (KeyValuePair<TriangleIndices, TrianglePairTester> pair in activePairTesters)
            {
                if (!pair.Value.Updated)
                {
                    if (!toRemove.Add(pair.Key))
                        break;
                }
                else
                    pair.Value.Updated = false;
            }



            for (int i = toRemove.count - 1; i >= 0; i--)
            {
                var pairTester = activePairTesters[toRemove[i]];
                pairTester.CleanUp();
                GiveBackTester(pairTester);
                activePairTesters.Remove(toRemove[i]);
            }


            //Some child types will want to do some extra post processing on the manifold.        
            ProcessCandidates(candidatesToAdd);


            //Check if adding the new contacts would overflow the manifold.
            if (contacts.count + candidatesToAdd.count > 4)
            {
                //Adding all the contacts would overflow the manifold.  Reduce to the best subset.
                ContactReducer.ReduceContacts(contacts, candidatesToAdd, contactIndicesToRemove, reducedCandidates);
                RemoveQueuedContacts();
                for (int i = reducedCandidates.count - 1; i >= 0; i--)
                {
                    Add(ref reducedCandidates.Elements[i]);
                    reducedCandidates.RemoveAt(i);
                }
            }
            else if (candidatesToAdd.count > 0)
            {
                //Won't overflow the manifold, so just toss it in PROVIDED that it isn't too close to something else.
                for (int i = 0; i < candidatesToAdd.count; i++)
                {
                    Add(ref candidatesToAdd.Elements[i]);
                }
            }

        

            candidatesToAdd.Clear();

        }

        void AddLocalContact(ref ContactData contact, ref Matrix3X3 orientation)
        {
            //Put the contact into world space.
            Matrix3X3.Transform(ref contact.Position, ref orientation, out contact.Position);
            Vector3.Add(ref contact.Position, ref convex.worldTransform.Position, out contact.Position);
            Matrix3X3.Transform(ref contact.Normal, ref orientation, out contact.Normal);
            //Check to see if the contact is unique before proceeding.
            if (IsContactUnique(ref contact))
            {
                candidatesToAdd.Add(ref contact);
            }
        }


        protected void GetNormal(ref Vector3 uncorrectedNormal, out Vector3 normal)
        {
            //Compute the normal of the triangle in the current convex's local space.
            //Note its reliance on the local triangle shape.  It must be initialized to the correct values before this is called.
            Vector3 AB, AC;
            Vector3.Subtract(ref localTriangleShape.vB, ref localTriangleShape.vA, out AB);
            Vector3.Subtract(ref localTriangleShape.vC, ref localTriangleShape.vA, out AC);
            //Compute the normal based on the sidedness.
            switch (localTriangleShape.sidedness)
            {
                case TriangleSidedness.DoubleSided:
                    //If it's double sided, then pick the triangle normal which points in the same direction
                    //as the contact normal that's going to be corrected.
                    float dot;
                    Vector3.Cross(ref AB, ref AC, out normal);
                    Vector3.Dot(ref normal, ref uncorrectedNormal, out dot);
                    if (dot < 0)
                        Vector3.Negate(ref normal, out normal);
                    break;
                case TriangleSidedness.Clockwise:
                    //If it's clockwise, always use ACxAB.
                    Vector3.Cross(ref AC, ref AB, out normal);
                    break;
                default:
                    //If it's counterclockwise, always use ABxAC.
                    Vector3.Cross(ref AB, ref AC, out normal);
                    break;
            }


        }

        bool AnalyzeCandidate(ref TriangleIndices indices, TrianglePairTester pairTester, ref ContactData contact)
        {
            switch (pairTester.GetRegion(ref contact))
            {
                case VoronoiRegion.A:
                    //Add the contact.
                    VertexContact vertexContact;
                    vertexContact.ContactData = contact;
                    vertexContact.Vertex = indices.A;
                    vertexContact.ShouldCorrect = pairTester.ShouldCorrectContactNormal;
                    if (vertexContact.ShouldCorrect)
                        GetNormal(ref contact.Normal, out vertexContact.CorrectedNormal);
                    else
                        vertexContact.CorrectedNormal = new Vector3();
                    vertexContacts.Add(ref vertexContact);

                    //Block all of the other voronoi regions.
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.B));
                    blockedEdgeRegions.Add(new Edge(indices.B, indices.C));
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.C));
                    blockedVertexRegions.Add(indices.B);
                    blockedVertexRegions.Add(indices.C);
                    break;
                case VoronoiRegion.B:
                    //Add the contact.
                    vertexContact.ContactData = contact;
                    vertexContact.Vertex = indices.B;
                    vertexContact.ShouldCorrect = pairTester.ShouldCorrectContactNormal;
                    if (vertexContact.ShouldCorrect)
                        GetNormal(ref contact.Normal, out vertexContact.CorrectedNormal);
                    else
                        vertexContact.CorrectedNormal = new Vector3();
                    vertexContacts.Add(ref vertexContact);

                    //Block all of the other voronoi regions.
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.B));
                    blockedEdgeRegions.Add(new Edge(indices.B, indices.C));
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.C));
                    blockedVertexRegions.Add(indices.A);
                    blockedVertexRegions.Add(indices.C);
                    break;
                case VoronoiRegion.C:
                    //Add the contact.
                    vertexContact.ContactData = contact;
                    vertexContact.Vertex = indices.C;
                    vertexContact.ShouldCorrect = pairTester.ShouldCorrectContactNormal;
                    if (vertexContact.ShouldCorrect)
                        GetNormal(ref contact.Normal, out vertexContact.CorrectedNormal);
                    else
                        vertexContact.CorrectedNormal = new Vector3();
                    vertexContacts.Add(ref vertexContact);

                    //Block all of the other voronoi regions.
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.B));
                    blockedEdgeRegions.Add(new Edge(indices.B, indices.C));
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.C));
                    blockedVertexRegions.Add(indices.A);
                    blockedVertexRegions.Add(indices.B);
                    break;
                case VoronoiRegion.AB:
                    //Add the contact.
                    EdgeContact edgeContact;
                    edgeContact.Edge = new Edge(indices.A, indices.B);
                    edgeContact.ContactData = contact;
                    edgeContact.ShouldCorrect = pairTester.ShouldCorrectContactNormal;
                    if (edgeContact.ShouldCorrect)
                        GetNormal(ref contact.Normal, out edgeContact.CorrectedNormal);
                    else
                        edgeContact.CorrectedNormal = new Vector3();
                    edgeContacts.Add(ref edgeContact);

                    //Block all of the other voronoi regions.
                    blockedEdgeRegions.Add(new Edge(indices.B, indices.C));
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.C));
                    blockedVertexRegions.Add(indices.A);
                    blockedVertexRegions.Add(indices.B);
                    blockedVertexRegions.Add(indices.C);
                    break;
                case VoronoiRegion.AC:
                    //Add the contact.
                    edgeContact.Edge = new Edge(indices.A, indices.C);
                    edgeContact.ContactData = contact;
                    edgeContact.ShouldCorrect = pairTester.ShouldCorrectContactNormal;
                    if (edgeContact.ShouldCorrect)
                        GetNormal(ref contact.Normal, out edgeContact.CorrectedNormal);
                    else
                        edgeContact.CorrectedNormal = new Vector3();
                    edgeContacts.Add(ref edgeContact);

                    //Block all of the other voronoi regions.
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.B));
                    blockedEdgeRegions.Add(new Edge(indices.B, indices.C));
                    blockedVertexRegions.Add(indices.A);
                    blockedVertexRegions.Add(indices.B);
                    blockedVertexRegions.Add(indices.C);
                    break;
                case VoronoiRegion.BC:
                    //Add the contact.
                    edgeContact.Edge = new Edge(indices.B, indices.C);
                    edgeContact.ContactData = contact;
                    edgeContact.ShouldCorrect = pairTester.ShouldCorrectContactNormal;
                    if (edgeContact.ShouldCorrect)
                        GetNormal(ref contact.Normal, out edgeContact.CorrectedNormal);
                    else
                        edgeContact.CorrectedNormal = new Vector3();
                    edgeContacts.Add(ref edgeContact);

                    //Block all of the other voronoi regions.
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.B));
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.C));
                    blockedVertexRegions.Add(indices.A);
                    blockedVertexRegions.Add(indices.B);
                    blockedVertexRegions.Add(indices.C);
                    break;
                default:
                    //Block all of the other voronoi regions.
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.B));
                    blockedEdgeRegions.Add(new Edge(indices.B, indices.C));
                    blockedEdgeRegions.Add(new Edge(indices.A, indices.C));
                    blockedVertexRegions.Add(indices.A);
                    blockedVertexRegions.Add(indices.B);
                    blockedVertexRegions.Add(indices.C);
                    //Should add the contact.
                    return true;
            }


            return false;
        }

        protected override void Add(ref ContactData contactCandidate)
        {
            ContactSupplementData supplement;
            supplement.BasePenetrationDepth = contactCandidate.PenetrationDepth;
            //The closest point method computes the local space versions before transforming to world... consider cutting out the middle man
            RigidTransform.TransformByInverse(ref contactCandidate.Position, ref convex.worldTransform, out supplement.LocalOffsetA);
            RigidTransform transform = MeshTransform;
            RigidTransform.TransformByInverse(ref contactCandidate.Position, ref transform, out supplement.LocalOffsetB);
            supplementData.Add(ref supplement);
            base.Add(ref contactCandidate);
        }

        protected override void Remove(int contactIndex)
        {
            supplementData.RemoveAt(contactIndex);
            base.Remove(contactIndex);
        }


        private bool IsContactUnique(ref ContactData contactCandidate)
        {

            float distanceSquared;
            RigidTransform meshTransform = MeshTransform;
            for (int i = 0; i < contacts.count; i++)
            {
                Vector3.DistanceSquared(ref contacts.Elements[i].Position, ref contactCandidate.Position, out distanceSquared);
                if (distanceSquared < CollisionDetectionSettings.ContactMinimumSeparationDistanceSquared)
                {
                    //This is a nonconvex manifold.  There will be times where a an object will be shoved into a corner such that
                    //a single position will have two reasonable normals.  If the normals aren't mostly aligned, they should NOT be considered equivalent.
                    Vector3.Dot(ref contacts.Elements[i].Normal, ref contactCandidate.Normal, out distanceSquared);
                    if (Math.Abs(distanceSquared) >= CollisionDetectionSettings.nonconvexNormalDotMinimum)
                    {
                        //Update the existing 'redundant' contact with the new information.
                        //This works out because the new contact is the deepest contact according to the previous collision detection iteration.
                        contacts.Elements[i].Normal = contactCandidate.Normal;
                        contacts.Elements[i].Position = contactCandidate.Position;
                        contacts.Elements[i].PenetrationDepth = contactCandidate.PenetrationDepth;
                        supplementData.Elements[i].BasePenetrationDepth = contactCandidate.PenetrationDepth;
                        RigidTransform.TransformByInverse(ref contactCandidate.Position, ref convex.worldTransform, out supplementData.Elements[i].LocalOffsetA);
                        RigidTransform.TransformByInverse(ref contactCandidate.Position, ref meshTransform, out supplementData.Elements[i].LocalOffsetB);
                        return false;
                    }
                }
            }
            for (int i = 0; i < candidatesToAdd.count; i++)
            {
                Vector3.DistanceSquared(ref candidatesToAdd.Elements[i].Position, ref contactCandidate.Position, out distanceSquared);
                if (distanceSquared < CollisionDetectionSettings.ContactMinimumSeparationDistanceSquared)
                {
                    //This is a nonconvex manifold.  There will be times where a an object will be shoved into a corner such that
                    //a single position will have two reasonable normals.  If the normals aren't mostly aligned, they should NOT be considered equivalent.
                    Vector3.Dot(ref candidatesToAdd.Elements[i].Normal, ref contactCandidate.Normal, out distanceSquared);
                    if (Math.Abs(distanceSquared) >= CollisionDetectionSettings.nonconvexNormalDotMinimum)
                        return false;
                }
            }
            //for (int i = 0; i < edgeContacts.count; i++)
            //{
            //    Vector3.DistanceSquared(ref edgeContacts.Elements[i].ContactData.Position, ref contactCandidate.Position, out distanceSquared);
            //    if (distanceSquared < CollisionDetectionSettings.ContactMinimumSeparationDistanceSquared)
            //    {
            //        return false;
            //    }
            //}
            //for (int i = 0; i < vertexContacts.count; i++)
            //{
            //    Vector3.DistanceSquared(ref vertexContacts.Elements[i].ContactData.Position, ref contactCandidate.Position, out distanceSquared);
            //    if (distanceSquared < CollisionDetectionSettings.ContactMinimumSeparationDistanceSquared)
            //    {
            //        return false;
            //    }
            //}
            return true;

        }

        protected virtual void ProcessCandidates(RawValueList<ContactData> candidates)
        {

        }


        ///<summary>
        /// Cleans up the manifold.
        ///</summary>
        public override void CleanUp()
        {
            supplementData.Clear();
            contacts.Clear();
            convex = null;
            foreach (KeyValuePair<TriangleIndices, TrianglePairTester> pair in activePairTesters)
            {
                pair.Value.CleanUp();
                GiveBackTester(pair.Value);
            }
            activePairTesters.Clear();
            CleanUpOverlappingTriangles();
            base.CleanUp();
        }

        struct Edge : IEquatable<Edge>
        {
            private int A;
            private int B;

            public Edge(int a, int b)
            {
                A = a;
                B = b;
            }

            public override int GetHashCode()
            {
                return A + B;
            }

            public bool Equals(Edge edge)
            {
                return (edge.A == A && edge.B == B) || (edge.A == B && edge.B == A);
            }
        }

        ///<summary>
        /// Stores indices of a triangle.
        ///</summary>
        public struct TriangleIndices : IEquatable<TriangleIndices>
        {
            ///<summary>
            /// First index in the triangle.
            ///</summary>
            public int A;
            ///<summary>
            /// Second index in the triangle.
            ///</summary>
            public int B;
            ///<summary>
            /// Third index in the triangle.
            ///</summary>
            public int C;

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            /// A 32-bit signed integer that is the hash code for this instance.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override int GetHashCode()
            {
                return A + B + C;
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(TriangleIndices other)
            {
                return A == other.A && B == other.B && C == other.C;
            }
        }

        struct EdgeContact
        {
            public bool ShouldCorrect;
            public Vector3 CorrectedNormal;
            public Edge Edge;
            public ContactData ContactData;
        }

        struct VertexContact
        {
            public bool ShouldCorrect;
            public Vector3 CorrectedNormal;
            public int Vertex;
            public ContactData ContactData;
        }
    }
}
