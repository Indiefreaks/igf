/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Plugins.DefaultModifiers
{
    using System;
    using System.ComponentModel.Composition;
    using Microsoft.Xna.Framework;
    using ProjectMercury.Modifiers;
    using ProjectMercury.PluginContracts;

    /// <summary>
    /// Defines an emitter plugin which provides access to the BoxForceModifier class.
    /// </summary>
    [Export(typeof(IModifierPlugin))]
    public sealed class BoxForceModifierPlugin : IModifierPlugin
    {
        /// <summary>
        /// Creates an instance of the modifier type exposed by this plugin, and returns a reference to it.
        /// </summary>
        /// <returns>A reference to a new modifier instance.</returns>
        public AbstractModifier ConstructInstance()
        {
            return new BoxForceModifier
            {
                Width = 10f,
                Height = 10f,
                Depth = 10f,
                
                Force = Vector3.Up,
                Strength = 1f,
            };
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value></value>
        public String Name
        {
            get { return "Box Force Modifier"; }
        }

        /// <summary>
        /// Gets a brief description of the plugin.
        /// </summary>
        /// <value></value>
        public String Description
        {
            get { return "A modifier which applies a force vector to particles when they enter an axis aligned box area."; }
        }

        /// <summary>
        /// Gets a display icon for the plugin.
        /// </summary>
        /// <value></value>
        public Uri DisplayIcon
        {
            get { return new Uri(""); }
        }

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        /// <value></value>
        public String Author
        {
            get { return "ProjectMercury Team Members"; }
        }

        /// <summary>
        /// Gets the version number of the plugin.
        /// </summary>
        /// <value></value>
        public Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        /// <summary>
        /// Gets the minimum version of the engine with which the plugin is compatible.
        /// </summary>
        /// <value></value>
        public Version RequiredEngineVersion
        {
            get { return new Version(4, 0, 0, 0); }
        }

        /// <summary>
        /// Gets the category of the modifier.
        /// </summary>
        public String Category
        {
            get { return "Forces"; }
        }
    }
}