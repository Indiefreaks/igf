using System;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Sessions.Local
{
    public class LocalSessionManager : SessionManager
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public LocalSessionManager(IManagerServiceProvider sceneInterface) : base(sceneInterface)
        {
        }

        #region Overrides of SessionManager

        /// <summary>
        /// Identifies a player
        /// </summary>
        /// <param name="playerInput">The PlayerInput instance used by the player to identify</param>
        public override void IdentifyPlayer(PlayerInput playerInput)
        {
            var identifiedPlayer = new LocalIdentifiedPlayer(playerInput);
            LocalPlayers.Add(playerInput.PlayerIndex, identifiedPlayer);

            OnPlayerLogin(identifiedPlayer);
        }

        /// <summary>
        /// Creates a Single Player Session
        /// </summary>
        /// <remarks>No network resources will be used</remarks>
        public override void CreateSinglePlayerSession()
        {
            if (LocalPlayers.Count == 0)
                throw new CoreException("No players identified");

            if (CurrentSession == null)
                CurrentSession = new LocalSession();

            OnSessionCreated();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a Join query to the Session asynchronously
        /// </summary>
        /// <param name="availableSession">The Session we are trying to join</param>
        public override void JoinSession(AvailableSession availableSession)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}