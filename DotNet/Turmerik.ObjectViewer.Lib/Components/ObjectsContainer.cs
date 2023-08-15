using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turmerik.ObjectViewer.Lib.Components
{
    public interface IObjectsContainer : IDictionary<string, object>
    {
    }

    public class ObjectsContainer : IObjectsContainer
    {
        private readonly Dictionary<string, object> inner;

        private Action<string, object> itemSet;
        private Action<string> keyRemoved;
        private Action cleared;

        private ObjectsContainer()
        {
            inner = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get => inner[key];

            set
            {
                inner[key] = value;
                itemSet?.Invoke(key, value);
            }
        }

        public ICollection<string> Keys => inner.Keys;

        public ICollection<object> Values => inner.Values;

        public int Count => inner.Count;

        public bool IsReadOnly => false;

        public event Action<string, object> ItemSet
        {
            add => itemSet += value;
            remove => itemSet -= value;
        }

        public event Action<string> KeyRemoved
        {
            add => keyRemoved += value;
            remove => keyRemoved -= value;
        }

        public event Action Cleared
        {
            add => cleared += value;
            remove => cleared -= value;
        }

        public void Add(string key, object value)
        {
            inner.Add(key, value);
            itemSet?.Invoke(key, value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            inner.Clear();
            cleared?.Invoke();
        }

        public bool Contains(KeyValuePair<string, object> item) => inner.Contains(item);

        public bool ContainsKey(string key) => inner.ContainsKey(key);

        public void CopyTo(
            KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, object>>)inner).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => inner.GetEnumerator();

        public bool Remove(string key)
        {
            bool removed = inner.Remove(key);

            if (removed)
            {
                keyRemoved?.Invoke(key);
            }

            return removed;
        }

        public bool Remove(
            KeyValuePair<string, object> item) => Remove(item.Key);

        public bool TryGetValue(
            string key,
            out object value) => inner.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
