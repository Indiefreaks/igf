﻿/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury
{
    using System;
    using System.Threading;
    using System.Collections.Generic;

#if WINDOWS
    using ResetEvent = System.Threading.ManualResetEventSlim;
#elif XBOX || WINDOWS_PHONE
    using ResetEvent = System.Threading.ManualResetEvent;
#endif

    /// <summary>
    /// Defines a class which handles updating of particle effects asynchronously.
    /// </summary>
    public class AsyncUpdateManager : IDisposable
    {
        /// <summary>
        /// Gets or sets the worker thread.
        /// </summary>
        private Thread WorkerThread;

        /// <summary>
        /// Thread event, raised when there is work available for the worker thread.
        /// </summary>
        private ResetEvent WorkAvailable;

        /// <summary>
        /// Thread event, raised when the worker thread has finished its current work.
        /// </summary>
        private ResetEvent WorkDone;

        /// <summary>
        /// Gets or sets the elapsed time in whole and fractional seconds.
        /// </summary>
        private volatile Single DeltaSeconds;

        /// <summary>
        /// Gets or sets the queue of particle effects which need to be updated by the worker thread.
        /// </summary>
        private Queue<ParticleEffect> WorkQueue;

        /// <summary>
        /// Notifies the worker thread when to stop running.
        /// </summary>
        private volatile Boolean RunWorkerThread;

        /// <summary>
        /// Initialises a new instance of the AsyncUpdateManager class.
        /// </summary>
#if WINDOWS || WINDOWS_PHONE
        public AsyncUpdateManager()
#elif XBOX
        public AsyncUpdateManager(Int32 processorAffinity)
#endif
        {
            this.WorkerThread = new Thread(new ThreadStart(this.WorkerThread_Body))
            {
                Name = "AsyncUpdateManager",
                IsBackground = true
            };
#if XBOX
            Check.ArgumentWithinRange("processortAffinity", processorAffinity, 3, 5);

            this.WorkerThread.SetProcessorAffinity(new Int32[] { processorAffinity });
#endif
            this.WorkAvailable = new ResetEvent(false);
            this.WorkDone      = new ResetEvent(true);
            this.WorkQueue     = new Queue<ParticleEffect>();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (this.WorkAvailable != null)
                {
                    this.WorkAvailable.Dispose();

                    this.WorkAvailable = null;
                }

                if (this.WorkDone != null)
                {
                    this.WorkDone.Dispose();

                    this.WorkDone = null;
                }
            }
        }

        /// <summary>
        /// Dispose any unmanaged resources being used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="AsyncUpdateManager"/>
        /// is reclaimed by garbage collection.
        /// </summary>
        ~AsyncUpdateManager()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Starts the worker thread running in the background.
        /// </summary>
        public void Start()
        {
            Check.False(this.RunWorkerThread, "AsyncUpdateManager has already been started!");

            this.RunWorkerThread = true;

            this.WorkerThread.Start();
        }

        /// <summary>
        /// Stops the worker thread.
        /// </summary>
        public void Stop()
        {
            Check.True(this.RunWorkerThread, "AsyncUpdateManager must first be started!");

            this.RunWorkerThread = false;

            this.WorkAvailable.Set();

            this.WorkerThread.Join(-1);

#if WINDOWS
            this.WorkAvailable.WaitHandle.Close();
            this.WorkDone.WaitHandle.Close();
#elif XBOX || WINDOWS_PHONE
            this.WorkAvailable.Close();
            this.WorkDone.Close();
#endif
        }

        /// <summary>
        /// Passes the specified particle effects to the worker thread for updating.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed time in whole and fractional seconds.</param>
        /// <param name="effects">The particle effects that should be updated.</param>
        public void BeginUpdate(Single deltaSeconds, params ParticleEffect[] effects)
        {
#if WINDOWS
            Check.True(this.WorkDone.IsSet, "Must call EndUpdate first!");
#endif
            this.DeltaSeconds = deltaSeconds;

            foreach (ParticleEffect effect in effects)
                this.WorkQueue.Enqueue(effect);

            this.WorkDone.Reset();
            this.WorkAvailable.Set();
        }

        /// <summary>
        /// Blocks the calling thread until the worker thread has finished updating outstanding particle effects.
        /// </summary>
        public void EndUpdate()
        {
#if WINDOWS
            Check.True(this.WorkAvailable.IsSet, "Must call BeginUpdate first!");

            this.WorkDone.Wait(-1, CancellationToken.None);
#elif XBOX || WINDOWS_PHONE
            this.WorkDone.WaitOne(-1);
#endif
            this.WorkAvailable.Reset();
        }

        /// <summary>
        /// Worker thread body.
        /// </summary>
        private void WorkerThread_Body()
        {
            while (this.RunWorkerThread)
            {
#if WINDOWS
                this.WorkAvailable.Wait(-1, CancellationToken.None);
#elif XBOX || WINDOWS_PHONE
                this.WorkAvailable.WaitOne(-1);
#endif
                lock (this.WorkQueue)
                {
                    while (this.WorkQueue.Count > 0)
                    {
                        ParticleEffect effect = this.WorkQueue.Dequeue();

                        lock (effect)
                        {
                            foreach (var emitter in effect.Emitters)
                                emitter.Update(this.DeltaSeconds);
                        }
                    }

                    this.WorkDone.Set();
                }
            }
        }
    }
}