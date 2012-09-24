using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Camera
{
    public abstract class Camera2D : ICamera
    {
        /// <summary>
        ///   Creates a new Camera instance
        /// </summary>
        /// <param name = "aspectRatio">The viewport aspect ratio</param>
        protected Camera2D(float aspectRatio)
        {
            AspectRatio = aspectRatio;
            Distance = 1.0f;
            
            SceneState = new SceneState();
            SceneEnvironment = new SceneEnvironment();
        }

        #region Implementation of ICamera

        /// <summary>
        ///   Returns the SunBurn SceneState associated with this camera
        /// </summary>
        public SceneState SceneState { get; private set; }

        /// <summary>
        /// Returns the SunBurn SceneEnvironment associated with this camera
        /// </summary>
        public SceneEnvironment SceneEnvironment { get; set; }

        void ICamera.Update(GameTime gameTime)
        {
            UpdateInput(Application.Input);
        }

        /// <summary>
        /// Initializes the Camera and SunBurn SceneState to be used for rendering
        /// </summary>
        /// <param name="gameTime"/>
        /// <param name="frameBuffers"/>
        public void BeginFrameRendering(GameTime gameTime, FrameBuffers frameBuffers)
        {
            SceneState.BeginFrameRendering(Position, Distance, AspectRatio, gameTime, SceneEnvironment, frameBuffers,
                                           true);
        }

        /// <summary>
        /// Finalizes Camera and SunBurn SceneState for this frame
        /// </summary>
        public void EndFrameRendering()
        {
            SceneState.EndFrameRendering();
        }

        #endregion

        /// <summary>
        ///   Gets or sets the position of the camera
        /// </summary>
        public Vector2 Position { get; set; }

        private float _distance;

        /// <summary>
        /// Gets or sets the distance of the camera
        /// </summary>
        /// <remarks>Value must be greater than 0.</remarks>
        public float Distance
        {
            get { return _distance; }
            set
            {
                if (value < 0)
                    return;

                _distance = value;
            }
        }

        public float AspectRatio { get; set; }

        /// <summary>
        ///   Override this method to catch input events and act on the camera
        /// </summary>
        /// <param name = "input">The current input instance</param>
        protected abstract void UpdateInput(InputManager input);
    }
}