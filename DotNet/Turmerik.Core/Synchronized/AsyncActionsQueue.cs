using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turmerik.Collections;

namespace Turmerik.Synchronized
{
    public interface IAsyncActionsQueue : IDisposable
    {
        void Enqueue(Func<Task> callback);
    }

    public class AsyncActionsQueue : IAsyncActionsQueue
    {
        private readonly IThreadSafeActionComponent threadSafeActionComponent;
        private readonly Queue<Func<Task>> callbacksQueue;

        private volatile int isConsuming;

        private Thread consumingThread;

        public AsyncActionsQueue(
            IThreadSafeActionComponent threadSafeActionComponent)
        {
            this.threadSafeActionComponent = threadSafeActionComponent ?? throw new ArgumentNullException(
                nameof(threadSafeActionComponent));

            callbacksQueue = new Queue<Func<Task>>();
        }

        public void Dispose()
        {
            threadSafeActionComponent.Dispose();
        }

        public void Enqueue(Func<Task> callback)
        {
            threadSafeActionComponent.Execute(() =>
            {
                callbacksQueue.Enqueue(callback);

                if (Interlocked.CompareExchange(ref isConsuming, 1, 0) == 0)
                {
                    consumingThread = new Thread(Consume);
                }
            });
        }

        private void Consume()
        {
            threadSafeActionComponent.Execute(() =>
            {
                if (callbacksQueue.TryDequeueVal(out var nextAction))
                {
                    nextAction.Invoke().ContinueWith(task =>
                    {
                        Consume();
                    });
                }
                else
                {
                    consumingThread = null;
                }
            });
        }
    }
}
