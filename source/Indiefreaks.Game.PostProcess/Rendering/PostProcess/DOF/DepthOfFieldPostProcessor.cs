using System;
using Indiefreaks.Xna.Rendering.PostProcess.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.PostProcess.DOF
{
    /// <summary>
    /// Adds a Depth of field effect to the current rendered scene
    /// @Credits: Bamyazi (Slightly modified) Thanks for this ;-)
    /// </summary>
    public class DepthOfFieldPostProcessor : BaseRenderTargetPostProcessor
    {
        // Gaussian Blur 
        private readonly Effect _blurEffect;
        private readonly Effect _dofEffect;
        private float _blurAmount;
        private Vector2[] _blurHorizOffsets;
        private float[] _blurKernel;
        private int _blurRadius;
        private float _blurSigma;
        private Vector2[] _blurVertOffsets;
        private Viewport _viewport;

        // Dof Focus

        /// <summary>
        /// Creates a BaseRenderTargetPostProcessor instance.
        /// </summary>
        public DepthOfFieldPostProcessor()
        {
            FarClip = 140f;
            FocalWidth = 50f;
            FocalDistance = 60f;
            Attenuation = 0.5f;
            BlurRadius = 2;
            BlurAmount = 0.5f;

            _viewport = SunBurnCoreSystem.Instance.GraphicsDeviceManager.GraphicsDevice.Viewport;

#if WINDOWS
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, WindowsPostProcessResources.ResourceManager);
#else
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, Xbox360PostProcessResources.ResourceManager);
#endif

            _blurEffect = shaderResources.Load<Effect>("GaussianBlur");
            _dofEffect = shaderResources.Load<Effect>("DepthOfField");

            ComputeKernel(_blurRadius, _blurAmount);
            ComputeOffsets(_viewport.Width, _viewport.Height);
        }

        #region Overrides of BaseRenderTargetPostProcessor

        /// <summary>
        /// Use to apply user quality and performance preferences to the resources managed by this object.
        /// </summary>
        /// <param name="preferences"/>
        public override void ApplyPreferences(ISystemPreferences preferences)
        {
        }

        public override void EndFrameRendering()
        {
            GraphicsDevice graphicsDevice = GraphicsDeviceManager.GraphicsDevice;
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.None;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;

            // CREATE BUFFERS FOR BLUR 
            CustomFrameBufferCollection blurBuffers = SceneState.FrameBuffers.GetCustomFrameBufferCollection("DepthOfField", true);
            if (blurBuffers.Count == 0)
            {
                const SurfaceFormat surfaceFormat = SurfaceFormat.Color;
                blurBuffers.Add(new RenderTarget2D(graphicsDevice, _viewport.Width/2, _viewport.Height/2, false, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.PlatformContents));
                blurBuffers.Add(new RenderTarget2D(graphicsDevice, _viewport.Width/2, _viewport.Height/2, false, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.PlatformContents));
                graphicsDevice.SetRenderTarget(null);
            }

            // HORIZONTAL BLUR
            graphicsDevice.SetRenderTarget(blurBuffers[0]);
            _blurEffect.CurrentTechnique = _blurEffect.Techniques["GaussianBlur"];
            _blurEffect.Parameters["SceneTexture"].SetValue(ProcessorRenderTarget);
            _blurEffect.Parameters["weights"].SetValue(_blurKernel);
            _blurEffect.Parameters["offsets"].SetValue(_blurHorizOffsets);
            _blurEffect.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.SamplerStates.Reset();

            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);

            // VERTICAL BLUR
            graphicsDevice.SetRenderTarget(blurBuffers[1]);
            _blurEffect.Parameters["SceneTexture"].SetValue(blurBuffers[0]);
            _blurEffect.Parameters["offsets"].SetValue(_blurVertOffsets);
            _blurEffect.CurrentTechnique.Passes[0].Apply();
            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);

            // DOF DEPTH COMBINE
            RenderTarget2D depth = SceneState.FrameBuffers.GetBuffer(FrameBufferType.DeferredDepthAndSpecularPower, false);
            graphicsDevice.SetRenderTarget(PreviousRenderTarget);

            _dofEffect.CurrentTechnique = _dofEffect.Techniques["DofPostProcess"];

            _dofEffect.Parameters["FocalDistance"].SetValue(FocalDistance);
            _dofEffect.Parameters["FocalWidth"].SetValue(FocalWidth);
            _dofEffect.Parameters["FarClip"].SetValue(FarClip);

            _dofEffect.Parameters["Attenuation"].SetValue(Attenuation);

            _dofEffect.Parameters["BlurScene"].SetValue(blurBuffers[1]);
            _dofEffect.Parameters["DepthMap"].SetValue(depth);
            _dofEffect.Parameters["DiffuseMap"].SetValue(ProcessorRenderTarget);
            _dofEffect.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.SamplerStates.Reset();

            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);

            base.EndFrameRendering();
        }

        #endregion

        /// <summary>
        /// Gets or sets the attenuation applied to the effect.
        /// </summary>
        /// <value>
        /// The attenuation.
        /// </value>
        public float Attenuation { get; set; }

        /// <summary>
        /// Gets or sets the radius of the Gaussian blur effect.
        /// </summary>
        /// <value>
        /// The blur radius.
        /// </value>
        public int BlurRadius
        {
            get { return _blurRadius; }
            set
            {
                if (_blurRadius != value)
                {
                    _blurRadius = value;

                    ComputeKernel(_blurRadius, _blurAmount);
                }
            }
        }

        /// <summary>
        /// Gets or sets the intensity of the Gaussian blur effect.
        /// </summary>
        /// <value>
        /// The blur amount.
        /// </value>
        public float BlurAmount
        {
            get { return _blurAmount; }
            set
            {
                if (_blurAmount != value)
                {
                    _blurAmount = value;

                    ComputeKernel(_blurRadius, _blurAmount);
                }
            }
        }

        /// <summary>
        /// Gets or sets the distance from the camera at with the focal area starts.
        /// </summary>
        /// <value>
        /// The focal distance.
        /// </value>
        public float FocalDistance { get; set; }

        /// <summary>
        /// Gets or sets the width of the focal area.
        /// </summary>
        /// <value>
        /// The width of the focal.
        /// </value>
        public float FocalWidth { get; set; }

        /// <summary>
        /// Gets or sets the cameras far clip plane.
        /// </summary>
        /// <value>
        /// The far clip.
        /// </value>
        public float FarClip { get; set; }

        /// <summary>
        /// Computes the kernel for the Gaussian blur effect.
        /// </summary>
        /// <param name="blurRadius">The blur radius.</param>
        /// <param name="blurAmount">The blur amount.</param>
        private void ComputeKernel(int blurRadius, float blurAmount)
        {
            BlurRadius = blurRadius;
            BlurAmount = blurAmount;

            _blurKernel = null;
            _blurKernel = new float[BlurRadius*2 + 1];
            _blurSigma = BlurRadius/BlurAmount;

            float twoSigmaSquare = 2.0f*_blurSigma*_blurSigma;
            var sigmaRoot = (float) Math.Sqrt(twoSigmaSquare*Math.PI);
            float total = 0.0f;
            float distance = 0.0f;
            int index = 0;

            for (int i = -BlurRadius; i <= BlurRadius; ++i)
            {
                distance = i*i;
                index = i + BlurRadius;
                _blurKernel[index] = (float) Math.Exp(-distance/twoSigmaSquare)/sigmaRoot;
                total += _blurKernel[index];
            }

            for (int i = 0; i < _blurKernel.Length; ++i)
                _blurKernel[i] /= total;
        }

        /// <summary>
        /// Computes the sample offsets for the Gaussian blur effect.
        /// </summary>
        /// <param name="textureWidth">Width of the texture.</param>
        /// <param name="textureHeight">Height of the texture.</param>
        private void ComputeOffsets(float textureWidth, float textureHeight)
        {
            _blurHorizOffsets = null;
            _blurHorizOffsets = new Vector2[BlurRadius*2 + 1];

            _blurVertOffsets = null;
            _blurVertOffsets = new Vector2[BlurRadius*2 + 1];

            int index = 0;
            float xOffset = 1.0f/textureWidth;
            float yOffset = 1.0f/textureHeight;

            for (int i = -BlurRadius; i <= BlurRadius; ++i)
            {
                index = i + BlurRadius;
                _blurHorizOffsets[index] = new Vector2(i*xOffset, 0.0f);
                _blurVertOffsets[index] = new Vector2(0.0f, i*yOffset);
            }
        }
    }
}