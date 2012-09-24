using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury.Renderers;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Particles
{
    /// <summary>
    /// 
    /// </summary>
    public class ParticleSystemManager : IParticleSystemManager
    {
        private readonly List<ParticleSystem> _particleSystems = new List<ParticleSystem>();
        public AbstractRenderer Renderer { get; private set; }
        private Vector3 _cameraPosition = Vector3.Zero;
        private Matrix _projection = Matrix.Identity;
        private ISceneState _sceneState;
        private Matrix _view = Matrix.Identity;
        private Matrix _world = Matrix.Identity;
        private readonly SystemStatistic _activeParticles;
        private readonly IManagerServiceProvider _sceneInterface;

        public static float ElapsedSeconds;
        public static BoundingFrustum Frustum;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ParticleSystemManager(IManagerServiceProvider sceneInterface)
        {
            _sceneInterface = sceneInterface;

            ManagerProcessOrder = 100;

            Renderer = new QuadRenderer()
                            {
                                GraphicsDeviceService = GraphicsDeviceManager,
                            };

            Renderer.LoadContent(Application.ContentManager);

            _activeParticles = SystemConsole.GetStatistic("ParticleSystem_ActiveParticles", SystemStatisticCategory.Rendering);
        }

        /// <summary>
        /// Returns the current list of ParticleSystems
        /// </summary>
        public ReadOnlyCollection<ParticleSystem> ParticleSystems
        {
            get { return _particleSystems.AsReadOnly(); }
        }

        public ParticleSystem FindParticleSystemByName(string name)
        {
            return _particleSystems.First(predicate => predicate.Effect.Name == name);
        }

        public ParticleSystem FindParticleSystemByIndex(int index)
        {
            if (index < 0 || index > _particleSystems.Count - 1)
                return null;

            return _particleSystems[index];
        }

        public int GetParticleSystemIndex(ParticleSystem particleSystem)
        {
            return _particleSystems.IndexOf(particleSystem);
        }

        /// <summary>
        /// Submits a new ParticleSystem instance to the current ParticleSystems list
        /// </summary>
        /// <param name="particleSystem"></param>
        public void SubmitParticleSystem(ParticleSystem particleSystem)
        {
            if(!_particleSystems.Contains(particleSystem))
                _particleSystems.Add(particleSystem);
            
            Application.GameState.Content.Add(particleSystem);
        }

        /// <summary>
        /// Removes a given ParticleSystem instance from the ParticleSystems list
        /// </summary>
        /// <param name="particleSystem"></param>
        public void Remove(ParticleSystem particleSystem)
        {
            _particleSystems.Remove(particleSystem);
        }

        #region Implementation of IUnloadable

        /// <summary>
        /// Disposes any graphics resource used internally by this object, and removes
        ///             scene resources managed by this object. Commonly used during Game.UnloadContent.
        /// </summary>
        public void Unload()
        {
            Clear();
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
            _particleSystems.Clear();
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
        public Type ManagerType
        {
            get { return typeof (ParticleSystemManager); }
        }

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

        #region Implementation of IRenderableManager

        /// <summary>
        /// Sets up the object prior to rendering.
        /// </summary>
        /// <param name="scenestate"/>
        public virtual void BeginFrameRendering(ISceneState scenestate)
        {
            _sceneState = scenestate;

            ElapsedSeconds = (float) _sceneState.GameTime.ElapsedGameTime.TotalSeconds;
            Frustum = _sceneState.ViewFrustum;

            for (int i = 0; i < ParticleSystems.Count; i++)
            {
                ParticleSystem particleSystem = ParticleSystems[i];
                if (particleSystem.Effect != null)
                {
                    particleSystem.Effect.Update(ElapsedSeconds);

                    _activeParticles.AccumulationValue += particleSystem.Effect.ActiveParticlesCount;
                }
            }
        }

        /// <summary>
        /// Finalizes rendering.
        /// </summary>
        public void EndFrameRendering()
        {
            _view = _sceneState.View;
            _projection = _sceneState.Projection;
            _cameraPosition = _sceneState.ViewToWorld.Translation;

            for (int i = 0; i < ParticleSystems.Count; i++)
            {
                ParticleSystem particleSystem = ParticleSystems[i];
                if (particleSystem.Effect != null)
                    Renderer.RenderEffect(particleSystem.Effect, ref _world, ref _view, ref _projection, ref _cameraPosition);
            }
        }

        /// <summary>
        /// The current GraphicsDeviceManager used by this object.
        /// </summary>
        public IGraphicsDeviceService GraphicsDeviceManager { get { return SunBurnCoreSystem.Instance.GraphicsDeviceManager; } }

        #endregion
    }
}