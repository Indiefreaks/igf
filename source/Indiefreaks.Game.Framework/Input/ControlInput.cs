namespace Indiefreaks.Xna.Input
{
    /// <summary>
    /// Enumeration for a player method of input (eg, gamepad, keyboard and mouse or touch)
    /// </summary>
    public enum ControlInput
    {
        /// <summary></summary>
        None = 4,
        /// <summary></summary>
        GamePad1 = 0,
        /// <summary></summary>
        GamePad2 = 1,
        /// <summary></summary>
        GamePad3 = 2,
        /// <summary></summary>
        GamePad4 = 3,
#if WINDOWS
        /// <summary></summary>
        KeyboardMouse = 5,
#endif
#if WINDOWS_PHONE
        TouchPanel = 6,
#endif
    } ;
}