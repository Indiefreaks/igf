﻿using System;
using System.Collections.Generic;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseSystems;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.Constraints;
using BEPUphysics.Constraints.Collision;
using BEPUphysics.DataStructures;
using BEPUutilities.DataStructures;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.CollisionTests;

namespace BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a compound-compound collision pair.
    ///</summary>
    public class StaticGroupCompoundPairHandler : StaticGroupPairHandler
    {

        CompoundCollidable compoundInfoB;

        public override Collidable CollidableB
        {
            get { return compoundInfoB; }
        }

        public override Entities.Entity EntityB
        {
            get { return compoundInfoB.entity; }
        }


        ///<summary>
        /// Initializes the pair handler.
        ///</summary>
        ///<param name="entryA">First entry in the pair.</param>
        ///<param name="entryB">Second entry in the pair.</param>
        public override void Initialize(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {
            compoundInfoB = entryB as CompoundCollidable;
            if (compoundInfoB == null)
            {
                throw new Exception("Inappropriate types used to initialize pair.");
            }

            base.Initialize(entryA, entryB);
        }


        ///<summary>
        /// Cleans up the pair handler.
        ///</summary>
        public override void CleanUp()
        {

            base.CleanUp();
            compoundInfoB = null;


        }

        //Some danger of unintuitive-to-address allocations here.  If these lists get huge, the user will see some RawList<<>> goofiness in the profiler.
        //They can still address it by clearing out the cached pair factories though.
        RawList<TreeOverlapPair<Collidable, CompoundChild>> overlappedElements = new RawList<TreeOverlapPair<Collidable, CompoundChild>>();
        protected override void UpdateContainedPairs()
        {
            staticGroup.Shape.CollidableTree.GetOverlaps(compoundInfoB.hierarchy.Tree, overlappedElements);
            for (int i = 0; i < overlappedElements.Count; i++)
            {
                var element = overlappedElements.Elements[i];
                var staticCollidable = element.OverlapA as StaticCollidable;
                TryToAdd(element.OverlapA, element.OverlapB.CollisionInformation,
                    staticCollidable != null ? staticCollidable.Material : staticGroup.Material, element.OverlapB.Material);
            }
            overlappedElements.Clear();
        }


    }
}
