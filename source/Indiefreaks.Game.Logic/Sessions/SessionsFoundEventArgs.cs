using System;
using System.Collections.Generic;

namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// Specialized event arguments used when a find sessions query ends
    /// </summary>
    public class SessionsFoundEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="sessionsFound">The list of AvailableSession instances returned by the query</param>
        public SessionsFoundEventArgs(IList<AvailableSession> sessionsFound)
        {
            SessionsFound = sessionsFound;
        }

        /// <summary>
        /// Returns the list of AvailableSession instances returned by the query
        /// </summary>
        public IList<AvailableSession> SessionsFound { get; private set; }
    }
}