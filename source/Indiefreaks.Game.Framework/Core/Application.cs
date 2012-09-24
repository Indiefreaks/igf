using System;
using System.Diagnostics;
using Indiefreaks.Xna.Extensions;
using Indiefreaks.Xna.Input;
using Indiefreaks.Xna.Rendering;
using Indiefreaks.Xna.Rendering.Camera;
using Indiefreaks.Xna.Storage;
using Indiefreaks.Xna.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Lights;
using SynapseGaming.LightingSystem.Rendering;
#if SUNBURN_PRO && !WINDOWS_PHONE
using SynapseGaming.LightingSystem.Rendering.Deferred;
using SynapseGaming.LightingSystem.Rendering.Forward;
using SynapseGaming.LightingSystem.Shadows.Deferred;
using SynapseGaming.LightingSystem.Shadows.Forward;
#elif !WINDOWS_PHONE
using SynapseGaming.LightingSystem.Rendering.Forward;
using SynapseGaming.LightingSystem.Shadows.Forward;
#elif WINDOWS_PHONE
using SynapseGaming.LightingSystem.Rendering.Forward;
using SynapseGaming.LightingSystem.Shadows;
#endif

namespace Indiefreaks.Xna.Core
{
    /// <summary>
    ///   The Application class is responsible of running the main game loop and initialize, load, manage and run GameStates
    /// </summary>
    public abstract class Application : Game
    {
        public static Application Instance;
        private LoadingGameState _loadingGameState;
        private SceneInterface _sceneInterface;
        private Color _transitionColor;

        private SpriteBatch _transitionRenderer;
        private Texture2D _transitionTexture;

        protected Application(string name, string contentDirectory, params Language[] supportedLanguages)
        {
            // set the game title
            Name = name;

            // set the default content directory
            Content.RootDirectory = contentDirectory;

            // set the static instance
            Instance = this;

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000d/60d);

            EnableClearDevice = true;
            ClearDeviceColor = Color.Black;

            // create our GraphicsDeviceManager instance
            GraphicsDeviceManager = new GraphicsDeviceManager(this)
                                        {
                                            SynchronizeWithVerticalRetrace = true,
#if !WINDOWS_PHONE
                                            PreferredBackBufferWidth = 1280,
                                            PreferredBackBufferHeight = 720,
#else
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 480,
#endif
                                        };

            GraphicsDeviceManager.PreparingDeviceSettings += (sender, e) =>
                                                                 {
#if WINDOWS
    // improves overall performance with dynamic shadows.
                e.GraphicsDeviceInformation.
                    PresentationParameters.
                    RenderTargetUsage =
                    RenderTargetUsage.PlatformContents;
#else
                                                                     // improves overall performance with dynamic shadows.
                                                                     e.GraphicsDeviceInformation.
                                                                         PresentationParameters.
                                                                         RenderTargetUsage =
                                                                         RenderTargetUsage.PreserveContents;
#endif
                                                                     // Used for advanced edge cleanup.
                                                                     e.GraphicsDeviceInformation.
                                                                         PresentationParameters.
                                                                         DepthStencilFormat =
                                                                         DepthFormat.Depth24Stencil8;
                                                                 };

            try
            {
                // create the SunBurn required SunBurnCoreSystem instance
                SunBurnCoreSystem = new SunBurnCoreSystem(Services, Content)
                                        {
                                            DetectOverSizedFrameBuffers = false
                                        };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // create the SunBurn SunBurnSystemPreferences instance used when creating a new SceneInterface instance: defaults to average quality
            SunBurnSystemPreferences = new SystemPreferences
                                           {
                                               EffectDetail = DetailPreference.Medium,
                                               LightingDetail = DetailPreference.Medium,
                                               MaxAnisotropy = 4,
                                               PostProcessingDetail = DetailPreference.Medium,
                                               ShadowDetail = DetailPreference.Medium,
                                               ShadowQuality = 2.0f,
                                               TextureSampling = SamplingPreference.Anisotropic,
                                           };

            // create and register InputManager instance for the game.
            new InputManager(this);

            // create and register StorageManager instance for the game.
            new StorageManager(this);
            StorageSettings.SetSupportedLanguages(supportedLanguages);

            // create our Thread pool
            Threads = new ThreadPool();
        }

