using System;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework.Content;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering
{
    /// <summary>
    /// This Layer renders the SunBurn SplashScreen
    /// </summary>
    /// <remarks>This layer must be activated at least once before adding a SunBurnLayer instance in the current GameState</remarks>
    public class SunBurnSplashScreen : Layer, IContentHost
    {
        private readonly bool _showDuringDevelopment = false;

        /// <summary>
        /// Creates a new instance of the SunBurnSplashScreen layer
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="showDuringDevelopment">True if you want to test SplashScreen display; false otherwise</param>
        public SunBurnSplashScreen(GameState gameState, bool showDuringDevelopment)
            : base(gameState)
        {
            _showDuringDevelopment = showDuringDevelopment;
        }

        /// <summary>
        /// Returns the SunBurn SplashScreen instance
        /// </summary>
        public SplashScreen SplashScreen { get; private set; }

        /// <summary>
        ///   Load all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        /// <param name = "manager">XNA content manage</param>
        public void LoadContent(IContentCatalogue catalogue, Microsoft.Xna.Framework.Content.ContentManager manager)
        {
            SplashScreen = new SplashScreen();
            SplashScreen.ShowDuringDevelopment = _showDuringDevelopment;
        }

        /// <summary>
        ///   Unload all XNA <see cref = "ContentManager" /> content
        /// </summary>
        /// <param name = "catalogue"></param>
        public void UnloadContent(IContentCatalogue catalogue)
        {
            
        }

        /// <summary>
        ///   Updates the SunBurn SplashScreen if required
        /// </summary>
        /// <param name = "gameTime" />
        public override void BeginDraw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.BeginDraw(gameTime);

            if(!SplashScreen.DisplayComplete)
                SplashScreen.Update(gameTime);
        }

        /// <summary>
        ///   Renders the SunBurn SplashScreen if required
        /// </summary>
        /// <param name = "gameTime" />
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            try
            {
                if (!SplashScreen.DisplayComplete)
                    SplashScreen.Render(gameTime);
            }
            catch (ArgumentException argumentException)
            {
                throw new CoreException("", argumentException);
            }
        }
    }
}