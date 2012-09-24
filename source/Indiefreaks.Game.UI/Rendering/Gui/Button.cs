using System;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Indiefreaks.Xna.Rendering.Gui
{
    /// <summary>
    /// The Button control provides a simple control that can be selected and clicked
    /// </summary>
    public class Button : Control
    {
        /// <summary>
        /// The Normal Control skin
        /// </summary>
        public ButtonSkin Normal;

        /// <summary>
        /// The Pressed Control skin
        /// </summary>
        public ButtonSkin Pressed;

        /// <summary>
        /// The Selected Control skin
        /// </summary>
        public ButtonSkin Selected;

        private string _text;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="fontPath">The font used by this control and its inner default ButtonSkins</param>
        /// <param name="text">The initial text for this button</param>
        public Button(string fontPath, string text)
        {
            Normal = new ButtonSkin(fontPath);
            Pressed = new ButtonSkin(fontPath) {TextColor = Color.Green};
            Selected = new ButtonSkin(fontPath) {TextColor = Color.Red};

            Text = text;
            State = ButtonState.Normal;

            FocusAcquired += OnFocusAcquired;
            FocusLost += OnFocusLost;
            Clicked += OnButtonClicked;
        }

        public override Control Clone()
        {
            var button = new Button("", "") {Normal = Normal, Pressed = Pressed, Selected = Selected};

            return button;
        }

        /// <summary>
        /// Tells if the current control can receive focus or not
        /// </summary>
        public override bool CanFocus
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the current State of the Control
        /// </summary>
        public ButtonState State { get; set; }

        /// <summary>
        /// Gets or sets the text of the Control
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
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="e"></param>
        private void OnButtonClicked(object button, EventArgs e)
        {
            if (((Button) button).State != ButtonState.Pressed)
            {
                ((Button) button).State = ButtonState.Pressed;
                Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="e"></param>
        private void OnFocusLost(object button, EventArgs e)
        {
            if (((Button) button).State != ButtonState.Normal)
            {
                ((Button) button).State = ButtonState.Normal;
                Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="e"></param>
        private void OnFocusAcquired(object button, EventArgs e)
        {
            if (((Button) button).State != ButtonState.Selected)
            {
                ((Button) button).State = ButtonState.Selected;
                Invalidate();
            }
        }

        /// <summary>
        /// Renders the control
        /// </summary>
        /// <param name="spriteRenderer"></param>
        public override void Render(SpriteBatch spriteRenderer)
        {
            base.Render(spriteRenderer);

            // what is the current state of the Control
            ButtonSkin state = State == ButtonState.Selected
                                   ? State == ButtonState.Pressed ? Pressed : Selected
                                   : Normal;

            // Render first the background texture if any
            if (state.BackgroundTexture != null)
                spriteRenderer.Draw(state.BackgroundTexture, Vector2.Zero, null,
                                    new Color(1.0f, 1.0f, 1.0f, state.BackgroundTextureAlpha), 0f, Vector2.Zero, Scale,
                                    SpriteEffects.None, 0);
            // Render the text
            spriteRenderer.DrawString(state.Font, _text, Vector2.Zero, state.TextColor, 0f, state.TextOffset, Scale,
                                      SpriteEffects.None, 1);
        }

        /// <summary>
        /// Refreshes the control properties when it requires to be redrawn to the RenderTarget
        /// </summary>
        /// <param name="device"></param>
        public override void Refresh(GraphicsDevice device)
        {
            // what is the current state of the Control
            ButtonSkin state = State == ButtonState.Selected
                                   ? State == ButtonState.Pressed ? Pressed : Selected
                                   : Normal;

            // we retrieve the size of the text
            Vector2 textSize = state.Font.MeasureString(_text);

            Vector2 backgroundSize = state.BackgroundTexture != null
                                         ? new Vector2(state.BackgroundTexture.Width, state.BackgroundTexture.Height)
                                         : Vector2.One;

            // we resize the Control to the highest size between the text size and the background texture size.
            Width = (int) (Math.Max(textSize.X - state.TextOffset.X, backgroundSize.X)*Scale.X);
            Height = (int) (Math.Max(textSize.Y - state.TextOffset.Y, backgroundSize.Y)*Scale.Y);
        }

        #region Implementation of IContentHost

        /// <summary>
        ///   Load all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        /// <param name = "manager">XNA content manage</param>
        public override void LoadContent(IContentCatalogue catalogue, ContentManager manager)
        {
            if (!string.IsNullOrEmpty(Normal.BackgroundTexturePath))
                Normal.BackgroundTexture = manager.Load<Texture2D>(Normal.BackgroundTexturePath);
            if (!string.IsNullOrEmpty(Selected.BackgroundTexturePath))
                Selected.BackgroundTexture = manager.Load<Texture2D>(Selected.BackgroundTexturePath);
            if (!string.IsNullOrEmpty(Pressed.BackgroundTexturePath))
                Pressed.BackgroundTexture = manager.Load<Texture2D>(Pressed.BackgroundTexturePath);
            if (!string.IsNullOrEmpty(Normal.FontPath))
                Normal.Font = manager.Load<SpriteFont>(Normal.FontPath);
            if (!string.IsNullOrEmpty(Selected.FontPath))
                Selected.Font = manager.Load<SpriteFont>(Selected.FontPath);
            if (!string.IsNullOrEmpty(Pressed.FontPath))
                Pressed.Font = manager.Load<SpriteFont>(Pressed.FontPath);

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