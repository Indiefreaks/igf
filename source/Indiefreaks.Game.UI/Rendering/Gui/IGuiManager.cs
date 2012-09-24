using SynapseGaming.LightingSystem.Core;

namespace Indiefreaks.Xna.Rendering.Gui
{
    public interface IGuiManager : IRenderableManager, IUpdatableManager, IManagerService
    {
        /// <summary>
        /// Renders the scene.
        /// </summary>
        void Render();

        /// <summary>
        /// Create a new Screen instance, adds it to the Screens managed here and returns it
        /// </summary>
        void AddScreen(Screen screen);

        /// <summary>
        /// Removes the provided Screen from the GuiManager
        /// </summary>
        /// <param name="screen"></param>
        void RemoveScreen(Screen screen);
    }
}