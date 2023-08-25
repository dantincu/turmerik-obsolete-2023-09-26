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
    public static class TreeViewDataAdapterIconsOptsCore
    {
        public interface IClnbl<TIcons, TValue>
        {
            TIcons NodeIcon { get; }
            TIcons SelectedNodeIcon { get; }
            TIcons StateNodeIcon { get; }
        }

        public class Immtbl<TIcons, TValue> : IClnbl<TIcons, TValue>
        {
            public Immtbl(IClnbl<TIcons, TValue> src)
            {
                NodeIcon = src.NodeIcon;
                SelectedNodeIcon = src.SelectedNodeIcon;
                StateNodeIcon = src.StateNodeIcon;
            }

            public TIcons NodeIcon { get; }
            public TIcons SelectedNodeIcon { get; }
            public TIcons StateNodeIcon { get; }
        }

        public class Mtbl<TIcons, TValue> : IClnbl<TIcons, TValue>
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl<TIcons, TValue> src)
            {
                NodeIcon = src.NodeIcon;
                SelectedNodeIcon = src.SelectedNodeIcon;
                StateNodeIcon = src.StateNodeIcon;
            }

            public TIcons NodeIcon { get; set; }
            public TIcons SelectedNodeIcon { get; set; }
            public TIcons StateNodeIcon { get; set; }
        }

        public static Immtbl<TIcons, TValue> ToImmtbl<TIcons, TValue>(
            this IClnbl<TIcons, TValue> src) => new Immtbl<TIcons, TValue>(src);

        public static Immtbl<TIcons, TValue> AsImmtbl<TIcons, TValue>(
            this IClnbl<TIcons, TValue> src) => src as Immtbl<TIcons, TValue> ?? src?.ToImmtbl();

        public static Mtbl<TIcons, TValue> ToMtbl<TIcons, TValue>(
            this IClnbl<TIcons, TValue> src) => new Mtbl<TIcons, TValue>(src);

        public static Mtbl<TIcons, TValue> AsMtbl<TIcons, TValue>(
            this IClnbl<TIcons, TValue> src) => src as Mtbl<TIcons, TValue> ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl<TIcons, TValue>> ToImmtblCllctn<TIcons, TValue>(
            this IEnumerable<IClnbl<TIcons, TValue>> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl<TIcons, TValue>> AsImmtblCllctn<TIcons, TValue>(
            this IEnumerable<IClnbl<TIcons, TValue>> src) =>
            src as ReadOnlyCollection<Immtbl<TIcons, TValue>> ?? src?.ToImmtblCllctn();

        public static List<Mtbl<TIcons, TValue>> ToMtblList<TIcons, TValue>(
            this IEnumerable<IClnbl<TIcons, TValue>> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl<TIcons, TValue>> AsMtblList<TIcons, TValue>(
            this IEnumerable<IClnbl<TIcons, TValue>> src) => src as List<Mtbl<TIcons, TValue>> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl<TIcons, TValue>> AsImmtblDictnr<TKey, TIcons, TValue>(
            IDictionaryCore<TKey, IClnbl<TIcons, TValue>> src) => src as ReadOnlyDictionary<TKey, Immtbl<TIcons, TValue>> ?? (
            src as Dictionary<TKey, Mtbl<TIcons, TValue>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl<TIcons, TValue>> AsMtblDictnr<TKey, TIcons, TValue>(
            IDictionaryCore<TKey, IClnbl<TIcons, TValue>> src) => src as Dictionary<TKey, Mtbl<TIcons, TValue>> ?? (
            src as ReadOnlyDictionary<TKey, Immtbl<TIcons, TValue>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsMtbl());

        public static IDictionaryCore<TKey, IClnbl<TIcons, TValue>> ToClnblDictnr<TKey, TIcons, TValue>(
            this Dictionary<TKey, Mtbl<TIcons, TValue>> src) => (IDictionaryCore<TKey, IClnbl<TIcons, TValue>>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TIcons, TValue>>());

        public static IDictionaryCore<TKey, IClnbl<TIcons, TValue>> ToClnblDictnr<TKey, TIcons, TValue>(
            this ReadOnlyDictionary<TKey, Immtbl<TIcons, TValue>> src) => (IDictionaryCore<TKey, IClnbl<TIcons, TValue>>)src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TIcons, TValue>>());
    }
}
