using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Instancing
{
    /// <summary>
    /// The InstancingManager allows developers to create Hardware instances of a given mesh using a simple approach.
    /// </summary>
    public class InstancingManager : IInstancingManager
    {
        private readonly List<InstanceFactory> _instanceFactories = new List<InstanceFactory>();
        private readonly IManagerServiceProvider _sceneInterface;
     
        public InstancingManager(IManagerServiceProvider sceneInterface)
        {
            _sceneInterface = sceneInterface;
        }

        #region Implementation of IUnloadable

        /// <summary>
        /// Disposes any graphics resource used internally by this object, and removes
        ///             scene resources managed by this object. Commonly used during Game.UnloadContent.
        /// </summary>
        public void Unload()
        {
        }

        #endregion

        #region Implementation of IManager

        /// <summary>
        /// Use to apply user quality and performance preferences to the resources managed by this object.
        /// </summary>
        /// <param name="preferences"/>
        public void ApplyPreferences(ISystemPreferences preferences)
        {
        }

        /// <summary>
        /// Removes resources managed by this object. Commonly used while clearing the scene.
        /// </summary>
        public void Clear()
        {
        }

        public IManagerServiceProvider OwnerSceneInterface
        {
            get { return _sceneInterface; }
        }

        #endregion

        #region Implementation of IManagerService

        /// <summary>
        /// Gets the manager specific Type used as a unique key for storing and
        ///             requesting the manager from the IManagerServiceProvider.
        /// </summary>
        public Type ManagerType { get { return typeof (IInstancingManager); } }

        /// <summary>
        /// Sets the order this manager is processed relative to other managers
        ///             in the IManagerServiceProvider. Managers with lower processing order
        ///             values are processed first.
        ///             In the case of BeginFrameRendering and EndFrameRendering, BeginFrameRendering
        ///             is processed in the normal order (lowest order value to highest), however
        ///             EndFrameRendering is processed in reverse order (highest to lowest) to ensure
        ///             the first manager begun is the last one ended (FILO).
        /// </summary>
        public int ManagerProcessOrder { get; set; }

        #endregion

        /// <summary>
        /// Creates an InstanceFactory used to create new InstanceEntity instances.
        /// </summary>
        /// <param name="source">The mesh data information used to create InstanceEntity instances.</param>
        /// <param name="shader">The shader shared accross all instances</param>
        /// <returns></returns>
        public InstanceFactory CreateInstanceFactory(IInstanceSource source, Effect shader)
        {
            var instanceFactory = new InstanceFactory(SunBurnCoreSystem.Instance.GraphicsDeviceManager.GraphicsDevice, source, shader);
            _instanceFactories.Add(instanceFactory);

            return instanceFactory;
        }
    }
}