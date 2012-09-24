using System;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;
using ProjectMercury.Proxies;
using SynapseGaming.LightingSystem.Core;
using SynapseGaming.LightingSystem.Editor;
using SynapseGaming.LightingSystem.Rendering;

namespace Indiefreaks.Xna.Rendering.Particles
{
#if WINDOWS
    [Serializable]
#endif
    [EditorCreatedObject]
    public class ParticleTrigger : SceneEntity
    {
        private float _tick;

        public int ParticlesPerSecond { get; set; }

        public ParticleTrigger() : base(string.Empty, false)
        {
            UpdateType = UpdateType.Automatic;
            HullType = HullType.Sphere;
            TransformMode = ParticleTransformMode.Global;
        }

        [EditorNumberPadOptions(0,-1,double.MaxValue,1)]
        public int ParticleSystemIndex
        {
            get { return _particleSystemIndex; }
            set
            {
                if (value != _particleSystemIndex)
                {
                    var particleSystem = Application.SunBurn.GetManager<ParticleSystemManager>(true).FindParticleSystemByIndex(value);

                    if (particleSystem != null)
                    {
                        ParticleSystem = particleSystem;
                        _particleSystemIndex = value;
                    }
                    else
                        ParticleSystemIndex = _particleSystemIndex;
                }
            }
        }

        private ParticleSystem _particleSystem;
        public ParticleSystem ParticleSystem
        {
            get { return _particleSystem; }
            set
            {
                _particleSystem = value;

                if (_particleSystem != null)
                {
                    _particleSystemIndex = Application.SunBurn.GetManager<ParticleSystemManager>(true).GetParticleSystemIndex(_particleSystem);

                    if (_transformMode == ParticleTransformMode.Local)
                    {
                        Proxy = new ParticleEffectProxy(_particleSystem.Effect);
                    }
                    else
                    {
                        Proxy = null;
                    }

                    CalculateBounds();
                }
            }
        }

        private ParticleTransformMode _transformMode;
        public ParticleTransformMode TransformMode
        {
            get { return _transformMode; }
            set
            {
                if (value == _transformMode) return;

                _transformMode = value;

                if (_transformMode != ParticleTransformMode.Local)
                {
                    Proxy = null;
                    return;
                }

                if (ParticleSystem != null)
                    Proxy = new ParticleEffectProxy(_particleSystem.Effect);
            }
        }

        private ParticleEffectProxy _proxy;
        private int _particleSystemIndex = -1;

        public ParticleEffectProxy Proxy
        {
            get { return _proxy; }
            private set
            {
                if (value != _proxy)
                {
                    _proxy = value;

                    if (_proxy != null)
                        _proxy.World = World;
                }
            }
        }

        public void Trigger(int numberOfParticles)
        {
            Trigger(numberOfParticles, Matrix.Identity);
        }

        public void Trigger(int numberOfParticles, Matrix world)
        {
            if (ParticleSystem == null)
                return;

            for (int i = 0; i < numberOfParticles; i++)
            {
                if (TransformMode == ParticleTransformMode.Local)
                {
                    if(world == Matrix.Identity)
                        Proxy.Trigger(ref ParticleSystemManager.Frustum, false);
                    else
                        Proxy.Trigger(ref ParticleSystemManager.Frustum, world, false);
                }
                else
                {
                    var position = World.Translation;
                    ParticleSystem.Effect.Trigger(ref position, ref ParticleSystemManager.Frustum, false);
                }
            }
        }
        
        protected override void CalculateObjectBounds(ref BoundingBox objectboundingbox, ref BoundingSphere objectboundingsphere)
        {
            if (ParticleSystem != null)
            {
                objectboundingsphere.Radius = ParticleSystem.Effect.BoundingRadius;
                objectboundingbox = BoundingBox.CreateFromSphere(objectboundingsphere);
            }
            else
            {
                base.CalculateObjectBounds(ref objectboundingbox, ref objectboundingsphere);
            }
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            
            if (ParticlesPerSecond >= 1)
            {
                _tick += ParticleSystemManager.ElapsedSeconds;

                if (_tick >= 1.0f / ParticlesPerSecond)
                {
                    Trigger(1);

                    _tick = 0f;
                }
            }
        }
        
        protected override void UpdateWorldAndWorldToObject(ref Matrix world, ref Matrix worldtoobj)
        {
            base.UpdateWorldAndWorldToObject(ref world, ref worldtoobj);

            if (TransformMode == ParticleTransformMode.Local)
            {
                Proxy.World = world;
            }
        }
    }
}