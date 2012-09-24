using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Camera
{
    public interface ICameraManager : IUpdatableManager, IManagerService
    {
        /// <summary>
        ///   Returns the currently active camera
        /// </summary>
        ICamera ActiveCamera { get; }

        /// <summary>
        ///   Registers a camera as the ActiveCamera
        /// </summary>
        /// <param name = "camera">The camera to be registered</param>
        /// <remarks>
        ///   The submitted and therefore active camera is getting updated each frame
        /// </remarks>
        void Submit(ICamera camera);
    }
}