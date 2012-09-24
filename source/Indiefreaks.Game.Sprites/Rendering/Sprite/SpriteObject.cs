using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Collision;
using SynapseGaming.LightingSystem.Rendering;
using SynapseGaming.LightingSystem.Effects;

namespace Indiefreaks.Xna.Rendering.Sprite
{
    /// <summary>
    /// The SpriteObject class stores a Material that is rendered with a SpriteContainer and reacts to CollisionManager events
    /// </summary>
    public class SpriteObject : SceneObject, IContentHost
    {
        private Vector2 _position;
        private float _rotation;
        private Vector2 _size;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public SpriteObject()
            //: base(Application.GeometryResources.Load<Model>("NullObject"), "NullObject")
            : base("", false)
        {
            Size = Vector2.One;
            UVSize = Vector2.One;
            AffectedByGravity = false;
        }

        /// <summary>
        /// Returns the path used to load the Material
        /// </summary>
        public string MaterialPath { get; internal set; }

        /// <summary>
        /// Returns the SpriteContainer instance used to render this SpriteObject
        /// </summary>
        public SpriteContainer SpriteContainer { get; internal set; }

        /// <summary>
        /// Gets or sets the Material used to render the Sprite
        /// </summary>
        public Effect Material { get; set; }

        /// <summary>
        /// Gets or sets the Size of this Sprite using World coordinates
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set
            {
                World = Matrix.CreateScale(value.X, value.Y, 1)*Matrix.CreateRotationZ(_rotation)*
                        Matrix.CreateTranslation(World.Translation);
                _size = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the Sprite using World coordinates (0,0) is the World center
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                Matrix tempMatrix = World;
                tempMatrix.M41 = value.X;
                tempMatrix.M42 = value.Y;
                World = tempMatrix;
                _position = value;
            }
        }

        /// <summary>
        /// Gets or sets the current SpriteObject rotation angle expressed in radians
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                float angle = MathHelper.WrapAngle(value);
                World = Matrix.CreateScale(_size.X, _size.Y, 1)*Matrix.CreateRotationZ(angle)*
                        Matrix.CreateTranslation(World.Translation);
                _rotation = angle;
            }
        }

        /// <summary>
        /// Gets or sets the Origin of the SpriteObject
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Gets or sets the UV size
        /// </summary>
        public Vector2 UVSize { get; set; }

        /// <summary>
        /// Gets or sets the UV position
        /// </summary>
        public Vector2 UVPosition { get; set; }

        /// <summary>
        /// Gets or sets the Depth used to sort this SpriteObject while rendering
        /// </summary>
        public float LayerDepth { get; set; }

        /// <summary>
        /// Calculates the object bounds.
        /// </summary>
        /// <param name="objectboundingbox">Object bounds to update.</param><param name="objectboundingsphere">Object bounds to update.</param>
        protected override void CalculateObjectBounds(ref BoundingBox objectboundingbox,
                                                      ref BoundingSphere objectboundingsphere)
        {
            base.CalculateObjectBounds(ref objectboundingbox, ref objectboundingsphere);

            objectboundingbox = new BoundingBox(new Vector3(-0.5f, -0.5f, -0.5f),
                                                new Vector3(0.5f, 0.5f, 0.5f));
            objectboundingsphere = BoundingSphere.CreateFromBoundingBox(objectboundingbox);
        }

        /// <summary>
        /// Updates the object world space and inverse world space transforms.
        ///             Override to perform custom code when the world transform changes.
        /// </summary>
        /// <param name="world">World space transform.</param><param name="worldtoobj">Inverse world space transform.</param>
        protected override void UpdateWorldAndWorldToObject(ref Matrix world, ref Matrix worldtoobj)
        {
            base.UpdateWorldAndWorldToObject(ref world, ref worldtoobj);

            _position.X = world.M41;
            _position.Y = world.M42;
        }

        #region Implementation of IContentHost

        /// <summary>
        ///   Load all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        /// <param name = "manager">XNA content manage</param>
        public virtual void LoadContent(IContentCatalogue catalogue, ContentManager manager)
        {
            if (!string.IsNullOrEmpty(MaterialPath))
            {
                Material = manager.Load<Effect>(MaterialPath);
                DefaultCollisionMaterial = new DefaultCollisionMaterial();
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