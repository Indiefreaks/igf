﻿using System;
using System.Collections.Generic;
using System.Threading;
using Indiefreaks.Xna.Collections;

namespace Indiefreaks.Xna.Threading
{
    /// <summary>
    /// A thread safe, non-blocking, object pool.
    /// </summary>
    /// <typeparam name="T">The type of item to store. Must be a class with a parameterless constructor.</typeparam>
    internal class Pool<T>
        : Singleton<Pool<T>>
        where T: class, new()
    {
        struct DequeEnumerator
        {
            public Deque<T> Deque;
            public IEnumerator<KeyValuePair<Thread, DequeEnumerator>> Enumerator;
        }

        Hashtable<Thread, DequeEnumerator> instances;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pool&lt;T&gt;"/> class.
        /// </summary>
        public Pool()
        {
#if WINDOWS_PHONE
            instances = new Hashtable<Thread, DequeEnumerator>(1);
#else
            instances = new Hashtable<Thread, DequeEnumerator>(Environment.ProcessorCount);
#endif
        }

        /// <summary>
        /// Gets an instance from the pool.
        /// </summary>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        public T Get()
        {
            DequeEnumerator de;
            if (instances.TryGet(Thread.CurrentThread, out de))
            {
                T instance = default(T);
                if (de.Deque.LocalPop(ref instance))
                    return instance;
                else
                {
                    de.Enumerator.Reset();
                    while (de.Enumerator.MoveNext())
                    {
                        if (de.Enumerator.Current.Value.Deque.TrySteal(ref instance))
                            return instance;
                    }
                }
            }

            return new T();
        }

        /// <summary>
        /// Returns an instance to the pool, so it is available for re-use.
        /// It is advised that the item is reset to a default state before being returned.
        /// </summary>
        /// <param name="instance">The instance to return to the pool.</param>
        public void Return(T instance)
        {
            DequeEnumerator de;
            if (instances.TryGet(Thread.CurrentThread, out de))
                de.Deque.LocalPush(instance);
            else
            {
                de = new DequeEnumerator()
                {
                    Deque = new Deque<T>(),
                    Enumerator = instances.GetEnumerator()
                };

                de.Deque.LocalPush(instance);

                instances.Add(Thread.CurrentThread, de);
            }
        }
    }
}
