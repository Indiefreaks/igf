namespace Indiefreaks.Xna.Input
{
    /// <summary>
    /// Structure storing the state of a digital (on/off) button
    /// </summary>
    public struct Button
    {
        private int _heldTicks;
        private long _pressTick;
        private bool _prev;
        private long _releaseTick;
        private int _releasedTicks;

        /// <summary>
        /// True if the button is pressed
        /// </summary>
        public bool IsDown { get; private set; }

        /// <summary>
        /// Will return true for the SINGLE FRAME the button changes state from unpressed to pressed
        /// </summary>
        public bool IsPressed
        {
            get { return IsDown && !_prev; }
        }

        /// <summary>
        /// Will return true for the SINGLE FRAME the button changes state from pressed to unpressed
        /// </summary>
        public bool IsReleased
        {
            get { return !IsDown && _prev; }
        }

        /// <summary>
        /// Number of seconds the button has been held down for
        /// </summary>
        public float DownDuration
        {
            get { return (float) ((_heldTicks)/10000000d); }
        }

        /// <summary>
        /// Number of seconds since the the botton was last released (May be useful for calculating double presses)
        /// </summary>
        public float ReleaseTime
        {
            get { return (float) ((_releasedTicks)/10000000d); }
        }

        internal void SetState(bool value, long tick)
        {
            if (value && !IsDown)
                _pressTick = tick;
            if (value)
                _heldTicks = (int) (tick - _pressTick);
            _prev = IsDown;
            IsDown = value;
            if (!IsDown && _prev)
                _releaseTick = tick;
            if (_releaseTick != 0)
                _releasedTicks = (int) (tick - _releasedTicks);
        }

        /// <summary>
        /// Implicit conversion to a boolean (equivalent of <see cref="IsDown"/>)
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static implicit operator bool(Button b)
        {
            return b.IsDown;
        }
    }
}