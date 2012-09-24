using System;
using System.Collections.Generic;
using System.Globalization;
using ICEBridgeLib;
using ICECoreLib;
using ICELandaLib;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.IndieCity
{
    public class IndieCityManager : IManagerService, IUpdatableManager, IEventHandler, IAchievementEventHandler, ILeaderboardEventHandler, IDisposable
    {
        private readonly uint _achievementsCookie;
        private readonly Dictionary<long, bool> _achievementsUnlockedAtIndieCity;
        private readonly uint _leaderboardsCookie;
        private readonly CoLeaderboardManager _leaderboardsManager;
        private readonly IManagerServiceProvider _sceneInterface;
        private readonly uint _sessionCookie;
        private readonly SessionEndDelegate _sessionEndDelegate;
        private readonly string _username;
        internal CoAchievementManager AchievementsManager;
        internal CoGameSession Session;
        private bool _achievementsReady;
        private AchievementsRefreshInfo _achievementsRefreshInfo;
        private CoLeaderboardPage _currentlyLoadingLeaderboardPage;
        private CoLeaderboardUserRows _currentlyLoadingUserScores;
        private LeaderboardPageLoadDelegate _leaderboardPageLoadDelegate;
        private bool _leaderboardsReady;
        private bool _sessionActive;
        private bool _sessionEndBecauseNoLicense;
        private NotificationDelegate _startDelegate;
        private SessionStartPhase _startPhase;
        private UserScoresLoadDelegate _userScoresLoadDelegate;

        public IndieCityManager(string gameId, string serviceId, string serviceSecret, bool hasAchievements, bool hasLeaderboards, IManagerServiceProvider serviceProvider, SessionEndDelegate sessionEndDelegate)
        {
            _sceneInterface = serviceProvider;
            _sessionEndDelegate = sessionEndDelegate;

            ManagerProcessOrder = 10;

            var service = new ServiceId
                              {
                                  Data1 = uint.Parse(serviceId.Substring(0, 8), NumberStyles.HexNumber),
                                  Data2 = ushort.Parse(serviceId.Substring(9, 4), NumberStyles.HexNumber),
                                  Data3 = ushort.Parse(serviceId.Substring(14, 4), NumberStyles.HexNumber),
                                  Data4 = new[]
                                              {
                                                  byte.Parse(serviceId.Substring(19, 2), NumberStyles.HexNumber),
                                                  byte.Parse(serviceId.Substring(21, 2), NumberStyles.HexNumber),
                                                  byte.Parse(serviceId.Substring(24, 2), NumberStyles.HexNumber),
                                                  byte.Parse(serviceId.Substring(26, 2), NumberStyles.HexNumber),
                                                  byte.Parse(serviceId.Substring(28, 2), NumberStyles.HexNumber),
                                                  byte.Parse(serviceId.Substring(30, 2), NumberStyles.HexNumber),
                                                  byte.Parse(serviceId.Substring(32, 2), NumberStyles.HexNumber),
                                                  byte.Parse(serviceId.Substring(34, 2), NumberStyles.HexNumber)
                                              }
                              };

            var bridge = new CoBridge2();
            bridge.Initialise(gameId);
            bridge.SetServiceCredentials(GameService.GameService_IndieCityLeaderboardsAndAchievements, service, serviceSecret);

            Session = bridge.CreateDefaultGameSession();
            _sessionCookie = Session.RegisterEventHandler(0, 0, this);
            _sessionActive = false;
            _startPhase = SessionStartPhase.NotStarted;

            _username = bridge.UserStore.GetUserFromId(Session.UserId).Name;

            _achievementsReady = !hasAchievements;
            if (hasAchievements)
            {
                AchievementsManager = new CoAchievementManager();
                AchievementsManager.SetGameSession(Session);
                AchievementsManager.InitialiseAchievements(null);
                _achievementsCookie = ((IAchievementService) AchievementsManager).RegisterAchievementEventHandler(this);
                _achievementsUnlockedAtIndieCity = new Dictionary<long, bool>();
            }

            _leaderboardsReady = !hasLeaderboards;
            if (hasLeaderboards)
            {
                _leaderboardsManager = new CoLeaderboardManager();
                _leaderboardsManager.SetGameSession(Session);
                _leaderboardsManager.InitialiseLeaderboards(null);
                _leaderboardsCookie = ((ILeaderboardService) _leaderboardsManager).RegisterLeaderboardEventHandler(this);
            }
        }

        public string Username
        {
            get { return _username; }
        }

        public bool SessionActive
        {
            get { return _sessionActive; }
        }

        public int UserId
        {
            get { return Session.UserId; }
        }

        #region Implementation of IManager

        #region IManagerService Members

        /// <summary>
        /// Use to apply user quality and performance preferences to the resources managed by this object.
        /// </summary>
        /// <param name="preferences"/>
        public void ApplyPreferences(ISystemPreferences preferences)
        {
        }

        /// <summary>
        /// Removes resources managed by this object. Commonly used while clearing the scene.
        /// </summary>
        public void Clear()
        {
        }

        /// <summary>
        /// Scene interface the manager was created by, or assigned during construction.
        /// </summary>
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
        public void Update(GameTime gameTime)
        {
            Session.UpdateSession();

            if (SessionActive)
            {
                if (_currentlyLoadingLeaderboardPage != null && _currentlyLoadingLeaderboardPage.PopulationState != LeaderboardPopulationState.LPS_PENDING)
                {
                    bool success = _currentlyLoadingLeaderboardPage.PopulationState == LeaderboardPopulationState.LPS_POPULATED;
                    _currentlyLoadingLeaderboardPage = null;
                    if (_leaderboardPageLoadDelegate != null)
                        _leaderboardPageLoadDelegate.Invoke(success);
                }

                if (_currentlyLoadingUserScores != null && _currentlyLoadingUserScores.PopulationState != LeaderboardPopulationState.LPS_PENDING)
                {
                    CoLeaderboardUserRows scores = _currentlyLoadingUserScores;
                    _currentlyLoadingUserScores = null;
                    if (_userScoresLoadDelegate != null)
                        _userScoresLoadDelegate.Invoke(scores.PopulationState == LeaderboardPopulationState.LPS_POPULATED ? scores : null);
                }
            }
        }

        #endregion

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Exécute les tâches définies par l'application associées à la libération ou à la redéfinition des ressources non managées.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (AchievementsManager != null)
                ((IAchievementService) AchievementsManager).UnregisterAchievementEventHandler(_achievementsCookie);
            if (_leaderboardsManager != null)
                ((ILeaderboardService) _leaderboardsManager).UnregisterLeaderboardEventHandler(_leaderboardsCookie);
            Session.UnregisterEventHandler(_sessionCookie); // unregister event handler first, so that EndSession below does not cause an event
            if (Session.IsSessionStarted())
                Session.EndSession();
        }

        #endregion

        #region Implementation of IEventHandler

        public void HandleEvent(uint eventId, uint eventType, Array args)
        {
            if ((GameSessionEventCategory) eventType == GameSessionEventCategory.GSC_GameSession)
            {
                if ((GameSessionEvent) eventId == GameSessionEvent.GSE_SessionStarted)
                {
                    var accessControl = new CoAccessControl();
                    accessControl.Initialise(Session);
                    if (accessControl.LicenseState == LicenseState.LS_NOLICENSE)
                    {
                        _sessionEndBecauseNoLicense = true;
                        Session.EndSession();
                    }
                    else if (!_achievementsReady || !_leaderboardsReady)
                    {
                        _startPhase = SessionStartPhase.WaitingForIndiecityData;
                        if (!_achievementsReady)
                            _achievementsRefreshInfo = new AchievementsRefreshInfo(this, null, false);
                    }
                    else
                        OnSessionActiveStateReached();
                }
                else if ((GameSessionEvent) eventId == GameSessionEvent.GSE_SessionEnded)
                {
                    _sessionActive = false;
                    if (_sessionEndDelegate != null)
                    {
                        SessionEndReason reason;
                        if (_sessionEndBecauseNoLicense)
                            reason = SessionEndReason.NoLicense;
                        else
                        {
                            switch ((GameSessionReasonCode) (uint) ((Object[]) args)[0])
                            {
                                default:
                                case GameSessionReasonCode.GSR_Unknown:
                                    reason = SessionEndReason.Unknown;
                                    break;
                                case GameSessionReasonCode.GSR_UserRequest:
                                    reason = SessionEndReason.UserRequest;
                                    break;
                                case GameSessionReasonCode.GSR_NoConnection:
                                    reason = SessionEndReason.NoConnection;
                                    break;
                                case GameSessionReasonCode.GSR_BadCredentials:
                                    reason = SessionEndReason.BadCredentials;
                                    break;
                                case GameSessionReasonCode.GSR_TimeOut:
                                    reason = SessionEndReason.Timeout;
                                    break;
                            }
                        }
                        _sessionEndDelegate.Invoke(reason);
                    }
                    _sessionEndBecauseNoLicense = false;
                }
            }
        }

        #endregion

        private void OnSessionActiveStateReached()
        {
            _sessionActive = true;
            _startPhase = SessionStartPhase.Active;
            if (_startDelegate != null)
                _startDelegate.Invoke();
        }

        public void RequestSessionStart(NotificationDelegate startedDelegate)
        {
            _startPhase = SessionStartPhase.RequestSession;
            _startDelegate = startedDelegate;
            Session.RequestStartSession();
        }

        public void EndSession()
        {
            Session.EndSession();
        }

        public int GetAchievementCount()
        {
            if (SessionActive)
                return (int) AchievementsManager.AchievementGroup.AchievementCount;

            return -1;
        }

        public CoAchievement GetAchievementById(long achievementId)
        {
            if (SessionActive)
                return AchievementsManager.AchievementGroup.GetAchievementFromId(achievementId);

            return null;
        }

        public CoAchievement GetAchievementByIndex(int index)
        {
            if (SessionActive)
                return AchievementsManager.AchievementGroup.GetAchievementFromIndex((uint) index);

            return null;
        }

        public bool IsAchievementUnlocked(long achievementId)
        {
            return _achievementsUnlockedAtIndieCity.ContainsKey(achievementId);
        }

        public void UnlockAchievement(long achievementId)
        {
            AchievementsManager.UnlockAchievement(achievementId);
            _achievementsUnlockedAtIndieCity[achievementId] = true;
        }

        public void RequestAchievementDataRefresh(NotificationDelegate refreshCompleteDelegate)
        {
            if (SessionActive)
            {
                if (_achievementsRefreshInfo != null)
                    throw new Exception("Previous achievement refresh request still being processed");

                _achievementsRefreshInfo = new AchievementsRefreshInfo(this, refreshCompleteDelegate, true);
            }
        }

        public void PostLeaderboardScore(int leaderboardId, long score)
        {
            _leaderboardsManager.GetLeaderboardFromId(leaderboardId).PostScore(score);
        }

        public CoLeaderboardPage RequestOpenLeaderboard(int leaderboardId, int pageSize, LeaderboardPageLoadDelegate pageCompleteDelegate)
        {
            if (SessionActive)
            {
                if (_currentlyLoadingLeaderboardPage != null)
                    throw new Exception("Previous leaderboard page request still being processed");

                _leaderboardPageLoadDelegate = pageCompleteDelegate;
                _currentlyLoadingLeaderboardPage = _leaderboardsManager.GetLeaderboardFromId(leaderboardId).GetGlobalPage((uint) pageSize);
                return _currentlyLoadingLeaderboardPage;
            }

            return null;
        }

        public void RequestFirstPage(CoLeaderboardPage page, LeaderboardPageLoadDelegate pageCompleteDelegate)
        {
            if (SessionActive)
            {
                if (_currentlyLoadingLeaderboardPage != null)
                    throw new Exception("Previous leaderboard page request still being processed");

                _leaderboardPageLoadDelegate = pageCompleteDelegate;
                _currentlyLoadingLeaderboardPage = page;
                page.RequestFirst();
            }
        }

        public void RequestLastPage(CoLeaderboardPage page, LeaderboardPageLoadDelegate pageCompleteDelegate)
        {
            if (SessionActive)
            {
                if (_currentlyLoadingLeaderboardPage != null)
                    throw new Exception("Previous leaderboard page request still being processed");

                _leaderboardPageLoadDelegate = pageCompleteDelegate;
                _currentlyLoadingLeaderboardPage = page;
                page.RequestLast();
            }
        }

        public void RequestPreviousPage(CoLeaderboardPage page, LeaderboardPageLoadDelegate pageCompleteDelegate)
        {
            if (SessionActive)
            {
                if (_currentlyLoadingLeaderboardPage != null)
                    throw new Exception("Previous leaderboard page request still being processed");

                _leaderboardPageLoadDelegate = pageCompleteDelegate;
                _currentlyLoadingLeaderboardPage = page;
                page.RequestPrev();
            }
        }

        public void RequestNextPage(CoLeaderboardPage page, LeaderboardPageLoadDelegate pageCompleteDelegate)
        {
            if (SessionActive)
            {
                if (_currentlyLoadingLeaderboardPage != null)
                    throw new Exception("Previous leaderboard page request still being processed");

                _leaderboardPageLoadDelegate = pageCompleteDelegate;
                _currentlyLoadingLeaderboardPage = page;
                page.RequestNext();
            }
        }

        public void RequestUserPage(CoLeaderboardPage page, LeaderboardPageLoadDelegate pageCompleteDelegate)
        {
            if (SessionActive)
            {
                if (_currentlyLoadingLeaderboardPage != null)
                    throw new Exception("Previous leaderboard page request still being processed");

                _leaderboardPageLoadDelegate = pageCompleteDelegate;
                _currentlyLoadingLeaderboardPage = page;
                page.RequestUserPage(Session.UserId);
            }
        }

        public CoLeaderboardUserRows RequestUsersScores(UserScoresLoadDelegate scoresCompleteDelegate)
        {
            if (SessionActive)
            {
                if (_currentlyLoadingUserScores != null)
                    throw new Exception("Previous user scores request still being processed");

                _userScoresLoadDelegate = scoresCompleteDelegate;
                _currentlyLoadingUserScores = _leaderboardsManager.GetUsersScores(Session.UserId);
                return _currentlyLoadingUserScores;
            }

            return null;
        }

        #region Implementation of IAchievementEventHandler

        public void OnAchievementUnlocked(int userId, CoAchievement achievement)
        {
        }

        public void OnUserAchievementsFetched(int userId, Array args)
        {
            if (_achievementsRefreshInfo != null && !_achievementsRefreshInfo.UserAchievementsFetched)
            {
                _achievementsUnlockedAtIndieCity.Clear();
                if (args != null)
                {
                    var achievements = (Object[]) args;
                    for (int i = 0; i < achievements.Length; i++)
                        _achievementsUnlockedAtIndieCity[((CoAchievement) achievements[i]).AchievementId] = true;
                }
                _achievementsRefreshInfo.UserAchievementsFetched = true;
                CheckIfAchievementDataComplete();
            }
        }

        public void OnAchievementUpdated(long achievementId, uint trueValue, uint awardCount)
        {
        }

        public void OnAchievementGroupInitialised(CoAchievementGroup pGroup, bool modificationsDetected)
        {
            OnAllAchievementsUpdated();
        }

        public void OnAllAchievementsUpdated()
        {
            if (_achievementsRefreshInfo != null && !_achievementsRefreshInfo.AchievementsUpdated)
            {
                _achievementsRefreshInfo.AchievementsUpdated = true;
                CheckIfAchievementDataComplete();
            }
        }

        private void CheckIfAchievementDataComplete()
        {
            if (_achievementsRefreshInfo.AchievementsUpdated && _achievementsRefreshInfo.UserAchievementsFetched)
            {
                try
                {
                    if (_startPhase == SessionStartPhase.WaitingForIndiecityData)
                    {
                        _achievementsReady = true;
                        if (_leaderboardsReady)
                            OnSessionActiveStateReached();
                    }
                    else if (_achievementsRefreshInfo.RefreshCompleteDelegate != null)
                        _achievementsRefreshInfo.RefreshCompleteDelegate.Invoke();
                }
                finally
                {
                    _achievementsRefreshInfo = null;
                }
            }
        }

        #endregion

        #region Implementation of ILeaderboardEventHandler

        public void OnRowsDelivered(RowRequestContext context, Array rows)
        {
        }

        public void OnLeaderboardsInitialised(bool modificationsDetected)
        {
            _leaderboardsReady = true;
            if (_achievementsReady)
                OnSessionActiveStateReached();
        }

        #endregion

        #region Implementation of IUnloadable

        /// <summary>
        /// Disposes any graphics resource used internally by this object, and removes
        ///             scene resources managed by this object. Commonly used during Game.UnloadContent.
        /// </summary>
        public void Unload()
        {
        }

        #endregion

        #region Implementation of IManagerService

        /// <summary>
        /// Gets the manager specific Type used as a unique key for storing and
        ///             requesting the manager from the IManagerServiceProvider.
        /// </summary>
        public Type ManagerType
        {
            get { return typeof (IndieCityManager); }
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