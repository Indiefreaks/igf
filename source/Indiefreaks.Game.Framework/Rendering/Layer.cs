using System;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Rendering
{
    /// <summary>
    /// Represents a Rendering layer. Used to separate multiple rendering logic.
    /// </summary>
    /// <remarks>
    /// Each Layer instance is added to its GameState in a given order and its Update and Draw methods are called using the same.
    /// This allows the developer to, for instance, have:
    /// - A layer responsible to render a Skybox
    /// - A layer responsible to render the 3D world
    /// - A layer responsible to render a Radar
    /// - A layer responsible to render some HUD
    /// - A layer responsible to render in game pause menu
    /// </remarks>
    public class Layer : ILayer
    {
        public Layer(GameState gameState)
        {
            GameState = gameState;
            IsVisible = true;
        }

        #region ILayer Members

        /// <summary>
        /// Initializes the layer
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void BeginDraw(GameTime gameTime)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void EndDraw(GameTime gameTime)
        {
        }

        /// <summary>
        /// Returns the GameState instance associated with this Layer
        /// </summary>
        public GameState GameState { get; private set; }

        /// <summary>
        /// Gets or sets the layer visibility
        /// </summary>
        public virtual bool IsVisible { get; set; }

        #endregion
        
        #region Implementation of IDraw

        /// <summary>
        /// Draws a frame
        /// </summary>
        /// <param name="gameTime"/>
        public virtual void Draw(GameTime gameTime)
        {
        }

        #endregion
    }
}