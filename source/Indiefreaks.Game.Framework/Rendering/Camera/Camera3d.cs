using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Camera
{
    /// <summary>
    ///   Abstract 3d camera
    /// </summary>
    /// <remarks>
    ///   Override UpdateInput and UpdateViewMatrix to create your own custom camera
    /// </remarks>
    public abstract class Camera3D : ICamera
    {
        private float _aspectRatio;
        private float _farPlaneDistance;
        private float _fieldOfView;
        private bool _isProjectionDirty;
        private float _nearPlaneDistance;

        /// <summary>
        ///   Creates a new Camera instance
        /// </summary>
        /// <param name = "aspectRatio">The viewport aspect ratio</param>
        /// <param name = "fieldOfView">The _instances of view expressed in radians</param>
        /// <param name = "nearPlaneDistance">The nearest point in projected space of the camera</param>
        /// <param name = "farPlaneDistance">The farest point in projected space of the camera</param>
        protected Camera3D(float aspectRatio, float fieldOfView, float nearPlaneDistance,
                         float farPlaneDistance)
        {
            _aspectRatio = aspectRatio;
            _fieldOfView = fieldOfView;
            _nearPlaneDistance = nearPlaneDistance;
            _farPlaneDistance = farPlaneDistance;

            _isProjectionDirty = true;

            SceneState = new SceneState();
            SceneEnvironment = new SceneEnvironment();
        }

        /// <summary>
        ///   Returns the SunBurn SceneState associated with this camera
        /// </summary>
        public SceneState SceneState { get; private set; }

        /// <summary>
        /// Returns the SunBurn SceneEnvironment associated with this camera
        /// </summary>
        public SceneEnvironment SceneEnvironment { get; set; }

        /// <summary>
        ///   Gets or sets the Vector3 serving as the target position
        /// </summary>
        public Vector3 TargetPosition { get; set; }

        /// <summary>
        ///   Returns the View matrix of the camera
        /// </summary>
        public Matrix ViewMatrix { get; private set; }

        /// <summary>
        ///   Returns the Projection matrix of the camera
        /// </summary>
        public Matrix ProjectionMatrix { get; private set; }

        /// <summary>
        ///   Gets or sets the _instances of view used to calculate the Projection matrix
        /// </summary>
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set
            {
                if (_fieldOfView != value)
                {
                    _fieldOfView = value;
                    _isProjectionDirty = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the aspect ratio used to calculate the Projection matrix
        /// </summary>
        public float AspectRatio
        {
            get { return _aspectRatio; }
            set
            {
                if (_aspectRatio != value)
                {
                    _aspectRatio = value;
                    _isProjectionDirty = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the near plane distance used to calculate the Projection matrix
        /// </summary>
        public float NearPlaneDistance
        {
            get { return _nearPlaneDistance; }
            set
            {
                if (_nearPlaneDistance != value)
                {
                    _nearPlaneDistance = value;
                    _isProjectionDirty = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the far plane distance used to calculate the Projection matrix
        /// </summary>
        public float FarPlaneDistance
        {
            get { return _farPlaneDistance; }
            set
            {
                if (_farPlaneDistance != value)
                {
                    _farPlaneDistance = value;
                    _isProjectionDirty = true;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the position of the camera
        /// </summary>
        public Vector3 Position { get; set; }

        void ICamera.Update(GameTime gameTime)
        {
            UpdateInput(Application.Input);

            if (SceneEnvironment.VisibleDistance != _farPlaneDistance && !_isProjectionDirty)
                FarPlaneDistance = SceneEnvironment.VisibleDistance;
            else if (SceneEnvironment.VisibleDistance != _farPlaneDistance && _isProjectionDirty)
                SceneEnvironment.VisibleDistance = _farPlaneDistance;
            
            // if one of the projection matrix properties has changed, we update the Projection matrix
            if (_isProjectionDirty)
            {
                ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _nearPlaneDistance,
                                                                       _farPlaneDistance);
                _isProjectionDirty = false;
            }

            ViewMatrix = UpdateViewMatrix(gameTime);
        }

        /// <summary>
        ///   Override this method to catch input events and act on the camera
        /// </summary>
        /// <param name = "input">The current input instance</param>
        protected abstract void UpdateInput(InputManager input);

        /// <summary>
        ///   Override this method to update the ViewMatrix property
        /// </summary>
        /// <param name = "gameTime" />
        protected abstract Matrix UpdateViewMatrix(GameTime gameTime);

        /// <summary>
        /// Initializes the Camera and SunBurn SceneState to be used for rendering
        /// </summary>
        /// <param name="gameTime"/>
        /// <param name="frameBuffers"/>
        public void BeginFrameRendering(GameTime gameTime, FrameBuffers frameBuffers)
        {
            SceneState.BeginFrameRendering(ViewMatrix, ProjectionMatrix, gameTime, SceneEnvironment, frameBuffers, true);
        }

        /// <summary>
        /// Finalizes Camera and SunBurn SceneState for this frame
        /// </summary>
        public void EndFrameRendering()
        {
            SceneState.EndFrameRendering();
        }
    }
}