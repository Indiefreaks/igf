using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Indiefreaks.Xna.Input
{
#if WINDOWS
    public struct KeyboardMouseState
    {
        private MouseState _ms;
        private Point _mousePos;

        /// <summary>
        /// Current keyboard state
        /// </summary>
        public KeyboardState KeyboardState { get; set; }

        private bool _windowFocused;

        internal bool WindowFocused
        {
            get { return _windowFocused; }
            set
            {
                if (value != _windowFocused)
                {
                    _windowFocused = value;
                    if (value)
                    {
                        _ms = new MouseState(_mousePos.X, _mousePos.Y, _ms.ScrollWheelValue, _ms.LeftButton, _ms.MiddleButton, _ms.RightButton, _ms.XButton1, _ms.XButton2);
                    }
                }
            }
        }

        /// <summary>
        /// [Windows Only]
        /// </summary>
        internal Point MousePositionPrevious
        {
            get { return _mousePos; }
            set { _mousePos = value; }
        }

        /// <summary>
        /// [Windows Only] Current mouse state
        /// </summary>
        public MouseState MouseState
        {
            get { return _ms; }
            set { _ms = value; }
        }
    }
#endif
}