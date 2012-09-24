using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.Instancing
{
    /// <summary>
    ///   Represents one single instance of a mesh.
    /// </summary>
    /// <remarks>
    ///   The World property can be used to move the instance. All other SunBurn properties aren't used
    /// </remarks>
    public class InstanceEntity : SceneEntity
    {
        /// <summary>
        ///   Creates a new instance of the InstanceEntity
        /// </summary>
        /// <param name = "name">The name of the instance</param>
        /// <param name = "index">The index used in the SceneObject.SkinBones array</param>
        /// <param name = "sceneObject">The SceneObject this instance pertains to</param>
        /// <param name = "transform">The matrix used to place the instance in the world</param>
        protected internal InstanceEntity(string name, int index, SceneObject sceneObject, Matrix transform)
            : base(name, false)
        {
            Index = index;
            Parent = sceneObject;
            World = transform;
        }

        /// <summary>
        ///   Returns the index used for rendering in the SceneObject.SkinBones array
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        ///   Returns the SceneObject this instance is attached to
        /// </summary>
        public SceneObject Parent { get; private set; }

        /// <summary>
        ///   Gets or sets the World position of the instance
        /// </summary>
        public new Matrix World
        {
            get { return base.World; }
            set
            {
                if (base.World == value) return;
                base.World = value;
                Parent.SkinBones[Index] = value;
            }
        }
    }
}