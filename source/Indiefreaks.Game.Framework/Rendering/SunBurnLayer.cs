using System;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering
{
    /// <summary>
    ///   Specialized Layer which Loads, Updates and Renders SunBurn SceneEntity and SceneObject instances
    /// </summary>
    public class SunBurnLayer : Layer, IContentHost
    {
        private readonly string _contentRepositoryPath;
        private readonly string _scenePath;
        
        /// <summary>
        ///   Creates a new instance of the SunBurnLayer
        /// </summary>
        public SunBurnLayer(GameState gameState)
            : base(gameState)
        {
        }

        /// <summary>
        ///   Creates a new instance of the SunBurnLayer with a ContentRepository and a Scene
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name = "repositoryPath">The path to the ContentRepository file in the Content project</param>
        /// <param name = "scenePath">The path to the SunBurn Scene file in the Content project</param>
        public SunBurnLayer(GameState gameState, string repositoryPath, string scenePath) : this(gameState)
        {
            _contentRepositoryPath = repositoryPath;
            _scenePath = scenePath;
        }

        /// <summary>
        ///   Returns the SunBurn ContentRepository
        /// </summary>
        public ContentRepository Repository { get; protected set; }

        /// <summary>
        ///   Returns the SunBurn Scene
        /// </summary>
        public Scene Scene { get; protected set; }

        /// <summary>
        ///   Draws a frame
        /// </summary>
        /// <param name = "gameTime" />
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameState.SunBurn.RenderManager.Render();
        }

        #region Implementation of IContentHost

        /// <summary>
        ///   Load all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        /// <param name = "manager">XNA content manage</param>
        public virtual void LoadContent(IContentCatalogue catalogue, ContentManager manager)
        {
            if (!string.IsNullOrEmpty(_contentRepositoryPath))
            {
                Repository = manager.Load<ContentRepository>(_contentRepositoryPath);

                if (!string.IsNullOrEmpty(_scenePath))
                {
                    Scene = Repository.Load<Scene>(_scenePath);
                }
                GameState.SunBurn.Submit(Scene);
            }
        }

        /// <summary>
        ///   Unload all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        public virtual void UnloadContent(IContentCatalogue catalogue)
        {
        }

        #endregion
    }
}