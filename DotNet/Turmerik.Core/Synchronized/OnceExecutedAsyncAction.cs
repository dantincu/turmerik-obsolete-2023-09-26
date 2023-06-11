using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Turmerik.Synchronized
{
    public interface IOnceExecutedAsyncAction
    {
        bool HasBeenExecuted { get; }

        Task<bool> ExecuteIfFirstTimeAsync();
    }

    public interface IOnceExecutedAsyncActionFactory
    {
        IOnceExecutedAsyncAction Create(Func<Task> action);
    }

    public class OnceExecutedAsyncAction : IOnceExecutedAsyncAction
    {
        private readonly Func<Task> action;

        private volatile int hasBeenExecuted;

        public OnceExecutedAsyncAction(
            Func<Task> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public bool HasBeenExecuted => hasBeenExecuted == 1;

        public async Task<bool> ExecuteIfFirstTimeAsync()
        {
            var execute = Interlocked.CompareExchange(
                ref this.hasBeenExecuted,
                1,
                this.hasBeenExecuted) == 0;

            if (execute)
            {
                await action();
            }

            return execute;
        }
    }

    public class OnceExecutedAsyncActionFactory : IOnceExecutedAsyncActionFactory
    {
        public IOnceExecutedAsyncAction Create(Func<Task> action) => new OnceExecutedAsyncAction(action);
    }
}
