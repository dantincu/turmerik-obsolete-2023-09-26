using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;

namespace Turmerik.MsVSTextTemplating.Components
{
    public static partial class ClnblTypesCodeGeneratorConfigCore
    {
        public static class HelperMethodNames
        {
            public interface IClnbl
            {
                string ToImmtbl { get; }
                string AsImmtbl { get; }
                string ToMtbl { get; }
                string AsMtbl { get; }
                string ToImmtblCllctn { get; }
                string AsImmtblCllctn { get; }
                string ToMtblList { get; }
                string AsMtblList { get; }
                string AsImmtblDictnr { get; }
                string AsMtblDictnr { get; }
            }

            public class Immtbl : IClnbl
            {
                public Immtbl(IClnbl src)
                {
                    ToImmtbl = src.ToImmtbl;
                    AsImmtbl = src.AsImmtbl;
                    ToMtbl = src.ToMtbl;
                    AsMtbl = src.AsMtbl;
                    ToImmtblCllctn = src.ToImmtblCllctn;
                    AsImmtblCllctn = src.AsImmtblCllctn;
                    ToMtblList = src.ToMtblList;
                    AsMtblList = src.AsMtblList;
                    AsImmtblDictnr = src.AsImmtblDictnr;
                    AsMtblDictnr = src.AsMtblDictnr;
                }

                public string ToImmtbl { get; }
                public string AsImmtbl { get; }
                public string ToMtbl { get; }
                public string AsMtbl { get; }
                public string ToImmtblCllctn { get; }
                public string AsImmtblCllctn { get; }
                public string ToMtblList { get; }
                public string AsMtblList { get; }
                public string AsImmtblDictnr { get; }
                public string AsMtblDictnr { get; }
            }

            public class Mtbl : IClnbl
            {
                public Mtbl()
                {
                }

                public Mtbl(IClnbl src)
                {
                    ToImmtbl = src.ToImmtbl;
                    AsImmtbl = src.AsImmtbl;
                    ToMtbl = src.ToMtbl;
                    AsMtbl = src.AsMtbl;
                    ToImmtblCllctn = src.ToImmtblCllctn;
                    AsImmtblCllctn = src.AsImmtblCllctn;
                    ToMtblList = src.ToMtblList;
                    AsMtblList = src.AsMtblList;
                    AsImmtblDictnr = src.AsImmtblDictnr;
                    AsMtblDictnr = src.AsMtblDictnr;
                }

                public string ToImmtbl { get; set; }
                public string AsImmtbl { get; set; }
                public string ToMtbl { get; set; }
                public string AsMtbl { get; set; }
                public string ToImmtblCllctn { get; set; }
                public string AsImmtblCllctn { get; set; }
                public string ToMtblList { get; set; }
                public string AsMtblList { get; set; }
                public string AsImmtblDictnr { get; set; }
                public string AsMtblDictnr { get; set; }
            }
        }

        public static HelperMethodNames.Immtbl ToImmtbl(
            this HelperMethodNames.IClnbl src) => new HelperMethodNames.Immtbl(src);

        public static HelperMethodNames.Immtbl AsImmtbl(
            this HelperMethodNames.IClnbl src) => (src as HelperMethodNames.Immtbl) ?? src?.ToImmtbl();

        public static HelperMethodNames.Mtbl ToMtbl(
            this HelperMethodNames.IClnbl src) => new HelperMethodNames.Mtbl(src);

        public static HelperMethodNames.Mtbl AsMtbl(
            this HelperMethodNames.IClnbl src) => (src as HelperMethodNames.Mtbl) ?? src?.ToMtbl();

        public static ReadOnlyCollection<HelperMethodNames.Immtbl> ToImmtblCllctn(
            this IEnumerable<HelperMethodNames.IClnbl> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<HelperMethodNames.Immtbl> AsImmtblCllctn(
            this IEnumerable<HelperMethodNames.IClnbl> src) => (
            src as ReadOnlyCollection<HelperMethodNames.Immtbl>) ?? src?.ToImmtblCllctn();

        public static List<HelperMethodNames.Mtbl> ToMtblList(
            this IEnumerable<HelperMethodNames.IClnbl> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<HelperMethodNames.Mtbl> AsMtblList(
            this IEnumerable<HelperMethodNames.IClnbl> src) => (src as List<HelperMethodNames.Mtbl>) ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, HelperMethodNames.Immtbl> AsImmtblDictnr<TKey>(
            IDictionaryCore<TKey, HelperMethodNames.IClnbl> src) => (
                src as ReadOnlyDictionary<TKey, HelperMethodNames.Immtbl>) ?? (
                src as Dictionary<TKey, HelperMethodNames.Mtbl>)?.ToDictionary(
                    kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, HelperMethodNames.Mtbl> AsMtblDictnr<TKey>(
            IDictionaryCore<TKey, HelperMethodNames.IClnbl> src) => (
                src as Dictionary<TKey, HelperMethodNames.Mtbl>) ?? (
                src as ReadOnlyDictionary<TKey, HelperMethodNames.Immtbl>)?.ToDictionary(
                    kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());
    }
}
