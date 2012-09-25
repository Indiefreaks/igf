using Indiefreaks.Xna.Rendering.PostProcess.Resources;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Indiefreaks.Xna.Rendering.PostProcess
{
    public class ColorCorrectionProcessor : BaseRenderTargetPostProcessor
    {
        private Effect _colorCorrectEffect;
        private float _brightness;
        private Viewport _viewport;

        public float Brightness
        {
            get { return _brightness; }
            set { _brightness = value; }
        }

        public ColorCorrectionProcessor()
        {
            _viewport = SunBurnCoreSystem.Instance.GraphicsDeviceManager.GraphicsDevice.Viewport;

#if WINDOWS
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, WindowsPostProcessResources.ResourceManager);
#else
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, Xbox360PostProcessResources.ResourceManager);
#endif
            _colorCorrectEffect = shaderResources.Load<Effect>("PostColorCorrect");
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

            CustomFrameBufferCollection buffers = SceneState.FrameBuffers.GetCustomFrameBufferCollection("colorcorrect", true);

            if (buffers.Count == 0)
            {
                SurfaceFormat surfaceFormat = SurfaceFormat.Color;
                buffers.Add(new RenderTarget2D(graphicsDevice, _viewport.Width, _viewport.Height, false, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.PlatformContents));
                graphicsDevice.SetRenderTarget(null);
            }
       
            graphicsDevice.SetRenderTarget(PreviousRenderTarget);
            _colorCorrectEffect.CurrentTechnique = _colorCorrectEffect.Techniques["ColorCorrect"];

            _colorCorrectEffect.Parameters["SceneTexture"].SetValue(ProcessorRenderTarget);
            _colorCorrectEffect.Parameters["Brightness"].SetValue(_brightness);

            _colorCorrectEffect.CurrentTechnique.Passes[0].Apply();
            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);

            base.EndFrameRendering();
        }

    }
}
