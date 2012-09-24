using System;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Indiefreaks.Xna.Rendering.Gui
{
    /// <summary>
    /// The Image control displays a texture
    /// </summary>
    /// <remarks>Make sure to set the Scale correctly as it is used to define the width and height of the control</remarks>
    public class Image : Control
    {
        private readonly string _texturePath;
        private Texture2D _texture;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="texturePath">The path to the texture to be used</param>
        public Image(string texturePath)
        {
            _texturePath = texturePath;
            Color = Color.White;
        }

        /// <summary>
        /// The color used to render the texture
        /// </summary>
        public Color Color { get; set; }

        #region Implementation of IContentHost

        /// <summary>
        ///   Load all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        /// <param name = "manager">XNA content manage</param>
        public override void LoadContent(IContentCatalogue catalogue, ContentManager manager)
        {
            _texture = manager.Load<Texture2D>(_texturePath);
            
            ((IGuiElement) this).Refresh(Application.Graphics.GraphicsDevice);
        }

        /// <summary>
        ///   Unload all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        public override void UnloadContent(IContentCatalogue catalogue)
        {
        }

        #endregion

        public override Control Clone()
        {
            var image = new Image(_texturePath) {_texture = _texture, Color = Color, Scale = Scale};

            return image;
        }

        /// <summary>
        /// Renders the control
        /// </summary>
        /// <param name="spriteRenderer"></param>
        public override void Render(SpriteBatch spriteRenderer)
        {
            base.Render(spriteRenderer);

            spriteRenderer.Draw(_texture, new Rectangle(0, 0, Width, Height), Color);
        }

        /// <summary>
        /// Refreshes the control properties when it requires to be redrawn to the RenderTarget
        /// </summary>
        /// <param name="device"></param>
        public override void Refresh(GraphicsDevice device)
        {
            Width = _texture.Width;
            Height = _texture.Height;
            Width = (int)(Width * Scale.X);
            Height = (int)(Height * Scale.Y);
            //Width = Scale.X > 1 ? (int) Scale.X : 1;
            //Height = Scale.Y > 1 ? (int) Scale.Y : 1;
        }
    }
}