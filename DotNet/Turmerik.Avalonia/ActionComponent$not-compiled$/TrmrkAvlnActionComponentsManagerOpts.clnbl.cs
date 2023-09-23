using Avalonia.Controls;
using Avalonia.Media;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Cloneable;
using Turmerik.Collections;
using Turmerik.Utils;

namespace Turmerik.Avalonia.ActionComponent
{
    public static class TrmrkAvlnActionComponentsManagerOpts
    {
        public interface IClnbl
        {
            Func<string> MsgTextBoxContentGetter { get; }
            Action<string> MsgTextBoxContentSetter { get; }
            Func<IBrush> MsgTextBoxForegroundGetter { get; }
            Action<IBrush> MsgTextBoxForegroundSetter { get; }

            IBrush MsgTextBoxDefaultForeground { get; }
            IBrush MsgTextBoxSuccessForeground { get; }
            IBrush MsgTextBoxErrorForeground { get; }
            LogLevel MinLogLevel { get; }
        }

        public class Immtbl : IClnbl
        {
            public Immtbl(IClnbl src)
            {
                MsgTextBoxContentGetter = src.MsgTextBoxContentGetter;
                MsgTextBoxContentSetter = src.MsgTextBoxContentSetter;
                MsgTextBoxForegroundGetter = src.MsgTextBoxForegroundGetter;
                MsgTextBoxForegroundSetter = src.MsgTextBoxForegroundSetter;
                MsgTextBoxDefaultForeground = src.MsgTextBoxDefaultForeground;
                MsgTextBoxSuccessForeground = src.MsgTextBoxSuccessForeground;
                MsgTextBoxErrorForeground = src.MsgTextBoxErrorForeground;
                MinLogLevel = src.MinLogLevel;
            }

            public Func<string> MsgTextBoxContentGetter { get; }
            public Action<string> MsgTextBoxContentSetter { get; }
            public Func<IBrush> MsgTextBoxForegroundGetter { get; }
            public Action<IBrush> MsgTextBoxForegroundSetter { get; }

            public IBrush MsgTextBoxDefaultForeground { get; }
            public IBrush MsgTextBoxSuccessForeground { get; }
            public IBrush MsgTextBoxErrorForeground { get; }
            public LogLevel MinLogLevel { get; }
        }

        public class Mtbl : IClnbl
        {
            public Mtbl()
            {
            }

            public Mtbl(IClnbl src)
            {
                MsgTextBoxContentGetter = src.MsgTextBoxContentGetter;
                MsgTextBoxContentSetter = src.MsgTextBoxContentSetter;
                MsgTextBoxForegroundGetter = src.MsgTextBoxForegroundGetter;
                MsgTextBoxForegroundSetter = src.MsgTextBoxForegroundSetter;
                MsgTextBoxDefaultForeground = src.MsgTextBoxDefaultForeground;
                MsgTextBoxSuccessForeground = src.MsgTextBoxSuccessForeground;
                MsgTextBoxErrorForeground = src.MsgTextBoxErrorForeground;
                MinLogLevel = src.MinLogLevel;
            }

            public Func<string> MsgTextBoxContentGetter { get; set; }
            public Action<string> MsgTextBoxContentSetter { get; set; }
            public Func<IBrush> MsgTextBoxForegroundGetter { get; set; }
            public Action<IBrush> MsgTextBoxForegroundSetter { get; set; }

            public IBrush MsgTextBoxDefaultForeground { get; set; }
            public IBrush MsgTextBoxSuccessForeground { get; set; }
            public IBrush MsgTextBoxErrorForeground { get; set; }
            public LogLevel MinLogLevel { get; set; }
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
}
