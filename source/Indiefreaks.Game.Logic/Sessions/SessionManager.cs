using System;
using System.Collections.Generic;
using System.Diagnostics;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using Indiefreaks.Xna.Logic;
using Indiefreaks.Xna.Threading;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// The SessionManager provides SunBurn with a Manager to identify Players, create, update and maintain a Session
    /// </summary>
    public abstract class SessionManager : ISessionManager
    {
        public static readonly Dictionary<PlayerIndex, IdentifiedPlayer> LocalPlayers =
            new Dictionary<PlayerIndex, IdentifiedPlayer>(1);

        private static readonly Dictionary<LogicalPlayerIndex, IdentifiedPlayer> LogicalPlayers = 
            new Dictionary<LogicalPlayerIndex, IdentifiedPlayer>(1); 

        private readonly IManagerServiceProvider _sceneInterface;

        protected static LogicalPlayerIndex GetNextFreeLogicalPlayerIndex()
        {
            LogicalPlayerIndex nextLogicalPlayerIndex;
            if (!LogicalPlayers.ContainsKey(LogicalPlayerIndex.One))
                nextLogicalPlayerIndex = LogicalPlayerIndex.One;
            else if (!LogicalPlayers.ContainsKey(LogicalPlayerIndex.Two))
                nextLogicalPlayerIndex = LogicalPlayerIndex.Two;
            else if (!LogicalPlayers.ContainsKey(LogicalPlayerIndex.Three))
                nextLogicalPlayerIndex = LogicalPlayerIndex.Three;
            else if (!LogicalPlayers.ContainsKey(LogicalPlayerIndex.Four))
                nextLogicalPlayerIndex = LogicalPlayerIndex.Four;
            else
                nextLogicalPlayerIndex = LogicalPlayerIndex.None;

            if (nextLogicalPlayerIndex == LogicalPlayerIndex.None)
                throw new CoreException("Too many players!");

            return nextLogicalPlayerIndex;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected SessionManager(IManagerServiceProvider sceneInterface)
        {
            _sceneInterface = sceneInterface;
            ManagerProcessOrder = 20;
        }

        /// <summary>
        /// Returns the current Session instance
        /// </summary>
        public static Session CurrentSession { get; protected set; }

        /// <summary>
        /// Returns true if the Session Manager is currently trying to identify players
        /// </summary>
        public bool IsIdentifyingPlayers { get; private set; }

        /// <summary>
        /// Returns the Identified player with the given logical player index
        /// </summary>
        /// <param name="logicalPlayerIndex"></param>
        /// <returns>Returns the corresponding IdentifiedPlayer if found; null otherwise</returns>
        public static IdentifiedPlayer GetIdentifiedPlayer(LogicalPlayerIndex logicalPlayerIndex)
        {
            if (LogicalPlayers.ContainsKey(logicalPlayerIndex))
                return LogicalPlayers[logicalPlayerIndex];

            throw new CoreException("No players identified as " + logicalPlayerIndex.ToString());
        }
        
        /// <summary>
        /// Starts listening to players A button pressed to identify them
        /// </summary>
        public virtual void BeginPlayerIdentification()
        {
            LogicalPlayers.Clear();
            IsIdentifyingPlayers = true;
        }

        /// <summary>
        /// Closes the identification process
        /// </summary>
        public virtual void EndPlayerIdentification()
        {
            IsIdentifyingPlayers = false;

            if (PlayerIdentificationEnded != null)
                PlayerIdentificationEnded(this, EventArgs.Empty);
        }

        /// <summary>
        /// Identifies a player
        /// </summary>
        /// <param name="playerInput">The PlayerInput instance used by the player to identify</param>
        public abstract void IdentifyPlayer(PlayerInput playerInput);

        /// <summary>
        /// Raised when a Player is identified
        /// </summary>
        public event EventHandler<IdentifiedPlayerEventArgs> PlayerLogin;

        /// <summary>
        /// Raised when a Player logs off
        /// </summary>
        public event EventHandler<IdentifiedPlayerEventArgs> PlayerLogoff;

        /// <summary>
        /// Raised when all potential players identified or when manually calling EndPlayerIdentification()
        /// </summary>
        public event EventHandler PlayerIdentificationEnded;

        /// <summary>
        /// Raised when a Player is identified
        /// </summary>
        public void OnPlayerLogin(IdentifiedPlayer identifiedPlayer)
        {
            identifiedPlayer.LogicalPlayerIndex = GetNextFreeLogicalPlayerIndex();
            LogicalPlayers.Add(identifiedPlayer.LogicalPlayerIndex, identifiedPlayer);

            if (PlayerLogin != null)
                PlayerLogin(this, new IdentifiedPlayerEventArgs(identifiedPlayer));
        }

        /// <summary>
        /// Raised when a Player logs off
        /// </summary>
        public void OnPlayerLogoff(IdentifiedPlayer identifiedPlayer)
        {
            LogicalPlayers.Remove(identifiedPlayer.LogicalPlayerIndex);

            if (PlayerLogoff != null)
                PlayerLogoff(this, new IdentifiedPlayerEventArgs(identifiedPlayer));

            identifiedPlayer.LogicalPlayerIndex = LogicalPlayerIndex.None;
        }

        /// <summary>
        /// Creates a Single Player Session
        /// </summary>
        /// <remarks>No network resources will be used</remarks>
        public abstract void CreateSinglePlayerSession();

        /// <summary>
        /// Creates a Split Screen Session
        /// </summary>
        /// <remarks>Not implemented yet</remarks>
        public abstract void CreateSplitScreenSession();

        /// <summary>
        /// Creates a local area network session
        /// </summary>
        /// <param name="maxPlayers">The total maximum players for this session</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to find this session on the network. Can be null</param>
        /// <remarks>it doesn't yet support multiple local players</remarks>
        public abstract void CreateLanSession(int maxPlayers, SessionProperties sessionProperties);

        /// <summary>
        /// Creates a wide area network session
        /// </summary>
        /// <param name="maxPlayers">The total maximum players for this session</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to find this session on the network. Can be null</param>
        /// <remarks>it doesn't yet support multiple local players</remarks>
        public abstract void CreateWanSession(int maxPlayers, SessionProperties sessionProperties);

        /// <summary>
        /// Raised when the Session is created
        /// </summary>
        public event EventHandler SessionCreated;

        /// <summary>
        /// Raised when the Session is created
        /// </summary>
        public void OnSessionCreated()
        {
            if (IsIdentifyingPlayers)
                EndPlayerIdentification();

            if (SessionCreated != null)
                SessionCreated(this, EventArgs.Empty);
        }

        public virtual void CloseSession()
        {
            CurrentSession.OnClosed();

            OnSessionClosed();

            CurrentSession = null;
        }

        public event EventHandler SessionClosed;

        public void OnSessionClosed()
        {
            if (SessionClosed != null)
                SessionClosed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sends a Find query on the network interface to look for AvailableSession instances asynchrnously
        /// </summary>
        /// <param name="sessionType">The SessionType we're looking for</param>
        /// <param name="maxLocalPlayers">The Maximum local players that can be added to the session used to filter sessions that have a limited number of opened public slots</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to filter query results. Can be null</param>
        public abstract void FindSessions(SessionType sessionType, int maxLocalPlayers,
                                          SessionProperties sessionProperties);

        /// <summary>
        /// Raised when the FindSessions query ends
        /// </summary>
        /// <remarks>The SessionsFound can return no results. Be sure to look in the SessionsFoundEventArgs if any AvailableSession was returned</remarks>
        public event EventHandler<SessionsFoundEventArgs> SessionsFound;

        /// <summary>
        /// Raised when the FindSessions query ends
        /// </summary>
        public void OnSessionsFound(IList<AvailableSession> sessionsFound)
        {
            if (SessionsFound != null)
                SessionsFound(this, new SessionsFoundEventArgs(sessionsFound));
        }

        /// <summary>
        /// Sends a Join query to the Session asynchronously
        /// </summary>
        /// <param name="availableSession">The Session we are trying to join</param>
        public abstract void JoinSession(AvailableSession availableSession);

        /// <summary>
        /// Raised when the Session is joined
        /// </summary>
        public event EventHandler SessionJoined;

        /// <summary>
        /// Raised when the Session is joined
        /// </summary>
        public void OnSessionJoined()
        {
            if (IsIdentifyingPlayers)
                EndPlayerIdentification();

            if (SessionJoined != null)
                SessionJoined(this, EventArgs.Empty);
        }

        #region Implementation of IUnloadable

        /// <summary>
        /// Disposes any graphics resource used internally by this object, and removes
        ///             scene resources managed by this object. Commonly used during Game.UnloadContent.
        /// </summary>
        public virtual void Unload()
        {
        }

        #endregion

        #region Implementation of IManager

        #region IManagerService Members

        /// <summary>
        /// Use to apply user quality and performance preferences to the resources managed by this object.
        /// </summary>
        /// <param name="preferences"/>
        public virtual void ApplyPreferences(ISystemPreferences preferences)
        {
        }

        /// <summary>
        /// Removes resources managed by this object. Commonly used while clearing the scene.
        /// </summary>
        public virtual void Clear()
        {
        }

        public IManagerServiceProvider OwnerSceneInterface
        {
            get { return _sceneInterface; }
        }

        #endregion

        #region IUpdatableManager Members

        /// <summary>
        /// Updates the object and its contained resources.
        /// </summary>
        /// <param name="gameTime"/>
        public virtual void Update(GameTime gameTime)
        {
            // If we are identifying players we loop through all input devices to get a player response on the standard A button
            if (IsIdentifyingPlayers)
            {
#if !WINDOWS_PHONE
                if (Application.Input.PlayerOne.Buttons.A.IsPressed || Application.Input.PlayerOne.Buttons.Start.IsPressed)
                {
                    IdentifyPlayer(Application.Input.PlayerOne);
                }
                if (Application.Input.PlayerTwo.Buttons.A.IsPressed || Application.Input.PlayerTwo.Buttons.Start.IsPressed)
                {
                    IdentifyPlayer(Application.Input.PlayerTwo);
                }
                if (Application.Input.PlayerThree.Buttons.A.IsPressed || Application.Input.PlayerThree.Buttons.Start.IsPressed)
                {
                    IdentifyPlayer(Application.Input.PlayerThree);
                }
                if (Application.Input.PlayerFour.Buttons.A.IsPressed || Application.Input.PlayerFour.Buttons.Start.IsPressed)
                {
                    IdentifyPlayer(Application.Input.PlayerFour);
                }
#else
                if (Application.Input.CurrentTouches.Count > 0)
                {
                    IdentifyPlayer(Application.Input.PlayerOne);
                }
#endif
            }

            // We update the Session
            if(Application.SunBurn.Editor != null && Application.SunBurn.Editor.EditorAttached)
                return;

            if (CurrentSession != null)
                ((IUpdate)CurrentSession).Update(gameTime);
        }

        #endregion

        #endregion

        #region Implementation of IManagerService

        /// <summary>
        /// Gets the manager specific Type used as a unique key for storing and
        ///             requesting the manager from the IManagerServiceProvider.
        /// </summary>
        public Type ManagerType
        {
            get { return typeof (ISessionManager); }
        }

        /// <summary>
        /// Sets the order this manager is processed relative to other managers
        ///             in the IManagerServiceProvider. Managers with lower processing order
        ///             values are processed first.
        ///             In the case of BeginFrameRendering and EndFrameRendering, BeginFrameRendering
        ///             is processed in the normal order (lowest order value to highest), however
        ///             EndFrameRendering is processed in reverse order (highest to lowest) to ensure
        ///             the first manager begun is the last one ended (FILO).
        /// </summary>
        public int ManagerProcessOrder { get; set; }

        #endregion
    }
}