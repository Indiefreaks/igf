using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering Behavior creating a force that tends to place the agent in a slightly offseted postion of a given target agent
    /// </summary>
    public class OffsetPursuit : SteeringBehavior
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public OffsetPursuit()
        {
            Weight = 1.0f;
            Probability = 1.0f;
        }

        /// <summary>
        /// The agent currently being pursued
        /// </summary>
        public IAutonomousEntity Target { get; set; }

        /// <summary>
        /// The offset vector applied to the agent curent position
        /// </summary>
        public Vector3 LocalOffset { get; set; }

        #region Overrides of SteeringBehavior

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public override bool CanCompute()
        {
            return base.CanCompute() && Target != null && Target != AutonomousAgent.ParentObject && LocalOffset != Vector3.Zero;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            Matrix leaderWorldMatrix = Matrix.Identity;
            leaderWorldMatrix.Forward = Target.EntityForward;
            leaderWorldMatrix.Right = Target.EntityRight;
            leaderWorldMatrix.Up = Target.EntityUp;
            leaderWorldMatrix.Translation = Target.Position;

            SteeringLibrary.OffsetPursuit(AutonomousAgent.Position, AutonomousAgent.Velocity, AutonomousAgent.MaxSpeed, (float) AutonomousAgent.Deceleration*AutonomousAgent.DecelerationTweaker, Target.Velocity, Target.MaxSpeed,
                                          Vector3.Transform(LocalOffset, leaderWorldMatrix), ForceInfluence, out ComputedSteeringForce);
        }

        #endregion
    }
}