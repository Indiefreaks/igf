/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Plugins.DefaultControllers
{
    using System;
    using System.ComponentModel.Composition;
    using Microsoft.Xna.Framework;
    using ProjectMercury.Controllers;
    using ProjectMercury.PluginContracts;

    /// <summary>
    /// Defines an controller plugin which provides access to the TriggerOffsetController.
    /// </summary>
    [Export(typeof(IControllerPlugin))]
    public sealed class TriggerOffsetControllerPlugin : IControllerPlugin
    {
        /// <summary>
        /// Constructs an instance of the controller type exposed by this plugin, and returns a reference to it.
        /// </summary>
        /// <returns>
        /// A reference to a new controller instance.
        /// </returns>
        public AbstractController ConstructInstance()
        {
            return new TriggerOffsetController
            {
                TriggerOffset = Vector3.Zero
            };
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        /// <value></value>
        public String Name
        {
            get { return "Trigger Offset Controller"; }
        }

        /// <summary>
        /// Gets a brief description of the plugin.
        /// </summary>
        /// <value></value>
        public String Description
        {
            get { return "A controller which adds an offset vector to a trigger."; }
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