using System;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjectMercury;
using ProjectMercury.Emitters;
using SynapseGaming.LightingSystem.Editor;

namespace Indiefreaks.Xna.Rendering.Particles
{
#if WINDOWS
    [Serializable]
#endif
    [EditorCreatedObject]
    public class ParticleSystem : IContentHost
    {
        private readonly string _particleEffectFilePath = string.Empty;

        public ParticleSystem()
        {
        }

        public ParticleSystem(string particleEffectFilePath)
        {
            _particleEffectFilePath = particleEffectFilePath;
        }

        /// <summary>
        /// 
        /// </summary>
        public ParticleEffect Effect { get; set; }

        #region Implementation of IContentHost

        /// <summary>
        ///   Load all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        /// <param name = "manager">XNA content manage</param>
        public void LoadContent(IContentCatalogue catalogue, ContentManager manager)
        {
            if (!string.IsNullOrEmpty(_particleEffectFilePath))
            {
                Effect = manager.Load<ParticleEffect>(_particleEffectFilePath).DeepCopy();
            }

            if (Effect != null)
            {
                foreach (AbstractEmitter emitter in Effect.Emitters)
                {
                    emitter.ParticleTexture = manager.Load<Texture2D>(emitter.ParticleTextureAssetPath);
                    if (!emitter.Initialised)
                        emitter.Initialise();
                }
            }
        }

        /// <summary>
        ///   Unload all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        public void UnloadContent(IContentCatalogue catalogue)
        {
        }

        #endregion
    }
}