/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Plugins.DefaultEmitters
{
    using System;
    using System.ComponentModel.Composition;
    using ProjectMercury.Emitters;
    using ProjectMercury.PluginContracts;

    /// <summary>
    /// Defines an emitter plugin which provides access to the point emitter.
    /// </summary>
    [Export(typeof(IEmitterPlugin))]
    public sealed class PointEmitterPlugin : IEmitterPlugin
    {
        /// <summary>
        /// Constructs an instance of the emitter type exposed by this plugin, and returns a reference to it.
        /// </summary>
        /// <returns>A reference to a new emitter instance.</returns>
        public AbstractEmitter ConstructInstance()
        {
            return new PointEmitter
            {
                BlendMode = EmitterBlendMode.Add,
                Budget = 1000,
                ReleaseColour = new ColourRange { Red = 1f, Blue = 1f, Green = 1f },
                ReleaseOpacity = 1f,
                ReleaseQuantity = 10,
                ReleaseRotation = new RotationRange
                {
                    Yaw = new Range { Minimum = -3.14157f, Maximum = 3.14157f },
                    Pitch = new Range { Minimum = -3.14157f, Maximum = 3.14157f },
                    Roll = new Range { Minimum = -3.14157f, Maximum = 3.14157f }
                },
                ReleaseScale = 16f,
                ReleaseSpeed = 1f,
                Term = 1f
            };
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value></value>
        public String Name
        {
            get { return "Point Emitter"; }
        }

        /// <summary>
        /// Gets a brief description of the plugin.
        /// </summary>
        /// <value></value>
        public String Description
        {
            get { return "An emitter which release particles from a single point in 3D space."; }
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
    }
}