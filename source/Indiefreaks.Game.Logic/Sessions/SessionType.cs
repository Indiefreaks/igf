namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// The SessionType enumeration defines singleplayer and common multiplayer types
    /// </summary>
    public enum SessionType
    {
        /// <summary>
        /// One unique player: no external networking
        /// </summary>
        SinglePlayer,

        /// <summary>
        /// Up to 4 players on the same device: no external networking
        /// </summary>
        SplitScreen,

        /// <summary>
        /// Up to 4 players on the same device and LAN support
        /// </summary>
        LocalAreaNetwork,

        /// <summary>
        /// Up to 4 players on the same device and Internet networking
        /// </summary>
        WideAreaNetwork,
    }
}