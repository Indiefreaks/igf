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
    using ProjectMercury.Modifiers;
    using ProjectMercury.PluginContracts;
    
    using Colour = Microsoft.Xna.Framework.Color;

    /// <summary>
    /// Defines an emitter plugin which provides access to the ColourInterpolator3 modifier.
    /// </summary>
    [Export(typeof(IModifierPlugin))]
    public sealed class ColourInterpolator3Plugin : IModifierPlugin
    {
        /// <summary>
        /// Creates an instance of the modifier type exposed by this plugin, and returns a reference to it.
        /// </summary>
        /// <returns>A reference to a new modifier instance.</returns>
        public AbstractModifier ConstructInstance()
        {
            return new ColourInterpolator3
            {
                InitialColour = Colour.Blue.ToVector3(),
                MedianColour  = Colour.Green.ToVector3(),
                Median        = 0.5f,
                FinalColour   = Colour.Fuchsia.ToVector3()
            };
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value></value>
        public String Name
        {
            get { return "Colour Interpolator (3 colours)"; }
        }

        /// <summary>
        /// Gets a brief description of the plugin.
        /// </summary>
        /// <value></value>
        public String Description
        {
            get { return "A modifier which interpolates the colour of a particle over the course of its lifetime."; }
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
            get { return "Colour"; }
        }
    }
}