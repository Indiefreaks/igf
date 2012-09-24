using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Gui
{
    /// <summary>
    /// The GuiManager is responsible for the Screens Update and Rendering
    /// </summary>
    public class GuiManager : IGuiManager
    {
        private readonly List<Screen> _screens;
        private readonly SpriteBatch _spriteRenderer;
        private readonly IManagerServiceProvider _sceneInterface;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public GuiManager(IManagerServiceProvider sceneInterface)
        {
            _sceneInterface = sceneInterface;
            
            // we retrieve the SunBurn RenderManager instance process order and set ours adding 2 to it so it always renders after the SunBurn and Custom rendering pipelines
            ManagerProcessOrder = 15;

            // we create an empty list of Screens ready to receive our screens
            _screens = new List<Screen>();

            // we create the SpriteBatch that will be used to render screens and all its inner controls
            _spriteRenderer = new SpriteBatch(SunBurnCoreSystem.Instance.GraphicsDeviceManager.GraphicsDevice);
        }

        #region IManagerService Members

        /// <summary>
        /// Determines which type this manager is registered under in the
        /// SceneInterface that contains it.
        /// 
        /// Please note: changing the return value to the ManagerType of
        /// another class will allow this manager to replace it in the
        /// SceneInterface (and provide replacement features and implementation).
        /// </summary>
        public Type ManagerType
        {
            get { return typeof (IGuiManager); }
        }

        /// <summary>
        /// Sets the order this manager is processed relative to other managers
        /// in the SceneInterface. Managers with lower processing order
        /// values are processed first.
        /// 
        /// In the case of BeginFrameRendering and EndFrameRendering, BeginFrameRendering
        /// is processed in the normal order (lowest value to highest), however
        /// EndFrameRendering is processed in reverse order (highest to lowest) to ensure
        /// the first manager begun is the last one ended (FILO).
        /// 
        /// For managers that do not require a specific order a value of 100 is recommended.
        /// </summary>
        public int ManagerProcessOrder { get; set; }

        #endregion

        #region IRenderableManager Members

        /// <summary>
        /// The current GraphicsDeviceManager used by this object.
        /// </summary>
        public IGraphicsDeviceService GraphicsDeviceManager { get { return SunBurnCoreSystem.Instance.GraphicsDeviceManager; } }

        /// <summary>
        /// Called when the game code sets the manager or SceneInterface preferences.
        /// </summary>
        /// <param name="preferences"></param>
        public void ApplyPreferences(ISystemPreferences preferences)
        {
        }

        /// <summary>
        /// Called when the game begins rendering the current frame.
        /// </summary>
        /// <param name="scenestate"></param>
        public void BeginFrameRendering(ISceneState scenestate)
        {
        }

        /// <summary>
        /// Called when the game finishes rendering the current frame.
        /// </summary>
        public void EndFrameRendering()
        {
            Render();
        }

        /// <summary>
        /// Called when the game clears the engine of objects (generally when
        /// clearing the current level / scene and before loading the next one).
        /// </summary>
        public void Clear()
        {
            _screens.Clear();
        }

        public IManagerServiceProvider OwnerSceneInterface
        {
            get { return _sceneInterface; }
        }

        /// <summary>
        /// Called when the game's graphics and disposable resources are no longer
        /// used or are invalid (due to exiting the game or the graphics device
        /// resetting).  All resources should be disposed before exiting this method.
        /// </summary>
        public void Unload()
        {
            Clear();
        }

        #endregion

        #region IUpdatableManager Members

        /// <summary>
        /// Called during Game.Update() to allow processing at regular intervals.
        /// </summary>
        /// <param name="gametime"></param>
        public void Update(GameTime gametime)
        {
            foreach (Screen screen in _screens)
            {
                if (screen.CanFocus && screen.HasFocus)
                    screen.Update(gametime);
            }
        }

        #endregion

        /// <summary>
        /// Renders the scene.
        /// </summary>
        public void Render()
        {
            // we render all screens
            foreach (Screen screen in _screens)
            {
                if (screen.IsVisible)
                    ((IGuiElement) screen).Render(_spriteRenderer);
            }

            _spriteRenderer.GraphicsDevice.SamplerStates.Reset();
        }

        /// <summary>
        /// Create a new Screen instance, adds it to the Screens managed here and returns it
        /// </summary>
        public void AddScreen(Screen screen)
        {
            _screens.Add(screen);
        }

        /// <summary>
        /// Removes the provided Screen from the GuiManager
        /// </summary>
        /// <param name="screen"></param>
        public void RemoveScreen(Screen screen)
        {
            _screens.Remove(screen);
        }
    }
}