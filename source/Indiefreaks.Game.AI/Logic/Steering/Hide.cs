using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering Behavior that creates a force that will tend the agent to hide behind an obstacle
    /// </summary>
    public class Hide : ContextualSteeringBehavior
    {
        private Vector3? _bestHidingSpot;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Hide()
        {
            Weight = 1.0f;
            Probability = 0.8f;

            DistanceFromObstacle = 30.0f;
        }

        /// <summary>
        /// Gets or sets the agent from which the current agent should hide from
        /// </summary>
        public IAutonomousEntity Hunter { get; set; }

        /// <summary>
        /// Gets or sets the distance from the obstacle this Steering Behavior should try to get to
        /// </summary>
        public float DistanceFromObstacle { get; set; }

        /// <summary>
        /// Returns the best hiding spot in the current Context
        /// </summary>
        public void GetBestHidingSpot()
        {
            float distanceToClosestObstacle = float.MaxValue;

            foreach (SceneEntity obstacle in Context.Keys)
            {
                if (obstacle == AutonomousAgent.ParentObject)
                    continue;

                float distAway = obstacle.WorldBoundingSphere.Radius + DistanceFromObstacle;

                Vector3 toObstacle = Vector3.Normalize(AutonomousAgent.Position - Hunter.Position);

                Vector3 hidingSpot = (toObstacle*distAway) + AutonomousAgent.Position;

                float distance = Vector3.DistanceSquared(hidingSpot, AutonomousAgent.Position);

                if (distance < distanceToClosestObstacle)
                {
                    distanceToClosestObstacle = distance;

                    _bestHidingSpot = hidingSpot;
                }
            }
        }

        #region Overrides of SteeringBehavior

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public override bool CanCompute()
        {
            if (base.CanCompute() && Hunter != null)
            {
                GetBestHidingSpot();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            if (_bestHidingSpot.HasValue)
                SteeringLibrary.Arrive(AutonomousAgent.Position, _bestHidingSpot.Value, (float) AutonomousAgent.Deceleration, AutonomousAgent.Velocity, AutonomousAgent.MaxSpeed, ForceInfluence, out ComputedSteeringForce);
            else
                SteeringLibrary.Evade(AutonomousAgent.Position, AutonomousAgent.Velocity, AutonomousAgent.MaxSpeed, Hunter.Position, Hunter.Velocity, Hunter.MaxSpeed, AutonomousAgent.PanicDistance, ForceInfluence, out ComputedSteeringForce);
        }

        #endregion
    }
}