        /// <summary>
        ///   Creates a new Application instance
        /// </summary>  
        /// <param name = "name">The name of the Application used in the game window</param>
        /// <param name="contentDirectory">The root directory for content</param>
        protected Application(string name, string contentDirectory)
            : this(name, contentDirectory, Language.English)
        {
        }

        /// <summary>
        ///   Returns the current InputManager instance
        /// </summary>
        public static InputManager Input
        {
            get { return Instance.Services.GetService(typeof (InputManager)) as InputManager; }
        }

        public static StorageManager Storage
        {
            get { return Instance.Services.GetService(typeof (StorageManager)) as StorageManager; }
        }

        /// <summary>
        /// Returns the current Application wide ContentManager instance
        /// </summary>
        public static ContentManager ContentManager
        {
            get { return Instance.Content; }
        }

        /// <summary>
        /// Returns the current Active GameState
        /// </summary>
        public static GameState GameState
        {
            get { return Instance.ActiveGameState; }
        }

        /// <summary>
        ///   Returns the current GraphicsDeviceManager instance
        /// </summary>
        public static GraphicsDeviceManager Graphics
        {
            get { return Instance.GraphicsDeviceManager; }
        }

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        /// <summary>
        ///   Returns the current SceneInterface instance
        /// </summary>
        public static SceneInterface SunBurn
        {
            get
            {
                if (Instance._sceneInterface == null)
                    Instance.CreateSceneInterface();

                return Instance._sceneInterface;
            }
        }

        /// <summary>
        ///   Returns the current SunBurn SunBurnCoreSystem instance
        /// </summary>
        public SunBurnCoreSystem SunBurnCoreSystem { get; private set; }

        /// <summary>
        ///   Returns the current SunBurn SunBurnSystemPreferences instance
        /// </summary>
        public SystemPreferences SunBurnSystemPreferences { get; private set; }

        /// <summary>
        ///   Returns the current Application ThreadPool
        /// </summary>
        public ThreadPool Threads { get; private set; }

        /// <summary>
        ///   Gets or sets the name of the application
        /// </summary>
        public string Name
        {
            get { return Window.Title; }
            private set { Window.Title = value; }
        }

        /// <summary>
        ///   Returns the current active GameState
        /// </summary>
        public GameState ActiveGameState { get; private set; }

        protected override void LoadContent()
        {
            base.LoadContent();

            // create the SpriteBatch used for transitions
            _transitionRenderer = new SpriteBatch(GraphicsDevice);

            // create the background texture used for transitions
            _transitionTexture = new Texture2D(GraphicsDevice, 1, 1);
            _transitionTexture.SetData(new[] {Color.White});
        }

        /// <summary>
        ///   Creates the unique SceneInterface instance used accross the entire Application.
        /// </summary>
        protected virtual void CreateSceneInterface()
        {
            _sceneInterface = new SceneInterface();

            _sceneInterface.Unload();
            _sceneInterface.AddManager(new ResourceManager(_sceneInterface));
            _sceneInterface.AddManager(new CameraManager(_sceneInterface));
            _sceneInterface.AddManager(new ObjectManager(_sceneInterface));
            _sceneInterface.AddManager(new LightManager(_sceneInterface));

#if WINDOWS_PHONE
            _sceneInterface.AddManager(new LightMapManager(_sceneInterface)); // required by WP7 projects to render properly.
#endif

            InitializeSunBurn();

            _sceneInterface.ApplyPreferences(SunBurnSystemPreferences);
        }

