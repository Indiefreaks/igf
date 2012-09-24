using System;
using Indiefreaks.Xna.Input;
using Indiefreaks.Xna.Sessions;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Editor;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Logic
{
    /// <summary>
    /// The PlayerAgent is an Agent that can react to Player Input.
    /// </summary>
    /// <remarks>You can have multiple PlayerAgents per player.</remarks>
    [EditorObject(true)]
    [Serializable]
    public class PlayerAgent : Agent
    {
        /// <summary>
        /// Returns the IdentifiedPlayer controlling this PlayerAgent
        /// </summary>
        public IdentifiedPlayer IdentifiedPlayer { get; internal set; }

        /// <summary>
        /// Returns the current PlayerAgent PlayerInput instance
        /// </summary>
        /// <remarks>If the PlayerAgent isn't local, it returns null</remarks>
        public PlayerInput Input
        {
            get { return IdentifiedPlayer.Input; }
        }

        /// <summary>
        /// Returns if the current PlayerAgent is Local to the machine or remote
        /// </summary>
        public bool IsLocal
        {
            get { return IdentifiedPlayer.Input != null; }
        }

        #region Overrides of Agent

        public override void OnRemovedFromManager(IManagerService manager)
        {
            base.OnRemovedFromManager(manager);

            if (manager.ManagerType == typeof (IObjectManager))
                SessionManager.CurrentSession.RemovePlayerAgent(this);
        }

        protected internal override void Process(GameTime gameTime)
        {
            if (!Enabled)
                return;

            if (IsLocal)
            {
                var elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
                // we loop through the PlayerAgent Behaviors
                foreach (Behavior behavior in BehaviorsCollection)
                {
                    ((IProcess) behavior).Process(elapsed);
                }
            }
        }

        #endregion
    }
}