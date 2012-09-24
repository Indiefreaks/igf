using System;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Indiefreaks.Xna.Rendering.Gui
{
    /// <summary>
    /// The Label is a simple control that displays text. It can't receive focus
    /// </summary>
    public class Label : Control
    {
        private readonly string _fontPath;
        private SpriteFont _spriteFont;
        private string _text;
        private Color _textColor;

        /// <summary>
        /// Creates a new instance with a font
        /// </summary>
        /// <param name="fontPath">The path to the font used by this Label</param>
        /// <param name="text">The text to display</param>
        public Label(string fontPath, string text)
        {
            _fontPath = fontPath;

            Text = text;
            TextColor = Color.White;
        }

        /// <summary>
        /// Gets or sets the text for this control
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the text
        /// </summary>
        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    Invalidate();
                }
            }
        }

        public SpriteFont Font
        {
            get { return _spriteFont; }
        }

        public override Control Clone()
        {
            var label = new Label(_fontPath, "") {_textColor = _textColor, _spriteFont = _spriteFont};

            return label;
        }

        /// <summary>
        /// Renders the control
        /// </summary>
        /// <param name="spriteRenderer"></param>
        public override void Render(SpriteBatch spriteRenderer)
        {
            base.Render(spriteRenderer);

            spriteRenderer.DrawString(_spriteFont, _text, Vector2.Zero, TextColor, 0f, Vector2.Zero, Scale,
                                      SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Refreshes the control properties when it requires to be redrawn to the RenderTarget
        /// </summary>
        /// <param name="device"></param>
        public override void Refresh(GraphicsDevice device)
        {
            // we resize the control so that we can see the whole text on screen
            Vector2 textSize = _spriteFont.MeasureString(_text);
            Width = textSize.X > 0 ? (int) (textSize.X*Scale.X) : 1;
            Height = textSize.Y > 0 ? (int) (textSize.Y*Scale.Y) : 1;
        }

        #region Implementation of IContentHost

        /// <summary>
        ///   Load all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        /// <param name = "manager">XNA content manage</param>
        public override void LoadContent(IContentCatalogue catalogue, ContentManager manager)
        {
            _spriteFont = manager.Load<SpriteFont>(_fontPath);

            ((IGuiElement)this).Refresh(Application.Graphics.GraphicsDevice);
        }

        /// <summary>
        ///   Unload all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        public override void UnloadContent(IContentCatalogue catalogue)
        {
        }

        #endregion
    }
}