using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Indiefreaks.Xna.Core;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Sessions.Lidgren
{
    public partial class LidgrenSession : Session
    {
        private const int MaximumSupportedGamersInSession = 31;
        private static readonly List<LidgrenAvailableSession> LidgrenSessionsFound = new List<LidgrenAvailableSession>();
        private readonly List<IdentifiedPlayer> _allPlayers = new List<IdentifiedPlayer>(1);
        private readonly bool _isHost;
        private readonly List<IdentifiedPlayer> _localPlayers = new List<IdentifiedPlayer>(1);
        private readonly int _maxGamers = MaximumSupportedGamersInSession;
        private readonly int _privateReservedSlots;
        private readonly Dictionary<IdentifiedPlayer, IPEndPoint> _remotePlayerIpEndPoints = new Dictionary<IdentifiedPlayer, IPEndPoint>();
        private readonly List<IdentifiedPlayer> _remotePlayers = new List<IdentifiedPlayer>();
        private readonly SessionProperties _sessionProperties;
        private readonly SessionType _sessionType;
        private SessionState _clientSessionState = SessionState.Lobby;
        private NetIncomingMessage _incomingMessage;
        private NetOutgoingMessage _outgoingMessage;
        private int _previousSecondBytesReceived;
        private int _previousSecondBytesSent;
        private double _secondTick;
        private SessionState _serverSessionState = SessionState.Lobby;
        private const int LocalDiscoveryTimeOut = 1000;
        private static bool _localDiscoveryRequested;

        private LidgrenSession(string serverHost, SessionType sessionType, int maxGamers, int privateReservedSlots, SessionProperties sessionProperties, bool isHost)
        {
            _isHost = isHost;
            _sessionType = sessionType;
            _sessionProperties = sessionProperties;

            if (maxGamers > MaximumSupportedGamersInSession)
                throw new CoreException("Cannot create sessions for more than " + MaximumSupportedGamersInSession + " players.");
            else
                _maxGamers = maxGamers;

            _privateReservedSlots = privateReservedSlots;

            if (_isHost)
            {
                LidgrenSessionManager.Server.Start();
                _previousSecondBytesSent = LidgrenSessionManager.Server.Statistics.SentBytes;
                _previousSecondBytesReceived = LidgrenSessionManager.Server.Statistics.ReceivedBytes;

                IdentifiedPlayer identifiedPlayer;
                for (int i = 0; i < 4; i++)
                {
                    if (SessionManager.LocalPlayers.TryGetValue((PlayerIndex) Enum.ToObject(typeof (PlayerIndex), i), out identifiedPlayer))
                    {
                        ((LidgrenIdentifiedPlayer) identifiedPlayer).SetIsHost();
                        break;
                    }
                }
            }

            LidgrenSessionManager.Client.Start();
            LidgrenSessionManager.Client.Connect(serverHost, LidgrenSessionManager.ServerPort);
            _previousSecondBytesSent += LidgrenSessionManager.Client.Statistics.SentBytes;
            _previousSecondBytesReceived += LidgrenSessionManager.Client.Statistics.ReceivedBytes;

            _clientSessionState = SessionState.Lobby;
            _serverSessionState = SessionState.Lobby;
        }

        internal static IAsyncResult BeginCreate(string serverHost, SessionType sessionType, int maxLocalGamers, int maxGamers, int privateReservedSlots, SessionProperties sessionProperties, AsyncCallback callback, object asyncState)
        {
            if ((maxLocalGamers < 1) || (maxLocalGamers > 4))
            {
                throw new ArgumentOutOfRangeException("maxLocalGamers");
            }
            if (maxGamers < 1 || maxGamers > 31)
            {
                throw new ArgumentOutOfRangeException("maxGamers");
            }
            if ((privateReservedSlots < 0) || (privateReservedSlots >= maxGamers))
            {
                throw new ArgumentOutOfRangeException("privateReservedSlots");
            }

            var asyncCreate = new AsynchronousCreate(Create);
            return asyncCreate.BeginInvoke(serverHost, sessionType, maxGamers, privateReservedSlots, sessionProperties, true, callback, asyncState);
        }
        
        private static LidgrenSession Create(string serverHost, SessionType sessionType, int maxGamers, int privateReservedSlots, SessionProperties sessionProperties, bool isHost)
        {
            var session = new LidgrenSession(serverHost, sessionType, maxGamers, privateReservedSlots, sessionProperties, isHost);

            return session;
        }

        internal static LidgrenSession EndCreate(IAsyncResult result)
        {
            return EndCreateOrJoin(result);
        }

        internal static IAsyncResult BeginFind(SessionType sessionType, int maxLocalPlayers, SessionProperties sessionProperties, AsyncCallback callback, object asyncState)
        {
            LidgrenSessionsFound.Clear();

            if (sessionType == SessionType.SinglePlayer || sessionType == SessionType.SplitScreen)
            {
                throw new CoreException("Cannot look for SinglePlayer or SplitScreen sessions");
            }

            _localDiscoveryRequested = true;

            var asyncFind = new AsynchronousFind(Find);
            return asyncFind.BeginInvoke(sessionType, maxLocalPlayers, sessionProperties, callback, asyncState);
        }

        private static List<LidgrenAvailableSession> Find(SessionType sessionType, int maxLocalPlayers, SessionProperties sessionProperties)
        {
            switch (sessionType)
            {
                case SessionType.LocalAreaNetwork:
                    {
                        LidgrenSessionManager.Client.DiscoverLocalPeers(LidgrenSessionManager.ServerPort);
                        break;
                    }
                case SessionType.WideAreaNetwork:
                    {
                        throw new NotImplementedException();
                    }
            }

            if (_localDiscoveryRequested)
            {
                Thread.Sleep(LocalDiscoveryTimeOut);
                _localDiscoveryRequested = false;
            }
            
            return new List<LidgrenAvailableSession>(LidgrenSessionsFound);
        }

        internal static List<LidgrenAvailableSession> EndFind(IAsyncResult result)
        {
            var availableSessions = new List<LidgrenAvailableSession>();

            try
            {
                var asyncResult = (AsyncResult) result;

                result.AsyncWaitHandle.WaitOne();

                if (asyncResult.AsyncDelegate is AsynchronousFind)
                    availableSessions.AddRange(((AsynchronousFind) asyncResult.AsyncDelegate).EndInvoke(result));
            }
            finally
            {
                result.AsyncWaitHandle.Close();
            }

            return availableSessions;
        }

        internal static IAsyncResult BeginJoin(LidgrenAvailableSession availableSession, AsyncCallback callback, object asyncState)
        {
            if (availableSession.OpenPublicSlots < SessionManager.LocalPlayers.Count)
            {
                throw new CoreException("To many local players to join");
            }

            var asyncCreate = new AsynchronousJoin(Join);
            return asyncCreate.BeginInvoke(availableSession, callback, asyncState);
        }

        internal static LidgrenSession Join(LidgrenAvailableSession availableSession)
        {
            var session = new LidgrenSession(availableSession.HostName, availableSession.SessionType, availableSession.CurrentGamerCount + availableSession.OpenPublicSlots + availableSession.OpenPrivateSlots, 0, availableSession.Properties, false);

            return session;
        }

        internal static LidgrenSession EndJoin(IAsyncResult result)
        {
            return EndCreateOrJoin(result);
        }

        private static LidgrenSession EndCreateOrJoin(IAsyncResult result)
        {
            LidgrenSession session = null;

            try
            {
                var asyncResult = (AsyncResult) result;

                result.AsyncWaitHandle.WaitOne();

                if (asyncResult.AsyncDelegate is AsynchronousCreate)
                    session = ((AsynchronousCreate) asyncResult.AsyncDelegate).EndInvoke(result);
            }
            finally
            {
                result.AsyncWaitHandle.Close();
            }

            return session;
        }

        #region Overrides of Session

        /// <summary>
        /// Returns the list of players in the current session
        /// </summary>
        public override IList<IdentifiedPlayer> AllPlayers
        {
            get { return _allPlayers.AsReadOnly(); }
        }

        /// <summary>
        /// Return the list of local players in the current session
        /// </summary>
        public override IList<IdentifiedPlayer> LocalPlayers
        {
            get { return _localPlayers.AsReadOnly(); }
        }

        /// <summary>
        /// Returns the list of remote players in the current session
        /// </summary>
        public override IList<IdentifiedPlayer> RemotePlayers
        {
            get { return _remotePlayers.AsReadOnly(); }
        }

        /// <summary>
        /// Returns the current SessionState
        /// </summary>
        public override SessionState Status
        {
            get { return _clientSessionState; }
        }

        /// <summary>
        /// Returns the current Session SessionType
        /// </summary>
        public override SessionType SessionType
        {
            get { return _sessionType; }
        }

        /// <summary>
        /// Returns if the current Session authorizes Host migration
        /// </summary>
        public override bool AllowHostMigration
        {
            get { return false; }
        }

        /// <summary>
        /// Returns if the current Session authorizes players to join the Session while playing
        /// </summary>
        public override bool AllowJoinInProgress
        {
            get { return false; }
        }

        /// <summary>
        /// Returns the current Session Host player
        /// </summary>
        public override IdentifiedPlayer Host
        {
            get
            {
                foreach (IdentifiedPlayer player in AllPlayers)
                    if (player.IsHost) return player;

                throw new CoreException("No Host Found");
            }
        }

        /// <summary>
        /// Returns the current Session Bytes/Second received
        /// </summary>
        public override int BytesPerSecondReceived
        {
            get { return _previousSecondBytesReceived; }
        }

        /// <summary>
        /// Returns the current Session Bytes/Second sent
        /// </summary>
        public override int BytesPerSecondSent
        {
            get { return _previousSecondBytesSent; }
        }

        /// <summary>
        /// Returns if the current Session is Host
        /// </summary>
        public override bool IsHost
        {
            get { return _isHost; }
        }

        private static void WriteNetworkValue(ref NetOutgoingMessage outgoingMessage, object networkValue)
        {
            if (networkValue == null)
                throw new CoreException("NetworkValue is null");
            else
            {
                if (networkValue is bool)
                    outgoingMessage.Write((bool) networkValue);
                else if (networkValue is byte)
                    outgoingMessage.Write((byte) networkValue);
                else if (networkValue is byte[])
                {
                    var tempByteArray = (byte[]) networkValue;
                    outgoingMessage.Write(tempByteArray.Length);
                    outgoingMessage.Write(tempByteArray);
                }
                else if (networkValue is char)
                    outgoingMessage.Write(BitConverter.GetBytes((char) networkValue));
                else if (networkValue is char[])
                {
                    var tempCharArray = (char[]) networkValue;
                    outgoingMessage.Write(tempCharArray.Length);
                    for (int i = 0; i < tempCharArray.Length; i++)
                    {
                        outgoingMessage.Write(BitConverter.GetBytes(tempCharArray[i]));
                    }
                }
                else if (networkValue is Color)
                    outgoingMessage.Write(((Color) networkValue).ToVector4());
                else if (networkValue is double)
                    outgoingMessage.Write((double) networkValue);
                else if (networkValue is float)
                    outgoingMessage.Write((float) networkValue);
                else if (networkValue is int)
                    outgoingMessage.Write((int) networkValue);
                else if (networkValue is long)
                    outgoingMessage.Write((long) networkValue);
                else if (networkValue is Matrix)
                    outgoingMessage.WriteMatrix((Matrix) networkValue);
                else if (networkValue is Quaternion)
                    outgoingMessage.WriteRotation((Quaternion) networkValue, 24);
                else if (networkValue is sbyte)
                    outgoingMessage.Write((sbyte) networkValue);
                else if (networkValue is short)
                    outgoingMessage.Write((short) networkValue);
                else if (networkValue is string)
                    outgoingMessage.Write((string) networkValue);
                else if (networkValue is uint)
                    outgoingMessage.Write((uint) networkValue);
                else if (networkValue is ulong)
                    outgoingMessage.Write((ulong) networkValue);
                else if (networkValue is ushort)
                    outgoingMessage.Write((ushort) networkValue);
                else if (networkValue is Vector2)
                    outgoingMessage.Write((Vector2) networkValue);
                else if (networkValue is Vector3)
                    outgoingMessage.Write((Vector3) networkValue);
                else if (networkValue is Vector4)
                    outgoingMessage.Write((Vector4) networkValue);
                else
                {
                    throw new CoreException("NetworkValue type isn't supported");
                }
            }
        }

        /// <summary>
        /// Converts DataTransferOptions to NetDeliveryOptions
        /// </summary>
        /// <param name="dataTransferOptions"></param>
        /// <returns></returns>
        private static NetDeliveryMethod ConvertToNetDeliveryMethod(DataTransferOptions dataTransferOptions)
        {
            switch (dataTransferOptions)
            {
                case DataTransferOptions.None:
                    return NetDeliveryMethod.Unknown;
                case DataTransferOptions.Reliable:
                    return NetDeliveryMethod.ReliableUnordered;
                case DataTransferOptions.InOrder:
                    return NetDeliveryMethod.ReliableSequenced;
                case DataTransferOptions.Chat:
                    return NetDeliveryMethod.Unreliable;
                default:
                case DataTransferOptions.ReliableInOrder:
                    return NetDeliveryMethod.ReliableOrdered;
            }
        }

        /// <summary>
        /// Update loop call
        /// </summary>
        /// <param name="gameTime"/>
        public override void Update(GameTime gameTime)
        {
            if (_isHost)
                LidgrenSessionManager.Server.FlushSendQueue();

            LidgrenSessionManager.Client.FlushSendQueue();

            _secondTick += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_secondTick >= 1000d)
            {
                if (_isHost)
                {
                    _previousSecondBytesSent = (LidgrenSessionManager.Server.Statistics.SentBytes + LidgrenSessionManager.Client.Statistics.SentBytes) - _previousSecondBytesSent;
                    _previousSecondBytesReceived = (LidgrenSessionManager.Server.Statistics.ReceivedBytes + LidgrenSessionManager.Client.Statistics.ReceivedBytes) - _previousSecondBytesReceived;
                }
                else
                {
                    _previousSecondBytesSent = LidgrenSessionManager.Client.Statistics.SentBytes - _previousSecondBytesSent;
                    _previousSecondBytesReceived = LidgrenSessionManager.Client.Statistics.ReceivedBytes - _previousSecondBytesReceived;
                }

                _secondTick = 0d;
            }
        }

        /// <summary>
        /// Starts the Session and changes its SessionState from Lobby to Playing
        /// </summary>
        public override void StartSession()
        {
            if (!IsHost)
                return;

            if (_serverSessionState != SessionState.Lobby)
                throw new CoreException("Server is not in Lobby");

            _serverSessionState = SessionState.Starting;

            SendSessionStateChangedToClients();
        }

        /// <summary>
        /// Ends the Session and changes its SessionState from Playing to Ended
        /// </summary>
        public override void EndSession()
        {
            if (!IsHost)
                return;

            if (_serverSessionState != SessionState.Playing)
                throw new CoreException("Server is not started");

            _serverSessionState = SessionState.Ended;

            SendSessionStateChangedToClients();
        }

        /// <summary>
        /// Closes the Session and changes its SessionState to Closed
        /// </summary>
        public override void OnClosed()
        {
            if (IsHost)
            {
                _serverSessionState = SessionState.Closed;

                SendSessionStateChangedToClients();
            }
            else
            {
                LidgrenSessionManager.Client.Shutdown("Closing Session");
            }
        }

        /// <summary>
        /// Listens to all data received from the network interface
        /// </summary>
        public override void ListenIncoming()
        {
            if (_isHost)
                ProcessServerMessages();

            ProcessClientMessages();
        }

        #endregion

        #region Nested type: AsynchronousCreate

        private delegate LidgrenSession AsynchronousCreate(string serverHost, SessionType sessionType, int maxGamers, int privateReservedSlots, SessionProperties sessionProperties, bool isHost);

        #endregion

        #region Nested type: AsynchronousFind

        private delegate List<LidgrenAvailableSession> AsynchronousFind(SessionType sessionType, int maxLocalPlayers, SessionProperties sessionProperties);

        #endregion

        #region Nested type: AsynchronousJoin

        private delegate LidgrenSession AsynchronousJoin(LidgrenAvailableSession availableSession);

        #endregion
    }
}