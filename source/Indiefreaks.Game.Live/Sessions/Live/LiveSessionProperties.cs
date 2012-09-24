using Microsoft.Xna.Framework.Net;

namespace Indiefreaks.Xna.Sessions.Live
{
    /// <summary>
    /// Helper methods to convert SessionProperties to and from NetworkSessionProperties
    /// </summary>
    public class LiveSessionProperties
    {
        /// <summary>
        /// Converts a SessionProperties instance to the NetworkSessionProperties
        /// </summary>
        internal static NetworkSessionProperties ConvertToLiveSessionProperties(SessionProperties sessionProperties)
        {
            if (sessionProperties == null)
                return null;

            var networkSessionProperties = new NetworkSessionProperties();
            
            for (int i = 0; i < sessionProperties.Count; i++)
            {
                networkSessionProperties[i] = sessionProperties[i];
            }

            return networkSessionProperties;
        }

        /// <summary>
        /// Converts a NetworkSessionProperties instance to the SessionProperties
        /// </summary>
        internal static SessionProperties ConvertFromLiveSessionProperties(NetworkSessionProperties networkSessionProperties)
        {
            if (networkSessionProperties == null)
                return null;

            var sessionProperties = new SessionProperties();

            for (int i = 0; i < networkSessionProperties.Count; i++)
            {
                sessionProperties[i] = networkSessionProperties[i];
            }

            return sessionProperties;
        }
    }
}