using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Extensions
{
    public static class MathExtensions
    {
        public static void BlendIntoAccumulator(float smoothRate, float newValue, ref float smoothedAccumulator)
        {
            smoothedAccumulator = MathHelper.Lerp(smoothedAccumulator, newValue, MathHelper.Clamp(smoothRate, 0, 1));
        }

        public static void BlendIntoAccumulator(float smoothRate, Vector3 newValue, ref Vector3 smoothedAccumulator)
        {
            smoothedAccumulator = Vector3.Lerp(smoothedAccumulator, newValue, MathHelper.Clamp(smoothRate, 0, 1));
        }
    }
}