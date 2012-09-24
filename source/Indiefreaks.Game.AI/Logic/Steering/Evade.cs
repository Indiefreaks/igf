namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering Behavior that continuously flees on the opposite predicted direction of a given Agent
    /// </summary>
    public class Evade : SteeringBehavior
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Evade()
        {
            Weight = 0.01f;
            Probability = 1.0f;
        }

        /// <summary>
        /// The Agent to flee from
        /// </summary>
        public IAutonomousEntity Pursuer { get; set; }

        #region Overrides of SteeringBehavior

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public override bool CanCompute()
        {
            return base.CanCompute() && Pursuer != null;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            SteeringLibrary.Evade(AutonomousAgent.Position, AutonomousAgent.Velocity, AutonomousAgent.MaxSpeed, Pursuer.Position, Pursuer.Velocity, Pursuer.Speed, AutonomousAgent.PanicDistance, ForceInfluence, out ComputedSteeringForce);
        }

        #endregion
    }
}