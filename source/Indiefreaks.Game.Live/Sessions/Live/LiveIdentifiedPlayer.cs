using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using Indiefreaks.Xna.Logic;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace Indiefreaks.Xna.Sessions.Live
{
    /// <summary>
    /// The LiveIdentifiedPlayer encapsulates Xbox Live SignedInGamer instance
    /// </summary>
    public class LiveIdentifiedPlayer : IdentifiedPlayer
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="signedInGamer">The Xbox Live player SignedInGamer instance</param>
        internal LiveIdentifiedPlayer(SignedInGamer signedInGamer)
            : base(Application.Input.GetPlayerInput(signedInGamer.PlayerIndex))
        {
            LiveGamer = signedInGamer;
            UniqueId = signedInGamer.Gamertag;
            DisplayName = signedInGamer.Gamertag;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="playerInput"></param>
        internal LiveIdentifiedPlayer(PlayerInput playerInput, LocalNetworkGamer localNetworkGamer)
            : base(playerInput)
        {
            LiveGamer = localNetworkGamer;
            UniqueId = localNetworkGamer.Gamertag;
            DisplayName = localNetworkGamer.Gamertag;

            IsHost = localNetworkGamer.IsHost;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="networkGamer"></param>
        internal LiveIdentifiedPlayer(NetworkGamer networkGamer) : base(networkGamer.Gamertag)
        {
            LiveGamer = networkGamer;
            DisplayName = networkGamer.Gamertag;
        }

        /// <summary>
        /// Returns the Xbox Live Gamer instance associated with this player
        /// </summary>
        public Gamer LiveGamer { get; private set; }
    }
}