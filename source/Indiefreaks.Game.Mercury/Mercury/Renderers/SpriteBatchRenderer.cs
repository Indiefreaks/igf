/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Renderers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a renderer which renders particles using an XNA SpriteBatch object.
    /// </summary>
    [TypeDescriptionProvider("ProjectMercury.Design.TypeDescriptorFactory, ProjectMercury.Design, Version=4.0.0.0")]
    public sealed class SpriteBatchRenderer : AbstractRenderer
    {
        /// <summary>
        /// Gets or sets the SpriteBatch object.
        /// </summary>
        private SpriteBatch SpriteBatch;

        /// <summary>
        /// Gets or sets the transformation matrix.
        /// </summary>
        public Matrix Transformation { get; set; }

        /// <summary>
        /// Loads any content items needed by the renderer.
        /// </summary>
        /// <param name="content">A content manager instance.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public override void LoadContent(ContentManager content)
        {
            Check.True(base.GraphicsDeviceService != null, "GraphicsDeviceService property has not been initialised with a valid value!");

            this.SpriteBatch = new SpriteBatch(base.GraphicsDeviceService.GraphicsDevice);
        }

        /// <summary>
        /// Performs rendering of particles.
        /// </summary>
        /// <param name="iterator">The particle iterator object.</param>
        /// <param name="context">The render context containing rendering information.</param>
        protected override void Render(ref RenderContext context, ref ParticleIterator iterator)
        {
            Vector2 origin = new Vector2(context.Texture.Width / 2f, context.Texture.Height / 2f);

            this.SpriteBatch.Begin(SpriteSortMode.Deferred, context.BlendState, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, this.Transformation);
            {
#if UNSAFE
                unsafe
                {
                    Particle* particle = iterator.First;
#else
                    Particle particle = iterator.First;
#endif
                    do
                    {
#if UNSAFE
                        Single scale = particle->Scale / context.Texture.Width;

                        Vector2 position = new Vector2
                        {
                            X = particle->Position.X,
                            Y = particle->Position.Y
                        };

                        this.SpriteBatch.Draw(context.Texture, position, null, new Color(particle->Colour), particle->Rotation.Z, origin, scale, SpriteEffects.None, 0f);
#else
                        Single scale = particle.Scale / context.Texture.Width;

                        Vector2 position = new Vector2
                        {
                            X = particle.Position.X,
                            Y = particle.Position.Y
                        };

                        this.SpriteBatch.Draw(context.Texture, position, null, new Color(particle.Colour), particle.Rotation.Z, origin, scale, SpriteEffects.None, 0f);
#endif
                    }
#if UNSAFE
                    while (iterator.MoveNext(&particle));
#else
                    while (iterator.MoveNext(ref particle));
#endif
#if UNSAFE
                }
#endif
                this.SpriteBatch.End();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.SpriteBatch != null)
                {
                    if (this.SpriteBatch.IsDisposed == false)
                        this.SpriteBatch.Dispose();

                    this.SpriteBatch = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}