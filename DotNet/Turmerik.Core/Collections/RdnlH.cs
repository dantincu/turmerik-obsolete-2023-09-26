using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Collections
{
    public static class RdnlH
    {
        public static ReadOnlyCollection<T> RdnlC<T>(this IList<T> list)
        {
            var colcnt = new ReadOnlyCollection<T>(list);
            return colcnt;
        }

        public static ReadOnlyCollection<T> RdnlC<T>(this IEnumerable<T> nmrbl)
        {
            var colcnt = nmrbl.ToArray().RdnlC();
            return colcnt;
        }

        public static ReadOnlyDictionary<TKey, TValue> RdnlD<TKey, TValue>(
            this IDictionary<TKey, TValue> dicntr)
        {
            var rdnlDictnr = new ReadOnlyDictionary<TKey, TValue>(dicntr);
            return rdnlDictnr;
        }

        public static Lazy<ReadOnlyCollection<TOut>> LzRdnlC<TIn, TOut>(
            this Lazy<ReadOnlyCollection<TIn>> lazy,
            Func<TIn, bool> filter,
            Func<TIn, TOut> selector)
        {
            var retLazy = new Lazy<ReadOnlyCollection<TOut>>(
                () => lazy.Value.Where(filter).Select(selector).RdnlC());

            return retLazy;
        }

        public static Lazy<ReadOnlyCollection<T>> LzRdnlC<T>(
            this Lazy<ReadOnlyCollection<T>> lazy,
            Func<T, bool> filter)
        {
            var retLazy = new Lazy<ReadOnlyCollection<T>>(
                () => lazy.Value.Where(filter).RdnlC());

            return retLazy;
        }
    }
}
