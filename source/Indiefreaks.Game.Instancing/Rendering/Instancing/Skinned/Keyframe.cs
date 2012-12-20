using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Rendering.Instancing.Skinned
{
    /// <summary>
    /// Describes the position of a single bone at a single point in time.
    /// This is copied directly from the SkinnedModel Sample at creators.xna.com
    /// </summary>
    public class Keyframe
    {
        #region Fields

        int boneValue;
        TimeSpan timeValue;
        Matrix transformValue;

        #endregion

        /// <summary>
        /// Constructs a new keyframe object.
        /// </summary>
        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            boneValue = bone;
            timeValue = time;
            transformValue = transform;
        }

        /// <summary>
        /// Gets the index of the target bone that is animated by this keyframe.
        /// </summary>
        public int Bone
        {
            get { return boneValue; }
        }


        /// <summary>
        /// Gets the time offset from the start of the animation to this keyframe.
        /// </summary>
        public TimeSpan Time
        {
            get { return timeValue; }
        }

        /// <summary>
        /// Gets the bone transform for this keyframe.
        /// </summary>
        public Matrix Transform
        {
            get { return transformValue; }
        }
    }
}
