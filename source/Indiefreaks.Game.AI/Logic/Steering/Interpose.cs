namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering Behavior that creates a force that will try to place the current agent in between two other agents
    /// </summary>
    public class Interpose : SteeringBehavior
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Interpose()
        {
            Weight = 1.0f;
            Probability = 1.0f;
        }

        /// <summary>
        /// The first agent
        /// </summary>
        public IAutonomousEntity AgentA { get; set; }

        /// <summary>
        /// The second agent
        /// </summary>
        public IAutonomousEntity AgentB { get; set; }

        #region Overrides of SteeringBehavior

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public override bool CanCompute()
        {
            return base.CanCompute() && AgentA != null && AgentB != null && AgentA != AutonomousAgent && AgentB != AutonomousAgent;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            SteeringLibrary.Interpose(AutonomousAgent.Position, AutonomousAgent.Velocity, AgentA.Position, AgentA.Velocity, AgentB.Position, AgentB.Velocity, AutonomousAgent.MaxSpeed, (float) AutonomousAgent.Deceleration, ForceInfluence,
                                      out ComputedSteeringForce);
        }

        #endregion
    }
}