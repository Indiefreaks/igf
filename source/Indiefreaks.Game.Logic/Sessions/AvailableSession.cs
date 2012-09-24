using System;

namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// The AvailableSession class provides information about the sessions found on the network
    /// </summary>
    public abstract class AvailableSession
    {
        /// <summary>
        /// Returns the number of Players currently on the Session
        /// </summary>
        public abstract int CurrentGamerCount { get; }

        /// <summary>
        /// Returns the name of the session Host
        /// </summary>
        public abstract string HostName { get; }

        /// <summary>
        /// Returns the number of private slots still open
        /// </summary>
        public abstract int OpenPrivateSlots { get; }

        /// <summary>
        /// Returns the number of public slots stil open
        /// </summary>
        public abstract int OpenPublicSlots { get; }

        /// <summary>
        /// Return the Session Properties of the Session
        /// </summary>
        public abstract SessionProperties Properties { get; }

        /// <summary>
        /// Returns the average time it takes to ping the session
        /// </summary>
        public abstract TimeSpan AverageRoundtripTime { get; }

        /// <summary>
        /// Returns the bytes per second downstream
        /// </summary>
        public abstract int BytesPerSecondDownstream { get; }

        /// <summary>
        /// Returns the bytes per second upstream
        /// </summary>
        public abstract int BytesPerSecondUpstream { get; }

        /// <summary>
        /// Returns the lowest ping time to this session
        /// </summary>
        public abstract TimeSpan MinimumRoundtripTime { get; }
    }
}