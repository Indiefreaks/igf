using System;
using System.Threading;
using System.Diagnostics;
using Indiefreaks.Xna.Collections;

namespace Indiefreaks.Xna.Threading
{
    class Worker
    {
        readonly Thread _thread;
        readonly Deque<Task> _tasks;
        readonly WorkStealingScheduler _scheduler;

        public bool LookingForWork { get; private set; }
        public AutoResetEvent Gate { get; private set; }

        static readonly Hashtable<Thread, Worker> Workers = new Hashtable<Thread, Worker>(Environment.ProcessorCount);
        public static Worker CurrentWorker
        {
            get
            {
                var currentThread = Thread.CurrentThread;
                Worker worker;
                if (Workers.TryGet(currentThread, out worker))
                    return worker;
                return null;
            }
        }

#if XBOX
        static int affinityIndex;
#endif

        public Worker(WorkStealingScheduler scheduler)
        {
            this._thread = new Thread(Work);
            this._thread.Name = "ParallelTasks Worker";
            this._thread.IsBackground = true;
            this._tasks = new Deque<Task>();
            this._scheduler = scheduler;
            this.Gate = new AutoResetEvent(false);

            Workers.Add(_thread, this);
        }

        public void Start()
        {
            _thread.Start();
        }

        public void AddWork(Task task)
        {
            _tasks.LocalPush(task);
        }

        private void Work()
        {
#if XBOX
            int i = Interlocked.Increment(ref affinityIndex) - 1;
            int affinity = ThreadPool.XboxProcessorAffinity[i % ThreadPool.XboxProcessorAffinity.Length];
            Thread.CurrentThread.SetProcessorAffinity((int)affinity);
#endif

            Task task = new Task();
            while (true)
            {
                if (_tasks.LocalPop(ref task))
                {
                    task.DoWork();
                    try { task.DoWork(); }
                    catch (Exception e) { Debug.WriteLine(e); throw; }
                }
                else
                    FindWork();
            }
        }

        private void FindWork()
        {
            LookingForWork = true;
            Task task;
            bool foundWork = false;
            do
            {
                if (_scheduler.TryGetTask(out task))
                {
                    foundWork = true;
                    break;
                }

                var replicable = WorkItem.Replicable;
                if (replicable.HasValue)
                {
                    replicable.Value.DoWork();
                    WorkItem.SetReplicableNull(replicable);
                }

                for (int i = 0; i < _scheduler.Workers.Count; i++)
                {
                    var worker = _scheduler.Workers[i];
                    if (worker == this)
                        continue;

                    if (worker._tasks.TrySteal(ref task))
                    {
                        foundWork = true;
                        break;
                    }
                }

                Gate.WaitOne();
            } while (!foundWork);

            LookingForWork = false;
            _tasks.LocalPush(task);
        }
    }
}
