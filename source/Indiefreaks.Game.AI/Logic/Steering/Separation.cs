using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering behavior that tends to separate the current agent from the provided list of agents
    /// </summary>
    public class Separation : ContextualSteeringBehavior
    {
        private readonly List<Vector3> _agentPositions;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public Separation()
        {
            Weight = 1.0f;
            Probability = 0.2f;

            _agentPositions = new List<Vector3>();
        }

        private void GetAgentPositions()
        {
            _agentPositions.Clear();

            foreach (SceneEntity agent in Context.Keys)
            {
                _agentPositions.Add(agent.World.Translation);
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
            if (base.CanCompute())
                GetAgentPositions();

            return _agentPositions.Count > 0;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            SteeringLibrary.Separation(AutonomousAgent.Position, _agentPositions, ForceInfluence, out ComputedSteeringForce);
        }

        #endregion
    }
}