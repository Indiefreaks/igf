using System;

namespace Indiefreaks.Xna.Threading
{
    /// <summary>
    /// An exception thrown when an unhandled exception is thrown within a task.
    /// </summary>
#if WINDOWS
    [Serializable]
#endif
    public class TaskException : Exception
    {
        /// <summary>
        /// Gets an array containing any unhandled exceptions that were thrown by the task.
        /// </summary>
        public Exception[] InnerExceptions { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="TaskException"/> class.
        /// </summary>
        /// <param name="inner">The unhandles exceptions thrown by the task.</param>
        public TaskException(Exception[] inner) 
            : base("An exception(s) was thrown while executing a task.", null) 
        {
            InnerExceptions = inner;
        }
    }
}
