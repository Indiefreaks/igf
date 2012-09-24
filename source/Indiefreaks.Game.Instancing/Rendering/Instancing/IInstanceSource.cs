using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Instancing
{
    /// <summary>
    ///   Common interface for providing instance mesh data
    /// </summary>
    public interface IInstanceSource
    {
        /// <summary>
        ///   Array of vertices describing the instance mesh.
        /// </summary>
        VertexPositionNormalTextureBump[] Vertices { get; }

        /// <summary>
        ///   Array of indices used to render the instance mesh.  Mesh primitives are assumed to be of the type TriangleList.
        /// </summary>
        short[] Indices { get; }
    }
}