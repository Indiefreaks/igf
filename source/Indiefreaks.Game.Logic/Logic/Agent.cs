using System;
using System.Collections.Generic;
using Indiefreaks.Xna.Core;
using Indiefreaks.Xna.Sessions;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Components;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Editor;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Logic
{
    /// <summary>
    /// The Agent provides the basic structure for Logic mechanisms.
    /// It inherits from SunBurn BaseComponentManualSerialization<ISceneEntity> so that it can be added to any game entity
    /// 
    /// Agents are given a way to control their ParentObject easily using Behaviors.
    /// </summary>
    [EditorObject(true)]
    [Serializable]
    public abstract class Agent : BaseComponentManualSerialization<ISceneEntity>
    {
        protected Agent()
        {
            Enabled = true;
            BehaviorsCollection = new List<Behavior>();
            Behaviors = new ReadonlyBehaviorCollection(BehaviorsCollection);
        }

        internal readonly List<Behavior> BehaviorsCollection;

        public ReadonlyBehaviorCollection Behaviors { get; private set; }

        public virtual void Add(Behavior behavior)
        {
            var containsTypeOfBehavior = false;
            BehaviorsCollection.ForEach(action =>
                                            {
                                                containsTypeOfBehavior = (action.GetType().Equals(behavior.GetType()));
                                            });
            if(containsTypeOfBehavior)
                throw new CoreException("A Behavior of the same Type is already present in the collection. You can use only one Behavior type per Agent");

            behavior.Agent = this;
            BehaviorsCollection.Add(behavior);
            Behaviors = new ReadonlyBehaviorCollection(BehaviorsCollection);
            behavior.Initialize();
        }

        public virtual void Remove(Behavior behavior)
        {
            BehaviorsCollection.Remove(behavior);
            Behaviors = new ReadonlyBehaviorCollection(BehaviorsCollection);
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            SessionManager.CurrentSession.RegisterSceneEntity(ParentObject);
        }

        protected internal abstract void Process(GameTime gameTime);

        public bool Enabled { get; set; }
    }
}