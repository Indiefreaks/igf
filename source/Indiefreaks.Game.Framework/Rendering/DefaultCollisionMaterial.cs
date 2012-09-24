using SynapseGaming.LightingSystem.Collision;

namespace Indiefreaks.Xna.Rendering
{
    public sealed class DefaultCollisionMaterial : ICollisionMaterial
    {
        public DefaultCollisionMaterial()
        {
            Elasticity = 0;
            Friction = 0;
        }
        #region Implementation of ICollisionMaterial

        public int CollisionId
        {
            get { return 0; }
        }

        /// <summary>
        /// Amount material absorbs impact force.
        /// </summary>
        public float Elasticity { get; set; }

        /// <summary>
        /// Amount material resists objects moving across its surface.
        /// </summary>
        public float Friction { get; set; }

        #endregion
    }
}