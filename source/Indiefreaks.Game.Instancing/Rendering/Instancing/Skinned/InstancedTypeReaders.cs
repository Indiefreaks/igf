using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Indiefreaks.Xna.Rendering.Instancing.Skinned
{
    /// <summary>
    /// Content pipeline support class for loading InstancedModel objects.
    /// </summary>
    public class InstancedModelReader : ContentTypeReader<InstancedSkinnedModel>
    {
        /// <summary>
        /// Reads instanced model data from an XNB file.
        /// </summary>
        protected override InstancedSkinnedModel Read(ContentReader input,
                                               InstancedSkinnedModel existingInstance)
        {
            return new InstancedSkinnedModel(input);
        }
    }

    /// <summary>
    /// Loads AnimationClip objects from compiled XNB format.
    /// </summary>
    public class InstancedAnimationClipReader : ContentTypeReader<InstancedAnimationClip>
    {
        protected override InstancedAnimationClip Read(ContentReader input,
                                              InstancedAnimationClip existingInstance)
        {
            TimeSpan duration = input.ReadObject<TimeSpan>();
            int startRow = input.ReadObject<int>();
            int endRow = input.ReadObject<int>();
            int frameRate = input.ReadObject<int>();

            return new InstancedAnimationClip(duration, startRow, endRow, frameRate);
        }
    }
}
