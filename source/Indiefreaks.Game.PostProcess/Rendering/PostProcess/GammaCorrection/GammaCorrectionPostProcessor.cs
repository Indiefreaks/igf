using Indiefreaks.Xna.Rendering.PostProcess.Resources;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Indiefreaks.Xna.Rendering.PostProcess.GammaCorrection
{
    public class GammaCorrectionPostProcessor : BaseRenderTargetPostProcessor
    {
        private readonly Effect _gammaCorrectEffect;
        private float _brightness;
        private Viewport _viewport;

        public float Brightness
        {
            get { return _brightness; }
            set { _brightness = value; }
        }

        public GammaCorrectionPostProcessor()
        {
            _brightness = 0.2f;

#if WINDOWS
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, WindowsPostProcessResources.ResourceManager);
#else
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, Xbox360PostProcessResources.ResourceManager);
#endif
            _gammaCorrectEffect = shaderResources.Load<Effect>("GammaCorrect");
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
            graphicsDevice.DepthStencilState = DepthStencilState.None;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            
            _viewport = graphicsDevice.Viewport;

            CustomFrameBufferCollection buffers = SceneState.FrameBuffers.GetCustomFrameBufferCollection("colorcorrect", true);

            if (buffers.Count == 0)
            {
                SurfaceFormat surfaceFormat = SurfaceFormat.Color;
                buffers.Add(new RenderTarget2D(graphicsDevice, SceneState.FrameBuffers.Width, SceneState.FrameBuffers.Height, false, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.PlatformContents));
                graphicsDevice.SetRenderTarget(null);
            }
       
            graphicsDevice.SetRenderTarget(PreviousRenderTarget);
            _gammaCorrectEffect.CurrentTechnique = _gammaCorrectEffect.Techniques["ColorCorrect"];

            _gammaCorrectEffect.Parameters["SceneTexture"].SetValue(ProcessorRenderTarget);
            _gammaCorrectEffect.Parameters["Brightness"].SetValue(_brightness);

            _gammaCorrectEffect.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.SamplerStates.Reset();
            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);

            base.EndFrameRendering();
        }

    }
}
