using System;
using System.Collections;
using System.Collections.Generic;

namespace Turmerik.Collections
{
    public class TransformedEnumerator<TSrcType, TDestnType> : IEnumerator<TDestnType>
    {
        private readonly IEnumerator<TSrcType> inputNmrtr;
        private readonly Func<TSrcType, TDestnType> convertor;

        public TransformedEnumerator(
            IEnumerator<TSrcType> inputNmrtr,
            Func<TSrcType, TDestnType> convertor)
        {
            this.inputNmrtr = inputNmrtr ?? throw new ArgumentNullException(nameof(inputNmrtr));
            this.convertor = convertor ?? throw new ArgumentNullException(nameof(convertor));
        }

        public TDestnType Current => convertor(inputNmrtr.Current);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            inputNmrtr.Dispose();
        }

        public bool MoveNext() => inputNmrtr.MoveNext();
        public void Reset() => inputNmrtr.Reset();
    }
}
