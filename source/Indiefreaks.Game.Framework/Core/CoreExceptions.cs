using System;

namespace Indiefreaks.Xna.Core
{
    /// <summary>
    ///   General Engine exception
    /// </summary>
    public class CoreException : Exception
    {
        public CoreException()
        {
        }

        public CoreException(string message)
            : base(message)
        {
        }

        public CoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    ///     Exception raised when something went wrong while loading content using the synchronous or asynchronous function
    /// </summary>
    public class ContentLoadingException : CoreException
    {
        public ContentLoadingException()
        {

        }

        public ContentLoadingException(string message)
            : base(message)
        {

        }

        public ContentLoadingException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}