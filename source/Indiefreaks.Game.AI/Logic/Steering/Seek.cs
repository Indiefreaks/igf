using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering behavior that tends to move the agent to the provided destination
    /// </summary>
    public class Seek : SteeringBehavior
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Seek()
        {
            Weight = 1.0f;
            Probability = 0.8f;
        }

        /// <summary>
        /// The current destination of the agent
        /// </summary>
        public Vector3? Destination { get; set; }

        #region Overrides of SteeringBehavior

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public override bool CanCompute()
        {
            return base.CanCompute() && Destination.HasValue;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            SteeringLibrary.Seek(AutonomousAgent.Position, Destination.Value, AutonomousAgent.Velocity, AutonomousAgent.MaxSpeed, ForceInfluence, out ComputedSteeringForce);
        }

        #endregion
    }
}