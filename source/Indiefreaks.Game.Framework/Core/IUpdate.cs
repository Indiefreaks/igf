using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Core
{
    /// <summary>
    /// Classes inheriting from this interface get update calls
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// Update loop call
        /// </summary>
        /// <param name="gameTime"/>
        void Update(GameTime gameTime);
    }
}