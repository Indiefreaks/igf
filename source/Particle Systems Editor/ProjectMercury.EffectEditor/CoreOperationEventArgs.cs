namespace ProjectMercury.EffectEditor
{
    using System;

    /// <summary>
    /// Defines the abstract base class for an event operation which is run in the application core.
    /// </summary>
    internal abstract class CoreOperationEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of the CoreOperationEventArgs class.
        /// </summary>
        protected CoreOperationEventArgs() { }

        /// <summary>
        /// Gets or sets the result of the operation which was run as the event handler.
        /// </summary>
        public CoreOperationResult Result { get; set; }
    }
}