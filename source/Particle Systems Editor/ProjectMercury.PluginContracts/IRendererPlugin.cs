/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.PluginContracts
{
    using ProjectMercury.Renderers;

    /// <summary>
    /// Defines the interface for a plugin which provides access to a Renderer.
    /// </summary>
    public interface IRendererPlugin : IPlugin
    {
        /// <summary>
        /// Constructs an instance of the renderer type exposed by this plugin, and returns a reference to it.
        /// </summary>
        /// <returns>A reference to a new renderer instance.</returns>
        AbstractRenderer ConstructInstance();
    }
}