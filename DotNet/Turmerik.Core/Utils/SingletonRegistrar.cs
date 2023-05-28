using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Utils
{
    public interface ISingletonRegistrar<TData>
        where TData : class
    {
        TData Data { get; }
        TData SynchronizedData { get; }
        event Action<TData> DataRegistered;
        void RegisterData(TData data);
    }

    public class SingletonRegistrar<TData> : ISingletonRegistrar<TData>
        where TData : class
    {
        private readonly object syncRoot = new object();

        private TData data;
        private Action<TData> dataRegistered;

        public TData Data => data;

        public TData SynchronizedData
        {
            get
            {
                TData data;

                lock (syncRoot)
                {
                    data = this.data;
                }

                return data;
            }
        }

        public void RegisterData(TData data)
        {
            if (this.data == null)
            {
                lock (syncRoot)
                {
                    if (this.data == null)
                    {
                        this.data = data;
                    }
                    else
                    {
                        ThrowCannotRegisterTwice();
                    }
                }
            }
            else
            {
                ThrowCannotRegisterTwice();
            }

            this.dataRegistered?.Invoke(data);
        }

        public event Action<TData> DataRegistered
        {
            add => this.dataRegistered += value;
            remove => this.dataRegistered -= value;
        }

        private void ThrowCannotRegisterTwice()
        {
            throw new InvalidOperationException("Cannot register the singleton twice");
        }
    }
}
