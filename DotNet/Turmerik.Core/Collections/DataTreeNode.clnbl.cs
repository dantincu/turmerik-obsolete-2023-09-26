using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Turmerik.Cloneable;
using Turmerik.Utils;

namespace Turmerik.Collections
{
    public static class DataTreeNode
    {
        public interface IClnblCore
        {
            object GetValue();

            IClnblCore GetParentCore();
            IEnumerable<IClnblCore> GetChildrenCore();
        }

        public interface IClnbl<TValue> : IClnblCore
        {
            TValue Value { get; }
            IClnbl<TValue> GetParent();
            IEnumerable<IClnbl<TValue>> GetChildren();
        }

        public class Immtbl<TValue> : IClnbl<TValue>
        {
            public Immtbl(IClnbl<TValue> src)
            {
                Value = src.Value;
                Parent = src.GetParent()?.AsImmtbl();
                Children = src.GetChildren().AsImmtblCllctn();
            }

            public TValue Value { get; }
            public Immtbl<TValue> Parent { get; }
            public ReadOnlyCollection<Immtbl<TValue>> Children { get; }

            public IClnblCore GetParentCore() => Parent;
            public IEnumerable<IClnblCore> GetChildrenCore() => Children;
            public IClnbl<TValue> GetParent() => Parent;
            public IEnumerable<IClnbl<TValue>> GetChildren() => Children;

            public object GetValue() => Value;
        }

        public class Mtbl<TValue> : IClnbl<TValue>
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl<TValue> src)
            {
                Value = src.Value;
                Parent = src.GetParent()?.AsMtbl();
                Children = src.GetChildren().AsMtblList();
            }

            public TValue Value { get; set; }
            public Mtbl<TValue> Parent { get; set; }
            public List<Mtbl<TValue>> Children { get; set; }

            public IClnblCore GetParentCore() => Parent;
            public IEnumerable<IClnblCore> GetChildrenCore() => Children;
            public IClnbl<TValue> GetParent() => Parent;
            public IEnumerable<IClnbl<TValue>> GetChildren() => Children;

            public object GetValue() => Value;
        }

        public static Immtbl<TValue> ToImmtbl<TValue>(
            this IClnbl<TValue> src) => new Immtbl<TValue>(src);

        public static Immtbl<TValue> AsImmtbl<TValue>(
            this IClnbl<TValue> src) => src as Immtbl<TValue> ?? src?.ToImmtbl();

        public static Mtbl<TValue> ToMtbl<TValue>(
            this IClnbl<TValue> src) => new Mtbl<TValue>(src);

        public static Mtbl<TValue> AsMtbl<TValue>(
            this IClnbl<TValue> src) => src as Mtbl<TValue> ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl<TValue>> ToImmtblCllctn<TValue>(
            this IEnumerable<IClnbl<TValue>> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl<TValue>> AsImmtblCllctn<TValue>(
            this IEnumerable<IClnbl<TValue>> src) =>
            src as ReadOnlyCollection<Immtbl<TValue>> ?? src?.ToImmtblCllctn();

        public static List<Mtbl<TValue>> ToMtblList<TValue>(
            this IEnumerable<IClnbl<TValue>> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl<TValue>> AsMtblList<TValue>(
            this IEnumerable<IClnbl<TValue>> src) => src as List<Mtbl<TValue>> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl<TValue>> AsImmtblDictnr<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> src) => src as ReadOnlyDictionary<TKey, Immtbl<TValue>> ?? (
            src as Dictionary<TKey, Mtbl<TValue>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl<TValue>> AsMtblDictnr<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> src) => src as Dictionary<TKey, Mtbl<TValue>> ?? (
            src as ReadOnlyDictionary<TKey, Immtbl<TValue>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> ToClnblDictnr<TKey, TValue>(
            this Dictionary<TKey, Mtbl<TValue>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TValue>>());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> ToClnblDictnr<TKey, TValue>(
            this ReadOnlyDictionary<TKey, Immtbl<TValue>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TValue>>());
    }
}