        /// <summary>
        /// Initialize SunBurn the way you want
        /// </summary>
        /// /// <remarks>
        ///   This method is automatically called the first time you want to acquire the SceneInterface.
        /// 
        ///   The minimum Managers we add are:
        ///   - ResourceManager
        ///   - CameraManager
        ///   - ObjectManager
        ///   - LightManager
        /// 
        ///   We do not setup the SceneInterface for rendering: you need to call CreateRenderer in this method override.
        /// </remarks>
        /// <example>
        ///   protected override void CreateSceneInterface()
        ///   {
        ///     CreateRenderer(Renderers.Deferred); // or you could instead use Renderers.Forward depending on your game's lighting requirements
        ///     
        ///     SunBurn.AddManager(new PostProcessManager(SunBurn.GraphicsDeviceManager);                   // Adds the post process manager
        ///     SunBurn.AddManager(new CollisionManager(GraphicsDeviceManager, plugin.SceneInterface));     // Adds the collision manager
        ///     SunBurn.AddManager(new LightingSystemEditor(GraphicsDeviceManager, plugin.SceneInterface)); // Adds the SunBurn editor manager
        ///   }
        /// </example>
        protected abstract void InitializeSunBurn();

        /// <summary>
        ///   Adds the Forward or Deferred SunBurn Renderers to the SunBurn SceneInterface.
        /// </summary>
        /// <param name = "renderer">Forward or Deferred</param>
        /// <remarks>
        ///   This method needs to be called once ideally in the overrided CreateSceneInterface() method.
        /// </remarks>
        protected void CreateRenderer(Renderers renderer)
        {
#if SUNBURN_PRO && !WINDOWS_PHONE
            switch (renderer)
            {
                case Renderers.Forward:
                    {
                        _sceneInterface.AddManager(new RenderManager(_sceneInterface));
                        _sceneInterface.AddManager(new ShadowMapManager(_sceneInterface));
                        break;
                    }
                case Renderers.Deferred:
                    {
                        _sceneInterface.AddManager(new DeferredRenderManager(_sceneInterface));
                        _sceneInterface.AddManager(new DeferredShadowMapManager(_sceneInterface));
                        break;
                    }
            }
#else
            _sceneInterface.AddManager(new RenderManager(_sceneInterface));
#if !WINDOWS_PHONE
            _sceneInterface.AddManager(new ShadowMapManager(_sceneInterface));
#endif
#endif
        }

        /// <summary>
        ///   Full update
        /// </summary>
        /// <param name = "gameTime">Time passed since the last call to Update.</param>
        protected override void Update(GameTime gameTime)
        {
#if WINDOWS
            if (SunBurn.Editor != null && SunBurn.Editor.EditorAttached)
                Input.Enabled = false;
            else
                Input.Enabled = true;
#endif

            base.Update(gameTime);

            // we update the ThreadPool
            Threads.Update();

            // we update the Interpolator and Timer providers
            Interpolator.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            Timer.Update((float) gameTime.ElapsedGameTime.TotalSeconds);

            // If the current GameState is loaded, we update it, otherwise, we update its LoadingGameState
            if (ActiveGameState != null && ActiveGameState.IsLoadingComplete &&
                (_loadingGameState == null || _loadingGameState.ShouldRenderLoadedGameState))
                ((IUpdate) ActiveGameState).Update(gameTime);
            if (_loadingGameState != null && _loadingGameState.IsLoadingComplete &&
                !_loadingGameState.ShouldRenderLoadedGameState)
                ((IUpdate) _loadingGameState).Update(gameTime);
        }

        public bool EnableClearDevice { get; set; }

        public Color ClearDeviceColor { get; set; }

        /// <summary>
        ///   Renders one frame
        /// </summary>
        /// <param name = "gameTime">Time passed since the last call to Draw.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (EnableClearDevice)
                GraphicsDevice.Clear(ClearDeviceColor);

