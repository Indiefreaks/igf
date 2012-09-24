using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering behavior aligning the current AutonomousAgent with others.
    /// </summary>
    public class Alignment : ContextualSteeringBehavior
    {
        private readonly List<Vector3> _agentForwards;

        /// <summary>
        /// Create a new instance
        /// </summary>
        public Alignment()
        {
            Weight = 1.0f;
            Probability = 0.3f;

            _agentForwards = new List<Vector3>();
        }

        private void GetAgentForwards()
        {
            _agentForwards.Clear();

            foreach (SceneEntity agent in Context.Keys)
            {
                _agentForwards.Add(agent.World.Forward);
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
                GetAgentForwards();

            return _agentForwards.Count > 0;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public override void Compute()
        {
            SteeringLibrary.Alignment(AutonomousAgent.EntityForward, _agentForwards, ForceInfluence, out ComputedSteeringForce);
        }

        #endregion
    }
}