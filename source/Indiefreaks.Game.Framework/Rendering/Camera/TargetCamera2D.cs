using System;
using Indiefreaks.Xna.Input;

namespace Indiefreaks.Xna.Rendering.Camera
{
    public class TargetCamera2D : Camera2D
    {
        public TargetCamera2D(float aspectRatio) : base(aspectRatio)
        {
        }

        #region Overrides of Camera2D

        /// <summary>
        ///   Override this method to catch input events and act on the camera
        /// </summary>
        /// <param name = "input">The current input instance</param>
        protected override void UpdateInput(InputManager input)
        {
        }

        #endregion
    }
}