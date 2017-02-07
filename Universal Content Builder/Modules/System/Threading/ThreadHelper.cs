using System;
using System.Collections.Generic;
using System.Threading;

namespace Modules.System.Threading
{
    public class ThreadHelper : List<Thread>, IDisposable
    {
        public void Add(ThreadStart threadStart)
        {
            Thread thread = new Thread(threadStart) { IsBackground = true };
            thread.Start();
            Add(thread);
        }

        public void Sleep(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }

        public bool isActive()
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].IsAlive)
                    return true;
            }

            return false;
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (this[i].IsAlive)
                            this[i].Abort();
                    }

                    Clear();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}