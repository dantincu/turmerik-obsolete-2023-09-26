using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turmerik.Collections
{
    public static partial class QueueH
    {
        public static bool TryDequeueVal<T>(
            this Queue<T> queue,
            out T value)
        {
            bool hasValue = queue.Any();

            if (hasValue)
            {
                value = queue.Dequeue();
            }
            else
            {
                value = default;
            }

            return hasValue;
        }
    }
}
