using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Synchronized
{
    public interface IOnceExecutedAction
    {
        bool HasBeenExecuted { get; }

        bool ExecuteIfFirstTime();
    }

    public interface IOnceExecutedActionFactory
    {
        IOnceExecutedAction Create(Action action);
    }

    public class OnceExecutedAction : IOnceExecutedAction
    {
        private readonly Action action;

        private volatile int hasBeenExecuted;

        public OnceExecutedAction(
            Action action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public bool HasBeenExecuted => hasBeenExecuted == 1;

        public bool ExecuteIfFirstTime()
        {
            var execute = Interlocked.CompareExchange(
                ref this.hasBeenExecuted,
                1,
                this.hasBeenExecuted) == 0;

            if (execute)
            {
                action();
            }

            return execute;
        }
    }

    public class OnceExecutedActionFactory : IOnceExecutedActionFactory
    {
        public IOnceExecutedAction Create(Action action) => new OnceExecutedAction(action);
    }
}
