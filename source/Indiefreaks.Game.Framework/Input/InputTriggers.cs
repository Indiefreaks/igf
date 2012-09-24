namespace Indiefreaks.Xna.Input
{
    /// <summary>
    /// Structure storing the current state of a gamepad's triggers
    /// </summary>
    public struct InputTriggers
    {
        internal float LeftTriggerFloat;
        internal float RightTriggerFloat;

        /// <summary></summary>
        public float LeftTrigger
        {
            get { return LeftTriggerFloat; }
        }

        /// <summary></summary>
        public float RightTrigger
        {
            get { return RightTriggerFloat; }
        }
    }
}