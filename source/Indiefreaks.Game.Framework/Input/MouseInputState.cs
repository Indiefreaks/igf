using Microsoft.Xna.Framework.Input;

namespace Indiefreaks.Xna.Input
{
#if WINDOWS
    public sealed class MouseInputState
    {
        private MouseState _state;
        private Button _left = new Button();
        private Button _right = new Button();
        private Button _middle = new Button();
        private Button _x1 = new Button();
        private Button _x2 =  new Button();
        private int _x, _y, _scroll, _scrollDelta;

        /// <summary>
        /// Horizontal position of the mouse cursor
        /// </summary>
        public int X { get { return _x; } }
        /// <summary>
        /// Vertical position of the mouse cursor
        /// </summary>
        public int Y { get { return _y; } }
        /// <summary>
        /// <see cref="Button"/> state of the left mouse button
        /// </summary>
        public Button LeftButton { get { return _left; } }
        /// <summary>
        /// <see cref="Button"/> state of the right mouse button
        /// </summary>
        public Button RightButton { get { return _right; } }
        /// <summary>
        /// <see cref="Button"/> state of the middle mouse button
        /// </summary>
        public Button MiddleButton { get { return _middle; } }
        /// <summary>
        /// <see cref="Button"/> state of the first X mouse button
        /// </summary>
        public Button XButton1 { get { return _x2; } }
        /// <summary>
        /// <see cref="Button"/> state of the second X mouse button
        /// </summary>
        public Button XButton2 { get { return _x1; } }
        /// <summary>
        /// Gets the total mouse scroll movement
        /// </summary>
        public int ScrollWheelValue { get { return _scroll; } }
        /// <summary>
        /// Gets the delta mouse scroll movement
        /// </summary>
        public int ScrollWheelDelta { get { return _scrollDelta; } }

        internal void Update(long tick, ref MouseState mouseState)
        {
            _state = mouseState;

            _x = _state.X;
            _y = _state.Y;
            _scrollDelta = _state.ScrollWheelValue - this._scroll;
            _scroll = _state.ScrollWheelValue;

            _left.SetState(_state.LeftButton == ButtonState.Pressed, tick);
            _right.SetState(_state.RightButton == ButtonState.Pressed, tick);
            _middle.SetState(_state.MiddleButton == ButtonState.Pressed, tick);
            _x1.SetState(_state.XButton1 == ButtonState.Pressed, tick);
            _x2.SetState(_state.XButton2 == ButtonState.Pressed, tick);
        }

        /// <summary></summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static implicit operator MouseState(MouseInputState input)
        {
            return input._state;
        }
    }
#endif
}