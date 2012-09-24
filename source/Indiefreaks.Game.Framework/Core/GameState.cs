using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Rendering;
using Indiefreaks.Xna.Rendering.Camera;
using Indiefreaks.Xna.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Core
{
    public abstract class GameState : IUpdate, IDraw, IDisposable
    {
        protected internal readonly List<IContentHost> ContentToLoad;
        protected readonly List<ILayer> Layers = new List<ILayer>();
        private readonly FrameBuffers _frameBuffers;
        private readonly ContentManager _localContentManager;
        protected internal bool LoadingComplete;
        protected internal bool LoadingInProgress;
        private float _loadingPercentage;
        private Task _loadingTask;

        protected GameState(string name, Application application, bool useApplicationContent, int bufferWidth, int bufferHeight, DetailPreference precisionMode, DetailPreference lightingRange)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("No valid name was provided to the GameState", "name");

            if (application == null)
                throw new ArgumentNullException("application", "No valid Application instance provided to the GameState");

            Name = name;
            Application = application;


            if (useApplicationContent)
            {
                Content = new ContentCatalogue(application);
            }
            else
            {
                _localContentManager = new ContentManager(application.Services, application.Content.RootDirectory);
                Content = new ContentCatalogue(application, _localContentManager);
            }
            // We create the IContentOwner List that will get populated with Content to be loaded with the GameState creation
            ContentToLoad = new List<IContentHost>();

            // create the SceneInterface instance from the Application
            SunBurn = Application.SunBurn;

            // create the Framebuffers and assigns the ownership to the SceneInterface
            _frameBuffers = new FrameBuffers(bufferWidth, bufferHeight, precisionMode, lightingRange);
            SunBurn.ResourceManager.AssignOwnership(_frameBuffers);
        }

        /// <summary>
        ///   Creates a new GameState instance
        /// </summary>
        /// <param name = "name">The name of the GameState</param>
        /// <param name = "application">The current Application instance</param>
        protected GameState(string name, Application application)
#if !WINDOWS_PHONE
            : this(name, application, 1152, 640, DetailPreference.High, DetailPreference.High)
#else
            : this(name, application, application.GraphicsDeviceManager.PreferredBackBufferWidth, application.GraphicsDeviceManager.PreferredBackBufferHeight, DetailPreference.High, DetailPreference.High)
#endif
        {
        }

        /// <summary>
        ///   Creates a new instance of the SunBurnGameState
        /// </summary>
        /// <param name = "name">The name of the GameState</param>
        /// <param name = "application">The Application instance</param>
        /// <param name = "bufferWidth">Custom buffer width</param>
        /// <param name = "bufferHeight">Custom buffer height</param>
        /// <param name = "precisionMode">Increases visual quality at the cost of performance.  Generally used in visualizations, most games do not need this option.</param>
        /// <param name = "lightingRange">Increases lighting quality at the cost of performance.  Adds additional lighting range when using HDR.</param>
        protected GameState(string name, Application application, int bufferWidth, int bufferHeight,
                            DetailPreference precisionMode, DetailPreference lightingRange) : this(name, application, false, bufferWidth, bufferHeight, precisionMode, lightingRange)
        {
        }

        public FrameBuffers FrameBuffers
        {
            get { return _frameBuffers; }
        }

        /// <summary>
        ///   Returns the Loading percentage.
        /// </summary>
        public float LoadingPercentage
        {
            get
            {
                lock (this)
                {
                    return _loadingPercentage;
                }
            }
        }

        /// <summary>
        ///   Returns true if the GameState finished loading; false otherwise
        /// </summary>
        public bool IsLoadingComplete
        {
            get
            {
                lock (this)
                {
                    return LoadingComplete;
                }
            }
        }

        /// <summary>
        ///   Returns the GameState's ContentCatalogue
        /// </summary>
        public ContentCatalogue Content { get; private set; }

        /// <summary>
        ///   Returns the SunBurn SceneInterface
        /// </summary>
        public SceneInterface SunBurn { get; private set; }

        /// <summary>
        ///   Returns the current Application instance
        /// </summary>
        public Application Application { get; private set; }

        /// <summary>
        ///   Returns the name of the GameState
        /// </summary>
        public string Name { get; private set; }

        #region Implementation of IUpdate

        void IUpdate.Update(GameTime gameTime)
        {
            SunBurn.Update(gameTime);

            Update(gameTime);
        }

        /// <summary>
        ///   Update loop call
        /// </summary>
        /// <param name = "gameTime" />
        public virtual void Update(GameTime gameTime)
        {
        }

        #endregion

        #region Implementation of IDraw

        /// <summary>
        ///   Draws a frame
        /// </summary>
        /// <param name = "gameTime" />
        void IDraw.Draw(GameTime gameTime)
        {
            bool sunburnSplashScreenDisplayComplete = SplashScreen.DisplayComplete;

            if (sunburnSplashScreenDisplayComplete)
                BeginFrame(gameTime);

            for (int i = 0; i < Layers.Count; i++)
            {
                ILayer layer = Layers[i];
                if (layer is SunBurnLayer && !sunburnSplashScreenDisplayComplete) continue;

                if (layer.IsVisible)
                {
                    layer.BeginDraw(gameTime);
                    layer.Draw(gameTime);
                    layer.EndDraw(gameTime);
                }
            }

            Draw(gameTime);

            if (sunburnSplashScreenDisplayComplete)
                EndFrame();
        }

        #endregion

        #region IDraw Members

        /// <summary>
        ///   Draws a frame
        /// </summary>
        /// <param name = "gameTime" />
        public virtual void Draw(GameTime gameTime)
        {
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Layers.Clear();

            if (_localContentManager != null)
            {
                _localContentManager.Unload();
                //_localContentManager.Dispose();
                //_localContentManager = null;
            }

            FrameBuffers.Unload();
        }

        #endregion

        /// <summary>
        ///   Raised when the current GameState is loaded
        /// </summary>
        public event EventHandler LoadingCompleted;

        /// <summary>
        ///   Raised when the GameState loading is completed
        /// </summary>
        protected virtual void OnLoadingCompleted()
        {
        }

        /// <summary>
        ///   Initializes the GameState. Override this method to load any non-graphics resources and query for any required services.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        ///   Loads content
        /// </summary>
        public virtual void Load()
        {
            if (LoadingInProgress)
                throw new InvalidOperationException(string.Format("{0} game state is loading!", Name));

            LoadingInProgress = true;

            _loadingTask = Application.Threads.StartBackground(LoadAsynchronously, () =>
                                                                                       {
                                                                                           if (_loadingTask.Exceptions != null && _loadingTask.Exceptions.Length != 0)
                                                                                           {
                                                                                               Exception exception = _loadingTask.Exceptions[0];
                                                                                               throw new ContentLoadingException(exception.Message, exception);
                                                                                           }

                                                                                           // We clear the content list that has been loaded
                                                                                           ContentToLoad.Clear();

                                                                                           if (LoadingCompleted != null)
                                                                                               LoadingCompleted(this, EventArgs.Empty);

                                                                                           OnLoadingCompleted();
                                                                                       });
        }

        private void LoadAsynchronously()
        {
            float inventoryItemCount100 = 0, index = 0;

            if (ContentToLoad.Count > 0)
                inventoryItemCount100 = 100f/ContentToLoad.Count;

            foreach (IContentHost t in ContentToLoad)
            {
                Content.Add(t);

                index += 1;
                lock (this)
                {
                    _loadingPercentage = index*inventoryItemCount100;
                }
            }

            lock (this)
            {
                LoadingComplete = true;
                _loadingPercentage = 100;
            }

            LoadingInProgress = false;
        }

        /// <summary>
        ///   Register an <see cref = "IContentHost" /> instance with this content manager
        /// </summary>
        /// <param name = "host"></param>
        public void PreLoad(IContentHost host)
        {
            if (!LoadingComplete && !LoadingInProgress)
            {
                ContentToLoad.Add(host);
            }
            else
            {
                Content.Add(host);
            }
        }

        /// <summary>
        ///   Adds a Layer instance at the end of the stack
        /// </summary>
        /// <param name = "layer">The layer instance to add</param>
        /// <remarks>
        ///   The layers are updated and rendered in the order they were added.
        /// </remarks>
        public void AddLayer(ILayer layer)
        {
            if (layer == null)
                throw new ArgumentNullException("layer", "No valid Layer instance provided to the method");

            layer.Initialize();

            if (layer is IContentHost)
                PreLoad((IContentHost) layer);

            Layers.Add(layer);
        }

        /// <summary>
        ///   Removes a Layer instance from the stack
        /// </summary>
        /// <param name = "layer">The layer instance to remove</param>
        public void RemoveLayer(ILayer layer)
        {
            Layers.Remove(layer);
        }

        private void BeginFrame(GameTime gameTime)
        {
            ICamera camera = SunBurn.GetManager<ICameraManager>(true).ActiveCamera;

            camera.BeginFrameRendering(gameTime, _frameBuffers);
            SunBurn.BeginFrameRendering(camera.SceneState);
        }

        private void EndFrame()
        {
            ICamera camera = SunBurn.GetManager<ICameraManager>(true).ActiveCamera;

            SunBurn.EndFrameRendering();
            camera.EndFrameRendering();
        }
    }

    /// <summary>
    ///   The LoadingGameState class is the base class that you should use to render loading screens while other game states are loading.
    /// </summary>
    /// <remarks>
    ///   The default behavior is set to wait 1 sec once the other GameState is loaded. If you want to change it, just override the OnGameStateLoadingCompleted method
    /// </remarks>
    public abstract class LoadingGameState : GameState
    {
        private GameState _gameStateLoading;

        /// <summary>
        ///   Creates a new instance of the LoadingGameState
        /// </summary>
        /// <param name = "name"></param>
        /// <param name = "application"></param>
        protected LoadingGameState(string name, Application application)
            : base(name, application)
        {
        }

        /// <summary>
        ///   Returns the GameState currently being loaded
        /// </summary>
        public GameState GameStateLoading
        {
            get { return _gameStateLoading; }
            internal set
            {
                _gameStateLoading = value;
                _gameStateLoading.LoadingCompleted += OnGameStateLoadingCompleted;
            }
        }

        /// <summary>
        ///   Indicates if this LoadingGameState should render
        /// </summary>
        internal bool ShouldRenderLoadedGameState { get; set; }

        /// <summary>
        ///   Occurs once the GameState is loaded; Unregisters from the LoadingCompleted event and waits 1 sec before switching to GameState rendering
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        protected virtual void OnGameStateLoadingCompleted(object sender, EventArgs e)
        {
            _gameStateLoading.LoadingCompleted -= OnGameStateLoadingCompleted;
            Timer.Create(1, false, tick =>
                                       {
                                           ShouldRenderLoadedGameState = true;
                                           Application.SuppressDraw();
                                       });
        }

        /// <summary>
        ///   Loads content directly.
        /// </summary>
        public override void Load()
        {
            foreach (IContentHost t in ContentToLoad)
            {
                Content.Add(t);
            }

            LoadingComplete = true;
        }
    }
}