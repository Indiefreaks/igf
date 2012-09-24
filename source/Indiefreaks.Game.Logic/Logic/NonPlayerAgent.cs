using System;
using Indiefreaks.Xna.Sessions;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Editor;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Logic
{
    [EditorObject(true)]
    [Serializable]
    public class NonPlayerAgent : Agent
    {
        #region Overrides of Agent

        public override void OnRemovedFromManager(IManagerService manager)
        {
            base.OnRemovedFromManager(manager);

            if (manager.ManagerType == typeof (IObjectManager))
                SessionManager.CurrentSession.RemoveNonPlayerAgent(this);
        }

        protected internal override void Process(GameTime gameTime)
        {
            if(!Enabled)
                return;

            var elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Behavior behavior in BehaviorsCollection)
            {
                ((IProcess) behavior).Process(elapsed);
            }
        }

        #endregion
    }
}