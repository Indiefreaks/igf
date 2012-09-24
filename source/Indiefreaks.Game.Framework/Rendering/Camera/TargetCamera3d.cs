using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Rendering.Camera
{
    /// <summary>
    ///   A simple static camera that can be placed anywhere in the world using its StaticPosition and Target position
    /// </summary>
    public class TargetCamera3D : Camera3D
    {
        /// <summary>
        ///   Creates a new instance of the camera
        /// </summary>
        /// <param name = "aspectRatio"></param>
        /// <param name = "fieldOfView"></param>
        /// <param name = "nearPlaneDistance"></param>
        /// <param name = "farPlaneDistance"></param>
        public TargetCamera3D(float aspectRatio, float fieldOfView, float nearPlaneDistance, float farPlaneDistance)
            : base(aspectRatio, fieldOfView, nearPlaneDistance, farPlaneDistance)
        {
            TargetPosition = Vector3.Forward;
        }
        
        #region Overrides of Camera

        /// <summary>
        ///   Override this method to catch input events and act on the camera
        /// </summary>
        /// <param name = "input">The current input instance</param>
        protected override void UpdateInput(InputManager input)
        {
        }

        /// <summary>
        ///   Override this method to update the ViewMatrix property
        /// </summary>
        /// <param name = "gameTime" />
        protected override Matrix UpdateViewMatrix(GameTime gameTime)
        {
            return Matrix.CreateLookAt(Position, TargetPosition, Vector3.Up);
        }

        #endregion
    }
}