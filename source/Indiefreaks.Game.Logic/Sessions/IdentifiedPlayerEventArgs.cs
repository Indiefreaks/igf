using System;

namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// Specialized event arguments used when a Player is identified, joined or left a Session
    /// </summary>
    public class IdentifiedPlayerEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="identifiedPlayer">The identified player</param>
        public IdentifiedPlayerEventArgs(IdentifiedPlayer identifiedPlayer)
        {
            IdentifiedPlayer = identifiedPlayer;
        }

        /// <summary>
        /// Returns the player that just got identified, joined or left a Session
        /// </summary>
        public IdentifiedPlayer IdentifiedPlayer { get; private set; }
    }
}