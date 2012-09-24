using System;
using System.Net;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Logic;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Sessions.Lidgren
{
    public partial class LidgrenSession
    {
        private void ProcessClientMessages()
        {
            while ((_incomingMessage = LidgrenSessionManager.Client.ReadMessage()) != null)
            {
                switch (_incomingMessage.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                        {
                            Console.WriteLine(_incomingMessage.ReadString());
                            break;
                        }
                    case NetIncomingMessageType.StatusChanged:
                        {
                            var status = (NetConnectionStatus) _incomingMessage.ReadByte();
                            switch (status)
                            {
                                case NetConnectionStatus.Connected:
                                    {
                                        SendAllLocalPlayersToServer();
                                        break;
                                    }
                                case NetConnectionStatus.Disconnected:
                                case NetConnectionStatus.Disconnecting:
                                case NetConnectionStatus.InitiatedConnect:
                                case NetConnectionStatus.None:
                                case NetConnectionStatus.RespondedAwaitingApproval:
                                case NetConnectionStatus.RespondedConnect:
                                default:
                                    {
                                        Console.WriteLine("Client Status changed  to " + status);
                                        break;
                                    }
                            }
                            break;
                        }
                    case NetIncomingMessageType.Data:
                        {
                            var msgType = (LidgrenMessages) _incomingMessage.ReadByte();
                            
                            switch (msgType)
                            {
                                case LidgrenMessages.SendPlayersListToJustConnectedClient:
                                    {
                                        foreach (var localPlayer in LidgrenSessionManager.LocalPlayers.Values)
                                        {
                                            _localPlayers.Add(localPlayer);

                                            OnPlayerJoined(localPlayer);
                                        }

                                        RetrieveRemotePlayersFromServer();

                                        Application.SunBurn.GetManager<ISessionManager>(true).OnSessionCreated();
                                        break;
                                    }
                                case LidgrenMessages.SendNewPlayersToClients:
                                    {
                                        RetrieveNewPlayersFromServer();
                                        break;
                                    }
                                case LidgrenMessages.SendDisconnectedPlayersToClients:
                                    {
                                        RetrieveDisconnectedPlayersFromServer();
                                        break;
                                    }
                                case LidgrenMessages.SendSessionStateChangedToClients:
                                    {
                                        RetrieveSessionStateChangedFromServer();
                                        break;
                                    }
                                case LidgrenMessages.SendCommandsToClients:
                                    {
                                        RetrieveCommandsFromServer();
                                        break;
                                    }
                                case LidgrenMessages.SendSceneEntitiesToClients:
                                    {
                                        RetrieveSceneEntitiesFromServer();
                                        break;
                                    }
                                case LidgrenMessages.ExecuteCommandOnServerDataExchanged:
                                    {
                                        ushort commandId = _incomingMessage.ReadUInt16();

                                        Command command = Commands[commandId];

                                        command.WaitingForServerReply = false;

                                        object networkValue = null;

                                        if (command.NetworkValueType == typeof (bool))
                                            networkValue = _incomingMessage.ReadBoolean();
                                        else if (command.NetworkValueType == typeof (byte))
                                            networkValue = _incomingMessage.ReadByte();
                                        else if (command.NetworkValueType == typeof (byte[]))
                                        {
                                            int tempNumberOfBytes = _incomingMessage.ReadInt32();
                                            networkValue = _incomingMessage.ReadBytes(tempNumberOfBytes);
                                        }
                                        else if (command.NetworkValueType == typeof (char))
                                            command.NetworkValue = Convert.ToChar(_incomingMessage.ReadByte());
                                        else if (command.NetworkValueType == typeof (char[]))
                                        {
                                            int tempNumberOfChars = _incomingMessage.ReadInt32();
                                            for (int i = 0; i < tempNumberOfChars; i++)
                                            {
                                                command.NetworkValue = _incomingMessage.ReadBytes(tempNumberOfChars*8);
                                            }
                                        }
                                        else if (command.NetworkValueType == typeof (Color))
                                            command.NetworkValue = new Color(_incomingMessage.ReadVector4());
                                        else if (command.NetworkValueType == typeof (double))
                                            networkValue = _incomingMessage.ReadDouble();
                                        else if (command.NetworkValueType == typeof (float))
                                            networkValue = _incomingMessage.ReadSingle();
                                        else if (command.NetworkValueType == typeof (int))
                                            networkValue = _incomingMessage.ReadInt32();
                                        else if (command.NetworkValueType == typeof (long))
                                            networkValue = _incomingMessage.ReadInt64();
                                        else if (command.NetworkValueType == typeof (Matrix))
                                            networkValue = _incomingMessage.ReadMatrix();
                                        else if (command.NetworkValueType == typeof (Quaternion))
                                            networkValue = _incomingMessage.ReadRotation(24);
                                        else if (command.NetworkValueType == typeof (sbyte))
                                            networkValue = _incomingMessage.ReadSByte();
                                        else if (command.NetworkValueType == typeof (short))
                                            networkValue = _incomingMessage.ReadInt16();
                                        else if (command.NetworkValueType == typeof (string))
                                            networkValue = _incomingMessage.ReadString();
                                        else if (command.NetworkValueType == typeof (uint))
                                            networkValue = _incomingMessage.ReadUInt32();
                                        else if (command.NetworkValueType == typeof (ulong))
                                            networkValue = _incomingMessage.ReadInt64();
                                        else if (command.NetworkValueType == typeof (ushort))
                                            networkValue = _incomingMessage.ReadUInt16();
                                        else if (command.NetworkValueType == typeof (Vector2))
                                            networkValue = _incomingMessage.ReadVector2();
                                        else if (command.NetworkValueType == typeof (Vector3))
                                            networkValue = _incomingMessage.ReadVector3();
                                        else if (command.NetworkValueType == typeof (Vector4))
                                            networkValue = _incomingMessage.ReadVector4();
                                        else
                                            throw new CoreException("Not supported NetworkValueType");

                                        if (networkValue == null)
                                            throw new CoreException("No value transfered");

                                        command.ApplyServerResult(command, networkValue);
                                        break;
                                    }
                                case LidgrenMessages.ExecuteCommandOnServerNoDataExchanged:
                                    {
                                        ushort commandId = _incomingMessage.ReadUInt16();

                                        Command command = Commands[commandId];

                                        command.WaitingForServerReply = false;

                                        object networkValue = null;

                                        command.ApplyServerResult(command, networkValue);
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case NetIncomingMessageType.DiscoveryResponse:
                        {
                            var sessionType = (SessionType) _incomingMessage.ReadByte();
                            int currentGamerCount = _incomingMessage.ReadVariableInt32();
                            string hostName = _incomingMessage.SenderEndpoint.Address.ToString();
                            int openPrivateSlots = _incomingMessage.ReadVariableInt32();
                            int openPublicSlots = _incomingMessage.ReadVariableInt32();
                            var averageRoundtripTime = new TimeSpan(0, 0, 0, 0, (int) (_incomingMessage.SenderConnection.AverageRoundtripTime*1000));

                            var sessionProperties = new SessionProperties();
                            int sessionPropertiesCount = _incomingMessage.ReadVariableInt32();
                            if (sessionPropertiesCount > 0)
                            {
                                for (int i = 0; i < sessionPropertiesCount; i++)
                                {
                                    sessionProperties[i] = Convert.ToInt32(_incomingMessage.ReadBytes(32));
                                }
                            }

                            var sessionFound = new LidgrenAvailableSession(sessionType, currentGamerCount, hostName, openPrivateSlots, openPublicSlots, sessionProperties, averageRoundtripTime);
                            LidgrenSessionsFound.Add(sessionFound);

                            break;
                        }
                    case NetIncomingMessageType.ConnectionApproval:
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                    case NetIncomingMessageType.DiscoveryRequest:
                    case NetIncomingMessageType.Error:
                    case NetIncomingMessageType.NatIntroductionSuccess:
                    case NetIncomingMessageType.Receipt:
                    case NetIncomingMessageType.UnconnectedData:
                    default:
                        {
                            Console.WriteLine("Client received " + _incomingMessage.MessageType);
                            break;
                        }
                }
            }
        }

        private void RetrieveSceneEntitiesFromServer()
        {
            if (SceneEntitiesToSynchronize.Count > 0)
            {
                var entity = (SceneEntity) SceneEntitiesToSynchronize.Dequeue();

                int entityUniqueId = _incomingMessage.ReadVariableInt32();

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

        private void RetrieveCommandsFromServer()
        {
            if (CommandsToSynchronize.Count > 0)
            {
                Command command = CommandsToSynchronize.Dequeue();

                ushort commandId = _incomingMessage.ReadUInt16();

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

        /// <summary>
        /// Notifies the server that the synchronization with the server is performed locally
        /// </summary>
        protected override void NotifyServerSynchronizationDoneOnClient()
        {
            _outgoingMessage = LidgrenSessionManager.Client.CreateMessage();
            _outgoingMessage.Write((byte) LidgrenMessages.SendSynchronizationDoneToServer);

            LidgrenSessionManager.Client.SendMessage(_outgoingMessage, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendAllLocalPlayersToServer()
        {
            _outgoingMessage = LidgrenSessionManager.Client.CreateMessage();
            _outgoingMessage.Write((byte) LidgrenMessages.SendLocalPlayersToServer);
            _outgoingMessage.WriteVariableInt32(LidgrenSessionManager.LocalPlayers.Count);

            foreach (IdentifiedPlayer player in LidgrenSessionManager.LocalPlayers.Values)
            {
                _outgoingMessage.Write(player.UniqueId);
                _outgoingMessage.Write(player.DisplayName);
            }

            LidgrenSessionManager.Client.SendMessage(_outgoingMessage, NetDeliveryMethod.ReliableOrdered);
        }

        private void RetrieveRemotePlayersFromServer()
        {
            int remotePlayers = _incomingMessage.ReadVariableInt32();
            for (int i = 0; i < remotePlayers; i++)
            {
                var uniqueId = _incomingMessage.ReadString();
                var displayName = _incomingMessage.ReadString();
                var isHost = _incomingMessage.ReadBoolean();
                var isOnServer = _incomingMessage.ReadBoolean();

                var player = new LidgrenIdentifiedPlayer(uniqueId)
                                 {
                                     DisplayName = displayName,
                                 };
                if(isHost)
                    player.SetIsHost();

                _remotePlayers.Add(player);
                _allPlayers.Add(player);

                if(isOnServer)
                    _remotePlayerIpEndPoints.Add(player, LidgrenSessionManager.Client.ServerConnection.RemoteEndpoint);
                else
                    _remotePlayerIpEndPoints.Add(player, _incomingMessage.ReadIPEndpoint());

                OnPlayerJoined(player);
            }
        }

        private void RetrieveNewPlayersFromServer()
        {
            int numberOfNewPlayers = _incomingMessage.ReadVariableInt32();
            IPEndPoint ipEndPoint = _incomingMessage.ReadIPEndpoint();

            for (int i = 0; i < numberOfNewPlayers; i++)
            {
                var player = new LidgrenIdentifiedPlayer(_incomingMessage.ReadString())
                                 {
                                     DisplayName = _incomingMessage.ReadString(),
                                 };

                _remotePlayers.Add(player);
                _remotePlayerIpEndPoints.Add(player, ipEndPoint);
                _allPlayers.Add(player);

                OnPlayerJoined(player);
            }
        }

        private void RetrieveDisconnectedPlayersFromServer()
        {
            int numberOfDisconnectedPlayers = _incomingMessage.ReadVariableInt32();

            for (int i = 0; i < numberOfDisconnectedPlayers; i++)
            {
                string playerUniqueId = _incomingMessage.ReadString();

                IdentifiedPlayer player = _remotePlayers.Find(match => match.UniqueId == playerUniqueId);

                _remotePlayers.Remove(player);
                _remotePlayerIpEndPoints.Remove(player);
                _allPlayers.Remove(player);

                OnPlayerLeft(player);
            }
        }

        /// <summary>
        /// Asks the Session to send a remote command call on the session host
        /// </summary>
        /// <param name="command">The command that should be executed</param>
        public override void ExecuteCommandOnServer(Command command)
        {
            _outgoingMessage = LidgrenSessionManager.Client.CreateMessage();
            
            if (command.NetworkValue != null)
            {
                _outgoingMessage.Write((byte) LidgrenMessages.ExecuteCommandOnServerDataExchanged);
            }
            else
            {
                _outgoingMessage.Write((byte) LidgrenMessages.ExecuteCommandOnServerNoDataExchanged);
            }

            _outgoingMessage.Write(command.Id);

            if (command.NetworkValue != null)
                WriteNetworkValue(ref _outgoingMessage, command.NetworkValue);

            LidgrenSessionManager.Client.SendMessage(_outgoingMessage, ConvertToNetDeliveryMethod(command.TransferOptions));
        }

        private void RetrieveSessionStateChangedFromServer()
        {
            var newStatus = (SessionState) _incomingMessage.ReadByte();
            _clientSessionState = newStatus;

            switch (_clientSessionState)
            {
                case SessionState.Lobby:
                    {
                        break;
                    }
                case SessionState.Starting:
                    {
                        OnStarting();
                        break;
                    }
                case SessionState.Playing:
                    {
                        OnStarted();
                        break;
                    }
                case SessionState.Ended:
                    {
                        OnEnded();
                        break;
                    }
                case SessionState.Closed:
                    {
                        LidgrenSessionManager.CloseSession();

                        break;
                    }
            }
        }
    }
}