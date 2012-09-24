using System;

namespace Indiefreaks.Xna.IndieCity
{
    internal class AchievementsRefreshInfo
    {
            public bool AchievementsUpdated;
            public bool UserAchievementsFetched;
            public NotificationDelegate RefreshCompleteDelegate;

            public AchievementsRefreshInfo(IndieCityManager ic, NotificationDelegate refreshCompleteDelegate, Boolean mustRefreshAchievementValues)
            {
                AchievementsUpdated = false;
                UserAchievementsFetched = false;
                RefreshCompleteDelegate = refreshCompleteDelegate;

                ic.AchievementsManager.GetUserAchievementList(ic.Session.UserId);
                if (mustRefreshAchievementValues)
                    ic.AchievementsManager.AchievementGroup.RefreshAchievementValues();
            }    
    }
}