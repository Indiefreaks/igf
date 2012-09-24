using System;
using System.Collections.Generic;
using System.Threading;

namespace Indiefreaks.Xna.Threading
{
    /// <summary>
    /// A static class containing factory methods for creating tasks.
    /// </summary>
    public class ThreadPool
    {
        internal static readonly int[] XboxProcessorAffinity = { 3, 4, 5, 1 };

        readonly WorkOptions DefaultOptions = new WorkOptions() { DetachFromParent = false, MaximumThreads = 1 };
        IWorkScheduler scheduler;
        Pool<List<Task>> taskPool = new Pool<List<Task>>();
        List<WorkItem> callbackBuffer = new List<WorkItem>();

        /// <summary>
        /// Executes all task callbacks on a single thread.
        /// This method is not re-entrant. It is suggested you call it only on the main thread.
        /// </summary>
        public void Update()
        {
            lock (WorkItem.AwaitingCallbacks)
            {
                callbackBuffer.AddRange(WorkItem.AwaitingCallbacks);
                WorkItem.AwaitingCallbacks.Clear();
            }

            for (int i = 0; i < callbackBuffer.Count; i++)
            {
                var item = callbackBuffer[i];
                item.Callback();
                item.Callback = null;
                item.Requeue();
            }

            callbackBuffer.Clear();
        }

        /// <summary>
        /// Gets or sets the work scheduler.
        /// This defaults to a <see cref="SimpleScheduler"/> instance.
        /// </summary>
        public IWorkScheduler Scheduler
        {
            get
            {
                if (scheduler == null)
                {
                    IWorkScheduler newScheduler = new WorkStealingScheduler(Environment.ProcessorCount);
                    Interlocked.CompareExchange(ref scheduler, newScheduler, null);
                }

                return scheduler;
            }

            set
            {
                Interlocked.Exchange(ref scheduler, value);
            }
        }

        /// <summary>
        /// Starts a task in a secondary worker thread. Intended for long running, blocking, work
        /// such as I/O.
        /// </summary>
        /// <param name="work">The work to execute.</param>
        /// <returns>A task which represents one execution of the work.</returns>
        public Task StartBackground(IWork work)
        {
            return StartBackground(work, null);
        }

        /// <summary>
        /// Starts a task in a secondary worker thread. Intended for long running, blocking, work
        /// such as I/O.
        /// </summary>
        /// <param name="work">The work to execute.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A task which represents one execution of the work.</returns>
        public Task StartBackground(IWork work, Action completionCallback)
        {
            if (work.Options.MaximumThreads < 1)
                throw new ArgumentException("work.Options.MaximumThreads cannot be less than one.");
            var workItem = WorkItem.Get();
            workItem.Callback = completionCallback;
            var task = workItem.PrepareStart(work);
            BackgroundWorker.StartWork(task);
            return task;
        }

        /// <summary>
        /// Starts a task in a secondary worker thread. Intended for long running, blocking, work
        /// such as I/O.
        /// </summary>
        /// <param name="action">The work to execute.</param>
        /// <returns>A task which represents one execution of the action.</returns>
        public Task StartBackground(Action action)
        {
            return StartBackground(action, null);
        }

        /// <summary>
        /// Starts a task in a secondary worker thread. Intended for long running, blocking, work
        /// such as I/O.
        /// </summary>
        /// <param name="action">The work to execute.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A task which represents one execution of the action.</returns>
        public Task StartBackground(Action action, Action completionCallback)
        {
            var work = DelegateWork.GetInstance();
            work.Action = action;
            work.Options = DefaultOptions;
            return StartBackground(work, completionCallback);
        }

        /// <summary>
        /// Creates and starts a task to execute the given work.
        /// </summary>
        /// <param name="work">The work to execute in parallel.</param>
        /// <returns>A task which represents one execution of the work.</returns>
        public Task Start(IWork work)
        {
            return Start(work, null);
        }

