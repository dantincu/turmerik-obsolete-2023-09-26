using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Cloneable;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.WinForms.Utils
{
    public static class DgvComboBoxCellOpts
    {
        public interface IClnbl<TCellValue> : DgvCellOptsCore.IClnbl<DataGridViewComboBoxCell, TCellValue>
        {
            object[] ComboBoxItems { get; }
            string DisplayMember { get; }
            string ValueMember { get; }
        }

        public class Immtbl<TCellValue> : DgvCellOptsCore.Immtbl<DataGridViewComboBoxCell, TCellValue>, IClnbl<TCellValue>
        {
            public Immtbl(IClnbl<TCellValue> src) : base(src)
            {
                ComboBoxItems = src.ComboBoxItems;
                DisplayMember = src.DisplayMember;
                ValueMember = src.ValueMember;
            }

            public object[] ComboBoxItems { get; }
            public string DisplayMember { get; }
            public string ValueMember { get; }
        }

        public class Mtbl<TCellValue> : DgvCellOptsCore.Mtbl<DataGridViewComboBoxCell, TCellValue>, IClnbl<TCellValue>
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl<TCellValue> src) : base(src)
            {
                ComboBoxItems = src.ComboBoxItems;
                DisplayMember = src.DisplayMember;
                ValueMember = src.ValueMember;
            }

            public object[] ComboBoxItems { get; set; }
            public string DisplayMember { get; set; }
            public string ValueMember { get; set; }
        }

        public static Immtbl<TCellValue> ToImmtbl<TCellValue>(
            this IClnbl<TCellValue> src) => new Immtbl<TCellValue>(src);

        public static Immtbl<TCellValue> AsImmtbl<TCellValue>(
            this IClnbl<TCellValue> src) => src as Immtbl<TCellValue> ?? src?.ToImmtbl();

        public static Mtbl<TCellValue> ToMtbl<TCellValue>(
            this IClnbl<TCellValue> src) => new Mtbl<TCellValue>(src);

        public static Mtbl<TCellValue> AsMtbl<TCellValue>(
            this IClnbl<TCellValue> src) => src as Mtbl<TCellValue> ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl<TCellValue>> ToImmtblCllctn<TCellValue>(
            this IEnumerable<IClnbl<TCellValue>> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl<TCellValue>> AsImmtblCllctn<TCellValue>(
            this IEnumerable<IClnbl<TCellValue>> src) =>
            src as ReadOnlyCollection<Immtbl<TCellValue>> ?? src?.ToImmtblCllctn();

        public static List<Mtbl<TCellValue>> ToMtblList<TCellValue>(
            this IEnumerable<IClnbl<TCellValue>> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl<TCellValue>> AsMtblList<TCellValue>(
            this IEnumerable<IClnbl<TCellValue>> src) => src as List<Mtbl<TCellValue>> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl<TCellValue>> AsImmtblDictnr<TKey, TCellValue>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TCellValue>>> src) => src as ReadOnlyDictionary<TKey, Immtbl<TCellValue>> ?? (
            src as Dictionary<TKey, Mtbl<TCellValue>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl<TCellValue>> AsMtblDictnr<TKey, TCellValue>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TCellValue>>> src) => src as Dictionary<TKey, Mtbl<TCellValue>> ?? (
            src as ReadOnlyDictionary<TKey, Immtbl<TCellValue>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsMtbl());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TCellValue>>> ToClnblDictnr<TKey, TCellValue>(
            this Dictionary<TKey, Mtbl<TCellValue>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TCellValue>>());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TCellValue>>> ToClnblDictnr<TKey, TCellValue>(
            this ReadOnlyDictionary<TKey, Immtbl<TCellValue>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TCellValue>>());
    }
}
