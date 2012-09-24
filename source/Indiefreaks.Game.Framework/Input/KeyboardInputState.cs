using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Indiefreaks.Xna.Input
{
#if WINDOWS
    public sealed class KeyboardInputState
    {
        private static readonly IntPtr Layout;
        private static readonly Keys[] KeyIndices = new Keys[256];
        private static readonly uint[] ScanCodes = new uint[256];
        private static readonly byte[] KeyStateBytes = new byte[256];
        internal readonly Button[] Buttons = new Button[256];
        private readonly KeyState _state;
        private KeyMap _currentFrame;
        private long _initialTick = -1;
        private KeyMap _previousFrame;

        static KeyboardInputState()
        {
            System.Reflection.FieldInfo[] enums =
                typeof (Keys).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            List<Keys> keys = new List<Keys>();
            List<Keys> control = new List<Keys>();

            foreach (System.Reflection.FieldInfo field in enums)
            {
                Keys key = (Keys) field.GetValue(null);
                keys.Add(key);

                if (char.IsControl((char) key))
                    control.Add(key);
            }

            Layout = GetKeyboardLayout(0);

            for (int scan = 0; scan < 255; scan++)
            {
                uint key = MapVirtualKeyEx((uint) scan, 1, Layout);
                if (key != 0)
                    ScanCodes[key] = (uint) scan;
            }

            KeyIndices = keys.ToArray();
        }

        internal KeyboardInputState()
        {
            _state = new KeyState(Buttons);
        }

        /// <summary>
        ///   Gets the current KeyState
        /// </summary>
        public KeyState KeyState
        {
            get { return _state; }
        }

        /// <summary>
        ///   Button Indexer (Keys)
        /// </summary>
        /// <param name = "key"></param>
        /// <returns></returns>
        public Button this[Keys key]
        {
            get { return Buttons[(int) key]; }
        }

        /// <summary>
        ///   Allocates an array of all values in the <see cref = "Keys" /> enumerator
        /// </summary>
        /// <returns></returns>
        public static Keys[] GetKeyArray()
        {
            return (Keys[]) KeyIndices.Clone();
        }

        /// <summary>
        ///   <para>Gets the character for a given <see cref = "Keys" /> key. Eg: <see cref = "Keys.A" /> will output 'a'. Uses the current keyboard modifier key state; Shift will convert to upper case, etc.</para>
        ///   <para>Returns false if the key character is unknown</para>
        /// </summary>
        /// <param name = "key"></param>
        /// <param name = "keyChar"></param>
        /// <returns></returns>
        public bool TryGetKeyChar(Keys key, out char keyChar)
        {
            keyChar = '\0';

            KeyStateBytes[0x10] = (byte) ((KeyState.LeftShift | KeyState.RightShift) ? 0x80 : 0);
            KeyStateBytes[0x11] = (byte) ((KeyState.LeftControl | KeyState.RightControl) ? 0x80 : 0);

            return ToUnicodeEx((uint) key, ScanCodes[(uint) key], KeyStateBytes, out keyChar, 1, 0, Layout) == 1;
        }

        /// <summary>
        /// </summary>
        /// <param name = "input"></param>
        /// <returns></returns>
        public static implicit operator KeyboardState(KeyboardInputState input)
        {
            return input._currentFrame.state;
        }

        /// <summary>
        ///   Returns true if a key is down
        /// </summary>
        /// <param name = "key"></param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            return Buttons[(int) key];
        }

        /// <summary>
        ///   Returns true if a key is up
        /// </summary>
        /// <param name = "key"></param>
        /// <returns></returns>
        public bool IsKeyUp(Keys key)
        {
            return !Buttons[(int) key];
        }

        /// <summary>
        ///   Gets the <see cref = "Button" /> state of a key
        /// </summary>
        /// <param name = "key"></param>
        /// <returns></returns>
        public Button GetKey(Keys key)
        {
            return Buttons[(int) key];
        }

        internal void Update(long tick, ref KeyboardState keyboardState)
        {
            if (_initialTick == -1)
            {
                _initialTick = tick;
                _currentFrame.state = keyboardState;
            }

            tick -= _initialTick;

            _previousFrame.state = _currentFrame.state;
            _currentFrame.state = keyboardState;

            for (int i = 0; i < 32; i++)
            {
                Buttons[32*0 + i].SetState((_currentFrame.currentState0 & ((uint) 1) << i) != 0, tick);
                Buttons[32*1 + i].SetState((_currentFrame.currentState1 & ((uint) 1) << i) != 0, tick);
                Buttons[32*2 + i].SetState((_currentFrame.currentState2 & ((uint) 1) << i) != 0, tick);
                Buttons[32*3 + i].SetState((_currentFrame.currentState3 & ((uint) 1) << i) != 0, tick);
                Buttons[32*4 + i].SetState((_currentFrame.currentState4 & ((uint) 1) << i) != 0, tick);
                Buttons[32*5 + i].SetState((_currentFrame.currentState5 & ((uint) 1) << i) != 0, tick);
                Buttons[32*6 + i].SetState((_currentFrame.currentState6 & ((uint) 1) << i) != 0, tick);
                Buttons[32*7 + i].SetState((_currentFrame.currentState7 & ((uint) 1) << i) != 0, tick);
            }
        }

        /// <summary>
        ///   Calls the <paramref name = "callback" /> for each <see cref = "Keys" /> key where <see cref = "Button.OnPressed" /> is true
        /// </summary>
        /// <param name = "callback"></param>
        public void GetPressedKeys(Action<Keys> callback)
        {
            if (callback == null)
                throw new ArgumentNullException();

            if (_currentFrame.currentState0 != _previousFrame.currentState0) PressCallback(callback, 0);
            if (_currentFrame.currentState1 != _previousFrame.currentState1) PressCallback(callback, 1);
            if (_currentFrame.currentState2 != _previousFrame.currentState2) PressCallback(callback, 2);
            if (_currentFrame.currentState3 != _previousFrame.currentState3) PressCallback(callback, 3);
            if (_currentFrame.currentState4 != _previousFrame.currentState4) PressCallback(callback, 4);
            if (_currentFrame.currentState5 != _previousFrame.currentState5) PressCallback(callback, 5);
            if (_currentFrame.currentState6 != _previousFrame.currentState6) PressCallback(callback, 6);
            if (_currentFrame.currentState7 != _previousFrame.currentState7) PressCallback(callback, 7);
        }

        /// <summary>
        ///   Adds a key to the <paramref name = "pressedList" /> for each <see cref = "Keys" /> key where <see cref = "Button.OnPressed" /> is true
        /// </summary>
        /// <param name = "pressedList"></param>
        public void GetPressedKeys(List<Keys> pressedList)
        {
            if (pressedList == null)
                throw new ArgumentNullException();
            pressedList.Clear();

            if (_currentFrame.currentState0 != _previousFrame.currentState0) PressList(pressedList, 0);
            if (_currentFrame.currentState1 != _previousFrame.currentState1) PressList(pressedList, 1);
            if (_currentFrame.currentState2 != _previousFrame.currentState2) PressList(pressedList, 2);
            if (_currentFrame.currentState3 != _previousFrame.currentState3) PressList(pressedList, 3);
            if (_currentFrame.currentState4 != _previousFrame.currentState4) PressList(pressedList, 4);
            if (_currentFrame.currentState5 != _previousFrame.currentState5) PressList(pressedList, 5);
            if (_currentFrame.currentState6 != _previousFrame.currentState6) PressList(pressedList, 6);
            if (_currentFrame.currentState7 != _previousFrame.currentState7) PressList(pressedList, 7);
        }

        /// <summary>
        ///   <para>Calls the <paramref name = "callback" /> for each <see cref = "Keys" /> key where <see cref = "Button.IsDown" /> is true</para>
        /// </summary>
        /// <param name = "callback"></param>
        public void GetHeldKeys(Action<Keys> callback)
        {
            if (callback == null)
                throw new ArgumentNullException();

            if (_currentFrame.currentState0 != 0) HeldCallback(callback, 0);
            if (_currentFrame.currentState1 != 0) HeldCallback(callback, 1);
            if (_currentFrame.currentState2 != 0) HeldCallback(callback, 2);
            if (_currentFrame.currentState3 != 0) HeldCallback(callback, 3);
            if (_currentFrame.currentState4 != 0) HeldCallback(callback, 4);
            if (_currentFrame.currentState5 != 0) HeldCallback(callback, 5);
            if (_currentFrame.currentState6 != 0) HeldCallback(callback, 6);
            if (_currentFrame.currentState7 != 0) HeldCallback(callback, 7);
        }

        /// <summary>
        ///   <para>Adds a key to the <paramref name = "heldKeyList" /> for each <see cref = "Keys" /> key where <see cref = "Button.IsDown" /> is true</para>
        ///   <para>The list will be cleared before any keys are added</para>
        /// </summary>
        /// <param name = "heldKeyList"></param>
        public void GetHeldKeys(List<Keys> heldKeyList)
        {
            if (heldKeyList == null)
                throw new ArgumentNullException();
            heldKeyList.Clear();

            if (_currentFrame.currentState0 != 0) HeldList(heldKeyList, 0);
            if (_currentFrame.currentState1 != 0) HeldList(heldKeyList, 1);
            if (_currentFrame.currentState2 != 0) HeldList(heldKeyList, 2);
            if (_currentFrame.currentState3 != 0) HeldList(heldKeyList, 3);
            if (_currentFrame.currentState4 != 0) HeldList(heldKeyList, 4);
            if (_currentFrame.currentState5 != 0) HeldList(heldKeyList, 5);
            if (_currentFrame.currentState6 != 0) HeldList(heldKeyList, 6);
            if (_currentFrame.currentState7 != 0) HeldList(heldKeyList, 7);
        }

        private void PressCallback(Action<Keys> callback, int group)
        {
            for (int i = 0; i < 32; i++)
                if (Buttons[32*group + i].IsPressed)
                    callback((Keys) (32*group + i));
        }

        private void PressList(List<Keys> list, int group)
        {
            for (int i = 0; i < 32; i++)
                if (Buttons[32*group + i].IsPressed)
                    list.Add((Keys) (32*group + i));
        }

        private void HeldCallback(Action<Keys> callback, int group)
        {
            for (int i = 0; i < 32; i++)
                if (Buttons[32*group + i].IsDown)
                    callback((Keys) (32*group + i));
        }

        private void HeldList(List<Keys> list, int group)
        {
            for (int i = 0; i < 32; i++)
                if (Buttons[32*group + i].IsDown)
                    list.Add((Keys) (32*group + i));
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, out char pwszBuff,
                                              int cchBuff, uint wFlags, IntPtr layout);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr layout);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr GetKeyboardLayout(uint thread);
    }
#endif
}