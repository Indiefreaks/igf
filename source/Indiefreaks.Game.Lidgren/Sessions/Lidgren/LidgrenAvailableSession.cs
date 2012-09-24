using System;

namespace Indiefreaks.Xna.Sessions.Lidgren
{
    public class LidgrenAvailableSession : AvailableSession
    {
        private readonly  SessionType _sessionType;
        private readonly int _currentGamerCount;
        private readonly string _hostName;
        private readonly int _openPrivateSlots;
        private readonly int _openPublicSlots;
        private readonly SessionProperties _sessionProperties;
        private readonly TimeSpan _averageRoundtripTime;

        internal LidgrenAvailableSession(SessionType sessionType, int currentGamerCount, string hostName, int openPrivateSlots, int openPublicSlots, SessionProperties sessionProperties, TimeSpan averageRoundtripTime)
        {
            _sessionType = sessionType;
            _currentGamerCount = currentGamerCount;
            _hostName = hostName;
            _openPrivateSlots = openPrivateSlots;
            _openPublicSlots = openPublicSlots;
            _sessionProperties = sessionProperties;
            _averageRoundtripTime = averageRoundtripTime;
        }

        #region Overrides of AvailableSession

        /// <summary>
        /// Returns the number of Players currently on the Session
        /// </summary>
        public override int CurrentGamerCount
        {
            get { return _currentGamerCount; }
        }

        /// <summary>
        /// Returns the name of the session Host
        /// </summary>
        public override string HostName
        {
            get { return _hostName; }
        }

        /// <summary>
        /// Returns the number of private slots still open
        /// </summary>
        public override int OpenPrivateSlots
        {
            get { return _openPrivateSlots; }
        }

        /// <summary>
        /// Returns the number of public slots stil open
        /// </summary>
        public override int OpenPublicSlots
        {
            get { return _openPublicSlots; }
        }

        /// <summary>
        /// Return the Session Properties of the Session
        /// </summary>
        public override SessionProperties Properties
        {
            get { return _sessionProperties; }
        }

        /// <summary>
        /// Returns the average time it takes to ping the session
        /// </summary>
        public override TimeSpan AverageRoundtripTime
        {
            get { return _averageRoundtripTime; }
        }

        /// <summary>
        /// Returns the bytes per second downstream
        /// </summary>
        public override int BytesPerSecondDownstream
        {
            get { return 0; }
        }

        /// <summary>
        /// Returns the bytes per second upstream
        /// </summary>
        public override int BytesPerSecondUpstream
        {
            get { return 0; }
        }

        /// <summary>
        /// Returns the lowest ping time to this session
        /// </summary>
        public override TimeSpan MinimumRoundtripTime
        {
            get { return _averageRoundtripTime; }
        }

        #endregion

        public SessionType SessionType { get { return _sessionType; }
        }
    }
}