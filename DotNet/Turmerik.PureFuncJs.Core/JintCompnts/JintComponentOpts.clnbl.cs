using Jint.Native.Object;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.PureFuncJs.Core.JintCompnts
{
    public static partial class JintComponentOpts
    {
        public interface IClnbl
        {
            string JsCode { get; }
            string CfgObjRetrieverCode { get; }
            string GlobalThisObjName { get; }
            IJintConsole JintConsole { get; }
            bool IncludeDefaultConsoleObj { get; }
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                JsCode = src.JsCode;
                CfgObjRetrieverCode = src.CfgObjRetrieverCode;
                GlobalThisObjName = src.GlobalThisObjName;
                JintConsole = src.JintConsole;
                IncludeDefaultConsoleObj = src.IncludeDefaultConsoleObj;
            }

            public string JsCode { get; }
            public string CfgObjRetrieverCode { get; }
            public string GlobalThisObjName { get; }
            public IJintConsole JintConsole { get; }
            public bool IncludeDefaultConsoleObj { get; }
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                JsCode = src.JsCode;
                CfgObjRetrieverCode = src.CfgObjRetrieverCode;
                GlobalThisObjName = src.GlobalThisObjName;
                JintConsole = src.JintConsole;
                IncludeDefaultConsoleObj = src.IncludeDefaultConsoleObj;
            }

            public string JsCode { get; set; }
            public string CfgObjRetrieverCode { get; set; }
            public string GlobalThisObjName { get; set; }
            public IJintConsole JintConsole { get; set; }
            public bool IncludeDefaultConsoleObj { get; set; }
        }

        public static Immtbl ToImmtbl(
            this IClnbl src) => new Immtbl(src);

        public static Immtbl AsImmtbl(
            this IClnbl src) => src as Immtbl ?? src?.ToImmtbl();

        public static Mtbl ToMtbl(
            this IClnbl src) => new Mtbl(src);

        public static Mtbl AsMtbl(
            this IClnbl src) => src as Mtbl ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl> ToImmtblCllctn(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl> AsImmtblCllctn(
            this IEnumerable<IClnbl> src) =>
            src as ReadOnlyCollection<Immtbl> ?? src?.ToImmtblCllctn();

        public static List<Mtbl> ToMtblList(
            this IEnumerable<IClnbl> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl> AsMtblList(
            this IEnumerable<IClnbl> src) => src as List<Mtbl> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl> AsImmtblDictnr<TKey>(
            IEnumerable<KeyValuePair<TKey, IClnbl>> src) => src as ReadOnlyDictionary<TKey, Immtbl> ?? (src as Dictionary<TKey, Mtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl> AsMtblDictnr<TKey>(
            IEnumerable<KeyValuePair<TKey, IClnbl>> src) => src as Dictionary<TKey, Mtbl> ?? (src as ReadOnlyDictionary<TKey, Immtbl>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value?.AsMtbl());

        public static IEnumerable<KeyValuePair<TKey, IClnbl>> ToClnblDictnr<TKey>(
            this Dictionary<TKey, Mtbl> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());

        public static IEnumerable<KeyValuePair<TKey, IClnbl>> ToClnblDictnr<TKey>(
            this ReadOnlyDictionary<TKey, Immtbl> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl>());
    }


    public static partial class JintComponentOpts
    {
        public interface IClnbl<TCfg> : IClnbl
        {
            Func<IJintComponent<TCfg>, ObjectInstance, TCfg> CfgFactory { get; }
        }

        public class Immtbl<TCfg> : Immtbl, IClnbl<TCfg>
        {
            public Immtbl(IClnbl<TCfg> src) : base(src)
            {
                CfgFactory = src.CfgFactory;
            }

            public Func<IJintComponent<TCfg>, ObjectInstance, TCfg> CfgFactory { get; }
        }

        public class Mtbl<TCfg> : Mtbl, IClnbl<TCfg>
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl<TCfg> src) : base(src)
            {
                CfgFactory = src.CfgFactory;
            }

            public Func<IJintComponent<TCfg>, ObjectInstance, TCfg> CfgFactory { get; set; }
        }

        public static Immtbl<TCfg> ToImmtbl<TCfg>(
            this IClnbl<TCfg> src) => new Immtbl<TCfg>(src);

        public static Immtbl<TCfg> AsImmtbl<TCfg>(
            this IClnbl<TCfg> src) => src as Immtbl<TCfg> ?? src?.ToImmtbl();

        public static Mtbl<TCfg> ToMtbl<TCfg>(
            this IClnbl<TCfg> src) => new Mtbl<TCfg>(src);

        public static Mtbl<TCfg> AsMtbl<TCfg>(
            this IClnbl<TCfg> src) => src as Mtbl<TCfg> ?? src?.ToMtbl();

        public static ReadOnlyCollection<Immtbl<TCfg>> ToImmtblCllctn<TCfg>(
            this IEnumerable<IClnbl<TCfg>> src) => src?.Select(
                item => item?.AsImmtbl()).RdnlC();

        public static ReadOnlyCollection<Immtbl<TCfg>> AsImmtblCllctn<TCfg>(
            this IEnumerable<IClnbl<TCfg>> src) =>
            src as ReadOnlyCollection<Immtbl<TCfg>> ?? src?.ToImmtblCllctn();

        public static List<Mtbl<TCfg>> ToMtblList<TCfg>(
            this IEnumerable<IClnbl<TCfg>> src) => src?.Select(
                item => item?.AsMtbl()).ToList();

        public static List<Mtbl<TCfg>> AsMtblList<TCfg>(
            this IEnumerable<IClnbl<TCfg>> src) => src as List<Mtbl<TCfg>> ?? src?.ToMtblList();

        public static ReadOnlyDictionary<TKey, Immtbl<TCfg>> AsImmtblDictnr<TKey, TCfg>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TCfg>>> src) => src as ReadOnlyDictionary<TKey, Immtbl<TCfg>> ?? (
            src as Dictionary<TKey, Mtbl<TCfg>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsImmtbl()).RdnlD();

        public static Dictionary<TKey, Mtbl<TCfg>> AsMtblDictnr<TKey, TCfg>(
            IEnumerable<KeyValuePair<TKey, IClnbl<TCfg>>> src) => src as Dictionary<TKey, Mtbl<TCfg>> ?? (
            src as ReadOnlyDictionary<TKey, Immtbl<TCfg>>)?.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.AsMtbl());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TCfg>>> ToClnblDictnr<TKey, TCfg>(
            this Dictionary<TKey, Mtbl<TCfg>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TCfg>>());

        public static IEnumerable<KeyValuePair<TKey, IClnbl<TCfg>>> ToClnblDictnr<TKey, TCfg>(
            this ReadOnlyDictionary<TKey, Immtbl<TCfg>> src) => src.ToDictionary(
                kvp => kvp.Key, kvp => kvp.Value.SafeCast<IClnbl<TCfg>>());
    }
}
