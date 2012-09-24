using System;
using Microsoft.Xna.Framework.Input;

namespace Indiefreaks.Xna.Input
{
#if WINDOWS
	public sealed class KeyboardMouseMapping
	{
		private KeyboardMouseMap _lsF, _rsF, _lsB, _rsB, _rsL, _lsL, _lsR, _rsR, _leftTrigger, _rightTrigger, _a, _b, _x, _y, _back, _start, _dpadF, _dpadB, _dpadL, _dpadR, _leftClick, _rightClick, _shoulderL, _shoulderR;
		
		/// <summary></summary>
		public KeyboardMouseMapping()
		{
			_lsF = Keys.W;
			_rsF = MouseInput.YAxis;
			_lsB = Keys.S;
			_rsB = MouseInput.YAxis;
			_lsL = Keys.A;
			_rsL = MouseInput.XAxis;
			_lsR = Keys.D;
			_rsR = MouseInput.XAxis;
			_leftTrigger = Keys.Q;
			_rightTrigger = Keys.R;
			_a = Keys.Space;
			_b = Keys.E;
			_x = Keys.Z;
			_y = Keys.C;
			_back = Keys.Escape;
			_start = Keys.Enter;
			_dpadF = Keys.Up;
			_dpadB = Keys.Down;
			_dpadL = Keys.Left;
			_dpadR = Keys.Right;
			_leftClick = MouseInput.LeftButton;
			_rightClick = MouseInput.RightButton;
			_shoulderL = Keys.F;
			_shoulderR = Keys.V;
		}

		/// <summary>
		/// Returns true there are analog/digital mismatches
		/// </summary>
		/// <returns></returns>
		public bool TestForAnalogDigitalConflicts()
		{
			KeyboardMouseMap[] maps = new KeyboardMouseMap[] { _lsF, _rsF, _lsB, _rsB, _lsL, _rsL, _lsR, _rsR, _leftTrigger, _rightTrigger, _shoulderL, _shoulderR, _a, _b, _x, _y, _back, _start, _dpadF, _dpadB, _dpadL, _dpadR, _leftClick, _rightClick };

			for (int i = 1; i < maps.Length; i++)
			{
				if (maps[i].IsAnalog)
					continue;
			}
			//make sure buttons are digital
			maps = new KeyboardMouseMap[] { _shoulderL, _shoulderR, _a, _b, _x, _y, _back, _start, _dpadF, _dpadB, _dpadL, _dpadR, _leftClick, _rightClick };
			foreach (KeyboardMouseMap map in maps)
				if (map.IsAnalog)
					return true;

			//directions must be analog or digital, not both
			if (_lsF.IsAnalog != _lsB.IsAnalog ||
				_lsL.IsAnalog != _lsR.IsAnalog ||
				_rsF.IsAnalog != _rsB.IsAnalog ||
				_rsL.IsAnalog != _rsR.IsAnalog)
				return true;

			return false;
		}

