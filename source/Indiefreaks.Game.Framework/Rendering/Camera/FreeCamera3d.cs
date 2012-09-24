using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Rendering.Camera
{
    /// <summary>
    ///   Implements a free 3d camera controlled by LogicalPlayerIndex.PlayerOne game pad
    /// </summary>
    public class FreeCamera3D : Camera3D
    {
        private const float PitchCap = (float) MathHelper.Pi/2.05f;

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        /// <summary>
        /// Gets or sets the speed of the camera when translating
        /// </summary>
        public float TranslationSpeed { get; set; }

        /// <summary>
        /// Gets or sets the speed of the camera when rotating
        /// </summary>
        public float RotationSpeed { get; set; }

        /// <summary>
        /// Gets or sets the current Rotation Matrix
        /// </summary>
        public Matrix Rotation { get; private set; }

        /// <summary>
        ///   Creates a new Free Camera
        /// </summary>
        /// <param name = "aspectRatio">The aspect ratio is generally defined by the Viewport's Aspect ratio found in the GraphicsDevice instance of the game</param>
        /// <param name = "fieldOfView"></param>
        /// <param name = "nearPlaneDistance"></param>
        /// <param name = "farPlaneDistance"></param>
        public FreeCamera3D(float aspectRatio, float fieldOfView, float nearPlaneDistance,
                          float farPlaneDistance)
            : base(aspectRatio, fieldOfView, nearPlaneDistance, farPlaneDistance)
        {
            TranslationSpeed = .3f;
            RotationSpeed = .3f;

            Rotation = Matrix.Identity;
        }

        #region Overrides of Camera

        /// <summary>
        ///   Override this method to catch input events and act on the camera
        /// </summary>
        /// <param name = "input">The current input instance</param>
        protected override void UpdateInput(InputManager input)
        {
            var player = input.PlayerOne;

            Yaw += player.ThumbSticks.RightStick.X * -.2f * RotationSpeed;
            Pitch += player.ThumbSticks.RightStick.Y * .2f * RotationSpeed;

            if (Pitch > PitchCap)
                Pitch = PitchCap;
            else if (Pitch < -PitchCap)
                Pitch = -PitchCap;

            MoveCamera(Rotation.Right * player.ThumbSticks.LeftStick.X);
            MoveCamera(Rotation.Forward * player.ThumbSticks.LeftStick.Y);
        }

        /// <summary>
        ///   Override this method to update the ViewMatrix property
        /// </summary>
        /// <param name = "gameTime"/>
        protected override Matrix UpdateViewMatrix(GameTime gameTime)
        {
            Rotation.Forward.Normalize();
            Rotation.Right.Normalize();
            Rotation.Up.Normalize();

            Rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, Roll);

            TargetPosition = Position + Rotation.Forward;

            return Matrix.CreateLookAt(Position, TargetPosition, Vector3.Up);
        }

        #endregion

        /// <summary>
        ///   Moves the camera
        /// </summary>
        /// <param name = "addedVector"></param>
        private void MoveCamera(Vector3 addedVector)
        {
            Position += TranslationSpeed * addedVector;
        }
    }
}