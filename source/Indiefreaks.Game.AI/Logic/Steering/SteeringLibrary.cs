using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// This static class implements all the Steering Behaviors actual algorithms
    /// </summary>
    public static class SteeringLibrary
    {
        /// <summary>
        /// Creates a force that tends to move towards a destination
        /// </summary>
        /// <param name="position">Current position of the agent</param>
        /// <param name="target">Current position of the destination</param>
        /// <param name="velocity">Current velocity of the agent</param>
        /// <param name="maxSpeed">Current maximum speed of the agent</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Seek(Vector3 position, Vector3 target, Vector3 velocity, float maxSpeed, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            steeringForce = Vector3.Normalize(target - position)*maxSpeed;
            steeringForce -= velocity;
            steeringForce *= forceInfluence;
        }

        /// <summary>
        /// Creates a force that tends to move away from a given position
        /// </summary>
        /// <param name="position">Current position of the agent</param>
        /// <param name="target">Current position of the threat</param>
        /// <param name="panicDistance">Agent's distance from which the threat should be considered</param>
        /// <param name="velocity">Current velocity of the agent</param>
        /// <param name="maxSpeed">Current maximum speed of the agent</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Flee(Vector3 position, Vector3 target, float panicDistance, Vector3 velocity, float maxSpeed, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            if (Vector3.DistanceSquared(position, target) > panicDistance)
            {
                steeringForce = Vector3.Zero;
                return;
            }

            steeringForce = Vector3.Normalize(position - target)*maxSpeed*forceInfluence;
            steeringForce -= velocity;
        }

        /// <summary>
        /// Creates a force that tends to move towards a position and decelerate when getting close to it
        /// </summary>
        /// <param name="position">Current position of the agent</param>
        /// <param name="destination">Current position of the destination</param>
        /// <param name="deceleration">Deceleration factor applied when reaching close to destination</param>
        /// <param name="velocity">Current velocity of the agent</param>
        /// <param name="maxSpeed">Current maximum speed of the agent</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Arrive(Vector3 position, Vector3 destination, float deceleration, Vector3 velocity, float maxSpeed, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            Vector3 toDestination = destination - position;

            float distance = toDestination.Length();

            if (distance > 0)
            {
                float speed = distance/deceleration;

                speed = Math.Min(speed, maxSpeed);

                steeringForce = toDestination*speed*forceInfluence/distance;
                steeringForce -= velocity;
                return;
            }

            steeringForce = Vector3.Zero;
        }

        /// <summary>
        /// Creates a force that tends to move behind a given agent using prediction
        /// </summary>
        /// <param name="pursuerPosition">Current agent position</param>
        /// <param name="pursuerForward">Current agent forward vector</param>
        /// <param name="pursuerVelocity">Curret agent velocity</param>
        /// <param name="pursuerMaxSpeed">Current agent maximum speed</param>
        /// <param name="evaderPosition">Target agent position</param>
        /// <param name="evaderForward">Target agent forward vector</param>
        /// <param name="evaderVelocity">Target agent velocity</param>
        /// <param name="evaderSpeed">Target agent current speed</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Pursuit(Vector3 pursuerPosition, Vector3 pursuerForward, Vector3 pursuerVelocity, float pursuerMaxSpeed, Vector3 evaderPosition, Vector3 evaderForward, Vector3 evaderVelocity, float evaderSpeed, Vector3 forceInfluence,
                                   out Vector3 steeringForce)
        {
            Vector3 toEvader = evaderPosition - pursuerPosition;

            float relativeHeading = Vector3.Dot(pursuerForward, evaderForward);

            if (Vector3.Dot(toEvader, pursuerForward) > 0f && relativeHeading < -0.95f)
            {
                Seek(pursuerPosition, evaderPosition, pursuerVelocity, pursuerMaxSpeed, forceInfluence, out steeringForce);
                return;
            }

            float lookAheadOfTime = toEvader.Length()/(pursuerMaxSpeed + evaderSpeed);

            Seek(pursuerPosition, (evaderPosition + evaderVelocity*lookAheadOfTime), pursuerVelocity, pursuerMaxSpeed, forceInfluence, out steeringForce);
        }

        /// <summary>
        /// Creates a force that tends to move away from a given agent using prediction
        /// </summary>
        /// <param name="evaderPosition">Current agent position</param>
        /// <param name="evaderVelocity">Current agent velocity</param>
        /// <param name="evaderMaxSpeed">Current agent maximum speed</param>
        /// <param name="pursuerPosition">Threatening agent position</param>
        /// <param name="pursuerVelocity">Threatening agent velocity</param>
        /// <param name="pursuerMaxSpeed">Threatening agent maximum speed</param>
        /// <param name="panicDistance">Agent's distance from which the threat should be considered</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Evade(Vector3 evaderPosition, Vector3 evaderVelocity, float evaderMaxSpeed, Vector3 pursuerPosition, Vector3 pursuerVelocity, float pursuerMaxSpeed, float panicDistance, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            Vector3 toPursuer = pursuerPosition - evaderPosition;

            float lookAheadOfTime = toPursuer.Length()/(evaderMaxSpeed + pursuerMaxSpeed);

            Flee(evaderPosition, (pursuerPosition + pursuerVelocity*lookAheadOfTime), panicDistance, evaderVelocity, evaderMaxSpeed, forceInfluence, out steeringForce);
        }

        /// <summary>
        /// Creates a force that randomly moves a given agent
        /// </summary>
        /// <param name="dice">A Random instance</param>
        /// <param name="position">Current agent position</param>
        /// <param name="velocity">Current agent velocity</param>
        /// <param name="entityForward">Current agent forward vector</param>
        /// <param name="wanderTarget">Current wander position</param>
        /// <param name="wanderRadius">Radius of the circle used to compute wander positions</param>
        /// <param name="wanderDistance">Distance of the circle center used to compute wander positions</param>
        /// <param name="wanderJitter">Factor applied to the new wander position</param>
        /// <param name="maxSpeed">Current agent maximum speed</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Wander(Random dice, Vector3 position, Vector3 velocity, Vector3 entityForward, ref Vector3 wanderTarget, float wanderRadius, float wanderDistance, float wanderJitter, float maxSpeed, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            wanderTarget += new Vector3(
                (float)((dice.NextDouble() * 2d) - 1d) * wanderJitter,
                (float)((dice.NextDouble() * 2d) - 1d) * wanderJitter,
                (float)((dice.NextDouble() * 2d) - 1d) * wanderJitter);

            wanderTarget = Vector3.Normalize(wanderTarget);
            wanderTarget *= wanderRadius;

            steeringForce = Vector3.Normalize(wanderTarget + (entityForward*wanderDistance))*maxSpeed*forceInfluence;
            steeringForce -= velocity;
        }

        /// <summary>
        /// Creates a force that tends to move away from a given obstacle
        /// </summary>
        /// <param name="obstacleLocalPosition">Obstacle local space position</param>
        /// <param name="obstacleBoundsRadius">Obstacle bounding radius</param>
        /// <param name="detectionBoxLength">Length of detection sensor</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void ObstacleAvoidance(Vector3 obstacleLocalPosition, float obstacleBoundsRadius, float detectionBoxLength, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            float multiplier = 1.0f - (detectionBoxLength + obstacleLocalPosition.Z) / detectionBoxLength;

            var force = Vector3.Zero;

            force.X = (obstacleBoundsRadius - obstacleLocalPosition.X) * multiplier * forceInfluence.X;
            force.Y = (obstacleBoundsRadius - obstacleLocalPosition.Y) * multiplier * forceInfluence.Y;

            force.Z = (obstacleBoundsRadius + obstacleLocalPosition.Z) * 0.25f * forceInfluence.Z;

            steeringForce = force;
        }

        public static void ObstacleAvoidanceNew(Vector3 entityPosition, Vector3 obstaclePosition, Vector3 obstacleForward, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            var localOffset = entityPosition - obstaclePosition;

            var forwardComponent = Vector3.Dot(localOffset, obstacleForward);
            var forwardOffset = obstacleForward * forwardComponent;

            var offForwardOffset = localOffset - forwardOffset;

            steeringForce = (offForwardOffset * -1) * forceInfluence;
        }
        
        /// <summary>
        /// Creates a force that tends to place the agent in between two targets
        /// </summary>
        /// <param name="entityPosition">Current agent position</param>
        /// <param name="entityVelocity">Current agent velocity</param>
        /// <param name="agentAPosition">First agent position</param>
        /// <param name="agentAVelocity">First agent velocity</param>
        /// <param name="agentBPosition">Second agent position</param>
        /// <param name="agentBVelocity">Second agent velocity</param>
        /// <param name="entityMaxSpeed">Current agent maximum speed</param>
        /// <param name="entityDeceleration">Deceleration used when getting close to interpose</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Interpose(Vector3 entityPosition, Vector3 entityVelocity, Vector3 agentAPosition, Vector3 agentAVelocity, Vector3 agentBPosition, Vector3 agentBVelocity, float entityMaxSpeed, float entityDeceleration,
                                     Vector3 forceInfluence, out Vector3 steeringForce)
        {
            Vector3 midPoint = (agentAPosition - agentBPosition)*0.5f;

            float timeToReachMidPoint = Vector3.Distance(entityPosition, midPoint)/entityMaxSpeed;

            Vector3 aFuturePos = agentAPosition + agentAVelocity*timeToReachMidPoint;
            Vector3 bFuturePos = agentBPosition + agentBVelocity*timeToReachMidPoint;

            midPoint = (aFuturePos - bFuturePos)*0.5f;

            Arrive(entityPosition, midPoint, entityDeceleration, entityVelocity, entityMaxSpeed, forceInfluence, out steeringForce);
        }

        /// <summary>
        /// Creates a force that tends to move towards an agent predicted position with an offset
        /// </summary>
        /// <param name="entityPosition">Current agent position</param>
        /// <param name="entityVelocity">Current agent velocity</param>
        /// <param name="entityMaxSpeed">Current agent maximum speed</param>
        /// <param name="entityDeceleration">Deceleration used when getting closer to target</param>
        /// <param name="leaderVelocity">Target velocity</param>
        /// <param name="leaderMaxSpeed">Target maximum speed</param>
        /// <param name="worldOffsetPosition">Offset position</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void OffsetPursuit(Vector3 entityPosition, Vector3 entityVelocity, float entityMaxSpeed, float entityDeceleration, Vector3 leaderVelocity, float leaderMaxSpeed, Vector3 worldOffsetPosition, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            var toOffset = worldOffsetPosition - entityPosition;

            var lookAheadOfTime = toOffset.Length()/(entityMaxSpeed + leaderMaxSpeed);

            Arrive(entityPosition, worldOffsetPosition + leaderVelocity * lookAheadOfTime, entityDeceleration, entityVelocity, entityMaxSpeed, forceInfluence, out steeringForce);
        }

        /// <summary>
        /// Creates a force that tends to move away from a list of other agents
        /// </summary>
        /// <param name="entityPosition">Current agent position</param>
        /// <param name="agentPositions">Considered agent positions</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Separation(Vector3 entityPosition, List<Vector3> agentPositions, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            steeringForce = Vector3.Zero;

            foreach (var agentPosition in agentPositions)
            {
                var toAgent = entityPosition - agentPosition;

                steeringForce += Vector3.Normalize(toAgent)/toAgent.Length()*forceInfluence;
            }
        }

        /// <summary>
        /// Creates a force that tends to align the current agent with a list of other agents
        /// </summary>
        /// <param name="entityForward">Current agent forward vector</param>
        /// <param name="agentForwards">Considered agent forward vectors</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Alignment(Vector3 entityForward, List<Vector3> agentForwards, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            steeringForce = Vector3.Zero;

            foreach (var agentForward in agentForwards)
            {
                steeringForce += agentForward;
            }

            steeringForce /= agentForwards.Count;
            steeringForce -= entityForward;
            steeringForce *= forceInfluence;
        }

        /// <summary>
        /// Creates a force that tends to move closer to a list of agents
        /// </summary>
        /// <param name="entityPosition">Current agent position</param>
        /// <param name="entityVelocity">Current agent velocity</param>
        /// <param name="entityMaxSpeed">Current agent maximum speed</param>
        /// <param name="agentPositions">Considered agent positions</param>
        /// <param name="forceInfluence">Limits for each axis</param>
        /// <param name="steeringForce">The resulting force</param>
        public static void Cohesion(Vector3 entityPosition, Vector3 entityVelocity, float entityMaxSpeed, List<Vector3> agentPositions, Vector3 forceInfluence, out Vector3 steeringForce)
        {
            Vector3 centerOfMass = Vector3.Zero;

            foreach (var agentPosition in agentPositions)
            {
                centerOfMass += agentPosition;
            }

            centerOfMass /= agentPositions.Count;

            Seek(entityPosition, centerOfMass, entityVelocity, entityMaxSpeed, forceInfluence, out steeringForce);
        }
    }
}