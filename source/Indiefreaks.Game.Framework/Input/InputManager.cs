using System;
using System.Collections.Generic;
using System.Diagnostics;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace Indiefreaks.Xna.Input
{
    /// <summary>
    ///   The InputManager is responsible of updating player inputs and ensure it complies to XBLIG input compliancy
    /// </summary>
    /// <remarks>
    ///   <para>In XBLIG, in order to pass Peer Review, a game must ensure that when a GamePad is disconnected, the game is aware of it 
    ///     and can pause if there are no other devices connected. If a GamePad is connected during a game, it must also be notified and
    ///     allow the newly plugged device to play</para>
    ///   <para>InputServices handles just that through a set of Connection, Disconnection events and a LogicalPlayerIndex enumeration
    ///     you can use to know which PlayerIndex is in your game</para>
    /// </remarks>
#if !WINDOWS_PHONE
    public class InputManager : GameComponent
#else
    public class InputManager : DrawableGameComponent
#endif
    {
#if WINDOWS
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private readonly KeyboardInputState _keyboard = new KeyboardInputState();
        private readonly MouseInputState _mouse = new MouseInputState();
        private Point _mousePrev;
        private bool _windowFocused = true, _mousePosSet;
        private int _focusSkip;
        private static bool _centreMouse, _mouseCentred, _centreMousePrevious;
        private static Point _mouseCentrePoint;
#elif WINDOWS_PHONE
        private readonly List<TouchLocation> _currentTouches = new List<TouchLocation>();

        public List<TouchLocation> CurrentTouches { get { return _currentTouches; } } 
#endif

        private readonly Dictionary<PlayerIndex, PlayerInput> _connectedPlayers;
        private readonly NoPlayerInput _noInputDeviceConnectedPlayer1;
        private readonly NoPlayerInput _noInputDeviceConnectedPlayer2;
        private readonly NoPlayerInput _noInputDeviceConnectedPlayer3;
        private readonly NoPlayerInput _noInputDeviceConnectedPlayer4;

        protected internal readonly PlayerInput Player1;
        protected internal readonly PlayerInput Player2;
        protected internal readonly PlayerInput Player3;
        protected internal readonly PlayerInput Player4;

        /// <summary>
        ///   Creates a new instance of the InputManager component and adds itself to the game's component collection as well as Service
        /// </summary>
        /// <param name = "application">The game instance</param>
        public InputManager(Application application)
            : base(application)
        {
            if (application == null)
                throw new ArgumentNullException("application", "InputManager requires a valid application instance");

            if (Application.Input != null)
                throw new CoreException("There already is an InputManager instance.");
            application.Components.Add(this);
            application.Services.AddService(typeof (InputManager), this);

            Player1 = new PlayerInput(PlayerIndex.One);
            Player1.Connected += OnPlayerConnected;
            Player1.Disconnected += OnPlayerDisconnected;

            Player2 = new PlayerInput(PlayerIndex.Two);
            Player2.Connected += OnPlayerConnected;
            Player2.Disconnected += OnPlayerDisconnected;

            Player3 = new PlayerInput(PlayerIndex.Three);
            Player3.Connected += OnPlayerConnected;
            Player3.Disconnected += OnPlayerDisconnected;

            Player4 = new PlayerInput(PlayerIndex.Four);
            Player4.Connected += OnPlayerConnected;
            Player4.Disconnected += OnPlayerDisconnected;

            _noInputDeviceConnectedPlayer1 = new NoPlayerInput(PlayerIndex.One);
            _noInputDeviceConnectedPlayer2 = new NoPlayerInput(PlayerIndex.Two);
            _noInputDeviceConnectedPlayer3 = new NoPlayerInput(PlayerIndex.Three);
            _noInputDeviceConnectedPlayer4 = new NoPlayerInput(PlayerIndex.Four);

            _connectedPlayers = new Dictionary<PlayerIndex, PlayerInput>();

            Player1.CheckConnectionStatus();
            Player2.CheckConnectionStatus();
            Player3.CheckConnectionStatus();
            Player4.CheckConnectionStatus();

#if WINDOWS
            Game.Activated += delegate { _windowFocused = true; };
            Game.Deactivated += delegate { _windowFocused = false; };
            _windowFocused = true;
#elif WINDOWS_PHONE
            VirtualGamePadSkin = new VirtualGamePadSkin();
#endif
        }

        /// <summary>
        ///   Returns true if at least one Input device is connected; false otherwise
        /// </summary>
        public bool IsInputDevicePlugged { get; private set; }

        /// <summary>
        ///   Returns the Player 1 PlayerInput instance
        /// </summary>
        public PlayerInput PlayerOne
        {
            get { return !_connectedPlayers.ContainsKey(PlayerIndex.One) ? _noInputDeviceConnectedPlayer1 : _connectedPlayers[PlayerIndex.One]; }
        }

        /// <summary>
        ///   Returns the Player 2 PlayerInput instance
        /// </summary>
        public PlayerInput PlayerTwo
        {
            get { return !_connectedPlayers.ContainsKey(PlayerIndex.Two) ? _noInputDeviceConnectedPlayer2 : _connectedPlayers[PlayerIndex.Two]; }
        }


        /// <summary>
        ///   Returns the Player 3 PlayerInput instance
        /// </summary>
        public PlayerInput PlayerThree
        {
            get { return !_connectedPlayers.ContainsKey(PlayerIndex.Three) ? _noInputDeviceConnectedPlayer3 : _connectedPlayers[PlayerIndex.Three]; }
        }

        /// <summary>
        ///   Returns the Player 4 PlayerInput instance
        /// </summary>
        public PlayerInput PlayerFour
        {
            get { return !_connectedPlayers.ContainsKey(PlayerIndex.Four) ? _noInputDeviceConnectedPlayer4 : _connectedPlayers[PlayerIndex.Four]; }
        }

        public PlayerInput GetPlayerInput(PlayerIndex playerIndex)
        {
            switch(playerIndex)
            {
                case PlayerIndex.Four:
                    return Player4;
                case PlayerIndex.Three:
                    return Player3;
                case PlayerIndex.Two:
                    return Player2;
                case PlayerIndex.One:
                    return Player1;
                default:
                    return null;
            }
        }
        
#if WINDOWS

        public KeyboardInputState KeyboardState
        {
            get { return _keyboard; }
        }

        public bool IsMouseVisible
        {
            get { return Game.IsMouseVisible; }
            set { Game.IsMouseVisible = value; }
        }

        public bool UseKeyboardAndMouse { get; set; }

        internal bool DesiredMouseCentered { get; set; }

        internal bool MouseCentred
        {
            get { return _mouseCentred && _windowFocused; }
        }

        internal Point MouseCentredPosition
        {
            get { return _mouseCentrePoint; }
        }

        internal bool WindowFocused
        {
            get { return _windowFocused; }
        }

        internal Point MousePreviousPosition
        {
            get { return _mousePrev; }
        }

        /// <summary>
        ///   [Windows Only]
        ///   Using <see cref = "PlayerInput" /> is recommended over direct state access
        /// </summary>
        public MouseInputState MouseState
        {
            get { return _mouse; }
        }
#endif

        /// <summary>
        ///   Subscribe to this event if you are looking to get noticed when no Input devices are connected
        /// </summary>
        /// <remarks>
        ///   Useful if you want to pause the game until an input device gets connected. You can then test for InputManager.IsInputDeviceConnected
        ///   property every other frame (i.e. in the Update call) to see if at least one input device is connected
        /// </remarks>
        public event EventHandler AllInputDevicesDisconnected;
        
        /// <summary>
        ///   Called when an input device gets connected to the machine.
        /// </summary>
        private void OnPlayerConnected(object sender, EventArgs e)
        {
            var playerInput = (PlayerInput) sender;
            
            _connectedPlayers.Add(playerInput.PlayerIndex, playerInput);
            
            IsInputDevicePlugged = _connectedPlayers.Count > 0;
        }

        /// <summary>
        ///   Called when an input device gets disconnected to the machine.
        /// </summary>
        private void OnPlayerDisconnected(object sender, EventArgs e)
        {
            var playerInput = (PlayerInput) sender;

            _connectedPlayers.Remove(playerInput.PlayerIndex);
            
            IsInputDevicePlugged = _connectedPlayers.Count > 0;

            if (!IsInputDevicePlugged)
                OnAllInputDevicesDisconnected();
        }

        /// <summary>
        ///   Called when no input devices are connected to the machine.
        /// </summary>
        private void OnAllInputDevicesDisconnected()
        {
            if (AllInputDevicesDisconnected != null)
                AllInputDevicesDisconnected(this, EventArgs.Empty);
        }

        /// <summary>
        ///   Updates the object and its contained resources.
        /// </summary>
        /// <param name = "gameTime" />
        public override void Update(GameTime gameTime)
        {
#if WINDOWS
            long ticks = gameTime.TotalGameTime.Ticks;
            UpdateWindowsInput(ticks);
#elif WINDOWS_PHONE
            UpdateWindowsTouchInput();
#endif
            Player1.Update(gameTime);
            Player2.Update(gameTime);
            Player3.Update(gameTime);
            Player4.Update(gameTime);
        }

#if WINDOWS
        private void UpdateWindowsInput(long tick)
        {
            _mousePrev = new Point(_mouse.X, _mouse.Y);

            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

            var p = new Point(Game.Window.ClientBounds.Width/2, Game.Window.ClientBounds.Height/2);

            if (_centreMouse && _windowFocused)
                Mouse.SetPosition(p.X, p.Y);

            if (_centreMousePrevious)
            {
                _mouseCentred = true;
                _mouseCentrePoint = p;
            }

            _centreMousePrevious = _centreMouse;

            if (!_windowFocused)
                _focusSkip = 5;
            if (!_mousePosSet || _focusSkip > 0)
            {
                _focusSkip--;
                _mousePrev = new Point(_mouse.X, _mouse.Y);
                _mouseCentrePoint = _mousePrev;
                _mousePosSet = true;
            }

            _centreMouse = DesiredMouseCentered && WindowFocused;

            _keyboard.Update(tick, ref _keyboardState);
            _mouse.Update(tick, ref _mouseState);

            Player1.SetKms(this);
            Player2.SetKms(this);
            Player3.SetKms(this);
            Player4.SetKms(this);
        }
#endif

#if WINDOWS_PHONE
        private SpriteBatch _virtualGamePadRenderer;

        private void UpdateWindowsTouchInput()
        {
            _currentTouches.Clear();

            var touches = TouchPanel.GetState();
            foreach (var touche in touches)
            {
                _currentTouches.Add(touche);
            }

            Player1.SetTouches(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _virtualGamePadRenderer = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _virtualGamePadRenderer.Begin();

            DrawStick(VirtualGamePadSkin.LeftStick, PlayerOne.VirtualGamePadMapping.LeftStick, elapsedTime);
            DrawStick(VirtualGamePadSkin.RightStick, PlayerOne.VirtualGamePadMapping.RightStick, elapsedTime);
            DrawButton(VirtualGamePadSkin.A, PlayerOne.VirtualGamePadMapping.A, PlayerOne.Buttons.A, elapsedTime);
            DrawButton(VirtualGamePadSkin.B, PlayerOne.VirtualGamePadMapping.B, PlayerOne.Buttons.B, elapsedTime);
            DrawButton(VirtualGamePadSkin.X, PlayerOne.VirtualGamePadMapping.X, PlayerOne.Buttons.X, elapsedTime);
            DrawButton(VirtualGamePadSkin.Y, PlayerOne.VirtualGamePadMapping.Y, PlayerOne.Buttons.Y, elapsedTime);
            DrawButton(VirtualGamePadSkin.DpadLeft, PlayerOne.VirtualGamePadMapping.DpadLeft, PlayerOne.Buttons.DpadLeft, elapsedTime);
            DrawButton(VirtualGamePadSkin.DpadRight, PlayerOne.VirtualGamePadMapping.DpadRight, PlayerOne.Buttons.DpadRight, elapsedTime);
            DrawButton(VirtualGamePadSkin.DpadUp, PlayerOne.VirtualGamePadMapping.DpadUp, PlayerOne.Buttons.DpadUp, elapsedTime);
            DrawButton(VirtualGamePadSkin.DpadDown, PlayerOne.VirtualGamePadMapping.DpadDown, PlayerOne.Buttons.DpadDown, elapsedTime);
            DrawButton(VirtualGamePadSkin.LeftShoulder, PlayerOne.VirtualGamePadMapping.LeftShoulder, PlayerOne.Buttons.LeftShoulder, elapsedTime);
            DrawButton(VirtualGamePadSkin.RightShoulder, PlayerOne.VirtualGamePadMapping.RightShoulder, PlayerOne.Buttons.RightShoulder, elapsedTime);
            DrawButton(VirtualGamePadSkin.Start, PlayerOne.VirtualGamePadMapping.Start, PlayerOne.Buttons.Start, elapsedTime);
            DrawButton(VirtualGamePadSkin.LeftTrigger, PlayerOne.VirtualGamePadMapping.LeftTrigger, PlayerOne.Triggers.LeftTrigger, elapsedTime);
            DrawButton(VirtualGamePadSkin.RightTrigger, PlayerOne.VirtualGamePadMapping.RightTrigger, PlayerOne.Triggers.RightTrigger, elapsedTime);

            _virtualGamePadRenderer.End();
        }

        private void DrawStick(VirtualGamePadStickSkin stickSkin, TouchMap touchMap, float elapsedTime)
        {
            if (stickSkin.AreaTexture == null || touchMap.TouchArea == Rectangle.Empty) return;

            var areaTexture = stickSkin.AreaTexture;
            Vector2 areaPosition, thumbPosition = Vector2.Zero;

            if(touchMap.Static)
            {
                areaPosition = new Vector2(touchMap.TouchArea.X + touchMap.TouchArea.Width/2f, touchMap.TouchArea.Y + touchMap.TouchArea.Height/2f);
            }
            else
            {
                areaPosition = touchMap.CenterPosition - new Vector2(areaTexture.Width/2f, areaTexture.Height/2f);
            }

            if(stickSkin.ThumbStickTexture != null)
            {
                if (touchMap.Id != -1)
                {
                    var direction = (touchMap.CenterPosition - touchMap.CurrentPosition) / stickSkin.MaxThumbStickDistance;
                    if(direction.LengthSquared() > 1f)
                        direction.Normalize();

                    thumbPosition = touchMap.CenterPosition - (direction * stickSkin.MaxThumbStickDistance) - new Vector2(stickSkin.ThumbStickTexture.Width / 2f, stickSkin.ThumbStickTexture.Height / 2f);
                }
                else
                {
                    thumbPosition = touchMap.CenterPosition - new Vector2(stickSkin.ThumbStickTexture.Width / 2f, stickSkin.ThumbStickTexture.Height / 2f);
                }
            }

            if (stickSkin.SensibleVisibility || touchMap.Id != -1)
            {
                stickSkin.Opacity = stickSkin.MaxOpacity;
            }
            else
            {
                stickSkin.Opacity -= elapsedTime;
                if (stickSkin.Opacity < stickSkin.MinOpacity)
                    stickSkin.Opacity = stickSkin.MinOpacity;
            }

            _virtualGamePadRenderer.Draw(
                areaTexture,
                areaPosition,
                Color.White * stickSkin.Opacity);

            if (stickSkin.ThumbStickTexture != null)
            {
                var thumbStickTexture = stickSkin.ThumbStickTexture;
                _virtualGamePadRenderer.Draw(
                    thumbStickTexture,
                    thumbPosition,
                    Color.White * stickSkin.Opacity);
            }
        }

        private void DrawButton(VirtualGamePadButtonSkin buttonSkin, TouchMap touchMap, Button button, float elapsedTime)
        {
            if (buttonSkin.SensibleVisibility || touchMap.Id != -1)
            {
                buttonSkin.Opacity = buttonSkin.MaxOpacity;
            }
            else
            {
                buttonSkin.Opacity -= elapsedTime;
                if (buttonSkin.Opacity < buttonSkin.MinOpacity)
                    buttonSkin.Opacity = buttonSkin.MinOpacity;
            }

            if (button.IsDown)
            {
                if (buttonSkin.PressedTexture != null && touchMap.TouchArea != Rectangle.Empty)
                {
                    _virtualGamePadRenderer.Draw(
                        buttonSkin.PressedTexture,
                        touchMap.TouchArea,
                        Color.White*buttonSkin.Opacity);
                }
            }
            else
            {
                if (buttonSkin.NormalTexture != null && touchMap.TouchArea != Rectangle.Empty)
                {
                    _virtualGamePadRenderer.Draw(
                        buttonSkin.NormalTexture,
                        touchMap.TouchArea,
                        Color.White*buttonSkin.Opacity);
                }
            }
        }

        public void DrawButton(VirtualGamePadButtonSkin buttonSkin, TouchMap touchMap, float value, float elapsedTime)
        {
            if (buttonSkin.SensibleVisibility || touchMap.Id != -1)
            {
                buttonSkin.Opacity = buttonSkin.MaxOpacity;
            }
            else
            {
                buttonSkin.Opacity -= elapsedTime;
                if (buttonSkin.Opacity < buttonSkin.MinOpacity)
                    buttonSkin.Opacity = buttonSkin.MinOpacity;
            }

            if (value > 0)
            {
                if (buttonSkin.PressedTexture != null && touchMap.TouchArea != Rectangle.Empty)
                {
                    _virtualGamePadRenderer.Draw(
                        buttonSkin.PressedTexture,
                        touchMap.TouchArea,
                        Color.White * buttonSkin.Opacity);
                }
            }
            else
            {
                if (buttonSkin.NormalTexture != null && touchMap.TouchArea != Rectangle.Empty)
                {
                    _virtualGamePadRenderer.Draw(
                        buttonSkin.NormalTexture,
                        touchMap.TouchArea,
                        Color.White * buttonSkin.Opacity);
                }
            }
        }

        public VirtualGamePadSkin VirtualGamePadSkin { get; private set; }
#endif
    }
}