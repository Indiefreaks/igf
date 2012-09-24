using System;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Steering Behavior that creates a force that tends to avoid provided agents
    /// </summary>
    public class ObstacleAvoidance : ContextualSteeringBehavior
    {
        private SceneEntity _closestObstacle;
        private Vector3? _closestObstacleLocalPosition;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ObstacleAvoidance()
        {
            Weight = 10.0f;
            Probability = 0.5f;

            FilterByDistance = false;
            DetectionLength = 100f;
            AllowedPenetration = 0f;
            UseLineOfSight = true;
        }

        /// <summary>
        /// Gets or sets the length of a virtual box positioned in front of the current agent to detect obstacles
        /// </summary>
        public float DetectionLength { get; set; }

        public float AllowedPenetration { get; set; }

        public bool UseLineOfSight { get; set; }

        /// <summary>
        /// Defines the closest obstacle
        /// </summary>
        public void DetectClosestObstacle()
        {
            float closestObstacleDistance = float.MaxValue;
            _closestObstacle = null;

            var localAgentMatrix = Matrix.Invert(AutonomousAgent.ParentObject.World);

            foreach (SceneEntity obstacle in Context.Keys)
            {
                Vector3 localPos = Vector3.Transform(obstacle.World.Translation, localAgentMatrix);

                if (UseLineOfSight)
                {
                    if (localPos.Z > 0f)
                        continue;
                }

                float expandedRadius = obstacle.WorldBoundingSphere.Radius + AutonomousAgent.ParentObject.WorldBoundingSphere.Radius - AllowedPenetration;

                var ray = new Ray(Vector3.Zero, Vector3.Forward);
                float? result = ray.Intersects(new BoundingSphere(localPos, expandedRadius));
                if (result.HasValue && result.Value < closestObstacleDistance)
                {
                    closestObstacleDistance = result.Value;

                    _closestObstacle = obstacle;
                    _closestObstacleLocalPosition = localPos;
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
            FilterDistance = DetectionLength + (AutonomousAgent.Speed / AutonomousAgent.MaxSpeed) * DetectionLength;

            if (base.CanCompute())
            {
                DetectClosestObstacle();
                return _closestObstacle != null && _closestObstacleLocalPosition.HasValue;
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
            //SteeringLibrary.ObstacleAvoidance(_closestObstacleLocalPosition.Value, _closestObstacle.WorldBoundingSphere.Radius, FilterDistance, ForceInfluence * Context[_closestObstacle], out ComputedSteeringForce);

            //var transposedInvertWorld = AutonomousAgent.ParentObject.World;
            //Matrix.Invert(ref transposedInvertWorld, out transposedInvertWorld);
            //Matrix.Transpose(ref transposedInvertWorld, out transposedInvertWorld);
            //Vector3.Transform(ref ComputedSteeringForce, ref transposedInvertWorld, out ComputedSteeringForce);
            
            SteeringLibrary.ObstacleAvoidanceNew(AutonomousAgent.Position, _closestObstacle.World.Translation, _closestObstacle.World.Forward, ForceInfluence * Context[_closestObstacle], out ComputedSteeringForce);
        }

        #endregion
    }
}