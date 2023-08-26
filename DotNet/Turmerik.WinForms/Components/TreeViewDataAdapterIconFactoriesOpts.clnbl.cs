using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.WinForms.Components
{
    public static class TreeViewDataAdapterIconFactoriesOpts
    {
        public interface IClnbl<TValue> : TreeViewDataAdapterOpts.IClnbl<Func<TreeNodeArg<TValue>, KeyValuePair<int, string>>, TValue>
        {
        }

        public class Immtbl<TValue> : TreeViewDataAdapterOpts.Immtbl<Func<TreeNodeArg<TValue>, KeyValuePair<int, string>>, TValue>, IClnbl<TValue>
        {
            public Immtbl(IClnbl<TValue> src) : base(src)
            {
            }
        }

        public class Mtbl<TValue> : TreeViewDataAdapterOpts.Mtbl<Func<TreeNodeArg<TValue>, KeyValuePair<int, string>>, TValue>, IClnbl<TValue>
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl<TValue> src) : base(src)
            {
            }
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
                kvp => kvp.Key, kvp => kvp.Value.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl<TValue>> AsMtblDictnr<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> src) => src as Dictionary<TKey, Mtbl<TValue>> ?? (
            src as ReadOnlyDictionary<TKey, Immtbl<TValue>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsMtbl());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> ToClnblDictnr<TKey, TValue>(
            this Dictionary<TKey, Mtbl<TValue>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TValue>>());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TValue>>> ToClnblDictnr<TKey, TValue>(
            this ReadOnlyDictionary<TKey, Immtbl<TValue>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TValue>>());
    }
}
