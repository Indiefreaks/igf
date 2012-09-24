using System;
using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Camera
{
    /// <summary>
    ///   The CameraManagerService is responsible of the Cameras used in the game
    /// </summary>
    public class CameraManager : ICameraManager
    {
        private readonly IManagerServiceProvider _sceneInterface;
        /// <summary>
        ///   Creates a new CameraManager instance
        /// </summary>
        public CameraManager(IManagerServiceProvider sceneInterface)
        {
            _sceneInterface = sceneInterface;
            
            // We reset the CameraManager
            Clear();

            ManagerProcessOrder = 60;
        }

        #region Implementation of IUnloadable

        /// <summary>
        ///   Disposes any graphics resource used internally by this object, and removes
        ///   scene resources managed by this object. Commonly used during Game.UnloadContent.
        /// </summary>
        public void Unload()
        {
        }

        #endregion

        #region Implementation of IManager

        /// <summary>
        ///   Use to apply user quality and performance preferences to the resources managed by this object.
        /// </summary>
        /// <param name = "preferences" />
        public void ApplyPreferences(ISystemPreferences preferences)
        {
        }

        /// <summary>
        ///   Removes resources managed by this object. Commonly used while clearing the scene.
        /// </summary>
        public void Clear()
        {
            // We submit a default camera so that it never tries to render without one
            Submit(new DefaultCamera(GraphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, 1.0f, 0.1f, 1000f));
        }

        public IManagerServiceProvider OwnerSceneInterface
        {
            get { return _sceneInterface; }
        }

        /// <summary>
        ///   Updates the object and its contained resources.
        /// </summary>
        /// <param name = "gameTime" />
        public void Update(GameTime gameTime)
        {
            ((ICamera)ActiveCamera).Update(gameTime);
        }

        #endregion

        #region Implementation of IManagerService

        /// <summary>
        ///   Gets the manager specific Type used as a unique key for storing and
        ///   requesting the manager from the IManagerServiceProvider.
        /// </summary>
        public Type ManagerType
        {
            get { return typeof(ICameraManager); }
        }

        /// <summary>
        ///   Sets the order this manager is processed relative to other managers
        ///   in the IManagerServiceProvider. Managers with lower processing order
        ///   values are processed first.
        ///   In the case of BeginFrameRendering and EndFrameRendering, BeginFrameRendering
        ///   is processed in the normal order (lowest order value to highest), however
        ///   EndFrameRendering is processed in reverse order (highest to lowest) to ensure
        ///   the first manager begun is the last one ended (FILO).
        /// </summary>
        public int ManagerProcessOrder { get; set; }

        #endregion

        /// <summary>
        ///   Returns the current GraphicsDeviceManager instance
        /// </summary>
        public IGraphicsDeviceService GraphicsDeviceManager { get { return SunBurnCoreSystem.Instance.GraphicsDeviceManager; } }

        /// <summary>
        ///   Returns the currently active camera
        /// </summary>
        public ICamera ActiveCamera { get; private set; }

        /// <summary>
        ///   Registers a camera as the ActiveCamera
        /// </summary>
        /// <param name = "camera">The camera to be registered</param>
        /// <remarks>
        ///   The submitted and therefore active camera is getting updated each frame
        /// </remarks>
        public void Submit(ICamera camera)
        {
            ActiveCamera = camera;
        }

        #region Nested type: DefaultCamera

        /// <summary>
        ///   Internal camera used solely to avoid rendering without one. See CameraManager.Clear()
        /// </summary>
        internal class DefaultCamera : Camera3D
        {
            /// <summary>
            ///   Creates a new instance of the DefaultCamera
            /// </summary>
            public DefaultCamera(float aspectRatio, float fieldOfView, float nearPlaneDistance, float farPlaneDistance)
                : base(aspectRatio, fieldOfView, nearPlaneDistance, farPlaneDistance)
            {
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
            /// <param name = "gameTime"/>
            protected override Matrix UpdateViewMatrix(GameTime gameTime)
            {
                return Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
            }

            #endregion
        }

        #endregion
    }
}