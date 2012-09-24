namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// List of steering behaviors computation sum
    /// </summary>
    public enum SteeringComputationMethod
    {
        /// <summary>
        /// Computed in order and truncated at the end of all steering behaviors computation
        /// </summary>
        TruncatedWeighted,

        /// <summary>
        /// Computed in a prioritized order and truncated after each steering behavior computation
        /// </summary>
        PrioritizedTruncatedWeighted,
        
        /// <summary>
        /// Computed in a prioritized order if probability of steering behavior hits
        /// </summary>
        PrioritizedDithering,
    } ;
}