/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

using ProjectMercury.Proxies;

namespace ProjectMercury.Renderers
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using ProjectMercury.Emitters;

    /// <summary>
    /// Defines the abstract base class for a particle effect renderer.
    /// </summary>
    public abstract class AbstractRenderer : IDisposable
    {
        /// <summary>
        /// Gets or sets a reference to the graphics device service.
        /// </summary>
        public IGraphicsDeviceService GraphicsDeviceService { get; set; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing) { }

        /// <summary>
        /// Dispose any unmanaged resources being used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="AbstractRenderer"/>
        /// is reclaimed by garbage collection.
        /// </summary>
        ~AbstractRenderer()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Loads any content items needed by the renderer.
        /// </summary>
        /// <param name="content">A content manager instance.</param>
        public virtual void LoadContent(ContentManager content) { }

        /// <summary>
        /// Allows the renderer class to perform one time set up for each particle effect.
        /// </summary>
        protected virtual void PreRender(ref Matrix worldMatrix, ref Matrix viewMatrix, ref Matrix projectionMatrix) { }

        /// <summary>
        /// Performs rendering of particles.
        /// </summary>
        /// <param name="context">The render context containing rendering information.</param>
        /// <param name="iterator">The particle iterator object.</param>
        protected abstract void Render(ref RenderContext context, ref ParticleIterator iterator);

        /// <summary>
        /// Renders the specified particle effect.
        /// </summary>
        /// <param name="effect">The particle effect to render.</param>
        /// <param name="worldMatrix">The world transformation matrix.</param>
        /// <param name="viewMatrix">The view matrix.</param>
        /// <param name="projectionMatrix">The projection matrix.</param>
        /// <param name="cameraPosition">The camera matrix.</param>
        public void RenderEffect(ParticleEffect effect, ref Matrix worldMatrix,
                                                        ref Matrix viewMatrix,
                                                        ref Matrix projectionMatrix,
                                                        ref Vector3 cameraPosition)
        {
#if UNSAFE
            unsafe
#endif
            {
                this.PreRender(ref worldMatrix, ref viewMatrix, ref projectionMatrix);

                //Pre-multiply any proxies world matrices with the passed in world matrix
                if (effect.Proxies != null && worldMatrix != Matrix.Identity)
                {
                    effect.SetFinalWorld(ref worldMatrix);
                }

                for (Int32 i = 0; i < effect.Emitters.Count; i++)
                {
                    AbstractEmitter emitter = effect.Emitters[i];

                    // Skip if the emitter does not have a texture...
                    if (emitter.ParticleTexture == null)
                        continue;

                    // Skip if the emitter blend mode is set to 'None'...
                    if (emitter.BlendMode == EmitterBlendMode.None)
                        continue;

                    // Skip if the emitter has no active particles...
                    if (emitter.ActiveParticlesCount == 0)
                        continue;

                    BlendState blendState = BlendStateFactory.GetBlendState(emitter.BlendMode);



                    RenderContext context = new RenderContext(emitter.BillboardStyle, emitter.BillboardRotationalAxis, blendState, emitter.ParticleTexture, ref worldMatrix, ref viewMatrix, ref projectionMatrix, ref cameraPosition, emitter.ActiveParticlesCount, emitter.UseVelocityAsBillboardAxis);

                    Counters.ParticlesDrawn += emitter.ActiveParticlesCount;
#if UNSAFE
                    fixed (Particle* buffer = emitter.Particles)
#else
                    Particle[] buffer = emitter.Particles;
#endif
                    {
                        ParticleIterator iterator = new ParticleIterator(buffer, emitter.Budget, emitter.ActiveIndex, emitter.ActiveParticlesCount);

                        this.Render(ref context, ref iterator);
                    }
                }
            }
        }
    }
}