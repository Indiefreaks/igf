using System;
using Microsoft.Xna.Framework.Input;

namespace Indiefreaks.Xna.Input
{
#if WINDOWS
	public struct KeyboardMouseMap : IComparable<KeyboardMouseMap>
	{
		private Keys _key;
		private bool _usemouse;
		private MouseInput _mouse;

		/// <summary>
		/// Create a mapping from a <see cref="Keys"/> enum (note this class can be implicitly assigned from <see cref="Keys"/>)
		/// </summary>
		/// <param name="key"></param>
		/// <remarks>
		/// <para>
		/// This class can be implicitly cast from a <see cref="Keys"/> enumeration
		/// </para>
		/// <para>eg:</para>
		/// <example>
		/// <code>
		/// KeyboardMouseControlMap map = Keys.W;
		/// </code>
		/// </example>
		/// </remarks>
		public KeyboardMouseMap(Keys key)
		{
			this._key = key;
			_usemouse = false;
			_mouse = MouseInput.LeftButton;
		}

		/// <summary>
		/// [Windows Only] Create a mapping from a <see cref="MouseInput"/> enum (note this class can be implicitly assigned from <see cref="MouseInput"/>)
		/// </summary>
		/// <param name="mouse"></param>
		/// <remarks>
		/// <para>
		/// This class can be implicitly cast from a <see cref="MouseInput"/> enumeration
		/// </para>
		/// <para>eg:</para>
		/// <example>
		/// <code>
		/// KeyboardMouseControlMap map = MouseInput.LeftButton;
		/// </code>
		/// </example>
		/// </remarks>
		public KeyboardMouseMap(MouseInput mouse)
		{
			_mouse = mouse;
			_usemouse = true;
			_key = (Keys)0;
		}

		/// <summary>
		/// <see cref="MouseInput"/> implicit cast
		/// </summary>
		/// <param name="mouse"></param>
		/// <returns></returns>
		public static implicit operator KeyboardMouseMap(MouseInput mouse)
		{
			return new KeyboardMouseMap(mouse);
		}

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public int CompareTo(KeyboardMouseMap map)
		{
			if (map._usemouse != _usemouse)
			{
				if (this._usemouse)
					return 1;
				else
					return -1;
			}

			if (_key != map._key)
				return (int)map._key - (int)_key;
			return (int)map._mouse - (int)_mouse;
		}

		/// <summary>Returns the name of the key or mouse selected</summary>
		/// <returns></returns>
		public override string ToString()
		{

			if (_usemouse)
				return _mouse.ToString();
			else
				return _key.ToString();
		}

		/// <summary>
		/// True if this is an analog input (variable), false if it is digital (on/off)
		/// </summary>
		public bool IsAnalog
		{
			get
			{
				if (_usemouse)
					return _mouse == MouseInput.YAxis || _mouse == MouseInput.XAxis || _mouse == MouseInput.ScrollWheel;

				return false;
			}
		}

		/// <summary>
		/// <see cref="Keys"/> implicit cast
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static implicit operator KeyboardMouseMap(Keys key)
		{
			return new KeyboardMouseMap(key);
		}

		/// <summary>
		/// [Windows Only] True if this mapping is using the mouse
		/// </summary>
		public bool UsingMouse
		{
			get { return _usemouse; }
		}

		/// <summary>
		/// [Windows Only] Gets/Sets the input directly as a <see cref="MouseInput"/>
		/// </summary>
		public MouseInput MouseInputMapping
		{
			get { if (!_usemouse) throw new ArgumentException("UsingMouse == false"); return _mouse; }
			set { _mouse = value; _usemouse = true; }
		}
		
		/// <summary>
		/// Gets/Sets the input directly as a <see cref="Keys"/>
		/// </summary>
		public Keys KeyMapping
		{
			get
			{
				if (_usemouse) throw new ArgumentException("UsingMouse == true");
				return _key;
			}
			set
			{
				_key = value;
				_usemouse = false;
			}
		}

		/// <summary>
		/// Get the value as a float
		/// </summary>
		public float GetValue(KeyboardMouseState inputState, bool invert)
		{
			if (!inputState.WindowFocused)
				return 0;

			KeyboardState ks = inputState.KeyboardState;
			MouseState ms = inputState.MouseState;

			if (_usemouse)
			{
				float val = 0;
				switch (_mouse)
				{
					case MouseInput.LeftButton:
						val = ms.LeftButton == ButtonState.Pressed ? 1 : 0;
						break;
					case MouseInput.MiddleButton:
						val = ms.MiddleButton == ButtonState.Pressed ? 1 : 0;
						break;
					case MouseInput.RightButton:
						val = ms.RightButton == ButtonState.Pressed ? 1 : 0;
						break;
					case MouseInput.ScrollWheel:
						if (invert)
							return 0;
						val = ms.ScrollWheelValue / 640.0f;
						break;
					case MouseInput.XButton1:
						val = ms.XButton1 == ButtonState.Pressed ? 1 : 0;
						break;
					case MouseInput.XButton2:
						val = ms.XButton2 == ButtonState.Pressed ? 1 : 0;
						break;
					case MouseInput.XAxis:
						if (invert)
							return 0;
						val = (ms.X - inputState.MousePositionPrevious.X) / 8.0f;
						break;
					case MouseInput.YAxis:
						if (invert)
							return 0;
						val = (ms.Y - inputState.MousePositionPrevious.Y) / -8.0f;
						break;
				}
				
				if (invert)
					return -val;
				return val;
			}
			else
			{
				if (ks.IsKeyDown(_key))
				{
					if (invert)
						return -1;
					return 1;
				}
				return 0;
			}
		}

		/// <summary>
		/// Get the value as a boolean
		/// </summary>
		public bool GetValue(KeyboardMouseState inputState)
		{
			if (!inputState.WindowFocused)
				return false;

			if (_usemouse)
			{
				switch (_mouse)
				{
					case MouseInput.LeftButton:
						return inputState.MouseState.LeftButton == ButtonState.Pressed;
					case MouseInput.MiddleButton:
						return inputState.MouseState.MiddleButton == ButtonState.Pressed;
					case MouseInput.RightButton:
						return inputState.MouseState.RightButton == ButtonState.Pressed;
					case MouseInput.XButton1:
						return inputState.MouseState.XButton1 == ButtonState.Pressed;
					case MouseInput.XButton2:
						return inputState.MouseState.XButton2 == ButtonState.Pressed;
				}
				throw new ArgumentException();
			}
			else
				return inputState.KeyboardState.IsKeyDown(_key);
		}
	}
#endif
}