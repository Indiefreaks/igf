using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Logic;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Sessions.Local
{
    public class LocalSession : Session
    {
        private readonly Dictionary<Command, ushort> _executeCommands = new Dictionary<Command, ushort>();
        private readonly List<IdentifiedPlayer> _localPlayers = new List<IdentifiedPlayer>(1);
        private readonly Dictionary<Command, ushort> _processingCommands = new Dictionary<Command, ushort>();
        private readonly List<IdentifiedPlayer> _remotePlayers = new List<IdentifiedPlayer>(0);
        private SessionState _currentState = SessionState.Lobby;
        private SessionState _previousState = SessionState.Lobby;

        #region Overrides of Session

        /// <summary>
        /// Returns the list of players in the current session
        /// </summary>
        public override IList<IdentifiedPlayer> AllPlayers
        {
            get { return _localPlayers.AsReadOnly(); }
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
            get { return _currentState; }
        }

        /// <summary>
        /// Returns the current Session SessionType
        /// </summary>
        public override SessionType SessionType
        {
            get { return SessionType.SinglePlayer; }
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
            get { return _localPlayers[0]; }
        }

        /// <summary>
        /// Returns the current Session Bytes/Second received
        /// </summary>
        public override int BytesPerSecondReceived
        {
            get { return 0; }
        }

        /// <summary>
        /// Returns the current Session Bytes/Second sent
        /// </summary>
        public override int BytesPerSecondSent
        {
            get { return 0; }
        }

        /// <summary>
        /// Returns if the current Session is Host
        /// </summary>
        public override bool IsHost
        {
            get { return true; }
        }

        /// <summary>
        /// Asks the Session to send a remote command call on the session host
        /// </summary>
        /// <param name="command">The command that should be executed</param>
        public override void ExecuteCommandOnServer(Command command)
        {
            command.WaitingForServerReply = true;

            if (command.NetworkValue == null)
                _executeCommands.Add(command, Constants.ExecuteCommandOnServerNoDataExchanged);
            else
                _executeCommands.Add(command, Constants.ExecuteCommandOnServerDataExchanged);
        }

        /// <summary>
        /// Asks the Session to send a remote command call on all session clients
        /// </summary>
        /// <param name="command">The command that should be executed</param>
        public override void ExecuteServerCommandOnClients(Command command)
        {
            if (command.NetworkValue == null)
                _executeCommands.Add(command, Constants.ExecuteServerCommandOnClientsNoDataExchanged);
            else
                _executeCommands.Add(command, Constants.ExecuteServerCommandOnClientsDataExchanged);
        }

        /// <summary>
        /// Update loop call
        /// </summary>
        /// <param name="gameTime"/>
        public override void Update(GameTime gameTime)
        {
            if (_sessionStatesRequests.Count == 0)
                return;

            var request = _sessionStatesRequests.Dequeue();

            if (request == SessionState.Starting && _currentState == SessionState.Lobby)
            {
                _currentState = request;
                OnStarting();
            }
            else if (request == SessionState.Playing && _currentState == SessionState.Starting)
            {
                _currentState = request;
                OnStarted();
            }
            else if (request == SessionState.Ended && _currentState == SessionState.Playing)
            {
                _currentState = request;
                OnEnded();
                _currentState = SessionState.Lobby;
            }
            else if (request == SessionState.Closed && _currentState != SessionState.Closed)
            {
                _currentState = request;
            }


            //if (_currentState == SessionState.Starting && _previousState == SessionState.Lobby)
            //{
            //    _previousState = _currentState;
            //    OnStarting();
            //}
            //else if (_currentState == SessionState.Playing && _previousState == SessionState.Starting)
            //{
            //    _previousState = _currentState;
            //    OnStarted();
            //}
            //else if (_currentState == SessionState.Ended && _previousState == SessionState.Playing)
            //{
            //    _previousState = _currentState;
            //    OnEnded();
            //    _currentState = SessionState.Lobby;
            //}
            //else if (_currentState == SessionState.Closed && _previousState != SessionState.Closed)
            //{
            //    _previousState = _currentState;
            //}
        }

        /// <summary>
        /// Notifies the server that the synchronization with the server is performed locally
        /// </summary>
        protected override void NotifyServerSynchronizationDoneOnClient()
        {
            ChangeSessionStateRequest(SessionState.Playing);
        }

        /// <summary>
        /// Starts the Session and changes its SessionState from Lobby to Playing
        /// </summary>
        public override void StartSession()
        {
            foreach (IdentifiedPlayer identifiedPlayer in SessionManager.LocalPlayers.Values)
            {
                _localPlayers.Add(identifiedPlayer);
            }

            ChangeSessionStateRequest(SessionState.Starting);
        }

        /// <summary>
        /// Ends the Session and changes its SessionState from Playing to Ended
        /// </summary>
        public override void EndSession()
        {
            ChangeSessionStateRequest(SessionState.Ended);
        }

        public override void OnClosed()
        {
            _localPlayers.Clear();

            ChangeSessionStateRequest(SessionState.Closed);
        }

        public override void SynchronizeCommandOnClients(Command command)
        {
        }

        public override void SynchronizeSceneEntitiesOnClients(ISceneEntity sceneEntity)
        {
        }

        /// <summary>
        /// Listens to all data received from the network interface
        /// </summary>
        public override void ListenIncoming()
        {
            if (CommandsToSynchronize.Count != 0)
            {
                int count = CommandsToSynchronize.Count;
                for (int i = 0; i < count; i++)
                {
                    Command command = CommandsToSynchronize.Dequeue();

                    if (command.Id == 0 && command.Id != command.LocalId)
                        CommandsToSynchronize.Enqueue(command);
                    else
                        Commands.Add(command.Id, command);
                }
            }

            if (SceneEntitiesToSynchronize.Count != 0)
            {
                int count = SceneEntitiesToSynchronize.Count;
                for (int i = 0; i < count; i++)
                {
                    ISceneEntity sceneEntity = SceneEntitiesToSynchronize.Dequeue();

                    if (!RegisteredEntities.ContainsKey(sceneEntity.UniqueId))
                        RegisteredEntities.Add(sceneEntity.UniqueId, sceneEntity);
                }
            }

            if (_executeCommands.Count != 0)
            {
                foreach (var executeCommand in _executeCommands)
                {
                    _processingCommands.Add(executeCommand.Key, executeCommand.Value);
                }
                _executeCommands.Clear();

                foreach (var processingCommand in _processingCommands)
                {
                    Command command = processingCommand.Key;
                    ushort execution = processingCommand.Value;

                    if (execution == Constants.ExecuteCommandOnServerDataExchanged || execution == Constants.ExecuteCommandOnServerNoDataExchanged)
                    {
                        if (command.Condition == null || (command.Condition.Invoke()))
                            command.NetworkValue = command.ServerExecution(command, command.NetworkValue);

                        if (command.ApplyServerResult != null)
                            ExecuteServerCommandOnClients(command);
                        else
                            command.WaitingForServerReply = false;
                    }
                    else if (execution == Constants.ExecuteServerCommandOnClientsDataExchanged || execution == Constants.ExecuteServerCommandOnClientsNoDataExchanged)
                    {
                        command.WaitingForServerReply = false;
                        command.ApplyServerResult(command, command.NetworkValue);
                    }
                }

                _processingCommands.Clear();
            }
        }

        #endregion
        
        private Queue<SessionState> _sessionStatesRequests = new Queue<SessionState>();

        private void ChangeSessionStateRequest(SessionState newStatus)
        {
            _sessionStatesRequests.Enqueue(newStatus);
        }
    }
}