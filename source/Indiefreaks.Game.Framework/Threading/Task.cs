using System;

namespace Indiefreaks.Xna.Threading
{
    /// <summary>
    /// A struct which represents a single execution of an IWork instance.
    /// </summary>
    public struct Task
    {
        private readonly bool _valid;
        internal WorkItem Item { get; private set; }
        internal int Id { get; private set; }

        /// <summary>
        /// Gets a value which indicates if this task has completed.
        /// </summary>
        public bool IsComplete
        {
            get { return !_valid || Item.RunCount != Id; }
        }

        /// <summary>
        /// Gets an array containing any exceptions thrown by this task.
        /// </summary>
        public Exception[] Exceptions
        {
            get
            {
                if (_valid && IsComplete)
                {
                    Exception[] e;
                    Item.Exceptions.TryGet(Id, out e);
                    return e;
                }
                return null;
            }
        }

        internal Task(WorkItem item)
            : this()
        {
            Id = item.RunCount;
            Item = item;
            _valid = true;
        }

        /// <summary>
        /// Waits for the task to complete.
        /// </summary>
        public void Wait()
        {
            if (_valid)
            {
                var currentTask = WorkItem.CurrentTask;
                if (currentTask.HasValue && currentTask.Value.Item == Item && currentTask.Value.Id == Id)
                    throw new InvalidOperationException("A task cannot wait on itself.");
                Item.Wait(Id);
            }
        }

        internal void DoWork()
        {
            if (_valid)
                Item.DoWork(Id);
        }
    }
}