            // If the Application is loading a GameState asynchronously, we render the LoadingGameState instance
            if (_loadingGameState != null && _loadingGameState.IsLoadingComplete &&
                !_loadingGameState.ShouldRenderLoadedGameState)
            {
                ((IDraw) _loadingGameState).Draw(gameTime);
            }
                // Otherwise, we just render the active GameState
            else if (ActiveGameState != null && ActiveGameState.IsLoadingComplete)
            {
                ((IDraw) ActiveGameState).Draw(gameTime);
            }

            // we render the transition if required
            if (_fadeActive)
            {
                _transitionRenderer.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                _transitionRenderer.Draw(_transitionTexture, GraphicsDevice.Viewport.Bounds, _transitionColor);
                _transitionRenderer.End();
                GraphicsDevice.SamplerStates.Reset();
            }

            base.Draw(gameTime);
        }

        private static bool _fadeActive;

        /// <summary>
        ///   Fades the whole Application screen in.
        /// </summary>
        /// <param name = "startColor">The Color used for start</param>
        /// <param name = "length">The length in seconds the fade operation should last</param>
        public static void FadeIn(Color startColor, float length)
        {
            if (_fadeActive)
            {
                Debug.WriteLine("Fading already active");
                return;
            }

            _fadeActive = true;

            Instance._transitionColor = startColor;

            Interpolator.Create(1.0f, 0.0f, length,
                                (step) =>
                                    {
                                        Instance._transitionColor = new Color(Convert.ToSingle(Instance._transitionColor.R),
                                                                              Convert.ToSingle(Instance._transitionColor.G),
                                                                              Convert.ToSingle(Instance._transitionColor.B),
                                                                              step.Value);
                                    }, completed => _fadeActive = false);
        }

        /// <summary>
        ///   Fades the whole Application screen in.
        /// </summary>
        /// <param name = "startColor">The Color used for start</param>
        /// <param name = "length">The length in seconds the fade operation should last</param>
        /// <param name="waitingTime">The seconds to wait before starting the fade</param>
        public static void FadeIn(Color startColor, float length, float waitingTime)
        {
            Timer.Create(waitingTime, false, tick => FadeIn(startColor, length));
        }

        /// <summary>
        ///   Fades the whole Application screen out.
        /// </summary>
        /// <param name = "endColor">The Color used for end</param>
        /// <param name = "length">The length in seconds the fade operation should last</param>
        public static void FadeOut(Color endColor, float length)
        {
            if (_fadeActive)
            {
                Debug.WriteLine("Fading already active");
                return;
            }

            _fadeActive = true;

            Instance._transitionColor = endColor*0f;

            Interpolator.Create(0.0f, 1.0f, length,
                                (step) =>
                                    {
                                        Instance._transitionColor = new Color(Convert.ToSingle(Instance._transitionColor.R),
                                                                              Convert.ToSingle(Instance._transitionColor.G),
                                                                              Convert.ToSingle(Instance._transitionColor.B),
                                                                              step.Value);
                                    }, completed => _fadeActive = false);
        }

        /// <summary>
        ///   Fades the whole Application screen out.
        /// </summary>
        /// <param name = "endColor">The Color used for end</param>
        /// <param name = "length">The length in seconds the fade operation should last</param>
        /// <param name="waitingTime">The seconds to wait before starting the fade</param>
        public static void FadeOut(Color endColor, float length, float waitingTime)
        {
            Timer.Create(waitingTime, false, tick => FadeOut(endColor, length));
        }

        /// <summary>
        ///   Loads the provided GameState instance
        /// </summary>
        /// <param name = "gameState">The GameState to load</param>
        public void LoadGameState(GameState gameState)
        {
            LoadGameState(gameState, null);
        }

