using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Indiefreaks.Xna.Rendering.Gui
{
    /// <summary>
    /// A screen contains multiple controls and supports direct rendering to screen or to a texture for a latter usage such as using a 3D quad
    /// </summary>
    public class Screen : IGuiElement, IUpdate
    {
        /// <summary>
        /// Only one Screen can receive input at a given time, this static field stores it
        /// </summary>
        private static Screen _focusedScreen;

        private readonly List<Control> _controls;
        private readonly List<Control> _focusableControls;
        private readonly PlayerInput _playerInput;
        private readonly Color _spriteBatchColor = Color.White;
        private bool _firstFocusedFrame;
        private int _focusedControlIndex;
        private bool _isDirty = true;
        private float _movementSleep;
        private RenderTarget2D _renderTarget2D;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="renderToScreen">Set it to true to render directly to screen; false to use the Texture property and render the Screen content elsewhere</param>
        /// <param name="isMenu">If set to true, the inner Buttons will react as in a standard menu; false means that you'll control how inner controls react to input events within their Control.HandleInput() method</param>
        /// <param name="playerInput">The player input state instance. One Screen can only be controlled by One PlayerInput instance</param>
        /// <param name="width">The width of the screen, this will be also be used for the inner RenderTarget2D if renderToScreen is set to true</param>
        /// <param name="height">The height of the screen, this will be also be used for the inner RenderTarget2D if renderToScreen is set to true</param>
        public Screen(bool renderToScreen, bool isMenu, PlayerInput playerInput, int width, int height)
        {
            Alpha = 1.0f;

            // we create the render target where the screen will be rendered to
            RendersToScreen = renderToScreen;

            // we create an empty controls list controls will be added to.
            _controls = new List<Control>();

            _focusableControls = new List<Control>();

            // we set the initial visibility status to true
            IsVisible = true;

            CanFocus = true;

            IsMenu = isMenu;

            _playerInput = playerInput;

            Width = width;

            Height = height;

            InputSensibility = 0.2f;
        }

        /// <summary>
        /// Creates a new instance which width and height are set to the current Viewport Width and Height
        /// </summary>
        /// <param name="renderToScreen">Set it to true to render directly to screen; false to use the Texture property and render the Screen content elsewhere</param>
        /// <param name="isMenu">If set to true, the inner Buttons will react as in a standard menu; false means that you'll control how inner controls react to input events within their Control.HandleInput() method</param>
        /// <param name="playerInput">The player input state instance. One Screen can only be controlled by One PlayerInput instance</param>
        public Screen(bool renderToScreen, bool isMenu, PlayerInput playerInput)
            : this(renderToScreen, isMenu, playerInput, Application.Graphics.PreferredBackBufferWidth, Application.Graphics.PreferredBackBufferHeight)
        {
        }

        /// <summary>
        /// Creates a new instance without input support
        /// </summary>
        /// <param name="renderToScreen">Set it to true to render directly to screen; false to use the Texture property and render the Screen content elsewhere</param>
        public Screen(bool renderToScreen) : this(renderToScreen, false, null)
        {
        }

        /// <summary>
        /// Creates a new instance that renders to screen
        /// </summary>
        public Screen() : this(true)
        {
        }

        /// <summary>
        /// Returns if the current Screen is used as a Menu
        /// </summary>
        public bool IsMenu { get; private set; }

        /// <summary>
        /// Returns if the current screen will be rendered directly to the screen or cached in a texture
        /// </summary>
        public bool RendersToScreen { get; private set; }

        /// <summary>
        /// Gets or sets the Alpha value used to render the whole Screen
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// Gets or sets the sensibility of the input device used to handle how fast the focus changes from one control to another
        /// </summary>
        public float InputSensibility { get; set; }

        #region IGuiElement Members

        /// <summary>
        /// Returns the cached rendered Screen as a texture
        /// </summary>
        public Texture2D Texture
        {
            get { return _renderTarget2D; }
        }

        /// <summary>
        /// Tells if the current Screen can receive focus or not
        /// </summary>
        public bool CanFocus { get; private set; }

        /// <summary>
        /// Returns if the current Screen has focus or not
        /// </summary>
        public bool HasFocus { get; set; }

        /// <summary>
        /// Raised when the screen acquires focus
        /// </summary>
        public event EventHandler FocusAcquired;

        /// <summary>
        /// Raised when the screen lost focus
        /// </summary>
        public event EventHandler FocusLost;

        /// <summary>
        /// Sets focus to the current control
        /// </summary>
        public void SetFocus()
        {
            if(!HasFocus)
                OnFocusAcquired();
        }

        /// <summary>
        /// Removes focus from the current control
        /// </summary>
        public void LoseFocus()
        {
            if(HasFocus)
                OnFocusLost();
        }

        /// <summary>
        /// Renders the Screen
        /// </summary>
        /// <param name="spriteRenderer"></param>
        void IGuiElement.Render(SpriteBatch spriteRenderer)
        {
            if (!IsVisible)
                return;

            GraphicsDevice device = spriteRenderer.GraphicsDevice;

            // we render each visible control to their respective RenderTargets
            foreach (Control control in _controls)
            {
                if (control.IsVisible)
                    ((IGuiElement) control).Render(spriteRenderer);
            }

            // we refresh this screen if required
            ((IGuiElement) this).Refresh(device);

            // if we're using a RenderTarget for this screen, we set it to the GraphicsDevice
            if (!RendersToScreen)
            {
                device.SetRenderTarget(_renderTarget2D);
                device.Clear(Color.Transparent);
            }

            spriteRenderer.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            // we render all the cached control textures to their valid positions
            foreach (Control control in _controls)
            {
                if (control.IsVisible)
                {
                    spriteRenderer.Draw(control.Texture,
                                        !RendersToScreen ? new Rectangle(control.X - X, control.Y - Y, control.Width, control.Height) : new Rectangle(control.X + X, control.Y + Y, control.Width, control.Height),
                                        null,
                                        _spriteBatchColor*Alpha,
                                        control.Rotation,
                                        control.Origin,
                                        SpriteEffects.None,
                                        0f);
                }
            }

            spriteRenderer.End();

            // finally, if we are rendering to the RenderTarget, we reset the GraphicsDevice to the backbuffer
            if (!RendersToScreen)
            {
                device.SetRenderTarget(null);
            }
        }

        /// <summary>
        /// Refreshes the Screen properties when it requires to be redrawn to the RenderTarget
        /// </summary>
        /// <param name="device"></param>
        void IGuiElement.Refresh(GraphicsDevice device)
        {
            Refresh(device);
            if (_isDirty)
                PrepareRenderTarget(device);

            _isDirty = false;
        }

        /// <summary>
        /// Marks this Screen as requiring to be refreshed
        /// </summary>
        public void Invalidate()
        {
            _isDirty = true;
        }

        /// <summary>
        /// Returns the Width of the current Screen
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Returns the Height of the current Screen
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Gets or sets the left screen coordinate position of the Screen
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the top screen coordinate position of the Screen
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Returns if the Screen should be rendered or not
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Returns the PlayerInput instance actually controlling this screen
        /// </summary>
        public PlayerInput PlayerInput
        {
            get { return _playerInput; }
        }

        #endregion

        /// <summary>
        /// Raised when B or Back is pressed (only when the Screen is used as a Menu)
        /// </summary>
        public event EventHandler BackButtonPressed;

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFocusAcquired()
        {
            HasFocus = true;
            _firstFocusedFrame = true;

            if (_focusedScreen != null && _focusedScreen != this)
                _focusedScreen.LoseFocus();

            _focusedScreen = this;

            // we specify to the selected menu item that he received focus so that it is displayed on screen
            if (_focusableControls.Count != 0)
                _focusableControls[_focusedControlIndex].SetFocus();

            if (FocusAcquired != null)
                FocusAcquired(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFocusLost()
        {
            HasFocus = false;

            if (_focusableControls.Count != 0)
                _focusableControls[_focusedControlIndex].LoseFocus();

            if (FocusLost != null)
                FocusLost(this, EventArgs.Empty);
        }

        /// <summary>
        /// Refreshes the Screen properties when it requires to be redrawn to the RenderTarget
        /// </summary>
        /// <param name="device"></param>
        public virtual void Refresh(GraphicsDevice device)
        {
        }

        /// <summary>
        /// Creates or recreate the RenderTarget where the screen will be rendered
        /// </summary>
        /// <param name="device"></param>
        public void PrepareRenderTarget(GraphicsDevice device)
        {
#if WINDOWS_PHONE
            const bool useMipMap = false;
#else
            const bool useMipMap = true;
#endif
            if (Width == 0 || Height == 0)
                _renderTarget2D = new RenderTarget2D(device, 100, 100, useMipMap, SurfaceFormat.Color, DepthFormat.None, Application.Graphics.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
            else
                _renderTarget2D = new RenderTarget2D(device, Width, Height, useMipMap, SurfaceFormat.Color, DepthFormat.None, Application.Graphics.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
        }

        /// <summary>
        /// Adds a control to the screen
        /// </summary>
        /// <param name="control"></param>
        public virtual void Add(Control control)
        {
            _controls.Add(control);

            control.Parent = this;

            if (control.CanFocus)
                _focusableControls.Add(control);

            Invalidate();
        }

        /// <summary>
        /// Removes a control from the screen
        /// </summary>
        /// <param name="control"></param>
        public virtual void Remove(Control control)
        {
            _controls.Remove(control);

            if (control.CanFocus)
                _focusableControls.Remove(control);

            Invalidate();
        }

        public virtual void RemoveAll()
        {
            _controls.Clear();
            _focusableControls.Clear();
        }

        #region Implementation of IUpdate

#if WINDOWS_PHONE
        private MouseState _previousMouseState;
        private MouseState _currentMouseState;
#endif

        #region IGuiElement Members

        /// <summary>
        /// Handles the input events from the player controlling the current control
        /// </summary>
        /// <param name="input">The current player input states</param>
        /// <param name="gameTime">The GameTime instance</param>
        public virtual void HandleInput(PlayerInput input, GameTime gameTime)
        {
            if (input.Buttons.B.IsReleased || input.Buttons.Back.IsReleased)
            {
                if (BackButtonPressed != null)
                    BackButtonPressed(this, EventArgs.Empty);
            }

            if (!IsMenu)
            {
                foreach (Control focusableControl in _focusableControls)
                {
                    if (focusableControl.IsVisible)
                        focusableControl.HandleInput(input, gameTime);
                }
            }
            else
            {
#if WINDOWS
                if (input.UseKeyboardMouseInput && Application.Input.IsMouseVisible)
                {
                    var mousePosition = new Point(input.KeyboardMouseState.MouseState.X, input.KeyboardMouseState.MouseState.Y);

                    for (int i = 0; i < _focusableControls.Count; i++)
                    {
                        Control focusableControl = _focusableControls[i];
                        var bounds = new Rectangle(focusableControl.X, focusableControl.Y, focusableControl.Width, focusableControl.Height);
                        if (bounds.Contains(mousePosition))
                        {
                            focusableControl.SetFocus();
                            _focusedControlIndex = i;
                        }
                        else
                        {
                            if (focusableControl.HasFocus)
                                focusableControl.LoseFocus();
                        }

                        if (focusableControl.HasFocus)
                        {
                            if (_movementSleep <= 0.2f)
                                _movementSleep += (float) gameTime.ElapsedGameTime.TotalSeconds;
                            else if (input.Buttons.LeftStickClick.IsReleased)
                                focusableControl.OnClicked();
                            else if (input.ThumbSticks.LeftStick.X < 0 && _movementSleep > InputSensibility)
                                focusableControl.OnInputLeft();
                            else if (input.ThumbSticks.LeftStick.X > 0 && _movementSleep > InputSensibility)
                                focusableControl.OnInputRight();
                            else if (input.ThumbSticks.LeftStick.Y < 0 && _movementSleep > InputSensibility)
                                focusableControl.OnInputDown();
                            else if (input.ThumbSticks.LeftStick.Y > 0 && _movementSleep > InputSensibility)
                                focusableControl.OnInputUp();
                        }
                    }

                    return;
                }
#elif WINDOWS_PHONE

                if (input.UseWindowsPhoneTouch)
                {
                    _previousMouseState = _currentMouseState;
                    _currentMouseState = Mouse.GetState();

                    Point mousePosition;
                    if(_currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);
                    else
                        return;

                    for (int i = 0; i < _focusableControls.Count; i++)
                    {
                        Control focusableControl = _focusableControls[i];

                        var bounds = new Rectangle(focusableControl.X, focusableControl.Y, focusableControl.Width, focusableControl.Height);
                        if (bounds.Contains(mousePosition))
                        {
                            if (focusableControl.HasFocus)
                            {
                                if (_currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && _previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                                    focusableControl.OnClicked();
                            }
                            else
                            {
                                focusableControl.SetFocus();
                                _focusedControlIndex = i;
                            }
                        }
                        else
                        {
                            if (focusableControl.HasFocus)
                            {
                                focusableControl.LoseFocus();
                            }
                        }
                    }

                    return;
                }
#endif

                if(_focusableControls.Count == 0)
                    return;

                if (input.Buttons.A.IsReleased)
                    _focusableControls[_focusedControlIndex].OnClicked();

                int newFocusedControlIndex = _focusedControlIndex;

                if (input.ThumbSticks.LeftStick.Y > 0)
                {
                    if (_movementSleep.Equals(0f) || _movementSleep >= InputSensibility)
                    {
                        newFocusedControlIndex--;

                        if (newFocusedControlIndex < 0)
                            newFocusedControlIndex = 0;

                        _focusableControls[newFocusedControlIndex].OnInputUp();
                        _movementSleep = 0f;
                    }

                    _movementSleep += (float)gameTime.ElapsedGameTime.TotalSeconds;                    
                }
                else if (input.ThumbSticks.LeftStick.Y < 0)
                {
                    if (_movementSleep.Equals(0f) || _movementSleep >= InputSensibility)
                    {
                        newFocusedControlIndex++;

                        if (newFocusedControlIndex > _focusableControls.Count - 1)
                            newFocusedControlIndex = _focusableControls.Count - 1;

                        _focusableControls[newFocusedControlIndex].OnInputDown();
                        _movementSleep = 0f;
                    }

                    _movementSleep += (float)gameTime.ElapsedGameTime.TotalSeconds;                    
                }
                else if (input.ThumbSticks.LeftStick.X > 0)
                {
                    if (_movementSleep.Equals(0f) || _movementSleep >= InputSensibility)
                    {
                        _focusableControls[_focusedControlIndex].OnInputRight();
                        _movementSleep = 0f;
                    }

                    _movementSleep += (float)gameTime.ElapsedGameTime.TotalSeconds;                    
                }
                else if (input.ThumbSticks.LeftStick.X < 0)
                {
                    if (_movementSleep.Equals(0f) || _movementSleep >= InputSensibility)
                    {
                        _focusableControls[_focusedControlIndex].OnInputLeft();
                        _movementSleep = 0f;
                    }

                    _movementSleep += (float)gameTime.ElapsedGameTime.TotalSeconds;                    
                }
                else
                {
                    _movementSleep = 0f;
                }

                if (newFocusedControlIndex != _focusedControlIndex)
                {
                    _focusableControls[_focusedControlIndex].LoseFocus();
                    _focusedControlIndex = newFocusedControlIndex;
                    _focusableControls[_focusedControlIndex].SetFocus();
                }
            }
        }

        #endregion

        #region IUpdate Members

        /// <summary>
        /// Update loop call
        /// </summary>
        /// <param name="gameTime"/>
        public virtual void Update(GameTime gameTime)
        {
            if (!HasFocus || _firstFocusedFrame)
            {
                _firstFocusedFrame = false;
                return;
            }

            if(HasFocus)
                HandleInput(PlayerInput, gameTime);
        }

        #endregion

        #endregion
    }
}