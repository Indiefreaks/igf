namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering Behavior creating a force that tends to move the agent so it stays close to the target agent using a predicted velocity vector
    /// </summary>
    public class Pursuit : SteeringBehavior
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Pursuit()
        {
            Weight = 1.0f;
            Probability = 1.0f;
        }

        /// <summary>
        /// The agent being pursued
        /// </summary>
        public IAutonomousEntity Target { get; set; }

        #region Overrides of SteeringBehavior

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public override bool CanCompute()
        {
            return base.CanCompute() && Target != null;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            SteeringLibrary.Pursuit(AutonomousAgent.Position, AutonomousAgent.EntityForward, AutonomousAgent.Velocity, AutonomousAgent.MaxSpeed, Target.Position, Target.EntityForward, Target.Velocity, Target.Speed, ForceInfluence,
                                    out ComputedSteeringForce);
        }

        #endregion
    }
}