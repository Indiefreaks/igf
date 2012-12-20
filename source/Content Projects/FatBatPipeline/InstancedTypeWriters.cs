using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;

using Indiefreaks.Xna.Rendering.Instancing.Skinned;

namespace FatBatPipeline
{
    /// <summary>
    /// Writes the instanced skinned model
    /// </summary>
    [ContentTypeWriter]
    public class InstancedSkinnedModelWriter : ContentTypeWriter<InstancedSkinnedModelContent>
    {
        protected override void Write(ContentWriter output, InstancedSkinnedModelContent value)
        {
            value.Write(output);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(InstancedModelReader).AssemblyQualifiedName;
        }
    }

    /// <summary>
    /// Writes the instanced animation clip data
    /// </summary>
    [ContentTypeWriter]
    public class InstancedAnimationClipWriter : ContentTypeWriter<InstancedAnimationClip>
    {
        protected override void Write(ContentWriter output, InstancedAnimationClip value)
        {
            output.WriteObject(value.Duration);
            output.WriteObject(value.StartRow);
            output.WriteObject(value.EndRow);
            output.WriteObject(value.FrameRate);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(InstancedAnimationClipReader).AssemblyQualifiedName;
        }
    }


}
