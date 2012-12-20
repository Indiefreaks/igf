using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using Indiefreaks.Xna.Rendering.Instancing.Skinned;

namespace FatBatPipeline
{
    public class InstancedSkinningDataContent
    {
        #region Fields
        private IDictionary<string, InstancedAnimationClip> animations;
        private TextureContent texture;


        #endregion


        /// <summary>
        /// Constructs a new skinning data object.
        /// </summary>
        public InstancedSkinningDataContent(IDictionary<string, InstancedAnimationClip> animationClips, TextureContent animationTexture)
        {
            this.animations = animationClips;
            this.texture = animationTexture;
        }

        public void Write(ContentWriter writer)
        {
            writer.WriteObject(this.texture);
            writer.WriteObject(this.animations);
        }
    }
}
