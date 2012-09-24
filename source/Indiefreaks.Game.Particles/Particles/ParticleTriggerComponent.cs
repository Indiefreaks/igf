using SynapseGaming.LightingSystem.Components;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.Particles
{
    public abstract class ParticleTriggerComponent : BaseComponent<ISceneEntity>
    {
        private bool _isHostedByParticleTrigger;
        protected ParticleTrigger ParticleTrigger;
        public bool Enabled { get; set; }

        protected ParticleTriggerComponent()
        {
            Enabled = true;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            _isHostedByParticleTrigger = ParentObject is ParticleTrigger;

            if (_isHostedByParticleTrigger)
                ParticleTrigger = ParentObject as ParticleTrigger;
        }

        public override void OnUpdate(Microsoft.Xna.Framework.GameTime gametime)
        {
            base.OnUpdate(gametime);

            if(!_isHostedByParticleTrigger)
                return;
        }
    }
}