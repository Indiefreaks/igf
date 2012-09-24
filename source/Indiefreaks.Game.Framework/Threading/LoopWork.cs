using System;
using System.Collections.Generic;
using System.Threading;

namespace Indiefreaks.Xna.Threading
{
    class ForLoopWork
        : IWork
    {
        private static readonly Pool<ForLoopWork> Pool = new Pool<ForLoopWork>();

        private Action<int> _action;
        private int _length;
        private int _stride;
        private int _index;

        public WorkOptions Options { get; private set; }

        public ForLoopWork()
        {
            Options = new WorkOptions() { MaximumThreads = int.MaxValue };
        }

        public void Prepare(Action<int> action, int startInclusive, int endExclusive, int stride)
        {
            this._action = action;
            this._index = startInclusive;
            this._length = endExclusive;
            this._stride = stride;
        }

        public void DoWork()
        {
            int start;
            while ((start = IncrementIndex()) < _length)
            {
                int end = Math.Min(start + _stride, _length);
                for (int i = start; i < end; i++)
                {
                    _action(i);
                }
            }
        }

        private int IncrementIndex()
        {
#if XBOX
            int x;
            do
            {
                x = _index;
            } while (Interlocked.CompareExchange(ref _index, x + _stride, x) != x);
            return x;
#else
            var i = _index;
            return Interlocked.Add(ref i, _stride) - _stride;
#endif
        }

        public static ForLoopWork Get()
        {
            return Pool.Get();
        }

        public void Return()
        {
            Pool.Return(this);
        }
    }

    class ForEachLoopWork<T>
        : IWork
    {
        static readonly Pool<ForEachLoopWork<T>> Pool = Pool<ForEachLoopWork<T>>.Instance;

        private Action<T> _action;
        private IEnumerator<T> _enumerator;
        private volatile bool _notDone;
        private readonly object _syncLock;

        public WorkOptions Options { get; private set; }

        public ForEachLoopWork()
        {
            Options = new WorkOptions() { MaximumThreads = int.MaxValue };
            _syncLock = new object();
        }

        public void Prepare(Action<T> action, IEnumerator<T> enumerator)
        {
            this._action = action;
            this._enumerator = enumerator;
            this._notDone = true;
        }

        public void DoWork()
        {
            T item = default(T);
            while (_notDone)
            {
                bool haveValue = false;
                lock (_syncLock)
                {
                    _notDone = _enumerator.MoveNext();
                    if (_notDone)
                    {
                        item = _enumerator.Current;
                        haveValue = true;
                    }
                }

                if (haveValue)
                    _action(item);
            }
        }

        public static ForEachLoopWork<T> Get()
        {
            return Pool.Get();
        }

        public void Return()
        {
            Pool.Return(this);
        }
    }
}
