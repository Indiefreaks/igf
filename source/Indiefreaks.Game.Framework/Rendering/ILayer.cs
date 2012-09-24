using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Rendering
{
    public interface ILayer : IDraw
    {
        /// <summary>
        /// Initializes the layer
        /// </summary>
        void Initialize();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        void BeginDraw(GameTime gameTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        void EndDraw(GameTime gameTime);

        /// <summary>
        /// Returns the GameState instance associated with this Layer
        /// </summary>
        GameState GameState { get; }

        /// <summary>
        /// Gets or sets the layer visibility
        /// </summary>
        bool IsVisible { get; set; }
    }
}