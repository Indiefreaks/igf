using System;
using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Indiefreaks.Xna.Rendering.Gui
{
    public interface IGuiElement
    {
        /// <summary>
        /// Returns the Width of the current control
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Returns the Height of the current control
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets or sets the left screen coordinate position of the control
        /// </summary>
        int X { get; set; }

        /// <summary>
        /// Gets or sets the top screen coordinate position of the control
        /// </summary>
        int Y { get; set; }

        /// <summary>
        /// Gets or sets the visibility of this control
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Returns the cached rendered control as a texture
        /// </summary>
        Texture2D Texture { get; }

        /// <summary>
        /// Tells if the current control can receive focus or not
        /// </summary>
        bool CanFocus { get; }

        /// <summary>
        /// Returns if the current control has focus or not
        /// </summary>
        bool HasFocus { get; }

        /// <summary>
        /// Renders the control
        /// </summary>
        /// <param name="spriteRenderer"></param>
        void Render(SpriteBatch spriteRenderer);

        /// <summary>
        /// Refreshes the control properties when it requires to be redrawn to the RenderTarget
        /// </summary>
        /// <param name="device"></param>
        void Refresh(GraphicsDevice device);

        /// <summary>
        /// Creates or recreate the RenderTarget where the screen will be rendered
        /// </summary>
        /// <param name="device"></param>
        void PrepareRenderTarget(GraphicsDevice device);

        /// <summary>
        /// Marks this control as requiring to be refreshed
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Raised when the Control acquired focus and switches to Selected
        /// </summary>
        event EventHandler FocusAcquired;

        /// <summary>
        /// Raised when the Control loses focus and switches back to Normal
        /// </summary>
        event EventHandler FocusLost;

        /// <summary>
        /// Sets focus to the current control
        /// </summary>
        void SetFocus();

        /// <summary>
        /// Removes focus from the current control
        /// </summary>
        void LoseFocus();

        /// <summary>
        /// Handles the input events from the player controlling the current control
        /// </summary>
        /// <param name="input">The current player input states</param>
        /// <param name="gameTime">The GameTime instance</param>
        void HandleInput(PlayerInput input, GameTime gameTime);
    }
}