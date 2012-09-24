using System;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering Behavior that creates a force to move away from a given position
    /// </summary>
    public class Flee : SteeringBehavior
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Flee()
        {
            Weight = 1.0f;
            Probability = 0.6f;
        }

        /// <summary>
        /// The Position that should be flown from
        /// </summary>
        public Vector3? DangerPosition { get; set; }

        #region Overrides of SteeringBehavior

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public override bool CanCompute()
        {
            return base.CanCompute() && DangerPosition.HasValue;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            SteeringLibrary.Flee(AutonomousAgent.Position, DangerPosition.Value, AutonomousAgent.PanicDistance, AutonomousAgent.Velocity, AutonomousAgent.MaxSpeed, ForceInfluence, out ComputedSteeringForce);
        }

        #endregion
    }
}