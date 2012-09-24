﻿using System;
using System.Collections.Generic;
using BEPUphysics.BroadPhaseSystems;
using BEPUphysics.Collidables;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Constraints;
using BEPUphysics.Constraints.Collision;
using BEPUphysics.DataStructures;
using BEPUphysics.ResourceManagement;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.CollisionTests;

namespace BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a compound and group collision pair.
    ///</summary>
    public abstract class CompoundGroupPairHandler : GroupPairHandler
    {
        protected CompoundCollidable compoundInfo;

        protected override Collidable CollidableA
        {
            get { return compoundInfo; }
        }
        protected override Entities.Entity EntityA
        {
            get { return compoundInfo.entity; }
        }



        ///<summary>
        /// Initializes the pair handler.
        ///</summary>
        ///<param name="entryA">First entry in the pair.</param>
        ///<param name="entryB">Second entry in the pair.</param>
        public override void Initialize(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {
            //Other member of the pair is initialized by the child.
            compoundInfo = entryA as CompoundCollidable;
            if (compoundInfo == null)
            {
                compoundInfo = entryB as CompoundCollidable;
                if (compoundInfo == null)
                {
                    throw new Exception("Inappropriate types used to initialize pair.");
                }
            }

            base.Initialize(entryA, entryB);
        }

        ///<summary>
        /// Cleans up the pair handler.
        ///</summary>
        public override void CleanUp()
        {
            base.CleanUp();

            compoundInfo = null;
            //Child type needs to null out other reference.
        }



    }
}
