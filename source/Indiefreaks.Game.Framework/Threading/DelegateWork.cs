using System;

namespace Indiefreaks.Xna.Threading
{
    class DelegateWork
            : IWork
    {
        static Pool<DelegateWork> instances = new Pool<DelegateWork>();

        public Action Action { get; set; }
        public WorkOptions Options { get; set; }

        public DelegateWork()
        {
        }

        public void DoWork()
        {
            Action();
            Action = null;
            instances.Return(this);
        }

        internal static DelegateWork GetInstance()
        {
            return instances.Get();
        }
    }
}
