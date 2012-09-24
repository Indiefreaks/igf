using System.Collections.ObjectModel;
using ProjectMercury.Renderers;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Particles
{
    public interface IParticleSystemManager : IManagerService, IRenderableManager
    {
        AbstractRenderer Renderer { get; }

        /// <summary>
        /// Returns the current list of ParticleSystems
        /// </summary>
        ReadOnlyCollection<ParticleSystem> ParticleSystems { get; }

        ParticleSystem FindParticleSystemByName(string name);
        ParticleSystem FindParticleSystemByIndex(int index);
        int GetParticleSystemIndex(ParticleSystem particleSystem);

        /// <summary>
        /// Submits a new ParticleSystem instance to the current ParticleSystems list
        /// </summary>
        /// <param name="particleSystem"></param>
        void SubmitParticleSystem(ParticleSystem particleSystem);

        /// <summary>
        /// Removes a given ParticleSystem instance from the ParticleSystems list
        /// </summary>
        /// <param name="particleSystem"></param>
        void Remove(ParticleSystem particleSystem);
    }
}