/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Renderers
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a unit of work for a particle renderer to complete.
    /// </summary>
    public struct RenderContext
    {
        /// <summary>
        /// A flag to decide if particles should be billboarded or not
        /// </summary>
        public readonly BillboardStyle BillboardStyle;

        /// <summary>
        /// A flag to decide if particles should be billboarded or not
        /// </summary>
        public readonly Vector3 BillboardRotationalAxis;

        /// <summary>
        /// Gets the blend state required for rendering.
        /// </summary>
        public readonly BlendState BlendState;

        /// <summary>
        /// Gets a reference to the texture required for each particle.
        /// </summary>
        public readonly Texture2D Texture;

        /// <summary>
        /// Gets the world matrix.
        /// </summary>
        public readonly Matrix World;

        /// <summary>
        /// Gets the view matrix.
        /// </summary>
        public readonly Matrix View;

        /// <summary>
        /// Gets the projection matrix.
        /// </summary>
        public readonly Matrix Projection;

        /// <summary>
        /// Gets the camera position.
        /// </summary>
        public readonly Vector3 CameraPosition;

        /// <summary>
        /// A quick flag to know if we were given an identity world
        /// </summary>
        public readonly bool WorldIsIdentity;

        /// <summary>
        /// The number of particles that will be drawn
        /// </summary>
        public readonly int Count;

        /// <summary>
        /// DO NOT USE THIS: DIRTY HACK : WILL BE REPLACED
        /// </summary>
        public bool UseVelocityAsBillboardAxis;

        /// <summary>
        /// Initialises a new instance of the RenderContext structure.
        /// </summary>
        /// <param name="billboardStyle">What style of billboard rendering to use</param>
        /// <param name="billboardRotationalAxis">The rotational axis to use for cylindrical billboarding</param>
        /// <param name="blendState">The desired blend state.</param>
        /// <param name="texture">The texture to use when rendering particles.</param>
        /// <param name="world">The world transform matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="cameraPosition">The camera position.</param>
        /// <param name="count">Numer of particles that will be drawn</param>
        /// <param name="useVelocityAsBillboardAxis">DO NOT USE: Will be replaced in future version</param>
        internal RenderContext(BillboardStyle billboardStyle, Vector3 billboardRotationalAxis, BlendState blendState, Texture2D texture, ref Matrix world, ref Matrix view, ref Matrix projection, ref Vector3 cameraPosition, int count, bool useVelocityAsBillboardAxis)
        {
            this.BillboardRotationalAxis = billboardRotationalAxis;
            this.UseVelocityAsBillboardAxis = useVelocityAsBillboardAxis;
            this.BillboardStyle = billboardStyle;
            this.BlendState = blendState;
            this.Texture        = texture;
            this.World          = world;
            this.WorldIsIdentity = (world == Matrix.Identity);
            this.View = view;
            this.Projection     = projection;
            this.CameraPosition = cameraPosition;
            this.WorldIsIdentity = (world == Matrix.Identity);
            this.Count = count;
        }
    }
}