using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using Indiefreaks.Xna.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Sessions.Live
{
    /// <summary>
    /// The LiveSessionManager is the Xbox Live SessionManager implementation
    /// </summary>
    public sealed class LiveSessionManager : SessionManager
    {
        private const string CreatingSession = "CreatingSession";
        private const string FindingSessions = "FindingSessions";
        private const string JoiningSession = "JoiningSession";

        private static NetworkSession _networkSession;
        private string _networkSessionLocker;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public LiveSessionManager(Application application) : base(Application.SunBurn)
        {
            if (!GamerServicesDispatcher.IsInitialized)
                GamerServicesDispatcher.Initialize(application.Services);

            GamerServicesDispatcher.WindowHandle = application.Window.Handle;

            SignedInGamer.SignedIn += OnLiveGamerSignedIn;
            SignedInGamer.SignedOut += OnLiveGamerSignedOut;
        }

        /// <summary>
        /// Raised when the Live player signed out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLiveGamerSignedOut(object sender, SignedOutEventArgs e)
        {
            IdentifiedPlayer identifiedPlayer = LocalPlayers[e.Gamer.PlayerIndex];
            LocalPlayers.Remove(e.Gamer.PlayerIndex);
         
            OnPlayerLogoff(identifiedPlayer);
        }

        /// <summary>
        /// Raised when the Live player signed in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLiveGamerSignedIn(object sender, SignedInEventArgs e)
        {
            var identifiedPlayer = new LiveIdentifiedPlayer(e.Gamer);
            LocalPlayers.Add(e.Gamer.PlayerIndex, identifiedPlayer);

            OnPlayerLogin(identifiedPlayer);
        }

        #region Overrides of SessionManager

        /// <summary>
        /// Identifies a player
        /// </summary>
        /// <param name="playerInput">The PlayerInput instance used by the player to identify</param>
        public override void IdentifyPlayer(PlayerInput playerInput)
        {
            if (Guide.IsVisible)
                return;

            if (LocalPlayers.ContainsKey(playerInput.PlayerIndex) && (IsIdentifyingPlayers || (CurrentSession != null && CurrentSession.Status == SessionState.Playing && CurrentSession.AllowJoinInProgress)))
            {
                OnPlayerLogin(LocalPlayers[playerInput.PlayerIndex]);
                return;
            }

            Guide.ShowSignIn(1, false);
        }

        /// <summary>
        /// Creates a Single Player Session
        /// </summary>
        /// <remarks>No network resources will be used</remarks>
        public override void CreateSinglePlayerSession()
        {
            if (LocalPlayers.Count == 0)
                throw new CoreException("No players identified");
            if(CurrentSession != null)
                throw new CoreException("A session already exists. Close the previous session first");
            
            _networkSessionLocker = CreatingSession;
            NetworkSession.BeginCreate(NetworkSessionType.Local, 1, 1, OnLiveSessionCreated,
                                        _networkSessionLocker);
        }

        /// <summary>
        /// Creates a Split Screen Session
        /// </summary>
        /// <remarks>Not implemented yet</remarks>
        public override void CreateSplitScreenSession()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Raised when the Session is created
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnLiveSessionCreated(IAsyncResult asyncResult)
        {
            _networkSession = NetworkSession.EndCreate(asyncResult);

            CurrentSession = new LiveSession(_networkSession);

            OnSessionCreated();
        }

        /// <summary>
        /// Creates a local area network session
        /// </summary>
        /// <param name="maxPlayers">The total maximum players for this session</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to find this session on the network. Can be null</param>
        /// <remarks>it doesn't yet support multiple local players</remarks>
        public override void CreateLanSession(int maxPlayers, SessionProperties sessionProperties)
        {
            if (LocalPlayers.Count > 1)
                throw new NotImplementedException();
            if (CurrentSession != null)
                throw new CoreException("A session already exists. Close the previous session first");

            _networkSessionLocker = CreatingSession;

            NetworkSession.BeginCreate(NetworkSessionType.SystemLink, 1, maxPlayers, 0,
                                       LiveSessionProperties.ConvertToLiveSessionProperties(sessionProperties),
                                       OnLiveSessionCreated, _networkSessionLocker);
        }

        /// <summary>
        /// Creates a wide area network session
        /// </summary>
        /// <param name="maxPlayers">The total maximum players for this session</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to find this session on the network. Can be null</param>
        /// <remarks>it doesn't yet support multiple local players</remarks>
        public override void CreateWanSession(int maxPlayers, SessionProperties sessionProperties)
        {
            if (LocalPlayers.Count > 1)
                throw new NotImplementedException();
            if (CurrentSession != null)
                throw new CoreException("A session already exists. Close the previous session first");

            _networkSessionLocker = CreatingSession;

            NetworkSession.BeginCreate(NetworkSessionType.PlayerMatch, 1, maxPlayers, 0,
                                       LiveSessionProperties.ConvertToLiveSessionProperties(sessionProperties),
                                       OnLiveSessionCreated, _networkSessionLocker);
        }

        /// <summary>
        /// Sends a Find query on the network interface to look for AvailableSession instances asynchrnously
        /// </summary>
        /// <param name="sessionType">The SessionType we're looking for</param>
        /// <param name="maxLocalPlayers">The Maximum local players that can be added to the session used to filter sessions that have a limited number of opened public slots</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to filter query results. Can be null</param>
        public override void FindSessions(SessionType sessionType, int maxLocalPlayers,
                                          SessionProperties sessionProperties)
        {
            _networkSessionLocker = FindingSessions;
            switch (sessionType)
            {
                case SessionType.WideAreaNetwork:
                    {
                        NetworkSession.BeginFind(NetworkSessionType.PlayerMatch, maxLocalPlayers,
                                                 LiveSessionProperties.ConvertToLiveSessionProperties(sessionProperties),
                                                 OnLiveSessionsFound,
                                                 _networkSessionLocker);
                        break;
                    }
                case SessionType.LocalAreaNetwork:
                    {
                        NetworkSession.BeginFind(NetworkSessionType.SystemLink, maxLocalPlayers,
                                                 LiveSessionProperties.ConvertToLiveSessionProperties(sessionProperties),
                                                 OnLiveSessionsFound, _networkSessionLocker);
                        break;
                    }
                default:
                case SessionType.SplitScreen:
                case SessionType.SinglePlayer:
                    throw new CoreException("Cannot look for a Device only session");
            }
        }

        /// <summary>
        /// Raised when a Live find query returns
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnLiveSessionsFound(IAsyncResult asyncResult)
        {
            var sessions = new List<AvailableSession>();
            AvailableNetworkSessionCollection availableLiveSessions = NetworkSession.EndFind(asyncResult);

            foreach (AvailableNetworkSession availableLiveSession in availableLiveSessions)
                sessions.Add(new LiveAvailableSession(availableLiveSession));

            OnSessionsFound(sessions.AsReadOnly());
        }

        /// <summary>
        /// Sends a Join query to the Session asynchronously
        /// </summary>
        /// <param name="availableSession">The Session we are trying to join</param>
        public override void JoinSession(AvailableSession availableSession)
        {
            _networkSessionLocker = JoiningSession;

            NetworkSession.BeginJoin(((LiveAvailableSession) availableSession).AvailableNetworkSession,
                                     OnLiveSessionJoined, _networkSessionLocker);
        }

        /// <summary>
        /// Raised when the the player joined the Session
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnLiveSessionJoined(IAsyncResult asyncResult)
        {
            NetworkSession networkSession = NetworkSession.EndJoin(asyncResult);
            CurrentSession = new LiveSession(networkSession);

            OnSessionJoined();
        }

        /// <summary>
        /// Updates the object and its contained resources.
        /// </summary>
        /// <param name="gameTime"/>
        public override void Update(GameTime gameTime)
        {
            GamerServicesDispatcher.Update();

            base.Update(gameTime);
        }

        #endregion
    }
}