        /// <summary>
        /// </summary>
        /// <param name = "gameState"></param>
        /// <param name = "loadingGameState"></param>
        public void LoadGameState(GameState gameState, LoadingGameState loadingGameState)
        {
            if (gameState == null)
                throw new ArgumentNullException("gameState");

            // If a LoadingGameState is already associated with the Application and isn't the same as the provided one, we dispose it for replacement
            if (_loadingGameState != null && _loadingGameState != loadingGameState)
            {
                _loadingGameState.Dispose();
                _loadingGameState = null;
                GC.Collect();
            }
            // We load the LoadingGameState synchronously to avoid waiting and we make sure it will render instead of the ActiveGameState.
            if (loadingGameState != null)
            {
                _loadingGameState = loadingGameState;
                _loadingGameState.Initialize();
                _loadingGameState.Load();
                _loadingGameState.ShouldRenderLoadedGameState = false;
            }

            // We associate the GameState currently being loaded so that we can retrieve its loading percentage.
            if (_loadingGameState != null)
            {
                _loadingGameState.GameStateLoading = gameState;
            }

            if (ActiveGameState != null)
            {
                ActiveGameState.Dispose();
                ActiveGameState = null;
                GC.Collect();
            }

            ActiveGameState = gameState;

            ActiveGameState.Initialize();

            ActiveGameState.Load();
        }

#if !WINDOWS_PHONE
        public static void Run<T>() where T : Application, new()
        {
            if (Debugger.IsAttached)
            {
                using (var g = new T())
                {
                    g.Run();
                }
            }
            else
            {
                try
                {
                    using (var g = new T())
                    {
                        g.Run();
                    }
                }
                catch (Exception e)
                {
#if XBOX
                    bool gamerServicesComponentFound = false;

                    if (Instance != null)
                    {
                        for (int i = 0; i < Instance.Components.Count; i++)
                        {
                            if (Instance.Components[i] is GamerServicesComponent)
                            {
                                gamerServicesComponentFound = true;
                                break;
                            }
                        }
                    }

                    using (var g = new ExceptionApplication(gamerServicesComponentFound, e))
                    {
                        g.Run();
                    }
#elif WINDOWS
                    throw;
#endif
                }
            }
        }
    }

    public sealed class ExceptionApplication : Game
    {
        private const string ErrorTitle = "Unexpected Error";
        private const string ErrorMessage = "The game had an unexpected error and had to shut down. " + "We're sorry for the inconvenience.";
        private static readonly string[] ErrorButtons = new[] {"Exit to Dashboard", "View Error Details"};
        private readonly Exception _exception;
        private SpriteBatch _batch;
        private bool _displayException;
        private SpriteFont _font;
        private bool _shownMessage;

        public ExceptionApplication(bool gamerServicesCompoenentAlreadyInitialized, Exception e)
        {
            new GraphicsDeviceManager(this) {PreferredBackBufferWidth = 1280, PreferredBackBufferHeight = 720};
            _exception = e;

            if(!gamerServicesCompoenentAlreadyInitialized)
                Components.Add(new GamerServicesComponent(this));

            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _batch = new SpriteBatch(GraphicsDevice);
            
#if WINDOWS
            var resourceContentManager = new ResourceContentManager(Services, Resources.WindowsCoreResources.ResourceManager);
#elif XBOX
            var resourceContentManager = new ResourceContentManager(Services, Resources.Xbox360CoreResources.ResourceManager);
#endif


            _font = resourceContentManager.Load<SpriteFont>("DebugFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (!_shownMessage)
            {
                try
                {
                    if (!Guide.IsVisible)
                    {
                        Guide.BeginShowMessageBox(PlayerIndex.One, ErrorTitle, ErrorMessage, ErrorButtons, 0, MessageBoxIcon.Error, result =>
                                                                                                                                        {
                                                                                                                                            int? choice = Guide.EndShowMessageBox(result);
                                                                                                                                            if (choice.HasValue && choice.Value == 1) _displayException = true;
                                                                                                                                            else Exit();
                                                                                                                                        }, null);
                        _shownMessage = true;
                    }
                }
                catch
                {
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (_displayException)
            {
                var p = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y);
                _batch.Begin();
                _batch.DrawString(_font, _exception.ToString(), p, Color.White);
                _batch.End();
            }
            base.Draw(gameTime);
        }
#endif
    }
}