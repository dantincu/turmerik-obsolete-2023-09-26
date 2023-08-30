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
    public static class DgvCellOptsCore
    {
        public interface IClnbl<TDgvCell, CellVal>
        {
            TDgvCell Cell { get; }
            Func<TDgvCell> CellFactory { get; }
            CellVal CellValue { get; }
            Func<CellVal, bool> IsValidValuePredicate { get; }
            Action<TDgvCell> Callback { get; }
            DataGridViewCellStyle Style { get; }
        }

        public class Immtbl<TDgvCell, CellVal> : IClnbl<TDgvCell, CellVal>
        {
            public Immtbl(IClnbl<TDgvCell, CellVal> src)
            {
                Cell = src.Cell;
                CellFactory = src.CellFactory;
                CellValue = src.CellValue;
                IsValidValuePredicate = src.IsValidValuePredicate;
                Callback = src.Callback;
                Style = src.Style;
            }

            public TDgvCell Cell { get; }
            public Func<TDgvCell> CellFactory { get; }
            public CellVal CellValue { get; }
            public Func<CellVal, bool> IsValidValuePredicate { get; }
            public Action<TDgvCell> Callback { get; }
            public DataGridViewCellStyle Style { get; }
        }

        public class Mtbl<TDgvCell, CellVal> : IClnbl<TDgvCell, CellVal>
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl<TDgvCell, CellVal> src)
            {
                Cell = src.Cell;
                CellFactory = src.CellFactory;
                CellValue = src.CellValue;
                IsValidValuePredicate = src.IsValidValuePredicate;
                Callback = src.Callback;
                Style = src.Style;
            }

            public TDgvCell Cell { get; set; }
            public Func<TDgvCell> CellFactory { get; set; }
            public CellVal CellValue { get; set; }
            public Func<CellVal, bool> IsValidValuePredicate { get; set; }
            public Action<TDgvCell> Callback { get; set; }
            public DataGridViewCellStyle Style { get; set; }
        }

        public static Immtbl<TDgvCell, CellVal> ToImmtbl<TDgvCell, CellVal>(
            this IClnbl<TDgvCell, CellVal> src) => new Immtbl<TDgvCell, CellVal>(src);

        public static Immtbl<TDgvCell, CellVal> AsImmtbl<TDgvCell, CellVal>(
            this IClnbl<TDgvCell, CellVal> src) => src as Immtbl<TDgvCell, CellVal> ?? src?.ToImmtbl();

        public static Mtbl<TDgvCell, CellVal> ToMtbl<TDgvCell, CellVal>(
            this IClnbl<TDgvCell, CellVal> src) => new Mtbl<TDgvCell, CellVal>(src);

        public static Mtbl<TDgvCell, CellVal> AsMtbl<TDgvCell, CellVal>(
            this IClnbl<TDgvCell, CellVal> src) => src as Mtbl<TDgvCell, CellVal> ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl<TDgvCell, CellVal>> ToImmtblCllctn<TDgvCell, CellVal>(
            this IEnumerable<IClnbl<TDgvCell, CellVal>> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl<TDgvCell, CellVal>> AsImmtblCllctn<TDgvCell, CellVal>(
            this IEnumerable<IClnbl<TDgvCell, CellVal>> src) =>
            src as ReadOnlyCollection<Immtbl<TDgvCell, CellVal>> ?? src?.ToImmtblCllctn();

        public static List<Mtbl<TDgvCell, CellVal>> ToMtblList<TDgvCell, CellVal>(
            this IEnumerable<IClnbl<TDgvCell, CellVal>> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl<TDgvCell, CellVal>> AsMtblList<TDgvCell, CellVal>(
            this IEnumerable<IClnbl<TDgvCell, CellVal>> src) => src as List<Mtbl<TDgvCell, CellVal>> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl<TDgvCell, CellVal>> AsImmtblDictnr<TKey, TDgvCell, CellVal>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TDgvCell, CellVal>>> src) => src as ReadOnlyDictionary<TKey, Immtbl<TDgvCell, CellVal>> ?? (
            src as Dictionary<TKey, Mtbl<TDgvCell, CellVal>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl<TDgvCell, CellVal>> AsMtblDictnr<TKey, TDgvCell, CellVal>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TDgvCell, CellVal>>> src) => src as Dictionary<TKey, Mtbl<TDgvCell, CellVal>> ?? (
            src as ReadOnlyDictionary<TKey, Immtbl<TDgvCell, CellVal>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsMtbl());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TDgvCell, CellVal>>> ToClnblDictnr<TKey, TDgvCell, CellVal>(
            this Dictionary<TKey, Mtbl<TDgvCell, CellVal>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TDgvCell, CellVal>>());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TDgvCell, CellVal>>> ToClnblDictnr<TKey, TDgvCell, CellVal>(
            this ReadOnlyDictionary<TKey, Immtbl<TDgvCell, CellVal>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TDgvCell, CellVal>>());
    }
}
