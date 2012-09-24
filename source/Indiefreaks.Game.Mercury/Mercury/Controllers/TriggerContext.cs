/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Encapsulates an emitter trigger.
    /// </summary>
    public struct TriggerContext
    {
        /// <summary>
        /// True if the trigger should be cancelled.
        /// </summary>
        public Boolean Cancelled;

        /// <summary>
        /// Gets or sets the position of the trigger.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Gets or sets the rotation of the trigger.
        /// </summary>
        public Vector3 Rotation;

        /// <summary>
        /// Gets or sets the number of particles which should be released.
        /// </summary>
        public Int32 ReleaseQuantity;

        /// <summary>
        /// A proxy index for the particles.
        /// </summary>
        public Int32 ProxyIndex;
    }
}