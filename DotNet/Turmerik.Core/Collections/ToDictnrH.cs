﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Core.Collections
{
    public static class ToDictnrH
    {
        public static Dictionary<TKey, TValue> Dictnr<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> nmrbl) where TKey : notnull
        {
            var dictnr = nmrbl.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value);

            return dictnr;
        }

        public static Dictionary<TKey, TValue> Dictnr<TItem, TKey, TValue>(
            this IEnumerable<TItem> nmrbl,
            Func<TItem, int, TKey> keySelector,
            Func<TItem, int, TValue> valueSelector) where TKey : notnull
        {
            var kvpNmrbl = nmrbl.Select(
                (item, idx) => new KeyValuePair<TKey, TValue>(
                    keySelector(item, idx),
                    valueSelector(item, idx)));

            var retDictnr = kvpNmrbl.Dictnr();
            return retDictnr;
        }
    }
}
