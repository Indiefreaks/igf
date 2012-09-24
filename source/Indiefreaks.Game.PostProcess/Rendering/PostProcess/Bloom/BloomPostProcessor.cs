using System;
using Indiefreaks.Xna.Rendering.PostProcess.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.PostProcess.Bloom
{
    public class BloomPostProcessor : BaseRenderTargetPostProcessor
    {
        private readonly Effect _bloomCombineEffect;
        private readonly Effect _bloomExtractEffect;
        private readonly Effect _gaussianBlurEffect;

        private float _baseIntensity;
        private float _baseSaturation;
        private float _bloomIntensity;
        private float _bloomSaturation;
        private float _bloomThreshold;

        private float _blurAmount;
        private Vector2[] _blurHorizOffsets;
        private float[] _blurKernel;
        private int _blurRadius;
        private float _blurSigma;
        private Vector2[] _blurVertOffsets;

        private Viewport _viewport;

        public BloomPostProcessor()
        {
            BlurRadius = 7;
            BlurAmount = 1f;
            BloomThreshold = 0.5f;
            BloomIntensity = 1.5f;
            BaseIntensity = 1;
            BloomSaturation = 2.5f;
            BaseSaturation = 1;

            _viewport = SunBurnCoreSystem.Instance.GraphicsDeviceManager.GraphicsDevice.Viewport;

#if WINDOWS
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, WindowsPostProcessResources.ResourceManager);
#else
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, Xbox360PostProcessResources.ResourceManager);
#endif
            _gaussianBlurEffect = shaderResources.Load<Effect>("GaussianBlur");
            _bloomExtractEffect = shaderResources.Load<Effect>("BloomExtract");
            _bloomCombineEffect = shaderResources.Load<Effect>("BloomCombine");
            
            ComputeKernel(_blurRadius, _blurAmount);
            ComputeOffsets(_viewport.Width, _viewport.Height);
        }


        public float BloomIntensity
        {
            get { return _bloomIntensity; }
            set { _bloomIntensity = value; }
        }

        public float BaseIntensity
        {
            get { return _baseIntensity; }
            set { _baseIntensity = value; }
        }

        public float BloomSaturation
        {
            get { return _bloomSaturation; }
            set { _bloomSaturation = value; }
        }

        public float BaseSaturation
        {
            get { return _baseSaturation; }
            set { _baseSaturation = value; }
        }

        public float BloomThreshold
        {
            get { return _bloomThreshold; }
            set { _bloomThreshold = value; }
        }

        /// <summary>
        /// Gets or sets the radius of the Gaussian blur effect.
        /// </summary>
        /// <value>
        /// The blur radius.
        /// </value>
        private int BlurRadius
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
        /// Computes the kernel for the Gaussian blur effect.
        /// </summary>
        /// <param name="blurRadius">The blur radius.</param>
        /// <param name="blurAmount">The blur amount.</param>
        private void ComputeKernel(int blurRadius, float blurAmount)
        {
            BlurRadius = blurRadius;
            BlurAmount = blurAmount;

            _blurKernel = null;
            _blurKernel = new float[BlurRadius * 2 + 1];
            _blurSigma = BlurRadius / BlurAmount;

            float twoSigmaSquare = 2.0f * _blurSigma * _blurSigma;
            var sigmaRoot = (float)Math.Sqrt(twoSigmaSquare * Math.PI);
            float total = 0.0f;
            float distance = 0.0f;
            int index = 0;

            for (int i = -BlurRadius; i <= BlurRadius; ++i)
            {
                distance = i * i;
                index = i + BlurRadius;
                _blurKernel[index] = (float)Math.Exp(-distance / twoSigmaSquare) / sigmaRoot;
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
            _blurHorizOffsets = new Vector2[BlurRadius * 2 + 1];

            _blurVertOffsets = null;
            _blurVertOffsets = new Vector2[BlurRadius * 2 + 1];

            int index = 0;
            float xOffset = 1.0f / textureWidth;
            float yOffset = 1.0f / textureHeight;

            for (int i = -BlurRadius; i <= BlurRadius; ++i)
            {
                index = i + BlurRadius;
                _blurHorizOffsets[index] = new Vector2(i * xOffset, 0.0f);
                _blurVertOffsets[index] = new Vector2(0.0f, i * yOffset);
            }
        }

        /// <summary>
        /// Use to apply user quality and performance preferences to the resources managed by this object.
        /// </summary>
        /// <param name="preferences"/>
        public override void ApplyPreferences(ISystemPreferences preferences)
        {
        }
        
        public override void EndFrameRendering()
        {
            GraphicsDevice graphicsDevice = base.GraphicsDeviceManager.GraphicsDevice;
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.BlendState = new BlendState() {ColorSourceBlend = Blend.SourceColor, ColorDestinationBlend = Blend.DestinationColor, AlphaSourceBlend = Blend.SourceColor, AlphaDestinationBlend = Blend.InverseSourceColor, AlphaBlendFunction = BlendFunction.Add, BlendFactor = Color.White, ColorBlendFunction = BlendFunction.Add};
            graphicsDevice.DepthStencilState = DepthStencilState.None;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;

            CustomFrameBufferCollection bloomBuffers = SceneState.FrameBuffers.GetCustomFrameBufferCollection("bloom", true);

            if (bloomBuffers.Count == 0)
            {
                SurfaceFormat surfaceFormat = SurfaceFormat.Color;
                bloomBuffers.Add(new RenderTarget2D(graphicsDevice, SceneState.FrameBuffers.Width / 2, SceneState.FrameBuffers.Height / 2, false, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.PlatformContents));
                bloomBuffers.Add(new RenderTarget2D(graphicsDevice, SceneState.FrameBuffers.Width / 2, SceneState.FrameBuffers.Height / 2, false, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.PlatformContents));
                bloomBuffers.Add(new RenderTarget2D(graphicsDevice, SceneState.FrameBuffers.Width / 2, SceneState.FrameBuffers.Height / 2, false, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.PlatformContents));
                graphicsDevice.SetRenderTarget(null);
            }

            // BLOOM EXTRACT PreviousRenderTarget
            graphicsDevice.SetRenderTarget(bloomBuffers[0]);
            _bloomExtractEffect.CurrentTechnique = _bloomExtractEffect.Techniques["BloomExtract"];
            _bloomExtractEffect.Parameters["SceneTexture"].SetValue(ProcessorRenderTarget);
            _bloomExtractEffect.Parameters["BloomThreshold"].SetValue(_bloomThreshold);
            _bloomExtractEffect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.SamplerStates.Reset();
            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);


            // HORIZONTAL BLUR
            graphicsDevice.SetRenderTarget(bloomBuffers[1]);
            _gaussianBlurEffect.CurrentTechnique = _gaussianBlurEffect.Techniques["GaussianBlur"];
            _gaussianBlurEffect.Parameters["SceneTexture"].SetValue(bloomBuffers[0]);
            _gaussianBlurEffect.Parameters["weights"].SetValue(_blurKernel);
            _gaussianBlurEffect.Parameters["offsets"].SetValue(_blurHorizOffsets);
            _gaussianBlurEffect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.SamplerStates.Reset();
            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);

            // VERTICAL BLUR
            graphicsDevice.SetRenderTarget(bloomBuffers[2]);
            _gaussianBlurEffect.Parameters["SceneTexture"].SetValue(bloomBuffers[1]);
            _gaussianBlurEffect.Parameters["offsets"].SetValue(_blurVertOffsets);
            _gaussianBlurEffect.CurrentTechnique.Passes[0].Apply();
            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);

            //// BLOOM COMBINE
            graphicsDevice.SetRenderTarget(PreviousRenderTarget);
            _bloomCombineEffect.CurrentTechnique = _bloomCombineEffect.Techniques["BloomCombine"];

            _bloomCombineEffect.Parameters["BloomIntensity"].SetValue(_bloomIntensity);
            _bloomCombineEffect.Parameters["BaseIntensity"].SetValue(_baseIntensity);
            _bloomCombineEffect.Parameters["BloomSaturation"].SetValue(_bloomSaturation);
            _bloomCombineEffect.Parameters["BaseSaturation"].SetValue(_baseSaturation);

            _bloomCombineEffect.Parameters["BaseMap"].SetValue(ProcessorRenderTarget);
            _bloomCombineEffect.Parameters["BloomMap"].SetValue(bloomBuffers[2]);

            _bloomCombineEffect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.SamplerStates.Reset();
            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            graphicsDevice.SamplerStates.Reset();

            base.EndFrameRendering();
        }
    }
}