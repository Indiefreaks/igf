using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.Camera
{
    /// <summary>
    /// 
    /// </summary>
    public class ArcBallCamera : Camera3D
    {
        private const float VerticalRotationMin = 0.01f;
        private const float VerticalRotationMax = MathHelper.Pi - 0.01f;

        private readonly float _maxRadius;
        private readonly float _minRadius;

        // Should we force these initial values to be supplied in the constructor ??
        private float _horizontalRotation;
        private float _radius = 30f;
        private float _verticalRotation = MathHelper.PiOver4;

        /// <summary>
        /// Creates a new instance of the camera
        /// </summary>
        /// <param name = "aspectRatio"></param>
        /// <param name = "fieldOfView"></param>
        /// <param name = "nearPlaneDistance"></param>
        /// <param name = "farPlaneDistance"></param>
        /// <param name="minRadius"></param>
        /// <param name="maxRadius"></param>
        public ArcBallCamera(float aspectRatio, float fieldOfView, float nearPlaneDistance, float farPlaneDistance, float minRadius, float maxRadius)
            : base(aspectRatio, fieldOfView, nearPlaneDistance, farPlaneDistance)
        {
            TargetPosition = Vector3.Forward;
            _minRadius = minRadius;
            _maxRadius = maxRadius;
        }

        public ISceneEntity TargetEntity { get; set; }

        /// <summary>
        /// Distance of camera from target
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set { _radius = MathHelper.Clamp(value, _minRadius, _maxRadius); }
        }

        /// <summary>
        /// Rotation of camera around vertical axis (Y)
        /// </summary>
        public float HorizontalRotation
        {
            get { return _horizontalRotation; }
            set { _horizontalRotation = value%MathHelper.TwoPi; }
        }

        /// <summary>
        /// Rotation of camera around horizontal axis (X)
        /// </summary>
        public float VerticalRotation
        {
            get { return _verticalRotation; }
            set { _verticalRotation = MathHelper.Clamp(value, VerticalRotationMin, VerticalRotationMax); }
        }

        /// <summary>
        /// Override this method to catch input events and act on the camera
        /// </summary>
        /// <param name = "input">The current input instance</param>
        protected override void UpdateInput(InputManager input)
        {
        }

        protected override Matrix UpdateViewMatrix(GameTime gameTime)
        {
            Vector3 cameraPosition = Vector3.Multiply(Vector3.Up, Radius);
            cameraPosition = Vector3.Transform(cameraPosition, Matrix.CreateRotationX(_verticalRotation));
            cameraPosition = Vector3.Transform(cameraPosition, Matrix.CreateRotationY(_horizontalRotation));
            Position = TargetPosition + cameraPosition;
            return Matrix.CreateLookAt(Position, TargetPosition, Vector3.Up);
        }
    }
}