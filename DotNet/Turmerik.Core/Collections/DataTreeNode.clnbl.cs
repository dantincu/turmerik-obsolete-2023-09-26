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
            bool IsLeaf { get; }
            int LoadedChildrenDepth { get; }

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
            public Immtbl(
                IClnbl<TValue> src,
                bool omitParent = true)
            {
                IsLeaf = src.IsLeaf;
                LoadedChildrenDepth = src.LoadedChildrenDepth;
                Value = src.Value;
                Children = src.GetChildren().AsImmtblCllctn();

                if (!omitParent)
                {
                    Parent = src.GetParent()?.AsImmtbl();
                }
            }

            public bool IsLeaf { get; }
            public int LoadedChildrenDepth { get; }
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

            public Mtbl(
                IClnbl<TValue> src,
                bool omitParent = true)
            {
                IsLeaf = src.IsLeaf;
                LoadedChildrenDepth = src.LoadedChildrenDepth;
                Value = src.Value;
                Children = src.GetChildren().AsMtblList();

                if (!omitParent)
                {
                    Parent = src.GetParent()?.AsMtbl();
                }
            }

            public bool IsLeaf { get; set; }
            public int LoadedChildrenDepth { get; set; }
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
            this IClnbl<TValue> src,
            bool omitParent = true) => new Immtbl<TValue>(src, omitParent);

        public static Immtbl<TValue> AsImmtbl<TValue>(
            this IClnbl<TValue> src,
            bool omitParent = true) => src as Immtbl<TValue> ?? src?.ToImmtbl(omitParent);

        public static Mtbl<TValue> ToMtbl<TValue>(
            this IClnbl<TValue> src,
            bool omitParent = true) => new Mtbl<TValue>(src, omitParent);

        public static Mtbl<TValue> AsMtbl<TValue>(
            this IClnbl<TValue> src,
            bool omitParent = true) => src as Mtbl<TValue> ?? src?.ToMtbl(omitParent);

        public static ReadOnlyCollection<Immtbl<TValue>> ToImmtblCllctn<TValue>(
            this IEnumerable<IClnbl<TValue>> src,
            bool omitParents = true) => src?.Select(
                item => item?.AsImmtbl(omitParents)).RdnlC();

        public static ReadOnlyCollection<Immtbl<TValue>> AsImmtblCllctn<TValue>(
            this IEnumerable<IClnbl<TValue>> src,
            bool omitParents = true) =>
            src as ReadOnlyCollection<Immtbl<TValue>> ?? src?.ToImmtblCllctn(omitParents);

        public static List<Mtbl<TValue>> ToMtblList<TValue>(
            this IEnumerable<IClnbl<TValue>> src,
            bool omitParents = true) => src?.Select(
                item => item?.AsMtbl(omitParents)).ToList();

        public static List<Mtbl<TValue>> AsMtblList<TValue>(
            this IEnumerable<IClnbl<TValue>> src,
            bool omitParents = true) => src as List<Mtbl<TValue>> ?? src?.ToMtblList(omitParents);

        public static ReadOnlyDictionary<TKey, Immtbl<TValue>> AsImmtblDictnr<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> src,
            bool omitParents = true) => src as ReadOnlyDictionary<TKey, Immtbl<TValue>> ?? (
            src as Dictionary<TKey, Mtbl<TValue>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl(omitParents)).RdnlD();

        public static Dictionary<TKey, Mtbl<TValue>> AsMtblDictnr<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> src,
            bool omitParents = true) => src as Dictionary<TKey, Mtbl<TValue>> ?? (
            src as ReadOnlyDictionary<TKey, Immtbl<TValue>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl(omitParents));

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> ToClnblDictnr<TKey, TValue>(
            this Dictionary<TKey, Mtbl<TValue>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TValue>>());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> ToClnblDictnr<TKey, TValue>(
            this ReadOnlyDictionary<TKey, Immtbl<TValue>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TValue>>());
    }
}