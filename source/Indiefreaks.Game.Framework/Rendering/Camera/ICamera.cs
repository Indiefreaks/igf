using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Camera
{
    public interface ICamera
    {
        /// <summary>
        ///   Returns the SunBurn SceneState associated with this camera
        /// </summary>
        SceneState SceneState { get; }

        /// <summary>
        /// Returns the SunBurn SceneEnvironment associated with this camera
        /// </summary>
        SceneEnvironment SceneEnvironment { get; set; }

        /// <summary>
        ///   Gets or sets the aspect ratio used to calculate the Projection matrix
        /// </summary>
        float AspectRatio { get; set; }

        void Update(GameTime gameTime);

        /// <summary>
        /// Initializes the Camera and SunBurn SceneState to be used for rendering
        /// </summary>
        /// <param name="gameTime"/>
        /// <param name="frameBuffers"/>
        void BeginFrameRendering(GameTime gameTime, FrameBuffers frameBuffers);

        /// <summary>
        /// Finalizes Camera and SunBurn SceneState for this frame
        /// </summary>
        void EndFrameRendering();
    }
}