using System;
using System.Collections.Generic;
using System.Text;
using Indiefreaks.Xna.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace Indiefreaks.Xna.Input
{
    /// <summary>
    ///   PlayerInput is a class that stores the current player's input state
    /// </summary>
    /// <remarks>
    ///   It provides automatic input device plug-in/out management through the Connected and Disconnected events
    /// </remarks>
    public class PlayerInput
    {
#if WINDOWS
        private KeyboardMouseState _kms;
        private KeyboardMouseMapping _kmb = new KeyboardMouseMapping();
        private bool _lockMouse;
#endif
        internal InputButtons InputButtons;
        internal InputThumbSticks InputThumbSticks;
        internal InputTriggers InputTriggers;
        private bool _connected;

#if !WINDOWS_PHONE
        internal List<Vibration> _activeVibrations = new List<Vibration>();
        internal List<Vibration> _removeVibrations = new List<Vibration>();
#endif

#if WINDOWS_PHONE
#endif

        internal PlayerInput(PlayerIndex index)
        {
            PlayerIndex = index;
            DeadZone = GamePadDeadZone.IndependentAxes;

            InputTriggers = new InputTriggers();
            InputThumbSticks = new InputThumbSticks();
            InputButtons = new InputButtons();
        }

        /// <summary>
        ///   Returns the Xna PlayerIndex this PlayerInput instance is associated with
        /// </summary>
        /// <remarks>
        ///   <para>PlayerIndex differs from LogicalPlayerIndex because it depends on when an input device is connected/disconnected.</para>
        /// </remarks>
        public PlayerIndex PlayerIndex { get; private set; }

        /// <summary>
        ///   Returns the type of input device used to control this instance
        /// </summary>
        public ControlInput ControlInput { get; internal set; }

        /// <summary>
        /// Gets or sets which GamePadDeadZone is used to retrieve GamePad ThumbSticks input. (GamePadDeadZone.IndependentAxes by default)
        /// </summary>
        public GamePadDeadZone DeadZone { get; set; }

        /// <summary>
        /// </summary>
        public InputTriggers Triggers
        {
            get { return InputTriggers; }
        }

        /// <summary>
        /// </summary>
        public InputThumbSticks ThumbSticks
        {
            get { return InputThumbSticks; }
        }

        /// <summary>
        /// </summary>
        public InputButtons Buttons
        {
            get { return InputButtons; }
        }

#if WINDOWS

        public bool CentreMouseToWindow
        {
            get { return _lockMouse; }
            set { _lockMouse = value; }
        }

        /// <summary>
        ///   Gets or sets if the current Player should use Keyboard and Mouse instead of GamePad
        /// </summary>
        public virtual bool UseKeyboardMouseInput { get; set; }

        /// <summary>
        ///   Current state of the keyboard and mouse. Prefer use of UpdateState.KeyboardState and UpdateState.MouseState.
        /// </summary>
        public KeyboardMouseState KeyboardMouseState
        {
            get { return _kms; }
            internal set { _kms = value; }
        }

        /// <summary>
        ///   Gets/Sets the class used for mapping keyboard/mouse controls to gamepad equivalent controls
        /// </summary>
        public KeyboardMouseMapping VirtualGamePadMapping
        {
            get { return _kmb; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _kmb = value;
            }
        }

        internal void SetKms(InputManager inputManager)
        {
            _kms.WindowFocused = inputManager.WindowFocused;
            _kms.MousePositionPrevious = inputManager.MousePreviousPosition;

            inputManager.DesiredMouseCentered |= _lockMouse && UseKeyboardMouseInput;

            if (_lockMouse && UseKeyboardMouseInput && inputManager.MouseCentred)
            {
                _kms.MousePositionPrevious = inputManager.MouseCentredPosition;
            }

            _kms.MouseState = inputManager.MouseState;
            _kms.KeyboardState = inputManager.KeyboardState;
        }
#endif

#if WINDOWS_PHONE
        private TouchMapping _touchMapping = new TouchMapping();
        
        private List<TouchLocation> _touches; 

        internal void SetTouches(InputManager inputManager)
        {
            _touches = inputManager.CurrentTouches;
        }

        public bool UseWindowsPhoneTouch { get; set; }

        /// <summary>
        ///   Gets/Sets the class used for mapping keyboard/mouse controls to gamepad equivalent controls
        /// </summary>
        public TouchMapping VirtualGamePadMapping
        {
            get { return _touchMapping; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _touchMapping = value;
            }
        }
#endif

        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public void Update(GameTime gameTime)
        {
            CheckConnectionStatus();

            if (!_connected)
            {
#if WINDOWS
                if (UseKeyboardMouseInput)
                {
                    ControlInput = ControlInput.KeyboardMouse;

                    UpdateState(gameTime.TotalGameTime.Ticks, _kms, _kmb);
                }
                else
                    ControlInput = ControlInput.None;
#elif WINDOWS_PHONE
                ControlInput = ControlInput.TouchPanel;
#else
                ControlInput = ControlInput.None;
#endif

                return;
            }

#if WINDOWS
            if (UseKeyboardMouseInput)
            {
                ControlInput = ControlInput.KeyboardMouse;

                UpdateState(gameTime.TotalGameTime.Ticks, _kms, _kmb);

                return;
            }
#endif

#if WINDOWS || XBOX
            ControlInput = (ControlInput) PlayerIndex;

            UpdateState(gameTime.TotalGameTime.Ticks, GamePad.GetState(PlayerIndex, DeadZone));

            UpdateVibration(gameTime.ElapsedGameTime.Ticks);
#elif WINDOWS_PHONE
            ControlInput = ControlInput.TouchPanel;

            UpdateState(gameTime.TotalGameTime.Ticks, GamePad.GetState(PlayerIndex, DeadZone), _touches, _touchMapping);
#endif
        }

        internal void CheckConnectionStatus()
        {
#if WINDOWS
            bool currentConnectionStatus = GamePad.GetCapabilities(PlayerIndex).IsConnected || UseKeyboardMouseInput;
#elif WINDOWS_PHONE
            var currentConnectionStatus = GamePad.GetCapabilities(PlayerIndex).IsConnected || UseWindowsPhoneTouch;
#else
            var currentConnectionStatus = GamePad.GetCapabilities(PlayerIndex).IsConnected;
#endif

            if (currentConnectionStatus && !_connected)
            {
                if (Connected != null)
                    Connected(this, EventArgs.Empty);
            }
            else if (!currentConnectionStatus && _connected)
            {
                if (Disconnected != null)
                    Disconnected(this, EventArgs.Empty);
            }

            _connected = currentConnectionStatus;
        }

#if !WINDOWS_PHONE
        private void UpdateState(long tick, GamePadState gamePadState)
        {
            InputButtons.AButton.SetState(gamePadState.Buttons.A == ButtonState.Pressed, tick);
            InputButtons.BButton.SetState(gamePadState.Buttons.B == ButtonState.Pressed, tick);
            InputButtons.XButton.SetState(gamePadState.Buttons.X == ButtonState.Pressed, tick);
            InputButtons.YButton.SetState(gamePadState.Buttons.Y == ButtonState.Pressed, tick);

            InputButtons.ShoulderLButton.SetState(gamePadState.Buttons.LeftShoulder == ButtonState.Pressed, tick);
            InputButtons.ShoulderRButton.SetState(gamePadState.Buttons.RightShoulder == ButtonState.Pressed, tick);

            InputButtons.DpadDButton.SetState(gamePadState.DPad.Down == ButtonState.Pressed, tick);
            InputButtons.DpadLButton.SetState(gamePadState.DPad.Left == ButtonState.Pressed, tick);
            InputButtons.DpadRButton.SetState(gamePadState.DPad.Right == ButtonState.Pressed, tick);
            InputButtons.DpadUButton.SetState(gamePadState.DPad.Up == ButtonState.Pressed, tick);

            InputButtons.BackButton.SetState(gamePadState.Buttons.Back == ButtonState.Pressed, tick);
            InputButtons.StartButton.SetState(gamePadState.Buttons.Start == ButtonState.Pressed, tick);
            InputButtons.LeftStickClickButton.SetState(gamePadState.Buttons.LeftStick == ButtonState.Pressed, tick);
            InputButtons.RightStickClickButton.SetState(gamePadState.Buttons.RightStick == ButtonState.Pressed, tick);

            InputTriggers.LeftTriggerFloat = gamePadState.Triggers.Left;
            InputTriggers.RightTriggerFloat = gamePadState.Triggers.Right;

            InputThumbSticks.LeftStickVector = gamePadState.ThumbSticks.Left;
            InputThumbSticks.RightStickVector = gamePadState.ThumbSticks.Right;
        }
#endif

#if WINDOWS
        private void UpdateState(long tick, KeyboardMouseState keyboardMouseState, KeyboardMouseMapping mapping)
        {
            InputButtons.AButton.SetState(mapping.A.GetValue(keyboardMouseState), tick);
            InputButtons.BButton.SetState(mapping.B.GetValue(keyboardMouseState), tick);
            InputButtons.XButton.SetState(mapping.X.GetValue(keyboardMouseState), tick);
            InputButtons.YButton.SetState(mapping.Y.GetValue(keyboardMouseState), tick);

            InputButtons.DpadDButton.SetState(mapping.DpadDown.GetValue(keyboardMouseState), tick);
            InputButtons.DpadUButton.SetState(mapping.DpadUp.GetValue(keyboardMouseState), tick);
            InputButtons.DpadLButton.SetState(mapping.DpadLeft.GetValue(keyboardMouseState), tick);
            InputButtons.DpadRButton.SetState(mapping.DpadRight.GetValue(keyboardMouseState), tick);

            InputButtons.ShoulderLButton.SetState(mapping.LeftShoulder.GetValue(keyboardMouseState), tick);
            InputButtons.ShoulderRButton.SetState(mapping.RightShoulder.GetValue(keyboardMouseState), tick);

            InputButtons.BackButton.SetState(mapping.Back.GetValue(keyboardMouseState), tick);
            InputButtons.StartButton.SetState(mapping.Start.GetValue(keyboardMouseState), tick);
            InputButtons.LeftStickClickButton.SetState(mapping.LeftStickClick.GetValue(keyboardMouseState), tick);
            InputButtons.RightStickClickButton.SetState(mapping.RightStickClick.GetValue(keyboardMouseState), tick);

            InputTriggers.LeftTriggerFloat = mapping.LeftTrigger.GetValue(keyboardMouseState, false);
            InputTriggers.RightTriggerFloat = mapping.RightTrigger.GetValue(keyboardMouseState, false);

            var v = new Vector2();

            v.Y = mapping.LeftStickForward.GetValue(keyboardMouseState, false) +
                  mapping.LeftStickBackward.GetValue(keyboardMouseState, true);
            v.X = mapping.LeftStickLeft.GetValue(keyboardMouseState, true) +
                  mapping.LeftStickRight.GetValue(keyboardMouseState, false);
            InputThumbSticks.LeftStickVector = v;

            v.Y = mapping.RightStickForward.GetValue(keyboardMouseState, false) +
                  mapping.RightStickBackward.GetValue(keyboardMouseState, true);
            v.X = mapping.RightStickLeft.GetValue(keyboardMouseState, true) +
                  mapping.RightStickRight.GetValue(keyboardMouseState, false);
            InputThumbSticks.RightStickVector = v;
        }
#endif

#if WINDOWS_PHONE
        private void UpdateState(long tick, GamePadState gamePadState, List<TouchLocation> touches, TouchMapping mapping)
        {
            InputButtons.AButton.SetState(mapping.A.TouchArea != Rectangle.Empty && mapping.A.GetBooleanValue(touches), tick);
            InputButtons.BButton.SetState(mapping.B.TouchArea != Rectangle.Empty && mapping.B.GetBooleanValue(touches), tick);
            InputButtons.XButton.SetState(mapping.X.TouchArea != Rectangle.Empty && mapping.X.GetBooleanValue(touches), tick);
            InputButtons.YButton.SetState(mapping.Y.TouchArea != Rectangle.Empty && mapping.Y.GetBooleanValue(touches), tick);

            InputButtons.DpadDButton.SetState(mapping.DpadDown.TouchArea != Rectangle.Empty && mapping.DpadDown.GetBooleanValue(touches), tick);
            InputButtons.DpadUButton.SetState(mapping.DpadUp.TouchArea != Rectangle.Empty && mapping.DpadUp.GetBooleanValue(touches), tick);
            InputButtons.DpadLButton.SetState(mapping.DpadLeft.TouchArea != Rectangle.Empty && mapping.DpadLeft.GetBooleanValue(touches), tick);
            InputButtons.DpadRButton.SetState(mapping.DpadRight.TouchArea != Rectangle.Empty && mapping.DpadRight.GetBooleanValue(touches), tick);

            InputButtons.ShoulderLButton.SetState(mapping.LeftShoulder.TouchArea != Rectangle.Empty && mapping.LeftShoulder.GetBooleanValue(touches), tick);
            InputButtons.ShoulderRButton.SetState(mapping.RightShoulder.TouchArea != Rectangle.Empty && mapping.RightShoulder.GetBooleanValue(touches), tick);

            InputButtons.BackButton.SetState(gamePadState.Buttons.Back == ButtonState.Pressed, tick);

            InputButtons.StartButton.SetState(mapping.Start.TouchArea != Rectangle.Empty && mapping.Start.GetBooleanValue(touches), tick);
            InputButtons.LeftStickClickButton.SetState(mapping.LeftStickClick.TouchArea != Rectangle.Empty && mapping.LeftStickClick.GetBooleanValue(touches), tick);
            InputButtons.RightStickClickButton.SetState(mapping.RightStickClick.TouchArea != Rectangle.Empty && mapping.RightStickClick.GetBooleanValue(touches), tick);

            InputTriggers.LeftTriggerFloat = mapping.LeftTrigger.TouchArea == Rectangle.Empty ? 0f : mapping.LeftTrigger.GetFloatValue(touches);
            InputTriggers.RightTriggerFloat = mapping.RightTrigger.TouchArea == Rectangle.Empty ? 0f : mapping.RightTrigger.GetFloatValue(touches);

            InputThumbSticks.LeftStickVector = mapping.LeftStick.TouchArea == Rectangle.Empty ? Vector2.Zero : mapping.LeftStick.GetVector2Value(touches, DeadZone);
            InputThumbSticks.RightStickVector = mapping.RightStick.TouchArea == Rectangle.Empty ? Vector2.Zero : mapping.RightStick.GetVector2Value(touches, DeadZone);
        }
#endif

#if WINDOWS || XBOX
        /// <summary>
        /// Add a vibration to the player input controller
        /// </summary>
        /// <param name="vibration">The vibration</param>
        public void AddVibration(Vibration vibration)
        {
            _activeVibrations.Add(vibration);
        }

        /// <summary>
        /// Add a vibration to the player input controller
        /// </summary>
        /// <param name="targetMotor">The target motor</param>
        /// <param name="frequency">The motor frequency.</param>
        /// <param name="duration">The duration of the vibration</param>
        public void AddVibration(Vibration.Motor targetMotor, float frequency, float duration)
        {
            var vibration = new Vibration(targetMotor, frequency, duration);
            _activeVibrations.Add(vibration);
        }

        /// <summary>
        /// Add a vibration to the player input controller
        /// </summary>
        /// <param name="targetMotor">The target motor</param>
        /// <param name="frequency">The motor frequency.</param>
        /// <param name="duration">The duration of the vibration</param>
        /// <param name="modifier">Frequency modifier delegate</param>
        public void AddVibration(Vibration.Motor targetMotor, float frequency, float duration, Func<Vibration, float> modifier)
        {
            var vibration = new Vibration(targetMotor, frequency, duration, modifier);
            _activeVibrations.Add(vibration);
        }

        private void UpdateVibration(long ticks)
        {
            // Convert ticks back to milliseconds
            float dt = ticks/TimeSpan.TicksPerMillisecond;
            _removeVibrations.Clear();

            foreach (Vibration vibration in _activeVibrations)
            {
                vibration.Update(ticks);
                if (vibration.TimeRemaining < 0)
                {
                    _removeVibrations.Add(vibration);
                }
            }
            foreach (Vibration vibration in _removeVibrations)
            {
                _activeVibrations.Remove(vibration);
            }
            float[] motors = {0, 0};
            foreach (Vibration vibration in _activeVibrations)
            {
                motors[(int) vibration.TargetMotor] = Math.Max(motors[(int) vibration.TargetMotor], vibration.ComputedFrequency);
            }
            GamePad.SetVibration(PlayerIndex, motors[0], motors[1]);
        }
#endif
    }
}