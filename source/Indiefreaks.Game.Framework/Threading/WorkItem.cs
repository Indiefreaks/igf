using System;
using System.Collections.Generic;
using System.Threading;
using Indiefreaks.Xna.Collections;

namespace Indiefreaks.Xna.Threading
{
    internal class WorkItem
    {
        private static SpinLock _replicableLock = new SpinLock();
        private static Task? _replicable;

        private static List<WorkItem> _awaitingCallbacks;
        private static readonly Pool<WorkItem> IdleWorkItems = new Pool<WorkItem>();

#if WINDOWS_PHONE
        private static readonly Hashtable<Thread, Stack<Task>> RunningTasks =
            new Hashtable<Thread, Stack<Task>>(1);
#else
        private static readonly Hashtable<Thread, Stack<Task>> RunningTasks =
            new Hashtable<Thread, Stack<Task>>(Environment.ProcessorCount);
#endif

        private readonly List<Task> _children;

        private readonly Hashtable<int, Exception[]> _exceptions;
        private readonly ManualResetEvent _resetEvent;
        private List<Exception> _exceptionBuffer;
        private volatile int _executing;
        private SpinLock _executionLock;
        private volatile int _runCount;
        private volatile int _waitCount;
        private IWork _work;

        public WorkItem()
        {
            _resetEvent = new ManualResetEvent(true);
            _exceptions = new Hashtable<int, Exception[]>(1);
            _children = new List<Task>();
            _waitCount = 0;
            _executionLock = new SpinLock();
        }


        internal static Task? Replicable
        {
            get
            {
                try
                {
                    _replicableLock.Enter();
                    return _replicable;
                }
                finally
                {
                    _replicableLock.Exit();
                }
            }
            set
            {
                try
                {
                    _replicableLock.Enter();
                    _replicable = value;
                }
                finally
                {
                    _replicableLock.Exit();
                }
            }
        }

        internal static List<WorkItem> AwaitingCallbacks
        {
            get
            {
                if (_awaitingCallbacks == null)
                {
                    var instance = new List<WorkItem>();
                    Interlocked.CompareExchange(ref _awaitingCallbacks, instance, null);
                }

                return _awaitingCallbacks;
            }
        }

        public int RunCount
        {
            get { return _runCount; }
        }

        public Hashtable<int, Exception[]> Exceptions
        {
            get { return _exceptions; }
        }

        public IWork Work
        {
            get { return _work; }
        }

        public Action Callback { get; set; }

        public static Task? CurrentTask
        {
            get
            {
                Stack<Task> tasks;
                if (RunningTasks.TryGet(Thread.CurrentThread, out tasks))
                {
                    if (tasks.Count > 0)
                        return tasks.Peek();
                }
                return null;
            }
        }

        // ReSharper disable PossibleInvalidOperationException
        internal static void SetReplicableNull(Task? task)
        {
            try
            {
                _replicableLock.Enter();
                if (!task.HasValue ||
                    (_replicable.HasValue && _replicable.Value.Id == task.Value.Id &&
                     _replicable.Value.Item == task.Value.Item))
                    _replicable = null;
            }
            finally
            {
                _replicableLock.Exit();
            }
        }
        // ReSharper restore PossibleInvalidOperationException

        public Task PrepareStart(IWork work)
        {
            this._work = work;
            _resetEvent.Reset();
            _children.Clear();
            _exceptionBuffer = null;

            var task = new Task(this);
            var currentTask = WorkItem.CurrentTask;
            if (currentTask.HasValue && currentTask.Value.Item == this)
                throw new Exception("whadafak?");
            if (!work.Options.DetachFromParent && currentTask.HasValue)
                currentTask.Value.Item.AddChild(task);

            return task;
        }

        public bool DoWork(int expectedId)
        {
            try
            {
                _executionLock.Enter();
                if (expectedId < _runCount)
                    return true;
                if (_executing == _work.Options.MaximumThreads)
                    return false;
                _executing++;
            }
            finally
            {
                _executionLock.Exit();
            }

            // associate the current task with this thread, so that Task.CurrentTask gives the correct result
            Stack<Task> tasks = null;
            if (!RunningTasks.TryGet(Thread.CurrentThread, out tasks))
            {
                tasks = new Stack<Task>();
                RunningTasks.Add(Thread.CurrentThread, tasks);
            }
            tasks.Push(new Task(this));

            // execute the task
            try
            {
                _work.DoWork();
            }
            catch (Exception e)
            {
                if (_exceptionBuffer == null)
                {
                    var newExceptions = new List<Exception>();
                    Interlocked.CompareExchange(ref _exceptionBuffer, newExceptions, null);
                }

                lock (_exceptionBuffer)
                    _exceptionBuffer.Add(e);
            }

            tasks.Pop();

            try
            {
                _executionLock.Enter();
                _executing--;
                if (_executing == 0)
                {
                    if (_exceptionBuffer != null)
                        _exceptions.Add(_runCount, _exceptionBuffer.ToArray());

                    // wait for all children to complete
                    foreach (var child in _children)
                        child.Wait();

                    _runCount++;

                    // open the reset event, so tasks waiting on this once can continue
                    _resetEvent.Set();

                    // wait for waiting tasks to all exit
                    while (_waitCount > 0) ;

                    if (Callback == null)
                    {
                        Requeue();
                    }
                    else
                    {
                        // if we have a callback, then queue for execution
                        lock (AwaitingCallbacks)
                            AwaitingCallbacks.Add(this);
                    }

                    return true;
                }
                return false;
            }
            finally
            {
                _executionLock.Exit();
            }
        }

        public void Requeue()
        {
            // requeue the WorkItem for reuse, but only if the runCount hasnt reached the maximum value
            // dont requeue if an exception has been caught, to stop potential memory leaks.
            if (_runCount < int.MaxValue && _exceptionBuffer == null)
                IdleWorkItems.Return(this);
        }

        public void Wait(int id)
        {
            WaitOrExecute(id);

            Exception[] e;
            if (_exceptions.TryGet(id, out e))
                throw new TaskException(e);
        }

        private void WaitOrExecute(int id)
        {
            if (_runCount != id)
                return;

            if (DoWork(id))
                return;

            var count = _waitCount;

            try
            {
                Interlocked.Increment(ref count);
                int i = 0;
                while (_runCount == id)
                {
                    if (i > 1000)
                        _resetEvent.WaitOne();
                    else
                        Thread.Sleep(0);
                    i++;
                }
            }
            finally
            {
                Interlocked.Decrement(ref count);
            }
        }

        public void AddChild(Task item)
        {
            _children.Add(item);
        }

        public static WorkItem Get()
        {
            return IdleWorkItems.Get();
        }
    }
}