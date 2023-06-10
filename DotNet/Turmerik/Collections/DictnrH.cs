﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.Collections
{
    public static class DictnrH
    {
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictnr,
            TKey key,
            Func<TKey, TValue> valueFactory)
        {
            TValue value;

            if (!dictnr.TryGetValue(key, out value))
            {
                value = valueFactory(key);
                dictnr.Add(key, value);
            }

            return value;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictnr,
            TKey key,
            Func<TKey, TValue> valueFactory = null)
        {
            TValue value;

            if (!dictnr.TryGetValue(key, out value))
            {
                if (valueFactory != null)
                {
                    value = valueFactory(key);
                }
                else
                {
                    value = default;
                }
            }
            return value;
        }

        public static TValue AddOrUpdate<TKey, TValue>(
            this IDictionary<TKey, TValue> dictnr,
            TKey key,
            Func<TKey, TValue> addValueFactory,
            Func<TKey, TValue, TValue> updateValueFactory)
        {
            if (!dictnr.TryGetValue(key, out var value))
            {
                value = addValueFactory(key);
            }
            else
            {
                value = updateValueFactory(key, value);
            }

            dictnr[key] = value;
            return value;
        }

        public static TValue AddOrUpdate<TKey, TValue>(
        this ConcurrentDictionary<TKey, TValue> dictnr,
        TKey key,
        Func<TKey, TValue> factory,
        UpdateDictnrValue<TKey, TValue> updateFunc) where TKey : notnull
        {
            var val = dictnr.AddOrUpdate(
            key,
                k => updateFunc(k, false, factory(k)),
                (k, v) => updateFunc(k, true, v));

            return val;
        }

        public static TValue AddOrUpdateValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dictnr,
            TKey key,
            Func<TKey, TValue> factory,
            UpdateDictnrValue<TKey, TValue> updateFunc) where TKey : notnull
        {
            var val = dictnr.AddOrUpdate(
                key,
                k => updateFunc(k, false, factory(k)),
                (k, v) => updateFunc(k, true, v));

            return val;
        }
    }
}