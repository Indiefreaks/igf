using System;
using Indiefreaks.Xna.Input;
using Indiefreaks.Xna.Logic;

namespace Indiefreaks.Xna.Sessions.Lidgren
{
    public class LidgrenIdentifiedPlayer : IdentifiedPlayer
    {
        /// <summary>
        /// Creates a new local player instance
        /// </summary>
        /// <param name="playerInput">The PlayerInput instance used by the Player</param>
        public LidgrenIdentifiedPlayer(PlayerInput playerInput) : base(playerInput)
        {
            UniqueId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Creates a new remote player instance
        /// </summary>
        /// <param name="uniqueId">The id of the remote player</param>
        public LidgrenIdentifiedPlayer(string uniqueId) : base(uniqueId)
        {
        }

        internal void SetIsHost()
        {
            IsHost = true;
        }
    }
}