using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Logic;
using Indiefreaks.Xna.Sessions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Sessions.Live
{
    /// <summary>
    /// A Xbox Live enabled Session implementation
    /// </summary>
    public sealed class LiveSession : Session
    {
        private readonly List<IdentifiedPlayer> _allPlayers = new List<IdentifiedPlayer>(1);
        private readonly List<IdentifiedPlayer> _localPlayers = new List<IdentifiedPlayer>(1);
        private readonly NetworkSession _networkSession;
        private readonly PacketReader _packetReader = new PacketReader();
        private readonly List<IdentifiedPlayer> _remotePlayers = new List<IdentifiedPlayer>();
        private PacketWriter _packetWriter = new PacketWriter();
        private SessionState _sessionState = SessionState.Lobby;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="networkSession">the newly created or joined NetworkSession instance</param>
        internal LiveSession(NetworkSession networkSession)
        {
            _networkSession = networkSession;
            _networkSession.GamerLeft += OnLivePlayerLeft;
            _networkSession.GamerJoined += OnLivePlayerJoined;
            _networkSession.GameStarted += OnLiveSessionStarted;
            _networkSession.GameEnded += OnLiveSessionEnded;
        }

        private void OnLiveSessionEnded(object sender, GameEndedEventArgs e)
        {
            OnEnded();
        }

        /// <summary>
        /// Raised when the Session started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLiveSessionStarted(object sender, GameStartedEventArgs e)
        {
            _sessionState = SessionState.Starting;
            OnStarting();
        }

        /// <summary>
        /// Raised when a player leaves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLivePlayerLeft(object sender, GamerLeftEventArgs e)
        {
            IdentifiedPlayer identifiedPlayer = null;

            if (e.Gamer.IsLocal)
            {
                SignedInGamer signedInGamer = ((LocalNetworkGamer) e.Gamer).SignedInGamer;
                _localPlayers.ForEach(action =>
                                          {
                                              if (action.UniqueId == signedInGamer.Gamertag)
                                              {
                                                  identifiedPlayer = action;
                                                  _localPlayers.Remove(action);
                                                  _allPlayers.Remove(action);
                                              }
                                          });
            }
            else
            {
                _remotePlayers.ForEach(action =>
                                           {
                                               if (action.UniqueId == e.Gamer.Gamertag)
                                               {
                                                   identifiedPlayer = action;
                                                   _remotePlayers.Remove(action);
                                                   _allPlayers.Remove(action);
                                               }
                                           });
            }

            OnPlayerLeft(identifiedPlayer);
        }

        /// <summary>
        /// Raised when a player joins the session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLivePlayerJoined(object sender, GamerJoinedEventArgs e)
        {
            IdentifiedPlayer identifiedPlayer = null;

            if (e.Gamer.IsLocal)
            {
                var localNetworkGamer = (LocalNetworkGamer) e.Gamer;

                IdentifiedPlayer[] localPlayersArray = new IdentifiedPlayer[SessionManager.LocalPlayers.Count];
                SessionManager.LocalPlayers.Values.CopyTo(localPlayersArray, 0);
                
                for (int i = 0; i < localPlayersArray.Length; i++)
                {
                    var localPlayer = localPlayersArray[i];

                    if (localPlayer.UniqueId == e.Gamer.Gamertag)
                    {
                        identifiedPlayer = new LiveIdentifiedPlayer(localPlayer.Input, localNetworkGamer);
                        _localPlayers.Add(identifiedPlayer);
                        _allPlayers.Add(identifiedPlayer);
                        break;
                    }
                }
            }
            else
            {
                NetworkGamer networkGamer = e.Gamer;
                identifiedPlayer = new LiveIdentifiedPlayer(networkGamer);
                _remotePlayers.Add(identifiedPlayer);
                _allPlayers.Add(identifiedPlayer);
            }

            OnPlayerJoined(identifiedPlayer);
        }

        /// <summary>
        /// Converts DataTransferOptions to Xbox Live SendDataOptions
        /// </summary>
        /// <param name="dataTransferOptions"></param>
        /// <returns></returns>
        private static SendDataOptions ConvertToSendDataOptions(DataTransferOptions dataTransferOptions)
        {
            switch (dataTransferOptions)
            {
                case DataTransferOptions.None:
                    return SendDataOptions.None;
                case DataTransferOptions.Reliable:
                    return SendDataOptions.Reliable;
                case DataTransferOptions.InOrder:
                    return SendDataOptions.InOrder;
                case DataTransferOptions.Chat:
                    return SendDataOptions.Chat;
                default:
                case DataTransferOptions.ReliableInOrder:
                    return SendDataOptions.ReliableInOrder;
            }
        }

        /// <summary>
        /// Helper method to write the network value to the PacketWriter
        /// </summary>
        /// <param name="packetWriter"></param>
        /// <param name="networkValue"></param>
        private static void WriteNetworkValue(ref PacketWriter packetWriter, object networkValue)
        {
            if (networkValue == null)
                throw new CoreException("NetworkValue is null");
            else
            {
                if (networkValue is bool)
                    packetWriter.Write((bool) networkValue);
                else if (networkValue is byte)
                    packetWriter.Write((byte) networkValue);
                else if (networkValue is byte[])
                {
                    var tempByteArray = (byte[]) networkValue;
                    packetWriter.Write(tempByteArray.Length);
                    packetWriter.Write(tempByteArray);
                }
                else if (networkValue is char)
                    packetWriter.Write((char) networkValue);
                else if (networkValue is char[])
                {
                    var tempCharArray = (char[]) networkValue;
                    packetWriter.Write(tempCharArray.Length);
                    packetWriter.Write(tempCharArray);
                }
                else if (networkValue is Color)
                    packetWriter.Write((Color) networkValue);
                else if (networkValue is double)
                    packetWriter.Write((double) networkValue);
                else if (networkValue is float)
                    packetWriter.Write((float) networkValue);
                else if (networkValue is int)
                    packetWriter.Write((int) networkValue);
                else if (networkValue is long)
                    packetWriter.Write((long) networkValue);
                else if (networkValue is Matrix)
                    packetWriter.Write((Matrix) networkValue);
                else if (networkValue is Quaternion)
                    packetWriter.Write((Quaternion) networkValue);
                else if (networkValue is sbyte)
                    packetWriter.Write((sbyte) networkValue);
                else if (networkValue is short)
                    packetWriter.Write((short) networkValue);
                else if (networkValue is string)
                    packetWriter.Write((string) networkValue);
                else if (networkValue is uint)
                    packetWriter.Write((uint) networkValue);
                else if (networkValue is ulong)
                    packetWriter.Write((ulong) networkValue);
                else if (networkValue is ushort)
                    packetWriter.Write((ushort) networkValue);
                else if (networkValue is Vector2)
                    packetWriter.Write((Vector2) networkValue);
                else if (networkValue is Vector3)
                    packetWriter.Write((Vector3) networkValue);
                else if (networkValue is Vector4)
                    packetWriter.Write((Vector4) networkValue);
                else
                {
                    throw new CoreException("NetworkValue type isn't supported");
                }
            }
        }

        #region Overrides of Session

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
        /// Returns the list of players in the current session
        /// </summary>
        public override IList<IdentifiedPlayer> AllPlayers
        {
            get { return _allPlayers.AsReadOnly(); }
        }

        /// <summary>
        /// Returns the current SessionState
        /// </summary>
        public override SessionState Status
        {
            get
            {
                switch (_networkSession.SessionState)
                {
                    case NetworkSessionState.Playing:
                        {
                            return _sessionState;
                        }
                    case NetworkSessionState.Ended:
                        return SessionState.Ended;
                    default:
                    case NetworkSessionState.Lobby:
                        return SessionState.Lobby;
                }
            }
        }

        /// <summary>
        /// Returns the current Session SessionType
        /// </summary>
        public override SessionType SessionType
        {
            get
            {
                switch (_networkSession.SessionType)
                {
                    case NetworkSessionType.SystemLink:
                        return SessionType.LocalAreaNetwork;
                    case NetworkSessionType.PlayerMatch:
                        return SessionType.WideAreaNetwork;
                    default:
                    case NetworkSessionType.Local:
                        return SessionType.SinglePlayer;
                }
            }
        }

        /// <summary>
        /// Returns if the current Session authorizes Host migration
        /// </summary>
        public override bool AllowHostMigration
        {
            get { return _networkSession.AllowHostMigration; }
        }

        /// <summary>
        /// Returns if the current Session authorizes players to join while playing
        /// </summary>
        public override bool AllowJoinInProgress
        {
            get { return _networkSession.AllowJoinInProgress; }
        }

        /// <summary>
        /// Returns the current Session Host IdentifiedPlayer
        /// </summary>
        public override IdentifiedPlayer Host
        {
            get
            {
                IdentifiedPlayer host = null;

                NetworkGamer liveHost = _networkSession.Host;

                _allPlayers.ForEach(
                    action => { if (((LiveIdentifiedPlayer) action).LiveGamer == liveHost) host = action; });

                return host;
            }
        }

        /// <summary>
        /// Returns the current Session Bytes/Second received
        /// </summary>
        public override int BytesPerSecondReceived
        {
            get { return _networkSession.BytesPerSecondReceived; }
        }

        /// <summary>
        /// Returns the current Session Bytes/Second sent
        /// </summary>
        public override int BytesPerSecondSent
        {
            get { return _networkSession.BytesPerSecondSent; }
        }

        /// <summary>
        /// Returns if the current Session is Host
        /// </summary>
        public override bool IsHost
        {
            get { return _networkSession.IsHost; }
        }

        /// <summary>
        /// Starts the Session and changes its SessionState from Lobby to Playing
        /// </summary>
        public override void StartSession()
        {
            if (_networkSession.IsHost && _networkSession.SessionState == NetworkSessionState.Lobby)
            {
                _networkSession.StartGame();
            }
        }

        /// <summary>
        /// Ends the Session and changes its SessionState from Playing to Ended
        /// </summary>
        public override void EndSession()
        {
            if (_networkSession.IsHost)
            {
                _networkSession.EndGame();
                OnEnded();
            }
        }

        /// <summary>
        /// Closes the Session and changes its SessionState to Closed
        /// </summary>
        public override void OnClosed()
        {
            _networkSession.GameStarted -= OnLiveSessionStarted;
            _networkSession.GamerJoined -= OnLivePlayerJoined;
            _networkSession.GamerLeft -= OnLivePlayerLeft;
            _networkSession.GameEnded -= OnLiveSessionEnded; 

            if(_networkSession != null)
                _networkSession.Dispose();
        }

        /// <summary>
        /// Sends a request to clients to synchronize commands created on Server
        /// </summary>
        /// <param name="command"></param>
        public override void SynchronizeCommandOnClients(Command command)
        {
            _packetWriter.Write(Constants.SynchronizeCommandOnClient);
            _packetWriter.Write(command.Id);

            ((LocalNetworkGamer) _networkSession.Host).SendData(_packetWriter, SendDataOptions.ReliableInOrder);
        }

        public override void SynchronizeSceneEntitiesOnClients(ISceneEntity sceneEntity)
        {
            _packetWriter.Write(Constants.SynchronizeSceneEntitiesOnClient);
            _packetWriter.Write(sceneEntity.UniqueId);

            ((LocalNetworkGamer)_networkSession.Host).SendData(_packetWriter, SendDataOptions.ReliableInOrder);
        }

        /// <summary>
        /// Listens to all data received from the network interface
        /// </summary>
        public override void ListenIncoming()
        {
            // we loop on all LocalPlayers to gather network data
            foreach (LiveIdentifiedPlayer identifiedPlayer in LocalPlayers)
            {
                var gamer = (LocalNetworkGamer) identifiedPlayer.LiveGamer;

                while (gamer.IsDataAvailable)
                {
                    NetworkGamer sender;

                    gamer.ReceiveData(_packetReader, out sender);

                    byte message = _packetReader.ReadByte();

                    // If the message is to execute commands on server, we execute them and ask all clients to execute them with the server computed value
                    if (message == Constants.ExecuteCommandOnServerDataExchanged)
                    {
                        if (!gamer.IsHost)
                            throw new CoreException("Gamer isn't host");

                        ushort commandId = _packetReader.ReadUInt16();

                        Command command = Commands[commandId];

                        if (command.NetworkValueType == typeof (bool))
                            command.NetworkValue = _packetReader.ReadBoolean();
                        else if (command.NetworkValueType == typeof (byte))
                            command.NetworkValue = _packetReader.ReadByte();
                        else if (command.NetworkValueType == typeof (byte[]))
                        {
                            int tempNumberOfBytes = _packetReader.ReadInt32();
                            command.NetworkValue = _packetReader.ReadBytes(tempNumberOfBytes);
                        }
                        else if (command.NetworkValueType == typeof (char))
                            command.NetworkValue = _packetReader.ReadChar();
                        else if (command.NetworkValueType == typeof (char[]))
                        {
                            int tempNumberOfChars = _packetReader.ReadInt32();
                            command.NetworkValue = _packetReader.ReadChars(tempNumberOfChars);
                        }
                        else if (command.NetworkValueType == typeof (Color))
                            command.NetworkValue = _packetReader.ReadColor();
                        else if (command.NetworkValueType == typeof (double))
                            command.NetworkValue = _packetReader.ReadDouble();
                        else if (command.NetworkValueType == typeof (float))
                            command.NetworkValue = _packetReader.ReadSingle();
                        else if (command.NetworkValueType == typeof (int))
                            command.NetworkValue = _packetReader.ReadInt32();
                        else if (command.NetworkValueType == typeof (long))
                            command.NetworkValue = _packetReader.ReadInt64();
                        else if (command.NetworkValueType == typeof (Matrix))
                            command.NetworkValue = _packetReader.ReadMatrix();
                        else if (command.NetworkValueType == typeof (Quaternion))
                            command.NetworkValue = _packetReader.ReadQuaternion();
                        else if (command.NetworkValueType == typeof (sbyte))
                            command.NetworkValue = _packetReader.ReadSByte();
                        else if (command.NetworkValueType == typeof (short))
                            command.NetworkValue = _packetReader.ReadInt16();
                        else if (command.NetworkValueType == typeof (string))
                            command.NetworkValue = _packetReader.ReadString();
                        else if (command.NetworkValueType == typeof (uint))
                            command.NetworkValue = _packetReader.ReadUInt32();
                        else if (command.NetworkValueType == typeof (ulong))
                            command.NetworkValue = _packetReader.ReadInt64();
                        else if (command.NetworkValueType == typeof (ushort))
                            command.NetworkValue = _packetReader.ReadUInt16();
                        else if (command.NetworkValueType == typeof (Vector2))
                            command.NetworkValue = _packetReader.ReadVector2();
                        else if (command.NetworkValueType == typeof (Vector3))
                            command.NetworkValue = _packetReader.ReadVector3();
                        else if (command.NetworkValueType == typeof (Vector4))
                            command.NetworkValue = _packetReader.ReadVector4();

                        if (command.NetworkValue == null)
                            throw new CoreException("No value transfered");

                        if (command.Condition == null || (command.Condition.Invoke()))
                            command.NetworkValue = command.ServerExecution(command, command.NetworkValue);

                        if(command.ApplyServerResult != null)
                            ExecuteServerCommandOnClients(command);
                        else
                            command.WaitingForServerReply = false;
                    }
                        // Some Commands may not return values and ask the server to compute it for all clients
                    else if (message == Constants.ExecuteCommandOnServerNoDataExchanged)
                    {
                        if (!gamer.IsHost)
                            throw new CoreException("Gamer isn't host");

                        ushort commandId = _packetReader.ReadUInt16();

                        Command command = Commands[commandId];

                        if (command.Condition == null || (command.Condition.Invoke()))
                            command.NetworkValue = command.ServerExecution(command, null);

                        if (command.ApplyServerResult != null)
                            ExecuteServerCommandOnClients(command);
                        else
                            command.WaitingForServerReply = false;
                    }
                    else if (message == Constants.SynchronizeCommandOnClient)
                    {
                        if (CommandsToSynchronize.Count > 0)
                        {
                            Command command = CommandsToSynchronize.Dequeue();

                            ushort commandId = _packetReader.ReadUInt16();

                            if (Commands.ContainsKey(commandId))
                            {
                                Commands[commandId] = command;
                            }
                            else
                            {
                                command.Id = commandId;
                                Commands.Add(command.Id, command);
                            }
                        }
                    }
                    else if (message == Constants.SynchronizeSceneEntitiesOnClient)
                    {
                        if (SceneEntitiesToSynchronize.Count > 0)
                        {
                            SceneEntity entity = (SceneEntity) SceneEntitiesToSynchronize.Dequeue();

                            var entityUniqueId = _packetReader.ReadInt32();

                            if (RegisteredEntities.ContainsKey(entityUniqueId))
                            {
                                RegisteredEntities[entityUniqueId] = entity;
                            }
                            else
                            {
                                entity.UniqueId = entityUniqueId;
                                RegisteredEntities.Add(entity.UniqueId, entity);
                            }
                        }
                    }
                    else if (message == Constants.SynchronizationDoneOnClient)
                    {
                        if(!gamer.IsHost)
                            throw new CoreException("Gamer isn't host");

                        var synchronizedPlayers = new List<IdentifiedPlayer>();

                        if (sender.IsLocal)
                        {
                            foreach (var player in PlayersToSynchronize)
                            {
                                if(player.IsLocal)
                                    synchronizedPlayers.Add(player);
                            }
                        }
                        else
                        {
                            var senderMachine = sender.Machine;

                            foreach (var player in PlayersToSynchronize)
                            {
                                if(!player.IsLocal)
                                {
                                    if (((NetworkGamer)((LiveIdentifiedPlayer)player).LiveGamer).Machine.Equals(senderMachine))
                                        synchronizedPlayers.Add(player);
                                }
                            }
                        }

                        if (synchronizedPlayers.Count > 0)
                        {
                            foreach (var synchronizedPlayer in synchronizedPlayers)
                            {
                                PlayersToSynchronize.Remove(synchronizedPlayer);
                            }

                            if (PlayersToSynchronize.Count == 0)
                            {
                                _sessionState = SessionState.Playing;
                                OnStarted();
                            }
                        }
                    }
                    else
                    {
                        ushort commandId = _packetReader.ReadUInt16();

                        Command command = Commands[commandId];

                        command.WaitingForServerReply = false;

                        object networkValue = null;

                        if (message == Constants.ExecuteServerCommandOnClientsDataExchanged)
                        {
                            if (command.NetworkValueType == typeof(bool))
                                networkValue = _packetReader.ReadBoolean();
                            else if (command.NetworkValueType == typeof(byte))
                                networkValue = _packetReader.ReadByte();
                            else if (command.NetworkValueType == typeof(byte[]))
                            {
                                int tempNumberOfBytes = _packetReader.ReadInt32();
                                networkValue = _packetReader.ReadBytes(tempNumberOfBytes);
                            }
                            else if (command.NetworkValueType == typeof(char))
                                networkValue = _packetReader.ReadChar();
                            else if (command.NetworkValueType == typeof(char[]))
                            {
                                int tempNumberOfChars = _packetReader.ReadInt32();
                                networkValue = _packetReader.ReadChars(tempNumberOfChars);
                            }
                            else if (command.NetworkValueType == typeof(Color))
                                networkValue = _packetReader.ReadColor();
                            else if (command.NetworkValueType == typeof(double))
                                networkValue = _packetReader.ReadDouble();
                            else if (command.NetworkValueType == typeof(float))
                                networkValue = _packetReader.ReadSingle();
                            else if (command.NetworkValueType == typeof(int))
                                networkValue = _packetReader.ReadInt32();
                            else if (command.NetworkValueType == typeof(long))
                                networkValue = _packetReader.ReadInt64();
                            else if (command.NetworkValueType == typeof(Matrix))
                                networkValue = _packetReader.ReadMatrix();
                            else if (command.NetworkValueType == typeof(Quaternion))
                                networkValue = _packetReader.ReadQuaternion();
                            else if (command.NetworkValueType == typeof(sbyte))
                                networkValue = _packetReader.ReadSByte();
                            else if (command.NetworkValueType == typeof(short))
                                networkValue = _packetReader.ReadInt16();
                            else if (command.NetworkValueType == typeof(string))
                                networkValue = _packetReader.ReadString();
                            else if (command.NetworkValueType == typeof(uint))
                                networkValue = _packetReader.ReadUInt32();
                            else if (command.NetworkValueType == typeof(ulong))
                                networkValue = _packetReader.ReadInt64();
                            else if (command.NetworkValueType == typeof(ushort))
                                networkValue = _packetReader.ReadUInt16();
                            else if (command.NetworkValueType == typeof(Vector2))
                                networkValue = _packetReader.ReadVector2();
                            else if (command.NetworkValueType == typeof(Vector3))
                                networkValue = _packetReader.ReadVector3();
                            else if (command.NetworkValueType == typeof(Vector4))
                                networkValue = _packetReader.ReadVector4();
                        }

                        command.ApplyServerResult(command, networkValue);
                    }
                }
            }
        }

        /// <summary>
        /// Asks the Session to send a remote command call on all session clients
        /// </summary>
        /// <param name="command">The command that should be executed</param>
        public override void ExecuteServerCommandOnClients(Command command)
        {
            if (!IsHost)
                throw new CoreException("Only the host can execute server commands on clients");

            if (command.NetworkValue != null)
                _packetWriter.Write(Constants.ExecuteServerCommandOnClientsDataExchanged);
            else
                _packetWriter.Write(Constants.ExecuteServerCommandOnClientsNoDataExchanged);

            _packetWriter.Write(command.Id);

            if (command.NetworkValue != null)
            {
                WriteNetworkValue(ref _packetWriter, command.NetworkValue);
            }

            ((LocalNetworkGamer) _networkSession.Host).SendData(_packetWriter,
                                                                ConvertToSendDataOptions(command.TransferOptions));
        }

        /// <summary>
        /// Update loop call
        /// </summary>
        /// <param name="gameTime"/>
        public override void Update(GameTime gameTime)
        {
            _networkSession.Update();
        }

        /// <summary>
        /// Notifies the server that the synchronization with the server is performed locally
        /// </summary>
        protected override void NotifyServerSynchronizationDoneOnClient()
        {
            _packetWriter.Write(Constants.SynchronizationDoneOnClient);
            ((LocalNetworkGamer) ((LiveIdentifiedPlayer) LocalPlayers[0]).LiveGamer).SendData(_packetWriter, SendDataOptions.ReliableInOrder, _networkSession.Host);
        }

        /// <summary>
        /// Asks the Session to send a remote command call on the session host
        /// </summary>
        /// <param name="command">The command that should be executed</param>
        public override void ExecuteCommandOnServer(Command command)
        {
            if (command.NetworkValue != null)
                _packetWriter.Write(Constants.ExecuteCommandOnServerDataExchanged);
            else
                _packetWriter.Write(Constants.ExecuteCommandOnServerNoDataExchanged);

            _packetWriter.Write(command.Id);

            if (command.NetworkValue != null)
                WriteNetworkValue(ref _packetWriter, command.NetworkValue);

            if (command.Behavior.Agent is PlayerAgent)
            {
                ((LocalNetworkGamer)
                 ((LiveIdentifiedPlayer) ((PlayerAgent) command.Behavior.Agent).IdentifiedPlayer).LiveGamer).SendData(
                     _packetWriter, ConvertToSendDataOptions(command.TransferOptions), _networkSession.Host);
            }
            else
            {
                ((LocalNetworkGamer) ((LiveIdentifiedPlayer) LocalPlayers[0]).LiveGamer).
                    SendData(_packetWriter, ConvertToSendDataOptions(command.TransferOptions), _networkSession.Host);
            }

            command.WaitingForServerReply = true;
        }

        #endregion
    }
}