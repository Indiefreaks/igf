using System;
using System.Collections.Generic;
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
        private void ProcessServerMessages()
        {
            while ((_incomingMessage = LidgrenSessionManager.Server.ReadMessage()) != null)
            {
                switch (_incomingMessage.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        {
                            var status = (NetConnectionStatus) _incomingMessage.ReadByte();
                            switch (status)
                            {
                                case NetConnectionStatus.Disconnected:
                                    {
                                        IPEndPoint lostConnectionIpEndPoint = _incomingMessage.SenderEndpoint;

                                        var playersToRemove = new List<IdentifiedPlayer>(1);

                                        foreach (var remotePlayerIpEndPoint in _remotePlayerIpEndPoints)
                                        {
                                            if (remotePlayerIpEndPoint.Value.Equals(lostConnectionIpEndPoint))
                                                playersToRemove.Add(remotePlayerIpEndPoint.Key);
                                        }

                                        foreach (IdentifiedPlayer identifiedPlayer in playersToRemove)
                                        {
                                            _remotePlayers.Remove(identifiedPlayer);
                                            _remotePlayerIpEndPoints.Remove(identifiedPlayer);
                                            _allPlayers.Remove(identifiedPlayer);
                                        }

                                        SendDisconnectedPlayersToClients(playersToRemove);
                                        break;
                                    }
                                case NetConnectionStatus.Connected:
                                case NetConnectionStatus.Disconnecting:
                                case NetConnectionStatus.InitiatedConnect:
                                case NetConnectionStatus.None:
                                case NetConnectionStatus.RespondedAwaitingApproval:
                                case NetConnectionStatus.RespondedConnect:
                                default:
                                    {
                                        Console.WriteLine("Server Status changed to " + status);
                                        break;
                                    }
                            }
                            break;
                        }
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                        {
                            Console.WriteLine(_incomingMessage.ReadString());
                            break;
                        }
                    case NetIncomingMessageType.Data:
                        {
                            var msgType = (LidgrenMessages) _incomingMessage.ReadByte();

                            switch (msgType)
                            {
                                case LidgrenMessages.SendLocalPlayersToServer:
                                    {
                                        List<LidgrenIdentifiedPlayer> clientPlayers = AddNewPlayersOnServer();

                                        if (AllPlayers.Count > 0)
                                            SendPlayersListToJustConnectedClient();

                                        if(LidgrenSessionManager.Server.ConnectionsCount > 1)
                                            SendNewPlayersToClients(clientPlayers);
                                        break;
                                    }
                                case LidgrenMessages.SendSynchronizationDoneToServer:
                                    {
                                        var synchronizedPlayers = new List<IdentifiedPlayer>();

                                        if (NetUtility.IsLocal(_incomingMessage.SenderEndpoint))
                                        {
                                            foreach (IdentifiedPlayer player in PlayersToSynchronize)
                                            {
                                                if (player.IsLocal)
                                                    synchronizedPlayers.Add(player);
                                            }
                                        }
                                        else
                                        {
                                            IPEndPoint remoteEndPoint = _incomingMessage.SenderEndpoint;

                                            foreach (IdentifiedPlayer player in PlayersToSynchronize)
                                            {
                                                if (!player.IsLocal)
                                                {
                                                    if (_remotePlayerIpEndPoints[player].Equals(remoteEndPoint))
                                                        synchronizedPlayers.Add(player);
                                                }
                                            }
                                        }

                                        if (synchronizedPlayers.Count > 0)
                                        {
                                            foreach (IdentifiedPlayer synchronizedPlayer in synchronizedPlayers)
                                            {
                                                PlayersToSynchronize.Remove(synchronizedPlayer);
                                            }

                                            if (PlayersToSynchronize.Count == 0)
                                            {
                                                _serverSessionState = SessionState.Playing;

                                                SendSessionStateChangedToClients();
                                            }
                                        }
                                        break;
                                    }
                                case LidgrenMessages.ExecuteCommandOnServerDataExchanged:
                                    {
                                        ushort commandId = _incomingMessage.ReadUInt16();

                                        Command command = Commands[commandId];

                                        if (command.NetworkValueType == typeof (bool))
                                            command.NetworkValue = _incomingMessage.ReadBoolean();
                                        else if (command.NetworkValueType == typeof (byte))
                                            command.NetworkValue = _incomingMessage.ReadByte();
                                        else if (command.NetworkValueType == typeof (byte[]))
                                        {
                                            int tempNumberOfBytes = _incomingMessage.ReadInt32();
                                            command.NetworkValue = _incomingMessage.ReadBytes(tempNumberOfBytes);
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
                                            command.NetworkValue = _incomingMessage.ReadDouble();
                                        else if (command.NetworkValueType == typeof (float))
                                            command.NetworkValue = _incomingMessage.ReadSingle();
                                        else if (command.NetworkValueType == typeof (int))
                                            command.NetworkValue = _incomingMessage.ReadInt32();
                                        else if (command.NetworkValueType == typeof (long))
                                            command.NetworkValue = _incomingMessage.ReadInt64();
                                        else if (command.NetworkValueType == typeof (Matrix))
                                            command.NetworkValue = _incomingMessage.ReadMatrix();
                                        else if (command.NetworkValueType == typeof (Quaternion))
                                            command.NetworkValue = _incomingMessage.ReadRotation(24);
                                        else if (command.NetworkValueType == typeof (sbyte))
                                            command.NetworkValue = _incomingMessage.ReadSByte();
                                        else if (command.NetworkValueType == typeof (short))
                                            command.NetworkValue = _incomingMessage.ReadInt16();
                                        else if (command.NetworkValueType == typeof (string))
                                            command.NetworkValue = _incomingMessage.ReadString();
                                        else if (command.NetworkValueType == typeof (uint))
                                            command.NetworkValue = _incomingMessage.ReadUInt32();
                                        else if (command.NetworkValueType == typeof (ulong))
                                            command.NetworkValue = _incomingMessage.ReadInt64();
                                        else if (command.NetworkValueType == typeof (ushort))
                                            command.NetworkValue = _incomingMessage.ReadUInt16();
                                        else if (command.NetworkValueType == typeof (Vector2))
                                            command.NetworkValue = _incomingMessage.ReadVector2();
                                        else if (command.NetworkValueType == typeof (Vector3))
                                            command.NetworkValue = _incomingMessage.ReadVector3();
                                        else if (command.NetworkValueType == typeof (Vector4))
                                            command.NetworkValue = _incomingMessage.ReadVector4();

                                        if (command.NetworkValue == null)
                                            throw new CoreException("No value transfered");

                                        if (command.Condition == null || (command.Condition.Invoke()))
                                            command.NetworkValue = command.ServerExecution(command, command.NetworkValue);

                                        if (command.ApplyServerResult != null)
                                            ExecuteServerCommandOnClients(command);
                                        else
                                            command.WaitingForServerReply = false;

                                        break;
                                    }
                                case LidgrenMessages.ExecuteCommandOnServerNoDataExchanged:
                                    {
                                        ushort commandId = _incomingMessage.ReadUInt16();

                                        Command command = Commands[commandId];

                                        if (command.Condition == null || (command.Condition.Invoke()))
                                            command.NetworkValue = command.ServerExecution(command, null);

                                        if (command.ApplyServerResult != null)
                                            ExecuteServerCommandOnClients(command);
                                        else
                                            command.WaitingForServerReply = false;
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case NetIncomingMessageType.DiscoveryRequest:
                        {
                            _outgoingMessage = LidgrenSessionManager.Server.CreateMessage();

                            _outgoingMessage.Write((byte) SessionType);
                            _outgoingMessage.WriteVariableInt32(AllPlayers.Count);
                            _outgoingMessage.WriteVariableInt32(_privateReservedSlots);
                            _outgoingMessage.WriteVariableInt32(_maxGamers - AllPlayers.Count - _privateReservedSlots);

                            _outgoingMessage.WriteVariableInt32(_sessionProperties.Count);
                            for (int i = 0; i < _sessionProperties.Count; i++)
                            {
                                _outgoingMessage.Write(BitConverter.GetBytes(_sessionProperties[i].Value));
                            }

                            LidgrenSessionManager.Server.SendDiscoveryResponse(_outgoingMessage, _incomingMessage.SenderEndpoint);

                            break;
                        }
                    case NetIncomingMessageType.ConnectionApproval:
                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                    case NetIncomingMessageType.DiscoveryResponse:
                    case NetIncomingMessageType.Error:
                    case NetIncomingMessageType.NatIntroductionSuccess:
                    case NetIncomingMessageType.Receipt:
                    case NetIncomingMessageType.UnconnectedData:
                    default:
                        {
                            Console.WriteLine("Server received " + _incomingMessage.MessageType);
                            break;
                        }
                }
            }
        }

        private List<LidgrenIdentifiedPlayer> AddNewPlayersOnServer()
        {
            var newPlayers = new List<LidgrenIdentifiedPlayer>();
            int numberOfPlayers = _incomingMessage.ReadVariableInt32();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                // we retrieve the player sent by the client
                var player = new LidgrenIdentifiedPlayer(_incomingMessage.ReadString())
                                 {
                                     DisplayName = _incomingMessage.ReadString(),
                                 };

                // we test if the provided player is local
                foreach ( var localPlayer in LidgrenSessionManager.LocalPlayers.Values)
                {
                    // if that is the case, we add the local player to the server session in AllPlayers list.
                    if (localPlayer == player)
                    {
                        _allPlayers.Add(localPlayer);
                    }
                    // otherwise, we add the remote player to the server session in AllPlayers list.
                    else
                    {
                        _allPlayers.Add(player);
                    }

                    newPlayers.Add(player);
                }
            }

            return newPlayers;
        }

        private void SendNewPlayersToClients(List<LidgrenIdentifiedPlayer> clientPlayers)
        {
            _outgoingMessage = LidgrenSessionManager.Server.CreateMessage();
            _outgoingMessage.Write((byte) LidgrenMessages.SendNewPlayersToClients);
            _outgoingMessage.WriteVariableInt32(clientPlayers.Count);
            _outgoingMessage.Write(_incomingMessage.SenderEndpoint);

            foreach (LidgrenIdentifiedPlayer identifiedPlayer in clientPlayers)
            {
                _outgoingMessage.Write(identifiedPlayer.UniqueId);
                _outgoingMessage.Write(identifiedPlayer.DisplayName);
            }

            LidgrenSessionManager.Server.SendToAll(_outgoingMessage, _incomingMessage.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
        }

        private void SendPlayersListToJustConnectedClient()
        {
            _outgoingMessage = LidgrenSessionManager.Server.CreateMessage();
            _outgoingMessage.Write((byte) LidgrenMessages.SendPlayersListToJustConnectedClient);
            
            _outgoingMessage.Write(_allPlayers.Count);

            foreach (IdentifiedPlayer identifiedPlayer in _allPlayers)
            {
                _outgoingMessage.Write(identifiedPlayer.UniqueId);
                _outgoingMessage.Write(identifiedPlayer.DisplayName);
                _outgoingMessage.Write(identifiedPlayer.IsHost);
                _outgoingMessage.Write(identifiedPlayer.IsLocal);
                // if true, the player is on the Server & the Client will have to retrieve the IPEndPoint from the connection to the server
                // if false, the player is on a remote client & the client will have to retrieve the IPEndPoint by reading another bytes on the message
                if(!identifiedPlayer.IsLocal)
                    _outgoingMessage.Write(_remotePlayerIpEndPoints[identifiedPlayer]);
            }

            LidgrenSessionManager.Server.SendMessage(_outgoingMessage, _incomingMessage.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }

        public override void SynchronizeCommandOnClients(Command command)
        {
            _outgoingMessage = LidgrenSessionManager.Server.CreateMessage();

            _outgoingMessage.Write((byte)LidgrenMessages.SendCommandsToClients);
            _outgoingMessage.Write(command.Id);

            LidgrenSessionManager.Server.SendToAll(_outgoingMessage, NetDeliveryMethod.ReliableOrdered);
        }

        public override void SynchronizeSceneEntitiesOnClients(ISceneEntity sceneEntity)
        {
            _outgoingMessage = LidgrenSessionManager.Server.CreateMessage();

            _outgoingMessage.Write((byte)LidgrenMessages.SendSceneEntitiesToClients);
            _outgoingMessage.WriteVariableInt32(sceneEntity.UniqueId);

            LidgrenSessionManager.Server.SendToAll(_outgoingMessage, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Asks the Session to send a remote command call on all session clients
        /// </summary>
        /// <param name="command">The command that should be executed</param>
        public override void ExecuteServerCommandOnClients(Command command)
        {
            if (!IsHost)
                throw new CoreException("Only the host can execute server commands on clients");
            
            _outgoingMessage = LidgrenSessionManager.Server.CreateMessage();

            if (command.NetworkValue != null)
            {
                _outgoingMessage.Write((byte)LidgrenMessages.ExecuteServerCommandOnClientsDataExchanged);
            }
            else
            {
                _outgoingMessage.Write((byte) LidgrenMessages.ExecuteServerCommandOnClientsNoDataExchanged);
            }

            _outgoingMessage.Write(command.Id);

            if (command.NetworkValue != null)
            {
                WriteNetworkValue(ref _outgoingMessage, command.NetworkValue);
            }

            LidgrenSessionManager.Server.SendToAll(_outgoingMessage, ConvertToNetDeliveryMethod(command.TransferOptions));
        }

        private void SendDisconnectedPlayersToClients(List<IdentifiedPlayer> playersToRemove)
        {
            _outgoingMessage = LidgrenSessionManager.Server.CreateMessage();
            _outgoingMessage.Write((byte) LidgrenMessages.SendDisconnectedPlayersToClients);
            _outgoingMessage.WriteVariableInt32(playersToRemove.Count);

            foreach (IdentifiedPlayer identifiedPlayer in playersToRemove)
            {
                _outgoingMessage.Write(identifiedPlayer.UniqueId);
            }

            if (LidgrenSessionManager.Server.ConnectionsCount > 0)
                LidgrenSessionManager.Server.SendToAll(_outgoingMessage, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendSessionStateChangedToClients()
        {
            _outgoingMessage = LidgrenSessionManager.Server.CreateMessage();
            _outgoingMessage.Write((byte) LidgrenMessages.SendSessionStateChangedToClients);
            _outgoingMessage.Write((byte) _serverSessionState);

            if (LidgrenSessionManager.Server.ConnectionsCount != 0)
                LidgrenSessionManager.Server.SendToAll(_outgoingMessage, NetDeliveryMethod.ReliableOrdered);
        }
    }
}