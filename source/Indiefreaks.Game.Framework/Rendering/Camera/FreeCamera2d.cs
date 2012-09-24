using System;
using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Rendering.Camera
{
    public class FreeCamera2D : Camera2D
    {
        public float Speed { get; set; }

        public FreeCamera2D(float aspectRatio) : base(aspectRatio)
        {
            Speed = .3f;
        }

        #region Overrides of Camera2D

        /// <summary>
        ///   Override this method to catch input events and act on the camera
        /// </summary>
        /// <param name = "input">The current input instance</param>
        protected override void UpdateInput(InputManager input)
        {
            var player = input.PlayerOne;

            Position += new Vector2(-player.ThumbSticks.LeftStick.X * Speed, player.ThumbSticks.LeftStick.Y * Speed);
            Distance += -player.ThumbSticks.RightStick.Y*Speed;
        }

        #endregion
    }
}