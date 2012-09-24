using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Logic;
using Indiefreaks.Xna.Threading;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// The Session class is the abstract access to a game session created with a SessionManager instance
    /// </summary>
    public abstract class Session : IUpdate
    {
        protected readonly Dictionary<ushort, Command> Commands = new Dictionary<ushort, Command>();
        protected readonly Queue<Command> CommandsToSynchronize = new Queue<Command>();
        protected readonly Dictionary<int, ISceneEntity> RegisteredEntities = new Dictionary<int, ISceneEntity>();
        protected readonly Queue<ISceneEntity> SceneEntitiesToSynchronize = new Queue<ISceneEntity>();
        protected readonly List<IdentifiedPlayer> PlayersToSynchronize = new List<IdentifiedPlayer>();

        private readonly List<NonPlayerAgent> _nonPlayerAgents = new List<NonPlayerAgent>();
        private Queue<NonPlayerAgent> _nonPlayerAgentsProcessQueue;
        private readonly List<PlayerAgent> _playerAgents = new List<PlayerAgent>();
        private Queue<PlayerAgent> _playerAgentsProcessQueue;
        private bool _addPlayersToSynchronize = false;
        
        /// <summary>
        /// Returns the list of players in the current session
        /// </summary>
        public abstract IList<IdentifiedPlayer> AllPlayers { get; }

        /// <summary>
        /// Return the list of local players in the current session
        /// </summary>
        public abstract IList<IdentifiedPlayer> LocalPlayers { get; }

        /// <summary>
        /// Returns the list of remote players in the current session
        /// </summary>
        public abstract IList<IdentifiedPlayer> RemotePlayers { get; }

        public IdentifiedPlayer PlayerOne
        {
            get { return SessionManager.GetIdentifiedPlayer(LogicalPlayerIndex.One); }
        }

        public IdentifiedPlayer PlayerTwo
        {
            get { return SessionManager.GetIdentifiedPlayer(LogicalPlayerIndex.Two); }
        }

        public IdentifiedPlayer PlayerThree
        {
            get { return SessionManager.GetIdentifiedPlayer(LogicalPlayerIndex.Three); }
        }

        public IdentifiedPlayer PlayerFour
        {
            get { return SessionManager.GetIdentifiedPlayer(LogicalPlayerIndex.Four); }
        }

        /// <summary>
        /// Returns the current SessionState
        /// </summary>
        public abstract SessionState Status { get; }

        /// <summary>
        /// Returns the current Session SessionType
        /// </summary>
        public abstract SessionType SessionType { get; }

        /// <summary>
        /// Returns if the current Session authorizes Host migration
        /// </summary>
        public abstract bool AllowHostMigration { get; }

        /// <summary>
        /// Returns if the current Session authorizes players to join the Session while playing
        /// </summary>
        public abstract bool AllowJoinInProgress { get; }

        /// <summary>
        /// Returns the current Session Host player
        /// </summary>
        public abstract IdentifiedPlayer Host { get; }

        /// <summary>
        /// Returns the current Session Bytes/Second received
        /// </summary>
        public abstract int BytesPerSecondReceived { get; }

        /// <summary>
        /// Returns the current Session Bytes/Second sent
        /// </summary>
        public abstract int BytesPerSecondSent { get; }

        /// <summary>
        /// Returns if the current Session is Host
        /// </summary>
        public abstract bool IsHost { get; }
        
        #region IUpdate Members

        /// <summary>
        /// Update loop call
        /// </summary>
        /// <param name="gameTime"/>
        void IUpdate.Update(GameTime gameTime)
        {
            var stat1 = SystemConsole.GetStatistic("Session_PlayerAgents_Count", SystemStatisticCategory.SceneGraph);
            stat1.AccumulationValue = _playerAgents.Count;
            var stat2 = SystemConsole.GetStatistic("Session_NonPlayerAgents_Count", SystemStatisticCategory.SceneGraph);
            stat2.AccumulationValue = _nonPlayerAgents.Count;
            var stat3 = SystemConsole.GetStatistic("Session_BytesPerSecondReceived", SystemStatisticCategory.SceneGraph);
            stat3.AccumulationValue = BytesPerSecondReceived;
            var stat4 = SystemConsole.GetStatistic("Session_BytesPerSecondSent", SystemStatisticCategory.SceneGraph);
            stat4.AccumulationValue = BytesPerSecondSent;

            if (IsHost)
            {
                if (CommandsToSynchronize.Count != 0)
                {
                    for (int i = 0; i < CommandsToSynchronize.Count; i++)
                    {
                        Command command = CommandsToSynchronize.Dequeue();
                        command.Id = command.LocalId;
                        SynchronizeCommandOnClients(command);
                        CommandsToSynchronize.Enqueue(command);
                    }

                    if(Status == SessionState.Starting || Status == SessionState.Playing)
                        _addPlayersToSynchronize = true;
                }
                if (SceneEntitiesToSynchronize.Count != 0)
                {
                    for (int i = 0; i < SceneEntitiesToSynchronize.Count; i++)
                    {
                        ISceneEntity entity = SceneEntitiesToSynchronize.Dequeue();
                        SynchronizeSceneEntitiesOnClients(entity);
                        SceneEntitiesToSynchronize.Enqueue(entity);
                    }

                    if (Status == SessionState.Starting || Status == SessionState.Playing)
                        _addPlayersToSynchronize = true;
                }

                if (_addPlayersToSynchronize)
                {
                    PlayersToSynchronize.AddRange(AllPlayers);
                    _addPlayersToSynchronize = false;
                }
            }

            // we loop through all local PlayerAgents to retrieve input commands
            if (_playerAgents.Count > 0)
            {
                _playerAgentsProcessQueue = new Queue<PlayerAgent>(_playerAgents);
                while (_playerAgentsProcessQueue.Count != 0)
                {
                    var playerAgent = _playerAgentsProcessQueue.Dequeue();
                    if(playerAgent.ParentObject != null)
                        playerAgent.Process(gameTime);
                }
            }

            if (_nonPlayerAgents.Count > 0)
            {
                _nonPlayerAgentsProcessQueue = new Queue<NonPlayerAgent>(_nonPlayerAgents);
                while (_nonPlayerAgentsProcessQueue.Count != 0)
                {
                    var nonPlayerAgent = _nonPlayerAgentsProcessQueue.Dequeue();
                    if(nonPlayerAgent.ParentObject != null)
                        nonPlayerAgent.Process(gameTime);
                }
            }

            // we update the session (sending & retrieving network data)
            Update(gameTime);

            // we retrieve all data incoming from the network
            ListenIncoming();

            // if commands and scene entities are created on the client, we tell the server synchronization is done so that the session can effectively start
            if (Status == SessionState.Starting && CommandsToSynchronize.Count == 0 && SceneEntitiesToSynchronize.Count == 0)
                NotifyServerSynchronizationDoneOnClient();
        }

        /// <summary>
        /// Notifies the server that the synchronization with the server is performed locally
        /// </summary>
        protected abstract void NotifyServerSynchronizationDoneOnClient();

        #endregion

        /// <summary>
        /// Raised when a player left the Session
        /// </summary>
        public event EventHandler<IdentifiedPlayerEventArgs> PlayerLeft;

        /// <summary>
        /// Raised when a player joined the Session
        /// </summary>
        public event EventHandler<IdentifiedPlayerEventArgs> PlayerJoined;

        /// <summary>
        /// Raised when a player left the Session
        /// </summary>
        /// <param name="identifiedPlayer"></param>
        protected virtual void OnPlayerLeft(IdentifiedPlayer identifiedPlayer)
        {
            if (PlayerLeft != null)
                PlayerLeft(this, new IdentifiedPlayerEventArgs(identifiedPlayer));
        }

        /// <summary>
        /// Raised when a player joined the Session
        /// </summary>
        /// <param name="identifiedPlayer"></param>
        protected virtual void OnPlayerJoined(IdentifiedPlayer identifiedPlayer)
        {
            if (PlayerJoined != null)
                PlayerJoined(this, new IdentifiedPlayerEventArgs(identifiedPlayer));
        }

        /// <summary>
        /// Asks the Session to send a remote command call on the session host
        /// </summary>
        /// <param name="command">The command that should be executed</param>
        public abstract void ExecuteCommandOnServer(Command command);

        /// <summary>
        /// Asks the Session to send a remote command call on all session clients
        /// </summary>
        /// <param name="command">The command that should be executed</param>
        public abstract void ExecuteServerCommandOnClients(Command command);

        /// <summary>
        /// Update loop call
        /// </summary>
        /// <param name="gameTime"/>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Starts the Session and changes its SessionState from Lobby to Starting
        /// </summary>
        public abstract void StartSession();

        /// <summary>
        /// Raised when the session is starting
        /// </summary>
        public event EventHandler Starting;

        /// <summary>
        /// Raised when the session is starting
        /// </summary>
        protected virtual void OnStarting()
        {
            if (Starting != null)
                Starting(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raised when the session started
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Raised when the session started
        /// </summary>
        protected virtual void OnStarted()
        {
            if (Started != null)
                Started(this, EventArgs.Empty);
        }

        /// <summary>
        /// Ends the Session and changes its SessionState from Playing to Ended
        /// </summary>
        public abstract void EndSession();

        /// <summary>
        /// Raised when the Session is ended
        /// </summary>
        public event EventHandler Ended;

        /// <summary>
        /// Raised when the Session is ended
        /// </summary>
        protected virtual void OnEnded()
        {
            if (Ended != null)
                Ended(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closes the Session and changes its SessionState to Closed
        /// </summary>
        public abstract void OnClosed();

        /// <summary>
        /// Creates a new PlayerAgent
        /// </summary>
        /// <param name="identifiedPlayer">The identified player instance</param>
        public PlayerAgent CreatePlayerAgent(IdentifiedPlayer identifiedPlayer)
        {
            return CreatePlayerAgent<PlayerAgent>(identifiedPlayer);
        }

        /// <summary>
        /// Creates a new PlayerAgent
        /// </summary>
        /// <param name="identifiedPlayer">The identified player instance</param>
        public T CreatePlayerAgent<T>(IdentifiedPlayer identifiedPlayer) where T : PlayerAgent, new()
        {
            if (Status != SessionState.Starting && Status != SessionState.Playing)
                throw new CoreException("Session must be started to create a PlayerAgent");

            var playerAgent = new T {IdentifiedPlayer = identifiedPlayer};
            _playerAgents.Add(playerAgent);

            return playerAgent;
        }

        /// <summary>
        /// Creates a new NonPlayerAgent
        /// </summary>
        /// <returns>Returns a new NonPlayerAgent instance</returns>
        public NonPlayerAgent CreateNonPlayerAgent()
        {
            return CreateNonPlayerAgent<NonPlayerAgent>();
        }

        public T CreateNonPlayerAgent<T>() where T : NonPlayerAgent, new()
        {
            if (Status != SessionState.Starting && Status != SessionState.Playing)
                throw new CoreException("Session must be started to create a PlayerAgent");

            var nonPlayerAgent = new T();
            _nonPlayerAgents.Add(nonPlayerAgent);

            return nonPlayerAgent;
        }

        /// <summary>
        /// Registers a command on the Session host and remotely creates the command on every client
        /// </summary>
        /// <param name="command">The command to be registered</param>
        internal void RegisterCommand(Command command)
        {
            CommandsToSynchronize.Enqueue(command);
        }

        internal void UnRegisterCommand(Command command)
        {
            if(Commands.ContainsKey(command.Id))
               Commands.Remove(command.Id);
        }

        /// <summary>
        /// Registers a SceneEntity on the session host and remotely synchronize its unique Id on every client
        /// </summary>
        /// <param name="sceneEntity"></param>
        internal void RegisterSceneEntity(ISceneEntity sceneEntity)
        {
            if(!RegisteredEntities.ContainsKey(sceneEntity.UniqueId))
                SceneEntitiesToSynchronize.Enqueue(sceneEntity);
        }
        
        public T GetRegisteredEntity<T>(int uniqueId) where T : class
        {
            if (RegisteredEntities.ContainsKey(uniqueId))
                return RegisteredEntities[uniqueId] as T;

            return null;
        }

        internal void UnRegisterSceneEntity(ISceneEntity sceneEntity)
        {
            if (RegisteredEntities.ContainsKey(sceneEntity.UniqueId))
                RegisteredEntities.Remove(sceneEntity.UniqueId);
        }

        public abstract void SynchronizeCommandOnClients(Command command);

        public abstract void SynchronizeSceneEntitiesOnClients(ISceneEntity sceneEntity);

        /// <summary>
        /// Listens to all data received from the network interface
        /// </summary>
        public abstract void ListenIncoming();

        internal void RemoveNonPlayerAgent(NonPlayerAgent nonPlayerAgent)
        {
            _nonPlayerAgents.Remove(nonPlayerAgent);
            foreach (var behavior in nonPlayerAgent.Behaviors)
                foreach (var command in behavior.Commands)
                    UnRegisterCommand(command);
        }

        internal void RemovePlayerAgent(PlayerAgent playerAgent)
        {
            _playerAgents.Remove(playerAgent);
            foreach (var behavior in playerAgent.Behaviors)
                foreach (var command in behavior.Commands)
                    UnRegisterCommand(command);
        }
    }
}