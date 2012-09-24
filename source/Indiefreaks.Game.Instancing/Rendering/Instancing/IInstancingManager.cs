using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Instancing
{
    public interface IInstancingManager : IManagerService
    {
        /// <summary>
        /// Creates an InstanceFactory used to create new InstanceEntity instances.
        /// </summary>
        /// <param name="source">The mesh data information used to create InstanceEntity instances.</param>
        /// <param name="shader">The shader shared accross all instances</param>
        /// <returns></returns>
        InstanceFactory CreateInstanceFactory(IInstanceSource source, Effect shader);
    }
}