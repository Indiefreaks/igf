using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Extensions
{
    public static class Vector2Extensions
    {
         public static Point ToPoint(this Vector2 vector)
         {
             return new Point((int)vector.X, (int)vector.Y);
         }
    }
}