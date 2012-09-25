using Indiefreaks.Xna.Rendering.PostProcess.Resources;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Indiefreaks.Xna.Rendering.PostProcess
{
    public class NightVisionProcessor : BaseRenderTargetPostProcessor
    {
        private Effect _nightVisionEffect;
        private Viewport _viewport;

        private int _linesOff;
        private int _linesOn;
        private float _offIntensity;

        public NightVisionProcessor()
        {
            _viewport = SunBurnCoreSystem.Instance.GraphicsDeviceManager.GraphicsDevice.Viewport;

#if WINDOWS
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, WindowsPostProcessResources.ResourceManager);
#else
            var shaderResources = new ResourceContentManager(SunBurnCoreSystem.Instance.Services, Xbox360PostProcessResources.ResourceManager);
#endif

            _nightVisionEffect = shaderResources.Load<Effect>("NightVision");
        }

        public int LinesOn
        {
            get { return _linesOn; }
            set { _linesOn = value; }
        }

        public int LinesOff
        {
            get { return _linesOff; }
            set { _linesOff = value; }
        }

        public float OffIntensity
        {
            get { return _offIntensity; }
            set { _offIntensity = value; }
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

            CustomFrameBufferCollection buffers = SceneState.FrameBuffers.GetCustomFrameBufferCollection("nightvision", true);

            if (buffers.Count == 0)
            {
                SurfaceFormat surfaceFormat = SurfaceFormat.Color;
                buffers.Add(new RenderTarget2D(graphicsDevice, _viewport.Width, _viewport.Height, false, surfaceFormat, DepthFormat.None, 0, RenderTargetUsage.PlatformContents));
                graphicsDevice.SetRenderTarget(null);
            }

            graphicsDevice.SetRenderTarget(PreviousRenderTarget);
            _nightVisionEffect.CurrentTechnique = _nightVisionEffect.Techniques["NightVision"];

            _nightVisionEffect.Parameters["SceneTexture"].SetValue(ProcessorRenderTarget);
            _nightVisionEffect.Parameters["LinesOn"].SetValue(_linesOn);
            _nightVisionEffect.Parameters["LinesOff"].SetValue(_linesOff);
            _nightVisionEffect.Parameters["OffIntensity"].SetValue(_offIntensity);

            _nightVisionEffect.CurrentTechnique.Passes[0].Apply();
            FullFrameQuad.Render(graphicsDevice, _viewport.Width, _viewport.Height);

            base.EndFrameRendering();
        }

    }
}
