using System;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Gui
{
    public abstract class Control : IGuiElement, IContentHost
    {
        private bool _isDirty = true;
        private RenderTarget2D _renderTarget2D;
        private float _rotation;

        /// <summary>
        /// Creates a new control instance
        /// </summary>
        protected Control()
        {
            X = 0;
            Y = 0;
            Width = 100;
            Height = 100;
            Scale = Vector2.One;
            Rotation = 0f;
            Origin = Vector2.Zero;
            IsVisible = true;
        }

        public abstract Control Clone();

        /// <summary>
        /// Gets or sets the scale factor to be applied to this control
        /// </summary>
        public Vector2 Scale { get; set; }
        
        /// <summary>
        /// Gets or sets the rotation angle (in radians) of the control
        /// </summary>
        public float Rotation
        {
            get { return _rotation;}
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the origin of the control (defaults to Vector2.Zero)
        /// </summary>
        public Vector2 Origin { get; set; }

        #region IGuiElement Members

        /// <summary>
        /// Returns the Width of the current control
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Returns the Height of the current control
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Gets or sets the left screen coordinate position of the control
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the top screen coordinate position of the control
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the visibility of this control
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Returns the cached rendered control as a texture
        /// </summary>
        /// <returns></returns>
        public Texture2D Texture
        {
            get { return _renderTarget2D; }
        }

        /// <summary>
        /// Tells if the current control can receive focus or not
        /// </summary>
        public virtual bool CanFocus
        {
            get { return false; }
        }

        /// <summary>
        /// Returns if the current control has focus or not
        /// </summary>
        public bool HasFocus { get; internal set; }

        /// <summary>
        /// Raised when the Control acquired focus and switches to Selected
        /// </summary>
        public event EventHandler FocusAcquired;

        /// <summary>
        /// Raised when the Control loses focus and switches back to Normal
        /// </summary>
        public event EventHandler FocusLost;

        /// <summary>
        /// Sets focus to the current control
        /// </summary>
        public void SetFocus()
        {
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
        /// Renders the control
        /// </summary>
        /// <param name="spriteRenderer"></param>
        void IGuiElement.Render(SpriteBatch spriteRenderer)
        {
            // if the control needs to be rendered back to the RenderTarget
            if (_isDirty && IsVisible)
            {
                GraphicsDevice device = spriteRenderer.GraphicsDevice;
                // we refresh the control properties
                ((IGuiElement) this).Refresh(device);

                device.SetRenderTarget(_renderTarget2D);
                device.Clear(Color.Transparent);

                spriteRenderer.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Render(spriteRenderer);
                spriteRenderer.End();

                device.SetRenderTarget(null);
                _isDirty = false;
            }
        }

        /// <summary>
        /// Refreshes the control properties when it requires to be redrawn to the RenderTarget
        /// </summary>
        /// <param name="device"></param>
        void IGuiElement.Refresh(GraphicsDevice device)
        {
            Refresh(device);
            PrepareRenderTarget(device);
        }

        /// <summary>
        /// Marks this control as requiring to be refreshed
        /// </summary>
        public void Invalidate()
        {
            _isDirty = true;
        }

        /// <summary>
        /// Handles the input events from the player controlling the current control
        /// </summary>
        /// <param name="input">The current player input states</param>
        /// <param name="gameTime">The GameTime instance</param>
        public virtual void HandleInput(PlayerInput input, GameTime gameTime)
        {
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnClicked()
        {
            if (Clicked != null)
                Clicked(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raised when the control receives an input clicked event
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        /// Rauised when the control receives an input left event
        /// </summary>
        public event EventHandler InputLeft;

        /// <summary>
        /// Rauised when the control receives an input right event
        /// </summary>
        public event EventHandler InputRight;

        /// <summary>
        /// Rauised when the control receives an input up event
        /// </summary>
        public event EventHandler InputUp;

        /// <summary>
        /// Rauised when the control receives an input down event
        /// </summary>
        public event EventHandler InputDown;

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnInputRight()
        {
            if (InputRight != null) InputRight(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnInputLeft()
        {
            if (InputLeft != null) InputLeft(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnInputUp()
        {
            if (InputUp != null) InputUp(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnInputDown()
        {
            if (InputDown != null) InputDown(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnFocusAcquired()
        {
            HasFocus = true;

            if (FocusAcquired != null)
                FocusAcquired(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnFocusLost()
        {
            HasFocus = false;

            if (FocusLost != null)
                FocusLost(this, EventArgs.Empty);
        }

        /// <summary>
        /// Refreshes the control properties when it requires to be redrawn to the RenderTarget
        /// </summary>
        /// <param name="device"></param>
        public virtual void Refresh(GraphicsDevice device)
        {
        }

        /// <summary>
        /// Creates or recreate the RenderTarget where the control will be rendered
        /// </summary>
        /// <param name="device"></param>
        public void PrepareRenderTarget(GraphicsDevice device)
        {
#if WINDOWS_PHONE
            const bool useMipMap = false;
            var width = Math.Min(Width, 2048);
            var height = Math.Min(Height, 2048);
#else
            const bool useMipMap = true;
            var width = Math.Min(Width, 4096);
            var height = Math.Min(Height, 4096);
#endif
            _renderTarget2D = new RenderTarget2D(device, Math.Max(width, 1), Math.Max(height,1), useMipMap, SurfaceFormat.Color, DepthFormat.None, Application.Graphics.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
        }

        /// <summary>
        /// Renders the control
        /// </summary>
        /// <param name="spriteRenderer"></param>
        public virtual void Render(SpriteBatch spriteRenderer)
        {
        }

        #region Implementation of IContentHost

        /// <summary>
        ///   Load all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        /// <param name = "manager">XNA content manage</param>
        public abstract void LoadContent(IContentCatalogue catalogue, ContentManager manager);

        /// <summary>
        ///   Unload all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        public abstract void UnloadContent(IContentCatalogue catalogue);

        #endregion

        public IGuiElement Parent { get; internal set; }
    }
}