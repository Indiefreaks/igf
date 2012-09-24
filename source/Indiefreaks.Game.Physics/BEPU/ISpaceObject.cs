﻿namespace BEPUphysics
{
    ///<summary>
    /// Defines an object which can be managed by an ISpace.
    ///</summary>
    public interface ISpaceObject
    {
        /// <summary>
        /// Gets the Space to which the object belongs.
        /// </summary>
        ISpace Space { get; set; }
        /// <summary>
        /// Called after the object is added to a space.
        /// </summary>
        /// <param name="newSpace"></param>
        void OnAdditionToSpace(ISpace newSpace);
        /// <summary>
        /// Called before an object is removed from its space.
        /// </summary>
        void OnRemovalFromSpace(ISpace oldSpace);
        /// <summary>
        /// Gets or sets the user data associated with this object.
        /// </summary>
        object Tag { get; set; }
    }
}
