using System;
using Microsoft.Xna.Framework.Net;

namespace Indiefreaks.Xna.Sessions.Live
{
    /// <summary>
    /// The LiveAvailableSession is the Xbox Live implementation of AvailableSession
    /// </summary>
    public sealed class LiveAvailableSession : AvailableSession
    {
        internal readonly AvailableNetworkSession AvailableNetworkSession;
        private readonly SessionProperties _sessionProperties;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="availableSession">The Xbox Live AvailableNetworkSession instance</param>
        internal LiveAvailableSession(AvailableNetworkSession availableSession)
        {
            AvailableNetworkSession = availableSession;
            _sessionProperties =
                LiveSessionProperties.ConvertFromLiveSessionProperties(availableSession.SessionProperties);
        }

        #region Overrides of AvailableSession

        /// <summary>
        /// Returns the number of Players currently on the Session
        /// </summary>
        public override int CurrentGamerCount
        {
            get { return AvailableNetworkSession.CurrentGamerCount; }
        }

        /// <summary>
        /// Returns the name of the session Host
        /// </summary>
        public override string HostName
        {
            get { return AvailableNetworkSession.HostGamertag; }
        }

        /// <summary>
        /// Returns the number of private slots still open
        /// </summary>
        public override int OpenPrivateSlots
        {
            get { return AvailableNetworkSession.OpenPrivateGamerSlots; }
        }

        /// <summary>
        /// Returns the number of public slots stil open
        /// </summary>
        public override int OpenPublicSlots
        {
            get { return AvailableNetworkSession.OpenPublicGamerSlots; }
        }

        /// <summary>
        /// Returns the session properties
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
            get { return AvailableNetworkSession.QualityOfService.AverageRoundtripTime; }
        }

        /// <summary>
        /// Returns the bytes per second downstream
        /// </summary>
        public override int BytesPerSecondDownstream
        {
            get { return AvailableNetworkSession.QualityOfService.BytesPerSecondDownstream; }
        }

        /// <summary>
        /// Returns the bytes per second upstream
        /// </summary>
        public override int BytesPerSecondUpstream
        {
            get { return AvailableNetworkSession.QualityOfService.BytesPerSecondUpstream; }
        }

        /// <summary>
        /// Returns the lowest ping time to this session
        /// </summary>
        public override TimeSpan MinimumRoundtripTime
        {
            get { return AvailableNetworkSession.QualityOfService.MinimumRoundtripTime; }
        }

        #endregion
    }
}