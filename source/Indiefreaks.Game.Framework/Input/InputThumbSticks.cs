using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Input
{
    /// <summary>
    /// Structure storing the current state of a gamepad's thumbsticks
    /// </summary>
    public struct InputThumbSticks
    {
        internal Vector2 LeftStickVector;
        internal Vector2 RightStickVector;

        /// <summary></summary>
        public Vector2 LeftStick
        {
            get { return LeftStickVector; }
        }

        /// <summary></summary>
        public Vector2 RightStick
        {
            get { return RightStickVector; }
        }
    }
}