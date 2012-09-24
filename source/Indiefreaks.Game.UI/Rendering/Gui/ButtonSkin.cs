using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Indiefreaks.Xna.Rendering.Gui
{
    /// <summary>
    /// A Skin to define how Button should render in a given state
    /// </summary>
    public class ButtonSkin
    {
        internal string BackgroundTexturePath;
        internal string FontPath;
        private float _backgroundTextureAlpha = 1.0f;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="fontPath"></param>
        public ButtonSkin(string fontPath)
            : this(string.Empty, fontPath)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="backgroundTexturePath"></param>
        /// <param name="fontPath"></param>
        public ButtonSkin(string backgroundTexturePath, string fontPath)
        {
            BackgroundTexturePath = backgroundTexturePath;
            FontPath = fontPath;
            TextColor = Color.White;
        }

        /// <summary>
        /// Returns the texture used to render background of the Control
        /// </summary>
        public Texture2D BackgroundTexture { get; internal set; }

        /// <summary>
        /// Gets or sets the Alpha value that should be used to render the background
        /// </summary>
        public float BackgroundTextureAlpha
        {
            get { return _backgroundTextureAlpha; }
            set { _backgroundTextureAlpha = MathHelper.Clamp(value, 0, 1); }
        }

        /// <summary>
        /// Returns the SpriteFont used to render text
        /// </summary>
        public SpriteFont Font { get; internal set; }

        /// <summary>
        /// Gets or sets the color to be used to render text
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// What should be the offset position of the text
        /// </summary>
        public Vector2 TextOffset { get; set; }
    }
}