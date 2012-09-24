using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Core
{
    /// <summary>
    /// Classes implementing this interface get draw calls
    /// </summary>
    public interface IDraw
    {
        /// <summary>
        /// Draws a frame
        /// </summary>
        /// <param name="gameTime"/>
        void Draw(GameTime gameTime);
    }
}