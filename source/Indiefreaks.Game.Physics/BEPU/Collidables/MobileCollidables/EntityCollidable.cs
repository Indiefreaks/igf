﻿using BEPUphysics.Collidables.Events;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Entities;
using BEPUphysics.MathExtensions;
using Microsoft.Xna.Framework;
using BEPUphysics.Settings;
using System;
using BEPUphysics.PositionUpdating;

namespace BEPUphysics.Collidables.MobileCollidables
{
    ///<summary>
    /// Mobile collidable acting as a collision proxy for an entity.
    ///</summary>
    public abstract class EntityCollidable : MobileCollidable
    {
        protected EntityCollidable()
        {
            //This constructor is used when the subclass is going to set the shape after doing some extra initialization.
            Events = new ContactEventManager<EntityCollidable>(this);
        }

        protected EntityCollidable(EntityShape shape)
            : this()
        {
            base.Shape = shape;
        }





        /// <summary>
        /// Gets the shape of the collidable.
        /// </summary>
        public new EntityShape Shape
        {
            get
            {
                return (EntityShape)shape;
            }
            protected set
            {
                base.Shape = value;
            }
        }

        protected internal Entity entity;
        ///<summary>
        /// Gets the entity owning the collidable.
        ///</summary>
        public Entity Entity
        {
            get
            {
                return entity;
            }
            protected internal set
            {
                entity = value;
                OnEntityChanged();
            }
        }


        protected virtual void OnEntityChanged()
        {
        }

        protected internal RigidTransform worldTransform;
        ///<summary>
        /// Gets or sets the world transform of the collidable.
        ///</summary>
        public RigidTransform WorldTransform
        {
            get
            {
                return worldTransform;
            }
            set
            {
                UpdateWorldTransform(ref value.Position, ref value.Orientation);
            }
        }

        protected internal override bool IsActive
        {
            get
            {
                return entity != null ? entity.activityInformation.IsActive : false;
            }
        }

        protected internal Vector3 localPosition;
        ///<summary>
        /// Gets or sets the local position of the collidable.
        /// The local position can be used to offset the collision geometry
        /// from an entity's center of mass.
        ///</summary>
        public Vector3 LocalPosition
        {
            get
            {
                return localPosition;
            }
            set
            {
                localPosition = value;
            }
        }

        /// <summary>
        /// Updates the bounding box to the current state of the entry.
        /// </summary>
        public override void UpdateBoundingBox()
        {
            UpdateBoundingBox(0);
        }

        ///<summary>
        /// Updates the bounding box of the mobile collidable.
        ///</summary>
        ///<param name="dt">Timestep with which to update the bounding box.</param>
        public override void UpdateBoundingBox(float dt)
        {
            //The world transform update isn't strictly required for uninterrupted simulation.
            //The entity update method manages the world transforms.
            //However, the redundancy allows a user to change the position in between frames.
            //If the order of the space update changes to position-update-first, this is completely unnecessary.
            UpdateWorldTransform(ref entity.position, ref entity.orientation);
            UpdateBoundingBoxInternal(dt);
        }

        ///<summary>
        /// Updates the world transform of the collidable.
        ///</summary>
        ///<param name="position">Position to use for the calculation.</param>
        ///<param name="orientation">Orientation to use for the calculation.</param>
        public virtual void UpdateWorldTransform(ref Vector3 position, ref Quaternion orientation)
        {
            Vector3.Transform(ref localPosition, ref orientation, out worldTransform.Position);
            Vector3.Add(ref worldTransform.Position, ref position, out worldTransform.Position);
            worldTransform.Orientation = orientation;
        }

        /// <summary>
        /// Updates the collidable's world transform and bounding box.
        /// This is a convenience method for external modification of the collidable's data.
        /// </summary>
        /// <param name="transform">Transform to use for the collidable.</param>
        /// <param name="dt">Duration of the simulation time step.  Used to expand the
        /// bounding box using the owning entity's velocity.  If the collidable
        /// does not have an owning entity, this must be zero.</param>
        public void UpdateBoundingBoxForTransform(ref RigidTransform transform, float dt)
        {
            worldTransform = transform;
            UpdateBoundingBoxInternal(dt);
        }




        protected internal abstract void UpdateBoundingBoxInternal(float dt);

        //Helper method for mobile collidables.
        internal void ExpandBoundingBox(ref BoundingBox boundingBox, float dt)
        {
            //Expand bounding box with velocity.
            if (dt > 0)
            {
                bool useExtraExpansion = MotionSettings.UseExtraExpansionForContinuousBoundingBoxes && entity.PositionUpdateMode == PositionUpdateMode.Continuous;
                float velocityScaling = useExtraExpansion ? 2 : 1;
                if (entity.linearVelocity.X > 0)
                    boundingBox.Max.X += entity.linearVelocity.X * dt * velocityScaling;
                else
                    boundingBox.Min.X += entity.linearVelocity.X * dt * velocityScaling;

                if (entity.linearVelocity.Y > 0)
                    boundingBox.Max.Y += entity.linearVelocity.Y * dt * velocityScaling;
                else
                    boundingBox.Min.Y += entity.linearVelocity.Y * dt * velocityScaling;

                if (entity.linearVelocity.Z > 0)
                    boundingBox.Max.Z += entity.linearVelocity.Z * dt * velocityScaling;
                else
                    boundingBox.Min.Z += entity.linearVelocity.Z * dt * velocityScaling;




                if (useExtraExpansion)
                {
                    float expansion = 0;
                    //It's possible that an object could have a small bounding box since its own
                    //velocity is low, but then a collision with a high velocity object sends
                    //it way out of its bounding box.  By taking into account high velocity objects
                    //in danger of hitting us and expanding our own bounding box by their speed,
                    //we stand a much better chance of not missing secondary collisions.
                    foreach (var e in OverlappedEntities)
                    {

                        float velocity = e.linearVelocity.LengthSquared();
                        if (velocity > expansion)
                            expansion = velocity;
                    }
                    expansion = (float)Math.Sqrt(expansion) * dt;


                    boundingBox.Min.X -= expansion;
                    boundingBox.Min.Y -= expansion;
                    boundingBox.Min.Z -= expansion;

                    boundingBox.Max.X += expansion;
                    boundingBox.Max.Y += expansion;
                    boundingBox.Max.Z += expansion;

                }

                //Could use this to incorporate angular motion.  Since the bounding box is an approximation to begin with,
                //this isn't too important.  If an updating system is used where the bounding box MUST fully contain the frame's motion
                //then the commented area should be used.
                //Math.Min(entity.angularVelocity.Length() * dt, Shape.maximumRadius) * velocityScaling;
                //TODO: consider using minimum radius 

            }
        }



        protected override void CollisionRulesUpdated()
        {
            //Try to activate the entity since our collision rules just changed; broadphase might need to update some stuff.
            //Beware, though; if this collidable is still being constructed, then the entity won't be available.
            if (entity != null)
                entity.activityInformation.Activate();
        }


        protected internal ContactEventManager<EntityCollidable> events;
        ///<summary>
        /// Gets or sets the event manager of the collidable.
        ///</summary>
        public ContactEventManager<EntityCollidable> Events
        {
            get
            {
                return events;
            }
            set
            {
                events = value;
            }
        }
        protected internal override IContactEventTriggerer EventTriggerer
        {
            get { return events; }
        }


        ///<summary>
        /// Gets an enumerable collection of all entities overlapping this collidable.
        ///</summary>
        public EntityCollidableCollection OverlappedEntities
        {
            get
            {
                return new EntityCollidableCollection(this);
            }
        }


    }
}
