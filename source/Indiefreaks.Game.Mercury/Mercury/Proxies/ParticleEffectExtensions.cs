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

    /// <summary>
    /// Adds extension methods to the <see cref="T:ProjectMercury.ParticleEffect"/> class for working
    /// with proxies. These methods will only become visible on importing the ProjectMercury.Proxies
    /// namespace, so wont clutter the API for people who don't want/need to use them.
    /// </summary>
    static public class ParticleEffectExtensions
    {
        /// <summary>
        /// Creates a proxy for the specified <see cref="T:ProjectMercury.ParticleEffect"/> instance.
        /// </summary>
        /// <param name="particleEffect">The extension instance.</param>
        /// <returns>A new <see cref="T:ProjectMercury.Proxies.ParticleEffectProxy"/> instance.</returns>
        static public ParticleEffectProxy CreateProxy(this ParticleEffect particleEffect)
        {
            if (particleEffect == null)
                throw new ArgumentNullException("particleEffect");

            return new ParticleEffectProxy(particleEffect);
        }

        /// <summary>
        /// Creates a proxy for the specified <see cref="T:ProjectMercury.ParticleEffect"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of proxy to create.</typeparam>
        /// <param name="particleEffect">The extension instance.</param>
        /// <returns>A new <see cref="T:ProjectMercury.Proxies.ParticleEffectProxy"/> instance.</returns>
        static public T CreateProxy<T>(this ParticleEffect particleEffect) where T : ParticleEffectProxy
        {
            if (particleEffect == null)
                throw new ArgumentNullException("particleEffect");

            // All proxy classes must expose a constructor that takes a single ParticleEffect parameter
            // for this to work, which is reasonable since the base class constructor requires it...

            var ctorParams = new [] { particleEffect };

            return Activator.CreateInstance(typeof(T), ctorParams) as T;
        }
    }
}