        /// <summary>
        /// Creates and starts a task to execute the given work.
        /// </summary>
        /// <param name="work">The work to execute in parallel.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A task which represents one execution of the work.</returns>
        public Task Start(IWork work, Action completionCallback)
        {
            if (work.Options.MaximumThreads < 1)
                throw new ArgumentException("work.Options.MaximumThreads cannot be less than one.");
            var workItem = WorkItem.Get();
            workItem.Callback = completionCallback;
            var task = workItem.PrepareStart(work);
            Scheduler.Schedule(task);
            return task;
        }

        /// <summary>
        /// Creates and starts a task to execute the given work.
        /// </summary>
        /// <param name="action">The work to execute in parallel.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A task which represents one execution of the work.</returns>
        public Task Start(Action action)
        {
            return Start(action, null);
        }

        /// <summary>
        /// Creates and starts a task to execute the given work.
        /// </summary>
        /// <param name="action">The work to execute in parallel.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A task which represents one execution of the work.</returns>
        public Task Start(Action action, Action completionCallback)
        {
            return Start(action, new WorkOptions() { MaximumThreads = 1, DetachFromParent = false, QueueFifo = false }, completionCallback);
        }

        /// <summary>
        /// Creates and starts a task to execute the given work.
        /// </summary>
        /// <param name="action">The work to execute in parallel.</param>
        /// <param name="options">The work options to use with this action.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A task which represents one execution of the work.</returns>
        public Task Start(Action action, WorkOptions options)
        {
            return Start(action, options, null);
        }

        /// <summary>
        /// Creates and starts a task to execute the given work.
        /// </summary>
        /// <param name="action">The work to execute in parallel.</param>
        /// <param name="options">The work options to use with this action.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A task which represents one execution of the work.</returns>
        public Task Start(Action action, WorkOptions options, Action completionCallback)
        {
            if (options.MaximumThreads < 1)
                throw new ArgumentOutOfRangeException("options", "options.MaximumThreads cannot be less than 1.");
            var work = DelegateWork.GetInstance();
            work.Action = action;
            work.Options = options;
            return Start(work, completionCallback);
        }

        /// <summary>
        /// Creates an starts a task which executes the given function and stores the resuly for later retreval.
        /// </summary>
        /// <typeparam name="T">The type of result the function returns.</typeparam>
        /// <param name="function">The function to execute in parallel.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A future which represults one execution of the function.</returns>
        public Future<T> Start<T>(Func<T> function)
        {
            return Start(function, null);
        }

        /// <summary>
        /// Creates an starts a task which executes the given function and stores the resuly for later retreval.
        /// </summary>
        /// <typeparam name="T">The type of result the function returns.</typeparam>
        /// <param name="function">The function to execute in parallel.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A future which represults one execution of the function.</returns>
        public Future<T> Start<T>(Func<T> function, Action completionCallback)
        {
            return Start<T>(function, DefaultOptions, completionCallback);
        }

        /// <summary>
        /// Creates an starts a task which executes the given function and stores the resuly for later retreval.
        /// </summary>
        /// <typeparam name="T">The type of result the function returns.</typeparam>
        /// <param name="function">The function to execute in parallel.</param>
        /// <param name="options">The work options to use with this action.</param>
        /// <returns>A future which represults one execution of the function.</returns>
        public Future<T> Start<T>(Func<T> function, WorkOptions options)
        {
            return Start<T>(function, options, null);
        }

        /// <summary>
        /// Creates an starts a task which executes the given function and stores the resuly for later retreval.
        /// </summary>
        /// <typeparam name="T">The type of result the function returns.</typeparam>
        /// <param name="function">The function to execute in parallel.</param>
        /// <param name="options">The work options to use with this action.</param>
        /// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
        /// <returns>A future which represults one execution of the function.</returns>
        public Future<T> Start<T>(Func<T> function, WorkOptions options, Action completionCallback)
        {
            if (options.MaximumThreads < 1)
                throw new ArgumentOutOfRangeException("options", "options.MaximumThreads cannot be less than 1.");
            var work = FutureWork<T>.GetInstance();
            work.Function = function;
            work.Options = options;
            var task = Start(work, completionCallback);
            return new Future<T>(task, work);
        }

