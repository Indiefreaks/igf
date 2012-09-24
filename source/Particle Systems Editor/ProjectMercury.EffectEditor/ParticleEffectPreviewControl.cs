/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using ProjectMercury.Renderers;
    using System;
    using System.Diagnostics;
    using System.IO;

    internal enum ImageOptions
    {
        Stretch,
        Center,
        Tile
    }

    internal class ParticleEffectPreviewControl : GraphicsDeviceControl
    {
        private Vector2 Origin { get; set; }

        private ImageOptions ImageOptions { get; set; }

        public ParticleEffect ParticleEffect { get; set; }

        public AbstractRenderer Renderer { get; set; }

        private Vector3 BackgroundColour;

        private SpriteBatch SpriteBatch;
        new private Texture2D BackgroundImage;

        private Matrix World;
        private Matrix View;
        private Matrix Projection;
        private Vector3 CameraPosition = new Vector3(0, 0, 200);

        protected override void Initialize()
        {
            this.SpriteBatch = new SpriteBatch(base.GraphicsDevice);            

            this.World      = Matrix.Identity;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, this.GraphicsDevice.Viewport.AspectRatio, 1f, 5000f);
            this.View       = Matrix.CreateLookAt(this.CameraPosition, new Vector3(0f, 0f, 0f), Vector3.Up);
        }        

        public void SetBackgroundColor(byte r, byte g, byte b)
        {
            this.BackgroundColour.X = r / 255f;
            this.BackgroundColour.Y = g / 255f;
            this.BackgroundColour.Z = b / 255f;
        }

        public void LoadBackgroundImage(string filePath)
        {
            try
            {
                using (FileStream inputStream = File.OpenRead(filePath))
                {
                    this.BackgroundImage = Texture2D.FromStream(base.GraphicsDevice, inputStream);
                }

                this.Origin = new Vector2(BackgroundImage.Width / 2, BackgroundImage.Height / 2);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
        }

        public void ClearBackgroundImage()
        {
            if (this.BackgroundImage != null)
            {
                this.BackgroundImage.Dispose();
                this.BackgroundImage = null;
            }
        }

        protected override void Draw()
        {
            base.GraphicsDevice.Clear(new Color(this.BackgroundColour));

            if (this.BackgroundImage != null)
            {
                this.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

                switch (this.ImageOptions)
                {
                    case ImageOptions.Stretch:
                        {
                            this.SpriteBatch.Draw(this.BackgroundImage, new Rectangle(0, 0, base.ClientSize.Width, base.ClientSize.Height), Color.White);
                         
                            break;
                        }
                    case ImageOptions.Center:
                        {
                            this.SpriteBatch.Draw(this.BackgroundImage, new Rectangle(base.ClientSize.Width / 2, base.ClientSize.Height / 2, this.BackgroundImage.Width, this.BackgroundImage.Height), null, Color.White, 0, this.Origin, SpriteEffects.None, 1);
                         
                            break;
                        }
                    case ImageOptions.Tile:
                        {
                            for (int x = 0; x < this.Width; x += this.BackgroundImage.Width)
                            {
                                for (int y = 0; y < this.Height; y += this.BackgroundImage.Height)
                                {
                                    this.SpriteBatch.Draw(this.BackgroundImage, new Rectangle(x, y, this.BackgroundImage.Width, this.BackgroundImage.Height), Color.White);
                                }
                            }
                         
                            break;
                        }
                }

                this.SpriteBatch.End();
            }

            if (this.Renderer != null)
                if (this.ParticleEffect != null)
                    this.Renderer.RenderEffect(this.ParticleEffect, ref this.World, ref this.View, ref this.Projection, ref this.CameraPosition);
        }

        internal void ImageOptionsChanged(ImageOptions imageOptions)
        {
            this.ImageOptions = imageOptions;
        }
    }
}