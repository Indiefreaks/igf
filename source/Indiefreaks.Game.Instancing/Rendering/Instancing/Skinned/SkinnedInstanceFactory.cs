using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Effects.Deferred;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.Instancing.Skinned
{
    public class SkinnedInstanceFactory
    {
        private readonly List<SkinnedInstanceEntity> _entities = new List<SkinnedInstanceEntity>();
        private readonly List<InstancedSkinnedSceneObject> _sceneObjects = new List<InstancedSkinnedSceneObject>();
        private readonly ISkinnedInstanceSource _source;

        private GraphicsDevice _graphicsDevice;
        private int _instancesCount;
        private DeferredSasEffect _shader;

        private InstancedSkinnedSceneObject _currentSceneObject;

        protected internal SkinnedInstanceFactory(GraphicsDevice graphicsDevice, ISkinnedInstanceSource source, DeferredSasEffect shader)
        {
            _graphicsDevice = graphicsDevice;
            _source = source;
            _shader = shader;           
        }

        /// <summary>
        ///   Returns the number of instances created
        /// </summary>
        public int InstancesCount
        {
            get { return _instancesCount; }
        }

        public SkinnedInstanceEntity CreateInstance(string name, Matrix transform)
        {
            if (_currentSceneObject == null || _currentSceneObject.InstancesCount == _currentSceneObject.MaxInstances)
            {
                CreateSceneObject();
            }
            return _currentSceneObject.CreateInstance(name, transform);            
        }

        private void CreateSceneObject()
        {
            _currentSceneObject = new InstancedSkinnedSceneObject(_graphicsDevice, _source, _shader);
            _sceneObjects.Add(_currentSceneObject);
            SceneInterface.ActiveSceneInterface.ObjectManager.Submit(_currentSceneObject);
        }
    }
}
