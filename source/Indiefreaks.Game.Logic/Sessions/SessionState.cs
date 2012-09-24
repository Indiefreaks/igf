namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// The SessionState enumeration tells the current session status
    /// </summary>
    public enum SessionState : byte
    {
        Lobby,
        Starting,
        Playing,
        Ended,
        Closed,
    }
}