using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering behavior that randomly but naturally moves an agent in world space
    /// </summary>
    public class Wander : SteeringBehavior
    {
        private Vector3 _wanderTarget;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Wander()
        {
            Weight = 1.0f;
            Probability = 0.8f;

            Radius = 1f;
            Distance = 1f;
            Jitter = 1f;
        }

        /// <summary>
        /// Gets or sets the radius of the circle used to compute wander position
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Gets or sets the distance of the circle center from the current agent used to compute wander position
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// Gets or sets the factor applied to the difference applied to the new wander position
        /// </summary>
        public float Jitter { get; set; }

        #region Overrides of SteeringBehavior

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public override bool CanCompute()
        {
            if (_wanderTarget == Vector3.Zero && AutonomousAgent.Seed != 0)
            {
                var theta = (float) (AutonomousAgent.Dice.NextDouble()*MathHelper.TwoPi);
                _wanderTarget = new Vector3(theta);
            }
            return base.CanCompute() && Jitter != 0 && Radius > 0 && AutonomousAgent.Dice != null;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            SteeringLibrary.Wander(AutonomousAgent.Dice, AutonomousAgent.Position, AutonomousAgent.Velocity, AutonomousAgent.EntityForward, ref _wanderTarget, Radius, Distance, Jitter, AutonomousAgent.MaxSpeed, ForceInfluence,
                                   out ComputedSteeringForce);
        }

        #endregion
    }
}