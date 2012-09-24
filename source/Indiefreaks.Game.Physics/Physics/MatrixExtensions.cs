using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Physics
{
    internal static class MatrixExtensions
    {
        private static Vector3 _scale;
        private static Quaternion _rotation;
        private static Vector3 _translation;

        public static void GetScaleComponent(this Matrix worldMatrix, out Vector3 scale)
        {
            worldMatrix.Decompose(out _scale, out _rotation, out _translation);
            scale = _scale;
        }

        public static void GetRotationAndTranslationComponents(this Matrix worldMatrix, out Quaternion rotation, out Vector3 translation)
        {
            worldMatrix.Decompose(out _scale, out _rotation, out _translation);
            rotation = _rotation;
            translation = _translation;

        }

        public static void SRTMatrixToRTMatrix(this Matrix worldMatrix, out Matrix rtMatrix)
        {
            worldMatrix.Decompose(out _scale, out _rotation, out _translation);
            rtMatrix = Matrix.CreateFromQuaternion(_rotation)*Matrix.CreateTranslation(_translation);
        }
    }
}