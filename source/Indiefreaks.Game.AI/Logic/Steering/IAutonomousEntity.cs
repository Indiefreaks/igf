using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// Interface defining the properties and methods required to work with the Steering Behavior system
    /// </summary>
    /// <remarks>
    /// If you want to use specific SceneEntity or SceneObject instances withut using an AutonomousAgent, implement this interface to your SceneEntity or SceneObject inherited classes.
    /// </remarks>
    public interface IAutonomousEntity : INamedObject
    {
        /// <summary>
        /// Gets or sets the Mass of the ParentObject
        /// </summary>
        float Mass { get; set; }

        /// <summary>
        /// Gets or sets the maximum force used to truncate Steering forces
        /// </summary>
        float MaxForce { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of degrees (in radians) the ParentObject will turn per second
        /// </summary>
        float MaxTurnRate { get; set; }

        /// <summary>
        /// Returns the direction the Velocity is heading the autonomous agent to
        /// </summary>
        Vector3 VelocityForward { get; }

        /// <summary>
        /// Returns the right direction of the Current Velocity
        /// </summary>
        Vector3 VelocityRight { get; }

        /// <summary>
        /// Returns the up direction of the current velocity
        /// </summary>
        Vector3 VelocityUp { get; }

        /// <summary>
        /// Returns the direction the entity is heading to
        /// </summary>
        Vector3 EntityForward { get; }

        /// <summary>
        /// Returns the right direction of the Current entity
        /// </summary>
        Vector3 EntityRight { get; }

        /// <summary>
        /// Returns the up direction of the current entity
        /// </summary>
        Vector3 EntityUp { get; }

        /// <summary>
        /// Returns the current velocity vector applied to the agent
        /// </summary>
        Vector3 Velocity { get; }

        /// <summary>
        /// Returns the current speed of the agent
        /// </summary>
        float Speed { get; }

        /// <summary>
        /// Returns the current Postion of the agent
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Gets or sets the maximum speed used to truncate velocity
        /// </summary>
        float MaxSpeed { get; set; }

        /// <summary>
        /// Gets or sets the Deceleration speed used in Steering Behaviors
        /// </summary>
        Deceleration Deceleration { get; set; }

        /// <summary>
        /// Gets or sets the deceleration factor used to compute real deceleration
        /// </summary>
        float DecelerationTweaker { get; set; }

        /// <summary>
        /// Gets or sets the distance to which the agent considers danger
        /// </summary>
        float PanicDistance { get; set; }

    }
}