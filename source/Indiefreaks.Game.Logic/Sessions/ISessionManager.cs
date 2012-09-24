using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Input;
using Indiefreaks.Xna.Logic;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Sessions
{
    public interface ISessionManager : IManagerService, IUpdatableManager
    {
        /// <summary>
        /// Returns true if the Session Manager is currently trying to identify players
        /// </summary>
        bool IsIdentifyingPlayers { get; }
        
        /// <summary>
        /// Starts listening to players A button pressed to identify them
        /// </summary>
        void BeginPlayerIdentification();

        /// <summary>
        /// Closes the identification process
        /// </summary>
        void EndPlayerIdentification();

        /// <summary>
        /// Identifies a player
        /// </summary>
        /// <param name="playerInput">The PlayerInput instance used by the player to identify</param>
        void IdentifyPlayer(PlayerInput playerInput);

        /// <summary>
        /// Raised when a Player is identified
        /// </summary>
        event EventHandler<IdentifiedPlayerEventArgs> PlayerLogin;

        /// <summary>
        /// Raised when a Player logs off
        /// </summary>
        event EventHandler<IdentifiedPlayerEventArgs> PlayerLogoff;

        /// <summary>
        /// Raised when all potential players identified or when manually calling EndPlayerIdentification()
        /// </summary>
        event EventHandler PlayerIdentificationEnded;

        /// <summary>
        /// Raised when a Player is identified
        /// </summary>
        void OnPlayerLogin(IdentifiedPlayer identifiedPlayer);

        /// <summary>
        /// Raised when a Player logs off
        /// </summary>
        void OnPlayerLogoff(IdentifiedPlayer identifiedPlayer);

        /// <summary>
        /// Creates a Single Player Session
        /// </summary>
        /// <remarks>No network resources will be used</remarks>
        void CreateSinglePlayerSession();

        /// <summary>
        /// Creates a Split Screen Session
        /// </summary>
        /// <remarks>Not implemented yet</remarks>
        void CreateSplitScreenSession();

        /// <summary>
        /// Creates a local area network session
        /// </summary>
        /// <param name="maxPlayers">The total maximum players for this session</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to find this session on the network. Can be null</param>
        /// <remarks>it doesn't yet support multiple local players</remarks>
        void CreateLanSession(int maxPlayers, SessionProperties sessionProperties);

        /// <summary>
        /// Creates a wide area network session
        /// </summary>
        /// <param name="maxPlayers">The total maximum players for this session</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to find this session on the network. Can be null</param>
        /// <remarks>it doesn't yet support multiple local players</remarks>
        void CreateWanSession(int maxPlayers, SessionProperties sessionProperties);

        /// <summary>
        /// Raised when the Session is created
        /// </summary>
        event EventHandler SessionCreated;

        /// <summary>
        /// Raised when the Session is created
        /// </summary>
        void OnSessionCreated();

        /// <summary>
        /// Sends a Find query on the network interface to look for AvailableSession instances asynchrnously
        /// </summary>
        /// <param name="sessionType">The SessionType we're looking for</param>
        /// <param name="maxLocalPlayers">The Maximum local players that can be added to the session used to filter sessions that have a limited number of opened public slots</param>
        /// <param name="sessionProperties">The SessionProperties that will be used to filter query results. Can be null</param>
        void FindSessions(SessionType sessionType, int maxLocalPlayers,
                                          SessionProperties sessionProperties);

        /// <summary>
        /// Raised when the FindSessions query ends
        /// </summary>
        /// <remarks>The SessionsFound can return no results. Be sure to look in the SessionsFoundEventArgs if any AvailableSession was returned</remarks>
        event EventHandler<SessionsFoundEventArgs> SessionsFound;

        /// <summary>
        /// Raised when the FindSessions query ends
        /// </summary>
        void OnSessionsFound(IList<AvailableSession> sessionsFound);

        /// <summary>
        /// Sends a Join query to the Session asynchronously
        /// </summary>
        /// <param name="availableSession">The Session we are trying to join</param>
        void JoinSession(AvailableSession availableSession);

        /// <summary>
        /// Raised when the Session is joined
        /// </summary>
        event EventHandler SessionJoined;

        /// <summary>
        /// Raised when the Session is joined
        /// </summary>
        void OnSessionJoined();

        void CloseSession();
        event EventHandler SessionClosed;
        void OnSessionClosed();
    }
}