        /// <summary>
        /// Executes the given work items potentially in parallel with each other.
        /// This method will block until all work is completed.
        /// </summary>
        /// <param name="a">Work to execute.</param>
        /// <param name="b">Work to execute.</param>
        public void Do(IWork a, IWork b)
        {
            Task task = Start(b);
            a.DoWork();
            task.Wait();
        }

        /// <summary>
        /// Executes the given work items potentially in parallel with each other.
        /// This method will block until all work is completed.
        /// </summary>
        /// <param name="work">The work to execute.</param>
        public void Do(params IWork[] work)
        {
            List<Task> tasks = taskPool.Get();

            for (int i = 0; i < work.Length; i++)
            {
                tasks.Add(Start(work[i]));
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].Wait();
            }

            tasks.Clear();
            taskPool.Return(tasks);
        }

        /// <summary>
        /// Executes the given work items potentially in parallel with each other.
        /// This method will block until all work is completed.
        /// </summary>
        /// <param name="action1">The work to execute.</param>
        /// <param name="action2">The work to execute.</param>
        public void Do(Action action1, Action action2)
        {
            var work = DelegateWork.GetInstance();
            work.Action = action2;
            work.Options = DefaultOptions;
            var task = Start(work);
            action1();
            task.Wait();
        }

        /// <summary>
        /// Executes the given work items potentially in parallel with each other.
        /// This method will block until all work is completed.
        /// </summary>
        /// <param name="actions">The work to execute.</param>
        public void Do(params Action[] actions)
        {
            List<Task> tasks = taskPool.Get();

            for (int i = 0; i < actions.Length; i++)
            {
                var work = DelegateWork.GetInstance();
                work.Action = actions[i];
                work.Options = DefaultOptions;
                tasks.Add(Start(work));
            }

            for (int i = 0; i < actions.Length; i++)
            {
                tasks[i].Wait();
            }

            tasks.Clear();
            taskPool.Return(tasks);
        }

        /// <summary>
        /// Executes a for loop, where each iteration can potentially occur in parallel with others.
        /// </summary>
        /// <param name="startInclusive">The index (inclusive) at which to start iterating.</param>
        /// <param name="endExclusive">The index (exclusive) at which to end iterating.</param>
        /// <param name="body">The method to execute at each iteration. The current index is supplied as the parameter.</param>
        public void For(int startInclusive, int endExclusive, Action<int> body)
        {
            For(startInclusive, endExclusive, body, 8);
        }

        /// <summary>
        /// Executes a for loop, where each iteration can potentially occur in parallel with others.
        /// </summary>
        /// <param name="startInclusive">The index (inclusive) at which to start iterating.</param>
        /// <param name="endExclusive">The index (exclusive) at which to end iterating.</param>
        /// <param name="body">The method to execute at each iteration. The current index is supplied as the parameter.</param>
        /// <param name="stride">The number of iterations that each processor takes at a time.</param>
        public void For(int startInclusive, int endExclusive, Action<int> body, int stride)
        {
            var work = ForLoopWork.Get();
            work.Prepare(body, startInclusive, endExclusive, stride);
            var task = Start(work);
            task.Wait();
            work.Return();
        }

        /// <summary>
        /// Executes a foreach loop, where each iteration can potentially occur in parallel with others.
        /// </summary>
        /// <typeparam name="T">The type of item to iterate over.</typeparam>
        /// <param name="collection">The enumerable data source.</param>
        /// <param name="action">The method to execute at each iteration. The item to process is supplied as the parameter.</param>
        public void ForEach<T>(IEnumerable<T> collection, Action<T> action)
        {
            var work = ForEachLoopWork<T>.Get();
            work.Prepare(action, collection.GetEnumerator());
            var task = Start(work);
            task.Wait();
            work.Return();
        }
    }
}
