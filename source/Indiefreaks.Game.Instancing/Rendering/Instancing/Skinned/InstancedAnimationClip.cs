using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indiefreaks.Xna.Rendering.Instancing.Skinned
{
    /// <summary>
    /// For instancing, the animation doesn't need to hold all the keyframes. We've got those in our texture.
    /// However, we do need to store some data - like the duration of the animation, the start index into the texture,
    /// and the frame rate of the animation
    /// </summary>
    public class InstancedAnimationClip
    {
        private TimeSpan durationValue;
        private int startRow;
        private int frameRate;
        private int endRow;

        /// <summary>
        /// Gets the total length of the animation.
        /// </summary>
        public TimeSpan Duration
        {
            get { return durationValue; }
        }

        /// <summary>
        /// Gets the starting row of this animation in the animation texture
        /// </summary>
        public int StartRow
        {
            get { return this.startRow; }
        }

        /// <summary>
        /// Ges the ending row of this animation in the animation texture
        /// </summary>
        public int EndRow
        {
            get { return this.endRow; }
        }

        /// <summary>
        /// Gets the framerate of this animation
        /// </summary>
        public int FrameRate
        {
            get { return this.frameRate; }
        }

        /// <summary>
        /// Constructs a new animation clip object.
        /// </summary>
        public InstancedAnimationClip(TimeSpan duration, int animationStartRow, int animationEndRow, int animationFrameRate)
        {
            this.durationValue = duration;
            this.startRow = animationStartRow;
            this.endRow = animationEndRow;
            this.frameRate = animationFrameRate;
        }

    }
}
