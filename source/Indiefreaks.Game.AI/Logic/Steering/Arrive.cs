using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering behavior that moves an AutonomousAgent towards a specific position decreasing its velocity as it gets closer to the destination.
    /// </summary>
    public class Arrive : SteeringBehavior
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Arrive()
        {
            Weight = 1.0f;
            Probability = 0.5f;
        }
        
        /// <summary>
        /// Gets or sets the current destination
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
            return base.CanCompute() && Destination.HasValue && AutonomousAgent.DecelerationTweaker > 0f;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            SteeringLibrary.Arrive(AutonomousAgent.Position, Destination.Value, (float)AutonomousAgent.Deceleration * AutonomousAgent.DecelerationTweaker, AutonomousAgent.Velocity, AutonomousAgent.MaxSpeed, ForceInfluence, out ComputedSteeringForce);
        }

        #endregion
    }
}