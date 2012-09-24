namespace ProjectMercury.EffectEditor
{
    using System;

    /// <summary>
    /// Defines the result of an operation which was executed in the application core.
    /// </summary>
    internal class CoreOperationResult
    {
        /// <summary>
        /// Defines an operation result which ran correctly with no errors.
        /// </summary>
        static public CoreOperationResult OK { get; private set; }

        /// <summary>
        /// Initializes the CoreOperationResult class.
        /// </summary>
        static CoreOperationResult()
        {
            CoreOperationResult.OK = new CoreOperationResult(null);
        }

        /// <summary>
        /// Creates a new instance of the CoreOperationResult class.
        /// </summary>
        /// <param name="exception">The exception which was raised during the operation.</param>
        public CoreOperationResult(Exception exception)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// Gets or sets the exception which was raised during the operation.
        /// </summary>
        public Exception Exception { get; private set; }
    }
}
