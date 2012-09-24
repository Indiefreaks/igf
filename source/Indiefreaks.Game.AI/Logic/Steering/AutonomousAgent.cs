using System;
using Indiefreaks.Xna.Extensions;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Collision;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Logic.Steering
{
    /// <summary>
    /// The AutonomousAgent class owns and exposes a set of Steering Behaviors that can be configured by the underlying SceneObject or SceneEntity.
    /// </summary>
    public class AutonomousAgent : NonPlayerAgent, IAutonomousEntity
    {
        private readonly ComputeSteeringForcesBehavior _computeSteeringForcesBehavior;
        private Vector3 _acceleration;
        private float _panicDistance;
        private Vector3 _steeringForce;

        /// <summary>
        /// Returns the Seed used for the current AutonmousAgent's Steering Behaviors computation
        /// </summary>
        /// <remarks>The seed is created once per AutonomousAgent and spreaded over all machines in the same session to avoid different behaviors</remarks>
        public int Seed { get; internal set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public AutonomousAgent()
        {
            ComputationMethod = SteeringComputationMethod.PrioritizedTruncatedWeighted;
            MaxForce = 2f;
            MinSpeed = 0f;
            MaxSpeed = 0f;
            MaxTurnRate = 1f;
            Mass = 1f;
            Deceleration = Deceleration.Normal;
            DecelerationTweaker = 0.5f;
            PanicDistance = 100.0f;
            SmoothRotation = true;
            UsesBanking = true;

            Add(new AutonomousAgentSeedInitializationBehavior());

            SteeringBehaviors = new Behaviors();

            _computeSteeringForcesBehavior = new ComputeSteeringForcesBehavior();
            Add(_computeSteeringForcesBehavior);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.WallAvoidance);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.ObstacleAvoidance);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Evade);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Flee);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Separation);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Alignment);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Cohesion);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Seek);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Arrive);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Wander);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Pursuit);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.OffsetPursuit);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Interpose);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.Hide);
            _computeSteeringForcesBehavior.Add(SteeringBehaviors.PathFollowing);


            Velocity = Vector3.Zero;
            VelocityForward = Vector3.Forward;
            VelocityRight = Vector3.Right;
            VelocityUp = Vector3.Up;
        }

        /// <summary>
        /// Enable or Disable the Smooth Rotation algorithm applied to the resulting Steering Force
        /// </summary>
        public bool SmoothRotation { get; set; }

        /// <summary>
        /// Returns the list of Steering Behaviors controlled by this AutonomousAgent
        /// </summary>
        public Behaviors SteeringBehaviors { get; private set; }

        /// <summary>
        /// Gets or sets how Steering force will be calculated
        /// </summary>
        public SteeringComputationMethod ComputationMethod { get; set; }

        /// <summary>
        /// Enable or disble the banking algorithm applied to the resulting Steering Force to simulate bird or plane movements
        /// </summary>
        public bool UsesBanking { get; set; }

        #region IAutonomousEntity Members

        /// <summary>
        /// Gets or sets the Mass of the ParentObject
        /// </summary>
        public float Mass { get; set; }

        /// <summary>
        /// Gets or sets the maximum force used to truncate Steering forces
        /// </summary>
        public float MaxForce { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of degrees (in radians) the ParentObject will turn per second
        /// </summary>
        public float MaxTurnRate { get; set; }

        /// <summary>
        /// Returns the current Postion of the agent
        /// </summary>
        public Vector3 Position
        {
            get { return ParentObject.World.Translation; }
        }

        /// <summary>
        /// Returns the direction the Velocity is heading the autonomous agent to
        /// </summary>
        public Vector3 VelocityForward { get; protected set; }

        /// <summary>
        /// Returns the right direction of the Current Velocity
        /// </summary>
        public Vector3 VelocityRight { get; protected set; }

        /// <summary>
        /// Returns the up direction of the current velocity
        /// </summary>
        public Vector3 VelocityUp { get; protected set; }

        /// <summary>
        /// Returns the direction the entity is heading to
        /// </summary>
        public Vector3 EntityForward
        {
            get { return ParentObject.World.Forward; }
        }

        /// <summary>
        /// Returns the right direction of the Current entity
        /// </summary>
        public Vector3 EntityRight
        {
            get { return ParentObject.World.Right; }
        }

        /// <summary>
        /// Returns the up direction of the current entity
        /// </summary>
        public Vector3 EntityUp
        {
            get { return ParentObject.World.Up; }
        }

        /// <summary>
        /// Returns the current velocity vector applied to the agent
        /// </summary>
        public Vector3 Velocity { get; protected set; }

        /// <summary>
        /// Returns the current speed of the agent
        /// </summary>
        public float Speed { get; protected set; }

        /// <summary>
        /// Gets or sets the minimum speed used to truncate velocity
        /// </summary>
        public float MinSpeed { get; set; }

        /// <summary>
        /// Gets or sets the maximum speed used to truncate velocity
        /// </summary>
        public float MaxSpeed { get; set; }

        /// <summary>
        /// Gets or sets the Deceleration speed used in Steering Behaviors
        /// </summary>
        public Deceleration Deceleration { get; set; }

        /// <summary>
        /// Gets or sets the deceleration factor used to compute real deceleration
        /// </summary>
        public float DecelerationTweaker { get; set; }

        /// <summary>
        /// Gets or sets the distance to which the agent considers danger
        /// </summary>
        public float PanicDistance
        {
            get { return (float) Math.Sqrt(_panicDistance); }
            set { _panicDistance = value*value; }
        }

        /// <summary>
        /// Gets or sets the current Random instance used to compute Steering Behaviors
        /// </summary>
        public Random Dice { get; set; }

        #endregion

        /// <summary>
        /// Event called when the component's parent object is assigned or reassigned.
        /// </summary>
        public override void OnInitialize()
        {
            base.OnInitialize();

            Name = ParentObject.Name;

            VelocityForward = ParentObject.World.Forward;
            VelocityRight = ParentObject.World.Right;
            VelocityUp = ParentObject.World.Up;

            if (ParentObject is SceneObject)
                Mass = ((SceneObject) ParentObject).Mass;
        }

        /// <summary>
        /// Event called when the parent object is updated during the game update loop.
        /// </summary>
        /// <param name="gametime"/>
        public override void OnUpdate(GameTime gametime)
        {
            base.OnUpdate(gametime);

            _steeringForce = Vector3.Zero;

            switch (ComputationMethod)
            {
                case SteeringComputationMethod.TruncatedWeighted:
                    {
                        ComputeTruncatedWeightedSteeringForce();
                        break;
                    }
                default:
                case SteeringComputationMethod.PrioritizedTruncatedWeighted:
                    {
                        ComputePrioritizedTruncatedWeightedSteeringForce();
                        break;
                    }
                case SteeringComputationMethod.PrioritizedDithering:
                    {
                        ComputePrioritizedDitheringSteeringForce();
                        break;
                    }
            }

            ApplySteeringForce(gametime);
            UpdateParentObject(gametime);

            ResetSteeringForces();
        }

        private void ResetSteeringForces()
        {
            foreach (SteeringBehavior steeringBehavior in _computeSteeringForcesBehavior.Behaviors)
            {
                if (steeringBehavior.Enabled)
                    steeringBehavior.ResetSteeringForce();
            }
        }

        private void ComputeTruncatedWeightedSteeringForce()
        {
            foreach (SteeringBehavior steeringBehavior in _computeSteeringForcesBehavior.Behaviors)
            {
                if (steeringBehavior.Enabled)
                    _steeringForce += steeringBehavior.SteeringForce*steeringBehavior.Weight;
            }

            _steeringForce = Vector3Extensions.Truncate(_steeringForce, MaxForce);
        }

        private void ComputePrioritizedTruncatedWeightedSteeringForce()
        {
            foreach (SteeringBehavior steeringBehavior in _computeSteeringForcesBehavior.Behaviors)
            {
                if (!steeringBehavior.Enabled) continue;

                Vector3 forceToAdd = steeringBehavior.SteeringForce*steeringBehavior.Weight;
                if (!AccumulateForce(ref _steeringForce, forceToAdd)) break;
            }
        }

        private bool AccumulateForce(ref Vector3 runningTot, Vector3 forceToAdd)
        {
            //calculate how much steering force the vehicle has used so far
            float magnitudeSoFar = runningTot.Length();

            //calculate how much steering force remains to be used by this vehicle
            float magnitudeRemaining = MaxForce - magnitudeSoFar;

            //return false if there is no more force left to use
            if (magnitudeRemaining <= 0.0) return false;

            //calculate the magnitude of the force we want to add
            float magnitudeToAdd = forceToAdd.Length();

            //if the magnitude of the sum of ForceToAdd and the running total
            //does not exceed the maximum force available to this vehicle, just
            //add together. Otherwise add as much of the ForceToAdd vector is
            //possible without going over the max.
            if (magnitudeToAdd < magnitudeRemaining)
            {
                runningTot += forceToAdd;
            }

            else
            {
                //add it to the steering force
                runningTot += Vector3.Normalize(forceToAdd)*magnitudeRemaining;
            }

            return true;
        }

        private void ComputePrioritizedDitheringSteeringForce()
        {
            foreach (SteeringBehavior steeringBehavior in _computeSteeringForcesBehavior.Behaviors)
            {
                if (steeringBehavior.Enabled && RandomExtensions.GetRandomFloat(Dice, 0f, 1.0f) < steeringBehavior.Probability)
                {
                    _steeringForce += steeringBehavior.SteeringForce*steeringBehavior.Weight/steeringBehavior.Probability;

                    if (_steeringForce != Vector3.Zero)
                        _steeringForce = Vector3Extensions.Truncate(_steeringForce, MaxForce);
                }
            }
        }

        private Vector3 AdjustVelocityForSmoothRotation(GameTime gameTime)
        {
            Vector3 velocityDirection = Velocity != Vector3.Zero ? Vector3.Normalize(Velocity) : EntityForward;

            Vector3 adjustedVelocity = Vector3.Normalize(EntityForward) * Velocity.Length();

            var angle = (float)Math.Acos(Vector3.Dot(velocityDirection, Vector3.Normalize(EntityForward)));

            if ((angle < 0.00000001f && angle > -0.00000001f) || float.IsNaN(angle)) return adjustedVelocity;

            float smooth = MathHelper.Clamp(MaxTurnRate * (float)gameTime.ElapsedGameTime.TotalSeconds / angle, 0f, 1f);
            return Vector3.Lerp(adjustedVelocity, Velocity, smooth);
        }

        /// <summary>
        /// Applies the Steering Force to the current Velocity and Speed values
        /// </summary>
        /// <param name="gameTime"></param>
        /// <remarks>Override this method to modify how the computed Steering Force is applied</remarks>
        public virtual void ApplySteeringForce(GameTime gameTime)
        {
            Vector3 newAcceleration = _steeringForce/Mass;

            if (gameTime.ElapsedGameTime.TotalSeconds > 0)
            {
                float smoothRate = MathHelper.Clamp(9*(float) gameTime.ElapsedGameTime.TotalSeconds, 0.15f, 0.4f);
                MathExtensions.BlendIntoAccumulator(smoothRate, newAcceleration, ref _acceleration);
            }
           
            Velocity += _acceleration*(float) gameTime.ElapsedGameTime.TotalSeconds;
           
            if(MaxSpeed > 0)
                Velocity = Vector3Extensions.Truncate(Velocity, MaxSpeed);

            if (MinSpeed > 0)
                Velocity = Vector3Extensions.Extend(Velocity, MinSpeed);

            if (Velocity.LengthSquared() > 0.000000001f)
            {
                if (SmoothRotation)
                {
                    Velocity = AdjustVelocityForSmoothRotation(gameTime);
                }
                
                VelocityForward = Vector3.Normalize(Velocity);
                VelocityRight = Vector3.Normalize(Vector3.Cross(VelocityForward, VelocityUp));
                VelocityUp = Vector3.Normalize(Vector3.Cross(VelocityRight, VelocityForward));

                if (UsesBanking)
                {
                    // the length of this global-upward-pointing vector controls the vehicle's
                    // tendency to right itself as it is rolled over from turning acceleration
                    var globalUp = new Vector3(0, 0.25f, 0);

                    Vector3 accelerationUp = _acceleration * 0.05f;

                    Vector3 bankUp = accelerationUp + globalUp;
                    bankUp.Z = 0f;

                    float smoothRate = (float)gameTime.ElapsedGameTime.TotalSeconds * 3;
                    Vector3 tempUp = VelocityUp;
                    MathExtensions.BlendIntoAccumulator(smoothRate, bankUp, ref tempUp);
                    VelocityUp = Vector3.Normalize(tempUp);
                    VelocityRight = Vector3.Normalize(Vector3.Cross(VelocityForward, VelocityUp));
                }
            }

            Speed = Velocity.Length();
        }

        public bool UsesPhysics { get; set; }

        /// <summary>
        /// Based on the computed Velocity and Speed, updates the parent object World Matrix
        /// </summary>
        /// <param name="gameTime"></param>
        /// <remarks>Override this method to modify how the computed Velocity is applied</remarks>
        public virtual void UpdateParentObject(GameTime gameTime)
        {
            if (Velocity.LengthSquared() > 0.000000001f)
            {
                Matrix finalRotation = Matrix.Identity;
                finalRotation.Forward = VelocityForward;
                finalRotation.Right = VelocityRight;
                finalRotation.Up = VelocityUp;

                var scale = new Vector3(ParentObject.World.Right.Length(), ParentObject.World.Up.Length(), ParentObject.World.Forward.Length());

                if (UsesPhysics && ParentObject is ICollisionObject && ((ICollisionObject)ParentObject).CollisionMove != null)
                {
                    ParentObject.World = Matrix.CreateScale(scale)*finalRotation*Matrix.CreateTranslation(Position);

                    var collisionObject = ParentObject as ICollisionObject;

                    collisionObject.CollisionMove.ApplyWorldForce(Velocity - (collisionObject.CollisionMove.Normal*collisionObject.CollisionMove.Distance), false);
                }
                else
                {
                    ParentObject.World = Matrix.CreateScale(scale) * finalRotation * Matrix.CreateTranslation(ParentObject.World.Translation + (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds));                    
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        public void ForceVelocity(Vector3 direction, float speed)
        {
            Velocity = direction*speed;
        }

        /// <summary>
        /// Resets Velocity to Zero immediately
        /// </summary>
        public void ResetVelocity()
        {
            Velocity = Vector3.Zero;
        }

        /// <summary>
        /// Disable all SteeringBehaviors
        /// </summary>
        public void DisableSteeringBehaviors()
        {
            foreach (var steeringBehavior in _computeSteeringForcesBehavior.Behaviors)
            {
                steeringBehavior.Enabled = false;
            }
        }
    }
}