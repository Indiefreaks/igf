using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Instancing.Skinned
{
    /// <summary>
    /// InstancedSkinnedModel is a wrapper over the Model class to add functionality
    /// for skinned instancing
    /// </summary>
    public class InstancedSkinnedModel : ISkinnedInstanceSource
    {
        private Model _model;
        private InstancedSkinningData _instancedSkinningData;
        private GraphicsDevice _graphicsDevice;
        private Matrix[] _boneArray;

        ///// <summary>
        ///// Gets the dictionary of animations for this model
        ///// </summary>
        //public IDictionary<string, InstancedAnimationClip> Animations
        //{
        //    get
        //    {
        //        return _instancedSkinningData.Animations;
        //    }
        //}

        public Model Model
        {
            get { return _model; }
        }

        /// <summary>
        /// Gets the skinning data for this model
        /// </summary>
        public InstancedSkinningData InstancedSkinningData
        {
            get { return this._instancedSkinningData; }
        }

        public InstancedSkinnedModel(ContentReader reader)
        {
            _model = reader.ReadObject<Model>();
            _instancedSkinningData = new InstancedSkinningData(reader);

            _graphicsDevice = ((IGraphicsDeviceService)reader.ContentManager.ServiceProvider.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;
        }
    }
}
