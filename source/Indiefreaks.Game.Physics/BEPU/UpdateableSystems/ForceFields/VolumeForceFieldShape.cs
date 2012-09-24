using System.Collections.Generic;
using BEPUphysics.BroadPhaseSystems;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Entities;
using BEPUphysics.DataStructures;

namespace BEPUphysics.UpdateableSystems.ForceFields
{
    /// <summary>
    /// Defines the area in which a force field works using an entity's shape.
    /// </summary>
    public class VolumeForceFieldShape : ForceFieldShape
    {
        private readonly RawList<Entity> affectedEntities = new RawList<Entity>();
        private readonly RawList<BroadPhaseEntry> affectedEntries = new RawList<BroadPhaseEntry>();

        /// <summary>
        /// Constructs a new force field shape using a detector volume.
        /// </summary>
        /// <param name="volume">Volume to use.</param>
        public VolumeForceFieldShape(DetectorVolume volume)
        {
            Volume = volume;
        }

        /// <summary>
        /// Gets or sets the volume used by the shape.
        /// </summary>
        public DetectorVolume Volume { get; set; }

        /// <summary>
        /// Determines the possibly involved entities.
        /// </summary>
        /// <returns>Possibly involved entities.</returns>
        public override IList<Entity> GetPossiblyAffectedEntities()
        {
            affectedEntities.Clear();
            ForceField.QueryAccelerator.GetEntries(Volume.TriangleMesh.Tree.BoundingBox, affectedEntries);
            for (int i = 0; i < affectedEntries.count; i++)
            {
                var EntityCollidable = affectedEntries[i] as EntityCollidable;
                if (EntityCollidable != null)
                    affectedEntities.Add(EntityCollidable.Entity);
            }
            affectedEntries.Clear();
            return affectedEntities;
        }

        /// <summary>
        /// Determines if the entity is affected by the force field.
        /// </summary>
        /// <param name="testEntity">Entity to test.</param>
        /// <returns>Whether the entity is affected.</returns>
        public override bool IsEntityAffected(Entity testEntity)
        {
            //TODO: Use boolean-only function map to speed this up
            return Volume.IsEntityIntersectingVolume(testEntity);
        }
    }
}