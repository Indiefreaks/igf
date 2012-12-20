using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Rendering;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Instancing.Skinned
{
    /// <summary>
    ///   Represents one single instance of a mesh.
    /// </summary>
    /// <remarks>
    ///   The World property can be used to move the instance. All other SunBurn properties aren't used
    /// </remarks>
    public class SkinnedInstanceEntity : SceneEntity
    {
        private InstancedAnimationClip _currentAnimation;
        private bool _animationPlaying = false;
        private bool _repeatAnimation = false;
        private float _currentFrame;

        /// <summary>
        ///   Creates a new instance of the InstanceEntity
        /// </summary>
        /// <param name = "name">The name of the instance</param>
        /// <param name = "index">The index used in the SceneObject.SkinBones array</param>
        /// <param name = "sceneObject">The SceneObject this instance pertains to</param>
        /// <param name = "transform">The matrix used to place the instance in the world</param>
        protected internal SkinnedInstanceEntity(string name, int index, InstancedSkinnedSceneObject sceneObject, Matrix transform)
            : base(name, false)
        {
            Index = index;
            Parent = sceneObject;
            World = transform;
            UpdateType = UpdateType.Automatic;
        }

        /// <summary>
        ///   Returns the index used for rendering in the SceneObject.SkinBones array
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        ///   Returns the SceneObject this instance is attached to
        /// </summary>
        public InstancedSkinnedSceneObject Parent { get; private set; }

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
                Parent.InstanceTransforms[Index] = value;
            }
        }

        public override void Update(GameTime gametime)
        {
            if (_currentAnimation != null && _animationPlaying) UpdateAnimation(gametime);
            base.Update(gametime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            _currentFrame += (float)(gameTime.ElapsedGameTime.TotalSeconds * _currentAnimation.FrameRate);
            if ((int)_currentFrame >= _currentAnimation.EndRow)
            {
                if (_repeatAnimation)
                {
                    _currentFrame = _currentAnimation.StartRow;
                }
                else
                {
                    StopAnimation();
                }
            }
            Parent.InstanceAnimationFrames[Index] = (int) _currentFrame;
        }

        public void PlayAnimation(string name, bool looped)
        {
            _currentAnimation = Parent.AnimationClips[name];
            _repeatAnimation = looped;
            _animationPlaying = true;
            _currentFrame = _currentAnimation.StartRow;
        }

        public void StopAnimation()
        {
            _currentFrame = _currentAnimation.EndRow;
            _currentAnimation = null;
            _animationPlaying = false;
            _repeatAnimation = false;
        }
    }
}
