namespace Indiefreaks.Xna.Input
{
#if WINDOWS
    /// <summary>
    /// [Windows Only] Mouse input method, such as buttons, axis and wheel
    /// </summary>
    public enum MouseInput
    {
        /// <summary></summary>
        LeftButton,
        /// <summary></summary>
        RightButton,
        /// <summary></summary>
        MiddleButton,
        /// <summary></summary>
        XButton1,
        /// <summary></summary>
        XButton2,
        /// <summary></summary>
        XAxis,
        /// <summary></summary>
        YAxis,
        /// <summary></summary>
        ScrollWheel
    }
#endif
}