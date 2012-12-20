using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace FatBatPipeline
{
    /// <summary>
    /// InstancedSkinnedModelContent is a wrapper around ModelContent
    /// Basically, we want to store some extra data, in addition to the Model
    /// </summary>
    public class InstancedSkinnedModelContent
    {
        private ModelContent modelContent;
        private InstancedSkinningDataContent instancedSkinningData;

        public InstancedSkinnedModelContent(ModelContent model,
            InstancedSkinningDataContent instancedSkinningInfo)
        {
            this.modelContent = model;
            this.instancedSkinningData = instancedSkinningInfo;
        }

        public void Write(ContentWriter writer)
        {
            writer.WriteObject<ModelContent>(this.modelContent);
            this.instancedSkinningData.Write(writer);
        }
    }
}
