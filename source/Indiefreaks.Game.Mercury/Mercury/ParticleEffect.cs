/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using ProjectMercury.Controllers;
    using ProjectMercury.Emitters;
    using ProjectMercury.Proxies;

    /// <summary>
    /// Defines the root of a particle effect hierarchy.
    /// </summary>
    public class ParticleEffect : ISupportDeepCopy<ParticleEffect>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ParticleEffect"/> class.
        /// </summary>
        public ParticleEffect()
        {
            this.Emitters = new EmitterCollection();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of ParticleEffect which is a copy of this instance.</returns>
        public ParticleEffect DeepCopy()
        {
            return new ParticleEffect
            {
                Name        = this.Name,
                Author      = this.Author,
                Description = this.Description,
                Emitters    = this.Emitters.DeepCopy(),
            };
        }

        private String _name;

        /// <summary>
        /// Gets or sets the name of the ParticleEffect.
        /// </summary>
        public String Name
        {
            get { return this._name; }
            set
            {
                if (this.Name != value)
                {
                    this._name = value;

                    this.OnNameChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Occurs when the name of the particle effect has been changed.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Raises the <see cref="E:ProjectMercury.ParticleEffect.NameChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnNameChanged(EventArgs e)
        {
            if (this.NameChanged != null)
                this.NameChanged(this, e);
        }

        /// <summary>
        /// Gets or sets the author of the ParticleEffect.
        /// </summary>
        public String Author { get; set; }

        /// <summary>
        /// Gets or sets the description of the ParticleEffect.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of emitters which comprise the particle effect.
        /// </summary>
        public EmitterCollection Emitters { get; set; }

        /// <summary>
        /// The maximum bounding radius for the effect. If this is set and a view Matrix is passed into Trigger then the effect will be automatically culled
        /// </summary>
        public Single BoundingRadius { get; set; }

        //The proxies associated with this effect
        internal List<ParticleEffectProxy> Proxies; //Will be lazily instantiated if needed

        /// <summary>
        /// How many proxies are there on this effect
        /// </summary>
        public Int32 ProxyCount
        {
            get
            {
                if (this.Proxies != null)
                    return this.Proxies.Count;

                return 0;
            }
        }

        ///<summary>
        /// Returns the total active particles for this effect across all emmitters
        ///</summary>
        public Int32 ActiveParticlesCount
        {
            get
            {
                var total = 0;
                
                foreach (var emitter in this.Emitters)
                    total += emitter.ActiveParticlesCount;
                
                return total;
            }
        }

        /// <summary>
        /// Updates the particle effect.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        public void Update(Single deltaSeconds)
        {
            var somethingActive = false;
            
            for (var i = 0; i < this.Emitters.Count; i++)
            {
                if (this.Emitters[i].ActiveParticlesCount >0)
                {
                    Counters.ParticlesUpdated += this.Emitters[i].ActiveParticlesCount;
                    
                    somethingActive = true;
                }
                
                this.Emitters[i].Update(deltaSeconds);
            }

            if (somethingActive)
            {
                Counters.ActiveEffects.Add(this);
            }
        }

        /// <summary>
        /// Triggers the particle effect at the specified position.
        /// </summary>
        /// <param name="position">The position of the trigger.</param>
        public void Trigger(ref Vector3 position)
        {
            // Call the trigger with index 0. We could use a default parameter but we really dont want the index being exposed to the public API as its internal workings...
            this.Trigger(ref position, 0);
        }

        /// <summary>
        /// Triggers the particle effect at the specified position.
        /// </summary>
        /// <param name="position">The position of the trigger.</param>
        /// <param name="proxyIndex">Index of the proxy to attach to the particles</param>
        internal void Trigger(ref Vector3 position, Int32 proxyIndex)
        {
            for (Int32 i = 0; i < this.Emitters.Count; i++)
            {
                var emitter = this.Emitters[i];

                TriggerContext context = new TriggerContext
                {
                    Cancelled       = false,
                    Position        = position,
                    ReleaseQuantity = emitter.ReleaseQuantity,
                    ProxyIndex      = proxyIndex,
                };

                emitter.Controllers.Process(ref context);

                if (context.Cancelled != true)
                {
                    emitter.Trigger(ref context);

                    Counters.ParticlesTriggered += context.ReleaseQuantity;
                }
            }
        }

        /// <summary>
        /// Triggers the particle effect at the specified position - only if the effect is within the view frustum
        /// </summary>
        /// <param name="position">The position of the trigger.</param>
        /// <param name="frustum">The current view frustum</param>
        public void Trigger(ref Vector3 position, ref BoundingFrustum frustum)
        {
            this.Trigger(ref position, ref frustum, false);
        }

        /// <summary>
        /// Triggers the particle effect at the specified position - only if the effect is within the view frustum
        /// </summary>
        /// <param name="position">The position of the trigger.</param>
        /// <param name="frustum">The current view frustum</param>
        /// <param name="checkNearFar">Should the near/far planes be checked in the culling. Its faster not to do it and your camera and gamestyle may not need it</param>
        public void Trigger(ref Vector3 position, ref BoundingFrustum frustum, bool checkNearFar)
        {
            var identity = Matrix.Identity;

            this.Trigger(ref position, ref frustum, ref identity, checkNearFar, 0);
        }

        /// <summary>
        /// Triggers the particle effect at the specified position - only if the effect is within the view frustm
        /// </summary>
        /// <param name="position">The position of the trigger.</param>
        /// <param name="frustums">An array of view frustums to test against - useful for split screen games</param>
        /// <param name="checkNearFar">Should the near/far planes be checked in the culling. Its faster not to do it and your camera and gamestyle may not need it</param>
        public void Trigger(ref Vector3 position, BoundingFrustum[] frustums)
        {
            this.Trigger(ref position, frustums, false);
        }
        
        /// <summary>
        /// Triggers the particle effect at the specified position - only if the effect is within the view frustm
        /// </summary>
        /// <param name="position">The position of the trigger.</param>
        /// <param name="frustums">An array of view frustums to test against - useful for split screen games</param>
        /// <param name="checkNearFar">Should the near/far planes be checked in the culling. Its faster not to do it and your camera and gamestyle may not need it</param>
        public void Trigger(ref Vector3 position, BoundingFrustum[] frustums, bool checkNearFar)
        {
            var identity = Matrix.Identity;

            this.Trigger(ref position, frustums, ref identity, checkNearFar, 0);
        }

        internal void Trigger(ref Vector3 position, ref BoundingFrustum frustum, ref Matrix world, Boolean checkNearFar, Int32 index)
        {
            // Avoiding triggering rather than avoiding drawing is more effecient. A particle system can have particles rendered all over the world - it would be far too expensive 
            // to cull by particle. If we never make the particles we never have to update or draw them.
            // TODO: trigger culling is now taken care of by the FrustumCullController...

            Vector3 transformedPosition;
            
            ParticleEffect.TransformPosition(ref position, ref world,  out transformedPosition);

            if (this.FastFrustumCheck(ref transformedPosition, ref frustum, checkNearFar))
            {
                // Passed the cull test - actually trigger the particle
                this.Trigger(ref position, index);
            }
            else
            {
                this.CountCulled();
            }
        }

        /// <summary>
        /// Terminates all Emitters within the ParticleEffect with immediate effect.
        /// </summary>
        public virtual void Terminate()
        {
            this.Emitters.Terminate();
        }

        private void CountCulled()
        {
            //Update counters with how many we ignored
            for (var j = 0; j < this.Emitters.Count; j++)
            {
                Counters.ParticleTriggersCulled += Emitters[j].ReleaseQuantity;
            }
        }

        internal void Trigger(ref Vector3 position, BoundingFrustum[] frustums, ref Matrix world,  bool checkNearFar, int index)
        {
            // Avoiding triggering rather than avoiding drawing is more effecient. A particle system can have particles rendered all over the world - it would be far to expensive 
            // to cull by particle. If we never make the particles we never have to update or draw them.
            // TODO: trigger culling is now taken care of by the FrustumCullController...

            Vector3 transformedPosition;
            
            ParticleEffect.TransformPosition(ref position, ref world, out transformedPosition);
            
            var triggered = false;

            for (var i = 0; i < frustums.Length; i++)
            {
                if (this.FastFrustumCheck(ref transformedPosition, ref frustums[i], checkNearFar))
                {
                    // Passed the cull test - actually trigger the particle
                    this.Trigger(ref position, index);

                    // no need to loop again once we have triggered..
                    triggered = true;
                    
                    break;
                }
            }

            if (!triggered)
            {
                this.CountCulled();
            }
        }

        private static void TransformPosition(ref Vector3 position, ref Matrix world, out Vector3 transformedPosition)
        {
            // TODO: Is a function call worth the inlining cost? What about the 16 comparisons in the common case of Identity
            if (world != Matrix.Identity) 
            {
                Vector3.Transform(ref position, ref world, out transformedPosition);
            }
            else
            {
                transformedPosition = position;
            }
        }

        private bool FastFrustumCheck(ref Vector3 position, ref BoundingFrustum frustum, bool checkNearFar)
        {
            //Uses faster, if slightly less accurate fast frustum check from here http://forums.create.msdn.com/forums/p/81153/490307.aspx#490307
            //original algorithm here http://zach.in.tu-clausthal.de/teaching/cg_literatur/lighthouse3d_view_frustum_culling/index.html
            var normal = frustum.Left.Normal;
            if (frustum.Left.D + (normal.X * position.X) + (normal.Y * position.Y) + (normal.Z * position.Z) > this.BoundingRadius)
                return false;
            normal = frustum.Right.Normal;
            if (frustum.Right.D + (normal.X * position.X) + (normal.Y * position.Y) + (normal.Z * position.Z) > this.BoundingRadius)
                return false;
            normal = frustum.Bottom.Normal;
            if (frustum.Bottom.D + (normal.X * position.X) + (normal.Y * position.Y) + (normal.Z * position.Z) > this.BoundingRadius)
                return false;
            normal = frustum.Top.Normal;
            if (frustum.Top.D + (normal.X * position.X) + (normal.Y * position.Y) + (normal.Z * position.Z) > this.BoundingRadius)
                return false;

            if (checkNearFar)
            {
                //Can ignore far/near plane when distant object culling is handled by another mechanism
                normal = frustum.Near.Normal;
                if (frustum.Near.D + (normal.X * position.X) + (normal.Y * position.Y) + (normal.Z * position.Z) > this.BoundingRadius)
                    return false;

                normal = frustum.Far.Normal;
                if (frustum.Far.D + (normal.X * position.X) + (normal.Y * position.Y) + (normal.Z * position.Z) > this.BoundingRadius)
                    return false;
            }
            return true;
        }

        internal void SetFinalWorld(ref Matrix worldMatrix)
        {
            for (int i = 1; i < Proxies.Count; i++) //ignore [0]
            {
                Matrix.Multiply(ref Proxies[i].World, ref worldMatrix, out Proxies[i].FinalWorld);
            }
        }
    }
}