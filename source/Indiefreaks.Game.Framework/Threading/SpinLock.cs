using System;
using System.Threading;

namespace Indiefreaks.Xna.Threading
{
    /// <summary>
    /// A struct which implements a spin lock.
    /// </summary>
    public struct SpinLock
    {
        private Thread _owner;
        private int _recursion;

        /// <summary>
        /// Enters the lock. The calling thread will spin wait until it gains ownership of the lock.
        /// </summary>
        public void Enter()
        {
            // get the current thread
            var caller = Thread.CurrentThread;

            // early out: return if the current thread already has ownership.
            if (_owner == caller)
            {
                Interlocked.Increment(ref _recursion);
                return;
            }

            // only set the owner to this thread if the current owner is null. keep trying.
            while (Interlocked.CompareExchange(ref _owner, caller, null) != null) ;
            Interlocked.Increment(ref _recursion);
        }

        /// <summary>
        /// Tries to enter the lock.
        /// </summary>
        /// <returns><c>true</c> if the lock was successfully taken; else <c>false</c>.</returns>
        public bool TryEnter()
        {
            // get the current thead
            var caller = Thread.CurrentThread;

            // early out: return if the current thread already has ownership.
            if (_owner == caller)
            {
                Interlocked.Increment(ref _recursion);
                return true;
            }

            // try to take the lock, if the current owner is null.
            bool success = Interlocked.CompareExchange(ref _owner, caller, null) == null;
            if (success)
                Interlocked.Increment(ref _recursion);
            return success;
        }

        /// <summary>
        /// Tries to enter the lock.
        /// Fails after the specified time has elapsed without aquiring the lock.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns><c>true</c> if the lock was successfully taken; else <c>false</c>.</returns>
        public bool TryEnter(TimeSpan timeout)
        {
            // get start time and current thread
            var startTime = DateTime.Now;
            var caller = Thread.CurrentThread;

            // early out: return if the current thread already has ownership.
            if (_owner == caller)
            {
                Interlocked.Increment(ref _recursion);
                return true;
            }

            // keep trying to set the owner as the current thread, but only if the owner is null.
            while (Interlocked.CompareExchange(ref _owner, caller, null) != null)
            {
                // give up if we have taken too long
                if (DateTime.Now - startTime > timeout)
                    return false;
            }

            Interlocked.Increment(ref _recursion);
            return true;
        }

        /// <summary>
        /// Exits the lock. This allows other threads to take ownership of the lock.
        /// </summary>
        public void Exit()
        {
            // get the current thread.
            var caller = Thread.CurrentThread;

            if (caller == _owner)
            {
                Interlocked.Decrement(ref _recursion);
                if (_recursion == 0)
                    _owner = null;
            }
            else
                throw new InvalidOperationException("Exit cannot be called by a thread which does not currently own the lock.");
        }
    }
}
