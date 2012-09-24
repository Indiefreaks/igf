/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury
{
    /// <summary>
    /// Defines the interface for a reference type which supports deep copy.
    /// </summary>
    public interface ISupportDeepCopy<T> where T : class
    {
        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        /// <returns>A new instance of <typeparamref name="T"/> which is a copy of this instance.</returns>
        T DeepCopy();
    }
}