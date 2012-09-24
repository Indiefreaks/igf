namespace ProjectMercury.Proxies
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// This proxy maintains a world matrix that transforms an effect that goes from (-.5, 0, 0) to (.5, 0, 0) 
    /// (unit vector centered on the origin) into a line from Start to End
    /// The effect will be translated and rotated to make the end points meet
    /// The x dimension will be scaled to make the ends meet
    /// The Y and Z dimensions will be scaled by the same ammount
    /// </summary>
    public class PointToPointProxy : ParticleEffectProxy
    {
        /// <summary>
        /// Creates a new point to point proxy linked to a particle effect
        /// </summary>
        /// <param name="effect"></param>
        public PointToPointProxy(ParticleEffect effect)
            : base(effect)
        {
        }

        private Single originalEffectSize = 1f;

        /// <summary>
        /// The 'size' of the originl effect. The PointToPoint proxy scales to effect to bridge the gap between points
        /// This factor is used to ensure that the scaling is correct
        /// </summary>
        public Single OriginalEffectSize
        {
            get { return originalEffectSize; }
            set
            {
                originalEffectSize = value;
                CalculateTransform();
            }
        }

        private Vector3 start;
        /// <summary>
        /// Sets the Start point (and recalculates the transform)
        /// </summary>
        public Vector3 Start
        {
            get { return start; }
            set
            {
                start = value;
                CalculateTransform();
            }
        }

        private Vector3 end;
        /// <summary>
        /// Sets the Start point (and recalculates the transform)
        /// Consider using SetStartAndEnd() to set both more efficiently
        /// </summary>
        public Vector3 End
        {
            get { return end; }
            set
            {
                end = value;
                CalculateTransform();
            }
        }

        private void CalculateTransform()
        {
            var offset = this.end - this.start;
            var center = this.start + (offset / 2f);

            float length = offset.Length();

            Matrix scale;
            Matrix translate;
            Matrix rotation;

            Matrix.CreateScale(length / OriginalEffectSize, out scale);
            Matrix.CreateTranslation(ref center, out translate);
            //Rotations by converting cartesian back to spherical angles
            //http://en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates
            Matrix.CreateFromYawPitchRoll((float)Math.Atan2(-offset.Z, offset.X), 0, (float)Math.Asin(offset.Y / length), out rotation);
            Matrix.Multiply(ref rotation, ref scale, out World);
            Matrix.Multiply(ref World, ref translate, out World);
        }

        /// <summary>
        /// Sets start and end at the same time and only recalculates the transform once
        /// </summary>
        /// <param name="start">Start point of the effect</param>
        /// <param name="end">End point of the effect</param>
        public void SetStartAndEnd(ref Vector3 start, ref Vector3 end)
        {
            this.start = start;

            this.end = end;

            this.CalculateTransform();
        }
    }
}