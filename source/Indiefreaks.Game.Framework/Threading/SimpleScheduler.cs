using System;
using System.Collections.Generic;
using System.Threading;

namespace Indiefreaks.Xna.Threading
{
    /// <summary>
    /// A simple work scheduler class, implemented with
    /// a blocking queue (producer-consumer).
    /// </summary>
    public class SimpleScheduler
        :IWorkScheduler
    {
#if XBOX
        static int affinityIndex;
#endif

        Stack<Task> scheduledItems;
        Semaphore semaphore;

        /// <summary>
        /// Creates a new instance of the <see cref="SimpleScheduler"/> class.
        /// </summary>
        public SimpleScheduler()
#if XBOX
            : this(4)
#else
            : this(Environment.ProcessorCount)
#endif
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SimpleScheduler"/> class.
        /// </summary>
        /// <param name="threadCount">The number of worker threads to create.</param>
        public SimpleScheduler(int threadCount)
        {
            scheduledItems = new Stack<Task>();
            semaphore = new Semaphore(0);

            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(new ThreadStart(WorkerLoop));
                thread.IsBackground = true;
                thread.Name = "ParallelTasks Worker";
                thread.Start();
            }
        }

        void WorkerLoop()
        {
#if XBOX
            int i = Interlocked.Increment(ref affinityIndex) - 1;
            int affinity = ThreadPool.XboxProcessorAffinity[i % ThreadPool.XboxProcessorAffinity.Length];
            Thread.CurrentThread.SetProcessorAffinity((int)affinity);
#endif
            semaphore.WaitOne();
            Task work = new Task();

            while (true)
            {
                if (scheduledItems.Count > 0)
                {
                    bool foundWork = false;
                    lock (scheduledItems)
                    {
                        if (scheduledItems.Count > 0)
                        {
                            work = scheduledItems.Pop();
                            foundWork = true;
                        }
                    }

                    if (foundWork)
                    {
                        work.DoWork();
                        semaphore.WaitOne();
                    }
                }
                else
                {
                    var replicable = WorkItem.Replicable;
                    if (replicable.HasValue)
                        replicable.Value.DoWork();
                    WorkItem.SetReplicableNull(replicable);
                }
            }
        }

        /// <summary>
        /// Schedules a task for execution.
        /// </summary>
        /// <param name="work">The task to schedule.</param>
        public void Schedule(Task work)
        {
            int threads = work.Item.Work.Options.MaximumThreads;
            lock (scheduledItems)
                scheduledItems.Push(work);
            if (threads > 0)
                WorkItem.Replicable = work;
            semaphore.Release();
        }
    }
}
