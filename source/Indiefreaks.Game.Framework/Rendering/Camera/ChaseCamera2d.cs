using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.Camera
{
    public class ChaseCamera2D : Camera2D
    {
        public ChaseCamera2D(float aspectRatio) : base(aspectRatio)
        {
        }

        public ISceneEntity TargetEntity { get; set; }

        #region Overrides of Camera2D

        /// <summary>
        ///   Override this method to catch input events and act on the camera
        /// </summary>
        /// <param name = "input">The current input instance</param>
        protected override void UpdateInput(InputManager input)
        {
            var position3D = TargetEntity.World.Translation;
            Position = new Vector2(position3D.X, position3D.Y);
        }

        #endregion
    }
}