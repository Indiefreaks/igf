using System;
using System.Collections.Generic;
using System.Net;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using Lidgren.Network;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Sessions.Lidgren
{
    /// <summary>
    /// The LidgrenSessionManager is a Windows SessionManager implementation using the Lidren Network library
    /// </summary>
    public class LidgrenSessionManager : SessionManager
    {
        internal const int ServerPort = 27000;
        internal const int ClientPort = 27001;
        internal static NetServer Server;
        internal static NetClient Client;

        private const string CreatingSession = "CreatingSession";
        private const string FindingSessions = "FindingSessions";
        private const string JoiningSession = "JoiningSession";
        private string _networkSessionLocker;

        static LidgrenSessionManager()
        {
            var serverConfiguration = new NetPeerConfiguration(Application.Instance.Name)
            {
                AcceptIncomingConnections = true,
                AutoFlushSendQueue = false,
                Port = ServerPort,
            };
            serverConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            Server = new NetServer(serverConfiguration);
            
            var clientConfiguration = new NetPeerConfiguration(Application.Instance.Name)
            {
                AcceptIncomingConnections = false,
                AutoFlushSendQueue = false,
                Port = ClientPort,
            };
            clientConfiguration.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            Client = new NetClient(clientConfiguration);
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public LidgrenSessionManager(IManagerServiceProvider sceneInterface) : base(sceneInterface)
        {
        }

        #region Overrides of SessionManager

        /// <summary>
        /// Identifies a player
        /// </summary>
        /// <param name="playerInput">The PlayerInput instance used by the player to identify</param>
        public override void IdentifyPlayer(PlayerInput playerInput)
        {
            var identifiedPlayer = new LidgrenIdentifiedPlayer(playerInput);

            LocalPlayers.Add(playerInput.PlayerIndex, identifiedPlayer);
         
            OnPlayerLogin(identifiedPlayer);
        }

        /// <summary>
        /// Creates a Single Player Session
        /// </summary>
        /// <remarks>No network resources will be used</remarks>
        public override void CreateSinglePlayerSession()
        {
            if(CurrentSession != null)
                throw new CoreException("Session is already running");

            IPAddress ipAddress;
            var host = NetUtility.GetMyAddress(out ipAddress);

            _networkSessionLocker = CreatingSession;
            LidgrenSession.BeginCreate(host.ToString(), SessionType.SinglePlayer, 1, 1, 0, new SessionProperties(), OnLidgrenSessionCreated, _networkSessionLocker);
        }

        private void OnLidgrenSessionCreated(IAsyncResult result)
        {
            CurrentSession = LidgrenSession.EndCreate(result);
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
        /// Creates a local area network session
        /// </summary>
        /// <param name="maxPlayers">The total maximum players for this session</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to find this session on the network. Can be null</param>
        /// <remarks>it doesn't yet support multiple local players</remarks>
        public override void CreateLanSession(int maxPlayers, SessionProperties sessionProperties)
        {
            IPAddress ipAddress;
            var host = NetUtility.GetMyAddress(out ipAddress);

            _networkSessionLocker = CreatingSession;
            LidgrenSession.BeginCreate(host.ToString(), SessionType.LocalAreaNetwork, 1, maxPlayers, 0, sessionProperties, OnLidgrenSessionCreated, _networkSessionLocker);
        }

        /// <summary>
        /// Creates a wide area network session
        /// </summary>
        /// <param name="maxPlayers">The total maximum players for this session</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to find this session on the network. Can be null</param>
        /// <remarks>it doesn't yet support multiple local players</remarks>
        public override void CreateWanSession(int maxPlayers, SessionProperties sessionProperties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a Find query on the network interface to look for AvailableSession instances asynchrnously
        /// </summary>
        /// <param name="sessionType">The SessionType we're looking for</param>
        /// <param name="maxLocalPlayers">The Maximum local players that can be added to the session used to filter sessions that have a limited number of opened public slots</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to filter query results. Can be null</param>
        public override void FindSessions(SessionType sessionType, int maxLocalPlayers, SessionProperties sessionProperties)
        {
            if (CurrentSession != null)
                throw new CoreException("Session is already running");

            _networkSessionLocker = FindingSessions;
            LidgrenSession.BeginFind(sessionType, maxLocalPlayers, sessionProperties, OnLidgrenSessionsFound, _networkSessionLocker);
        }

        private void OnLidgrenSessionsFound(IAsyncResult ar)
        {
            var foundSessions = new List<AvailableSession>(LidgrenSession.EndFind(ar));
            OnSessionsFound(foundSessions);
        }

        /// <summary>
        /// Sends a Join query to the Session asynchronously
        /// </summary>
        /// <param name="availableSession">The Session we are trying to join</param>
        public override void JoinSession(AvailableSession availableSession)
        {
            if (CurrentSession != null)
                throw new CoreException("Session is already running");

            _networkSessionLocker = JoiningSession;
            LidgrenSession.BeginJoin(availableSession as LidgrenAvailableSession, OnLidgrenSessionJoined, _networkSessionLocker);
        }

        private void OnLidgrenSessionJoined(IAsyncResult ar)
        {
            OnSessionJoined();
        }

        internal static void CloseSession()
        {
            if(CurrentSession == null)
                return;

            Client.Shutdown("Closing Session");

            if (CurrentSession.IsHost)
                Server.Shutdown("Closing Session");

            CurrentSession = null;
        }

        #endregion
    }
}