using Microsoft.Xna.Framework.Graphics;

namespace Indiefreaks.Xna.Extensions
{
    public static class GraphicsDeviceExtensions
    {
        public static void Reset(this SamplerStateCollection samplerStateCollection)
        {
            for (int i = 0; i < 8; i++)
            {
                samplerStateCollection[i] = SamplerState.LinearClamp;
                samplerStateCollection[i] = SamplerState.PointClamp;
            }
        }
    }
}