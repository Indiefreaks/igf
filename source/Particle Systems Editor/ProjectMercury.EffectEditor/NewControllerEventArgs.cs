/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using ProjectMercury.PluginContracts;
    using ProjectMercury.Emitters;
    using ProjectMercury.Controllers;

    internal class NewControllerEventArgs : CoreOperationEventArgs
    {
        public NewControllerEventArgs(AbstractEmitter parentEmitter, IControllerPlugin plugin)
        {
            this.ParentEmitter = parentEmitter;
            this.Plugin = plugin;
        }

        public AbstractEmitter ParentEmitter { get; private set; }

        public IControllerPlugin Plugin { get; private set; }

        public AbstractController AddedController { get; set; }
    }
}