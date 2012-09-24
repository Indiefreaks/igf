using System;
using Indiefreaks.Xna.Input;
using Indiefreaks.Xna.Logic;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Sessions.Local
{
    public class LocalIdentifiedPlayer : IdentifiedPlayer
    {
        /// <summary>
        /// Creates a new local player instance
        /// </summary>
        /// <param name="playerInput">The PlayerInput instance used by the Player</param>
        internal LocalIdentifiedPlayer(PlayerInput playerInput) : base(playerInput)
        {
            UniqueId = playerInput.PlayerIndex.ToString();
            IsHost = playerInput != null;
            DisplayName = Enum.GetName(typeof (PlayerIndex), playerInput.PlayerIndex);
        }
    }
}