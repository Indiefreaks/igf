using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// The abstract class for all Steering Behaviors
    /// </summary>
    public abstract class SteeringBehavior
    {
        protected Vector3 ComputedSteeringForce;
        private Vector3 _forceInfluence;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected SteeringBehavior() : this(1.0f)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="weight">The weight of this steering behavior when computed with others</param>
        protected SteeringBehavior(float weight) : this(weight, 1.0f)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="weight">The weight of this steering behavior when computed with others</param>
        /// <param name="probability">A float value between 0f and 1f defining the chances this steering behavior may compute when used with Dithering Computation algorithm</param>
        protected SteeringBehavior(float weight, float probability)
        {
            _forceInfluence = Vector3.One;
            Weight = weight;
            Probability = MathHelper.Clamp(probability, 0f, 1f);
        }

        /// <summary>
        /// The Agent this steering behavior is managing
        /// </summary>
        public AutonomousAgent AutonomousAgent { get; internal set; }

        /// <summary>
        /// Returns the priority set to this steering behavior
        /// </summary>
        /// <remarks>The higher the value, the higher the steering behavior will be considered in the computation</remarks>
        public float Priority { get; internal set; }

        /// <summary>
        /// Gets or sets the importance of the current steering behavior in the Steering Force computation
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Gets or sets the chances that this steering behavior gets computed when using the Dithering Computation algorithm
        /// </summary>
        public float Probability { get; set; }

        /// <summary>
        /// Gets or sets a 3 dimensions factor that is applied to the current steering behavior steering force computation
        /// </summary>
        /// <remarks>This allows to define how much of the steering force should be considered in all axis. Very useful if looking to apply a steering behavior on a plane</remarks>
        public Vector3 ForceInfluence
        {
            get { return _forceInfluence; }
            set
            {
                if (value.Length() > 1f)
                    value.Normalize();

                _forceInfluence = value;
            }
        }

        /// <summary>
        /// Returns the resulting computed force
        /// </summary>
        public Vector3 SteeringForce
        {
            get { return ComputedSteeringForce; }
        }

        /// <summary>
        /// Gets or sets if the current steering behavior should compute or not
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Defines if the current steering behavior can execute or not.
        /// </summary>
        /// <returns>Returns true if it can, false otherwise</returns>
        /// <remarks>Override this method to add a global condition to this behavior</remarks>
        public virtual bool CanCompute()
        {
            return Enabled;
        }

        /// <summary>
        /// Computes the current Steering Behavior
        /// </summary>
        /// <returns></returns>
        public abstract void Compute();

        public void ResetSteeringForce()
        {
            ComputedSteeringForce = Vector3.Zero;
        }
    }
}