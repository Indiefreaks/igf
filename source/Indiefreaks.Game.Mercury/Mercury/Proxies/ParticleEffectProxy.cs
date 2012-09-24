/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury.Proxies
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A proxy to a particle effect
    /// </summary>
    public class ParticleEffectProxy
    {
        internal static List<ParticleEffectProxy> Proxies = new List<ParticleEffectProxy>();

        private static Vector3 _zero = Vector3.Zero;
        
        private Matrix _identity = Matrix.Identity;

        /// <summary>
        /// The world matrix of this effect proxy - used to give individual effect instances a transformation
        /// </summary>
        public Matrix World = Matrix.Identity;

        /// <summary>
        /// Set the world matrix to a simple translation
        /// </summary>
        public Vector3 Position
        {
            set { Matrix.CreateTranslation(ref value, out World); }
        }

        /// <summary>
        /// Our index into the global list.
        /// </summary>
        internal readonly Int32 Index;

        ///<summary>
        /// The efect that we are proxying
        ///</summary>
        public readonly ParticleEffect Effect;
        
        //The proxy world multiplied by the full world matrix. Cached here before rendering to save multiplying this per particle
        internal Matrix FinalWorld;

        static ParticleEffectProxy()
        {
            //Add an empty proxy to the start of the list - we use an index of 0 to mean 'no proxy'
            Proxies.Add(null);
        }

        /// <summary>
        /// Creates a new proxy effect
        /// </summary>
        public ParticleEffectProxy(ParticleEffect effect)
        {
            this.Effect = effect;
            
            this.Index = Proxies.Count;

            // Add ourselves to the static list...
            Proxies.Add(this);
            
            if (effect.Proxies == null)
            {
                effect.Proxies = new List<ParticleEffectProxy>();
            }
            
            effect.Proxies.Add(this); 
        }

        /// <summary>
        /// Trigger an effect on this proxy. All particles will be triggered at 0,0,0 and are positioned with the World matrix
        /// </summary>
        public void Trigger()
        {
            this.Effect.Trigger(ref _zero, this.Index);
        }

        /// <summary>
        /// Trigger an effect on this proxy. All particles will be triggered at 0,0,0 and are positioned with the World matrix of the proxy
        /// The effect will be checked against the frustum and culled if not within the effect Bounding Radius
        /// </summary>
        public void Trigger(ref BoundingFrustum frustum)
        {
            this.Trigger(ref frustum, false);
        }

        /// <summary>
        /// Trigger an effect on this proxy. All particles will be triggered at 0,0,0 and are positioned with the World matrix of the proxy
        /// The effect will be checked against the frustum and culled if not within the effect Bounding Radius
        /// </summary>
        public void Trigger(ref BoundingFrustum frustum, Boolean checkNearFar)
        {
            this.Effect.Trigger(ref _zero, ref frustum, ref World, checkNearFar, this.Index);
        }

        /// <summary>
        /// Trigger an effect on this proxy. All particles will be triggered at 0,0,0 and are positioned with the World matrix of the proxy
        /// The effect will be checked against the frustums and culled if not within the effect Bounding Radius
        /// </summary>
        public void Trigger(BoundingFrustum[] frustums)
        {
            Trigger(frustums, false);
        }

        /// <summary>
        /// Trigger an effect on this proxy. All particles will be triggered at 0,0,0 and are positioned with the World matrix of the proxy
        /// The effect will be checked against the frustums and culled if not within the effect Bounding Radius
        /// </summary>
        public void Trigger(BoundingFrustum[] frustums, Boolean checkNearFar)
        {
            Effect.Trigger(ref _zero, frustums, ref World, checkNearFar, this.Index);
        }

        /// <summary>
        /// Trigger an effect on this proxy. All particles will be triggered at 0,0,0 and are positioned with the World matrix of the proxy and the supplied world
        /// The effect will be checked against the frustum and culled if not within the effect Bounding Radius
        /// </summary>
        public void Trigger(ref BoundingFrustum frustum, Matrix world)
        {
            Trigger(ref frustum, world, false);
        }

        /// <summary>
        /// Trigger an effect on this proxy. All particles will be triggered at 0,0,0 and are positioned with the World matrix of the proxy and the supplied world
        /// The effect will be checked against the frustum and culled if not within the effect Bounding Radius
        /// </summary>
        public void Trigger(ref BoundingFrustum frustum, Matrix world, Boolean checkNearFar)
        {
            Matrix finalWorld;
            Matrix.Multiply(ref World, ref world, out finalWorld);
            Effect.Trigger(ref _zero, ref frustum, ref finalWorld, checkNearFar, this.Index);
        }

        /// <summary>
        /// Trigger an effect on this proxy. All particles will be triggered at 0,0,0 and are positioned with the World matrix of the proxy and the supplied world
        /// The effect will be checked against the frustums and culled if not within the effect Bounding Radius
        /// </summary>
        public void Trigger(BoundingFrustum[] frustums, Matrix world)
        {
            Trigger(frustums, world, false);
        }

        /// <summary>
        /// Trigger an effect on this proxy. All particles will be triggered at 0,0,0 and are positioned with the World matrix of the proxy and the supplied world
        /// The effect will be checked against the frustums and culled if not within the effect Bounding Radius
        /// </summary>
        public void Trigger(BoundingFrustum[] frustums, Matrix world, Boolean checkNearFar)
        {
            Matrix finalWorld;
            Matrix.Multiply(ref World, ref world, out finalWorld);
            Effect.Trigger(ref _zero, frustums, ref finalWorld, checkNearFar, this.Index);
        }

        /// <summary>
        /// Premultiply worldMatrix into the proxies world matrix before rendering to save repeated multiplications per particle
        /// </summary>
        /// <param name="worldMatrix">the global world matrix to transform by</param>
        internal static void SetWorldWorld(ref Matrix worldMatrix)
        {

        }
    }
}