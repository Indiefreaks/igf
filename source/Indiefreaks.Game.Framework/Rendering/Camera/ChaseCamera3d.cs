using System;
using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.Camera
{
    public class ChaseCamera3D : Camera3D
    {
        public ISceneEntity TargetEntity { get; set; }

        #region Desired camera positioning (set when creating camera or changing view)

        public Vector3 ChaseCoords
        {
            get { return chaseCoords; }
            set { chaseCoords = value; }
        }
        private Vector3 chaseCoords = Vector3.One;

        /// <summary>
        /// Desired camera position in the chased object's coordinate system.
        /// </summary>
        public Vector3 DesiredPositionOffset
        {
            get { return desiredPositionOffset; }
            set { desiredPositionOffset = value; }
        }
        private Vector3 desiredPositionOffset = new Vector3(14, 10, -42);

        /// <summary>
        /// Desired camera position in world space.
        /// </summary>
        public Vector3 DesiredPosition
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return desiredPosition;
            }
        }
        private Vector3 desiredPosition;

        /// <summary>
        /// Look at point in the chased object's coordinate system.
        /// </summary>
        public Vector3 TargetPositionOffset
        {
            get { return targetPositionOffset; }
            set { targetPositionOffset = value; }
        }
        private Vector3 targetPositionOffset = new Vector3(17, 5, 50);

        /// <summary>
        /// Look at point in world space.
        /// </summary>
        public new Vector3 TargetPosition
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return targetPosition;
            }
            set { targetPosition = value; }
        }
        private Vector3 targetPosition;

        #endregion

        #region Camera physics (typically set when creating camera)

        /// <summary>
        /// Physics coefficient which controls the influence of the camera's position
        /// over the spring force. The stiffer the spring, the closer it will stay to
        /// the chased object.
        /// </summary>
        public float Stiffness
        {
            get { return stiffness; }
            set { stiffness = value; }
        }
        private float stiffness = 1800.0f;

        /// <summary>
        /// Physics coefficient which approximates internal friction of the spring.
        /// Sufficient damping will prevent the spring from oscillating infinitely.
        /// </summary>
        public float Damping
        {
            get { return damping; }
            set { damping = value; }
        }
        private float damping = 600.0f;

        /// <summary>
        /// Mass of the camera body. Heaver objects require stiffer springs with less
        /// damping to move at the same rate as lighter objects.
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        private float mass = 100.0f;

        #endregion

        #region Current camera properties (updated by camera physics)

        /// <summary>
        /// Velocity of camera.
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
        }
        private Vector3 velocity;

        #endregion

        #region Methods

        /// <summary>
        ///   Creates a new instance of the camera
        /// </summary>
        /// <param name = "aspectRatio"></param>
        /// <param name = "fieldOfView"></param>
        /// <param name = "nearPlaneDistance"></param>
        /// <param name = "farPlaneDistance"></param>
        public ChaseCamera3D(float aspectRatio, float fieldOfView, float nearPlaneDistance, float farPlaneDistance)
            : base(aspectRatio, fieldOfView, nearPlaneDistance, farPlaneDistance)
        {
            targetPosition = Vector3.Forward;
        }

        /// <summary>
        /// Rebuilds object space values in world space. Invoke before publicly
        /// returning or privately accessing world space values.
        /// </summary>
        private void UpdateWorldPositions()
        {
            if (TargetEntity != null)
            {
                // Calculate desired camera properties in world space
                Vector3 chaseDesiredPosition = TargetEntity.World.Translation + DesiredPositionOffset;  //new Vector3((chaseCoords.X > 0 ? TargetEntity.World.Translation.X : desiredPosition.X), (chaseCoords.Y > 0 ? TargetEntity.World.Translation.Y : desiredPosition.Y), (chaseCoords.Z > 0 ? TargetEntity.World.Translation.Z : desiredPosition.Z));
                Vector3 chaseTargetPosition = TargetEntity.World.Translation + TargetPositionOffset;  //new Vector3((chaseCoords.X > 0 ? TargetEntity.World.Translation.X : TargetPosition.X), (chaseCoords.Y > 0 ? TargetEntity.World.Translation.Y : TargetPosition.Y), (chaseCoords.Z > 0 ? TargetEntity.World.Translation.Z : TargetPosition.Z));

                if (chaseCoords.X == 0)
                {
                    chaseDesiredPosition.X = desiredPosition.X;
                    chaseTargetPosition.X = targetPosition.X;
                }

                if (chaseCoords.Y == 0)
                {
                    chaseDesiredPosition.Y = desiredPosition.Y;
                    chaseTargetPosition.Y = targetPosition.Y;
                }

                if (chaseCoords.Z == 0)
                {
                    chaseDesiredPosition.Z = desiredPosition.Z;
                    chaseTargetPosition.Z = targetPosition.Z;
                }

                desiredPosition = chaseDesiredPosition;
                targetPosition = chaseTargetPosition;
            }
        }

        /// <summary>
        /// Rebuilds camera's view and projection matricies.
        /// </summary>
        private Matrix UpdateMatrices()
        {
            return Matrix.CreateLookAt(this.Position, this.targetPosition, Vector3.Up);
        }

        /// <summary>
        /// Forces camera to be at desired position and to stop moving. The is useful
        /// when the chased object is first created or after it has been teleported.
        /// Failing to call this after a large change to the chased object's position
        /// will result in the camera quickly flying across the world.
        /// </summary>
        public void Reset()
        {
            // Activate
            chaseCoords = Vector3.One;

            UpdateWorldPositions();

            // Stop motion
            velocity = Vector3.Zero;

            // Force desired position
            Position = desiredPosition;

            UpdateMatrices();
        }


        /// <summary>
        ///   Override this method to catch input events and act on the camera
        /// </summary>
        /// <param name = "input">The current input instance</param>
        protected override void UpdateInput(InputManager input)
        {
        }

        /// <summary>
        /// Animates the camera from its current position towards the desired offset
        /// behind the chased object. The camera's animation is controlled by a simple
        /// physical spring attached to the camera and anchored to the desired position.
        /// </summary>
        protected override Matrix UpdateViewMatrix(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            UpdateWorldPositions();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate spring force
            Vector3 stretch = Position - desiredPosition;
            Vector3 force = -stiffness * stretch - damping * velocity;

            // Apply acceleration
            Vector3 acceleration = force / mass;
            velocity += acceleration * elapsed;

            // Apply velocity
            Position += velocity * elapsed;

            return UpdateMatrices();
        }

        #endregion
    }
}