		/// <summary>
		/// Returns true if two or more mappings share the same value
		/// </summary>
		/// <returns></returns>
		public bool TestForConflicts()
		{
			KeyboardMouseMap[] maps = new KeyboardMouseMap[] { _lsF, _rsF, _lsB, _rsB, _lsL, _rsL, _lsR, _rsR, _leftTrigger, _rightTrigger, _shoulderL, _shoulderR, _a, _b, _x, _y, _back, _start, _dpadF, _dpadB, _dpadL, _dpadR, _leftClick, _rightClick };
			Array.Sort<KeyboardMouseMap>(maps);
			for (int i = 1; i < maps.Length; i++)
			{
				if (maps[i - 1].CompareTo(maps[i]) == 0)
					return true;
			}
			return false;
		}
		
		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the right trigger (default is <see cref="Keys.R"/>)
		/// </summary>
		public KeyboardMouseMap RightTrigger
		{
			
			get { return _rightTrigger; }
			set { _rightTrigger = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the left trigger (default is <see cref="Keys.Q"/>)
		/// </summary>
		public KeyboardMouseMap LeftTrigger
		{
			get { return _leftTrigger; }
			set { _leftTrigger = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the right stick click button (default is <see cref="MouseInput.RightButton"/>)
		/// </summary>
		public KeyboardMouseMap RightStickClick
		{
			get { return _rightClick; }
			set { if (value.IsAnalog) throw new ArgumentException(); _rightClick = value; }
		}


		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the left stick click button (default is <see cref="MouseInput.LeftButton"/>)
		/// </summary>
		public KeyboardMouseMap LeftStickClick
		{
			get { return _leftClick; }
			set { if (value.IsAnalog) throw new ArgumentException(); _leftClick = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the right shoulder button (default is <see cref="Keys.V"/>)
		/// </summary>
		public KeyboardMouseMap RightShoulder
		{
			get { return _shoulderR; }
			set { if (value.IsAnalog) throw new ArgumentException(); _shoulderR = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the left shoulder button (default is <see cref="Keys.F"/>)
		/// </summary>
		public KeyboardMouseMap LeftShoulder
		{
			get { return _shoulderL; }
			set { if (value.IsAnalog) throw new ArgumentException(); _shoulderL = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the d-pad right button (default is <see cref="Keys.Right"/>)
		/// </summary>
		public KeyboardMouseMap DpadRight
		{
			get { return _dpadR; }
			set { if (value.IsAnalog) throw new ArgumentException(); _dpadR = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the d-pad left button (default is <see cref="Keys.Left"/>)
		/// </summary>
		public KeyboardMouseMap DpadLeft
		{
			get { return _dpadL; }
			set { if (value.IsAnalog) throw new ArgumentException(); _dpadL = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the d-pad down button (default is <see cref="Keys.Down"/>)
		/// </summary>
		public KeyboardMouseMap DpadDown
		{
			get { return _dpadB; }
			set { if (value.IsAnalog) throw new ArgumentException(); _dpadB = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the d-pad up button (default is <see cref="Keys.Up"/>)
		/// </summary>
		public KeyboardMouseMap DpadUp
		{
			get { return _dpadF; }
			set { if (value.IsAnalog) throw new ArgumentException(); _dpadF = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the start button (default is <see cref="Keys.Enter"/>)
		/// </summary>
		public KeyboardMouseMap Start
		{
			get { return _start; }
			set { if (value.IsAnalog) throw new ArgumentException(); _start = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the back button (default is <see cref="Keys.Back"/>)
		/// </summary>
		public KeyboardMouseMap Back
		{
			get { return _back; }
			set { if (value.IsAnalog) throw new ArgumentException(); _back = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the 'x' button (default is <see cref="Keys.Z"/>)
		/// </summary>
		public KeyboardMouseMap X
		{
			get { return _x; }
			set { if (value.IsAnalog) throw new ArgumentException(); _x = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the 'y' button (default is <see cref="Keys.C"/>)
		/// </summary>
		public KeyboardMouseMap Y
		{
			get { return _y; }
			set { if (value.IsAnalog) throw new ArgumentException(); _y = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the 'b' button (default is <see cref="Keys.E"/>)
		/// </summary>
		public KeyboardMouseMap B
		{
			get { return _b; }
			set { if (value.IsAnalog) throw new ArgumentException(); _b = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> mapping for the 'a' button (default is <see cref="Keys.Space"/>)
		/// </summary>
		public KeyboardMouseMap A
		{
			get { return _a; }
			set { if (value.IsAnalog) throw new ArgumentException(); _a = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the left stick's right direction (default is <see cref="Keys.D"/>)
		/// </summary>
		public KeyboardMouseMap LeftStickRight
		{
			get { return _lsR; }
			set { _lsR = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the left stick's left direction (default is <see cref="Keys.A"/>)
		/// </summary>
		public KeyboardMouseMap LeftStickLeft
		{
			get { return _lsL; }
			set { _lsL = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the left stick's backwards direction (default is <see cref="Keys.S"/>)
		/// </summary>
		public KeyboardMouseMap LeftStickBackward
		{
			get { return _lsB; }
			set { _lsB = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the left stick's forwards direction (default is <see cref="Keys.W"/>)
		/// </summary>
		public KeyboardMouseMap LeftStickForward
		{
			get { return _lsF; }
			set { _lsF = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the right stick's right direction (default is <see cref="MouseInput.XAxis"/>)
		/// </summary>
		public KeyboardMouseMap RightStickRight
		{
			get { return _rsR; }
			set { _rsR = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the right stick's left direction (default is <see cref="MouseInput.XAxis"/>)
		/// </summary>
		public KeyboardMouseMap RightStickLeft
		{
			get { return _rsL; }
			set { _rsL = value; }
		}
		
		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the right stick's backwards direction (default is <see cref="MouseInput.YAxis"/>)
		/// </summary>
		public KeyboardMouseMap RightStickBackward
		{
			get { return _rsB; }
			set { _rsB = value; }
		}

		/// <summary>
		/// Gets/Sets the <see cref="Keys"/> or <see cref="Mouse"/> mapping for the right stick's forwards direction (default is <see cref="MouseInput.YAxis"/>)
		/// </summary>
		public KeyboardMouseMap RightStickForward
		{
			get { return _rsF; }
			set { _rsF = value; }
		}
	}
#endif
}