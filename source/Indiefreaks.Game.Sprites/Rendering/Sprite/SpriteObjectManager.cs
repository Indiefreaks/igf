using System.Collections.Generic;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.Sprite
{
    /// <summary>
    /// The SpriteObjectManager replaces the SunBurn SpriteManager to enable SpriteObject rendering in your game
    /// </summary>
    public class SpriteObjectManager : SpriteManager, IRenderableManager
    {
        private readonly Dictionary<SpriteContainer, List<SpriteObject>> _containers =
            new Dictionary<SpriteContainer, List<SpriteObject>>();
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public SpriteObjectManager(IManagerServiceProvider sceneInterface) : base(sceneInterface)
        {
        }

        /// <summary>
        /// Creates a new SpriteObject instance
        /// </summary>
        /// <typeparam name="T">Used for custom SpriteObject inherited classes</typeparam>
        /// <param name="name">The name of the SpriteObject</param>
        /// <param name="materialPath">The path to the material you want to load with the SpriteObject</param>
        /// <param name="spriteContainer">The SpriteContainer to associate the SpriteObject with</param>
        /// <returns>returns a new SpriteObject instance</returns>
        public T CreateSpriteObject<T>(string name, string materialPath, SpriteContainer spriteContainer)
            where T : SpriteObject, new()
        {
            var sprite = new T
                             {
                                 Name = name,
                                 MaterialPath = materialPath,
                                 SpriteContainer = spriteContainer,
                                 UpdateType = UpdateType.Automatic
                             };

            if (!_containers.ContainsKey(spriteContainer))
            {
                spriteContainer.AffectedByGravity = false;
                _containers.Add(spriteContainer, new List<SpriteObject>());
                Application.SunBurn.ObjectManager.Submit(spriteContainer);
            }

            _containers[spriteContainer].Add(sprite);

            return sprite;
        }

        #region Implementation of IRenderableManager

        /// <summary>
        /// Sets up the object prior to rendering.
        /// </summary>
        /// <param name="scenestate"/>
        public void BeginFrameRendering(ISceneState scenestate)
        {
            foreach (SpriteContainer container in _containers.Keys)
            {
                if (container.RenderableMeshes.Count == _containers[container].Count &&
                    container.UpdateType == UpdateType.None)
                    continue;

                container.Begin();

                foreach (SpriteObject sprite in _containers[container])
                {
                    if (sprite.UpdateType == UpdateType.Automatic)
                    {
                        if (sprite.Material == null) continue;

                        container.Add(sprite.Material, sprite.Size, sprite.Position, sprite.Rotation, sprite.Origin,
                                      sprite.UVSize, sprite.UVPosition, sprite.LayerDepth);
                    }
                }

                container.End();
            }
        }

        /// <summary>
        /// Finalizes rendering.
        /// </summary>
        public void EndFrameRendering()
        {
        }

        /// <summary>
        /// The current GraphicsDeviceManager used by this object.
        /// </summary>
        public IGraphicsDeviceService GraphicsDeviceManager
        {
            get { return SunBurnCoreSystem.Instance.GraphicsDeviceManager; }
        }

        #endregion
    }
}